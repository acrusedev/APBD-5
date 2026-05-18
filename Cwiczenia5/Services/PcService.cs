using Cwiczenia5.Data;
using Cwiczenia5.DTOs;
using Cwiczenia5.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia5.Services;

public class PcService(AppDbContext dbContext) : IPcService
{
    public async Task<IReadOnlyCollection<PcListItemDto>> GetAllAsync()
    {
        return await dbContext.Pcs
            .AsNoTracking()
            .OrderBy(pc => pc.Id)
            .Select(pc => new PcListItemDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock
            })
            .ToListAsync();
    }

    public async Task<PcDetailsDto?> GetByIdWithComponentsAsync(int id)
    {
        return await dbContext.Pcs
            .AsNoTracking()
            .Where(pc => pc.Id == id)
            .Select(pc => new PcDetailsDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock,
                Components = pc.PcComponents
                    .OrderBy(pcComponent => pcComponent.ComponentCode)
                    .Select(pcComponent => new PcComponentDto
                    {
                        Amount = pcComponent.Amount,
                        Component = new ComponentDto
                        {
                            Code = pcComponent.Component.Code,
                            Name = pcComponent.Component.Name,
                            Description = pcComponent.Component.Description,
                            Manufacturer = new ComponentManufacturerDto
                            {
                                Id = pcComponent.Component.Manufacturer.Id,
                                Abbreviation = pcComponent.Component.Manufacturer.Abbreviation,
                                FullName = pcComponent.Component.Manufacturer.FullName,
                                FoundationDate = pcComponent.Component.Manufacturer.FoundationDate
                            },
                            Type = new ComponentTypeDto
                            {
                                Id = pcComponent.Component.Type.Id,
                                Abbreviation = pcComponent.Component.Type.Abbreviation,
                                Name = pcComponent.Component.Type.Name
                            }
                        }
                    })
                    .ToList()
            })
            .SingleOrDefaultAsync();
    }

    public async Task<PcListItemDto> CreateAsync(CreateOrUpdatePcRequestDto request)
    {
        var pc = new Pc
        {
            Name = request.Name,
            Weight = request.Weight,
            Warranty = request.Warranty,
            CreatedAt = request.CreatedAt,
            Stock = request.Stock
        };

        dbContext.Pcs.Add(pc);
        await dbContext.SaveChangesAsync();

        return MapPc(pc);
    }

    public async Task<bool> UpdateAsync(int id, CreateOrUpdatePcRequestDto request)
    {
        var pc = await dbContext.Pcs.SingleOrDefaultAsync(x => x.Id == id);
        if (pc is null)
        {
            return false;
        }

        pc.Name = request.Name;
        pc.Weight = request.Weight;
        pc.Warranty = request.Warranty;
        pc.CreatedAt = request.CreatedAt;
        pc.Stock = request.Stock;

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pc = await dbContext.Pcs.SingleOrDefaultAsync(x => x.Id == id);
        if (pc is null)
        {
            return false;
        }

        dbContext.Pcs.Remove(pc);
        await dbContext.SaveChangesAsync();
        return true;
    }

    private static PcListItemDto MapPc(Pc pc)
    {
        return new PcListItemDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }
}
