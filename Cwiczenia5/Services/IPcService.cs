using Cwiczenia5.DTOs;

namespace Cwiczenia5.Services;

public interface IPcService
{
    Task<IReadOnlyCollection<PcListItemDto>> GetAllAsync();
    Task<PcDetailsDto?> GetByIdWithComponentsAsync(int id);
    Task<PcListItemDto> CreateAsync(CreateOrUpdatePcRequestDto request);
    Task<bool> UpdateAsync(int id, CreateOrUpdatePcRequestDto request);
    Task<bool> DeleteAsync(int id);
}
