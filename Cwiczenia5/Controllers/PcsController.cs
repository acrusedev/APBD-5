using Cwiczenia5.DTOs;
using Cwiczenia5.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PcsController(IPcService pcService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<PcListItemDto>>> GetAll()
    {
        try
        {
            var pcs = await pcService.GetAllAsync();
            return Ok(pcs);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpGet("{id:int}/components")]
    public async Task<ActionResult<PcDetailsDto>> GetByIdWithComponents(int id)
    {
        try
        {
            var pc = await pcService.GetByIdWithComponentsAsync(id);
            if (pc is null)
            {
                return NotFound();
            }

            return Ok(pc);
        } catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
      
    }

    [HttpPost]
    public async Task<ActionResult<PcListItemDto>> Create([FromBody] CreateOrUpdatePcRequestDto request)
    {
        try
        {
            var createdPc = await pcService.CreateAsync(request);
            return CreatedAtAction(nameof(GetByIdWithComponents), new { id = createdPc.Id }, createdPc);
        } catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateOrUpdatePcRequestDto request)
    {
        try
        {
            var updated = await pcService.UpdateAsync(id, request);
            if (!updated)
            {
                return NotFound();
            }

            return Ok();
        } catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await pcService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        } catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
       
    }
}
