using System.ComponentModel.DataAnnotations;

namespace Cwiczenia5.DTOs;

public class CreateOrUpdatePcRequestDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Range(0.01, double.MaxValue)]
    public double Weight { get; set; }

    [Range(0, int.MaxValue)]
    public int Warranty { get; set; }

    public DateTime CreatedAt { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}

public class PcListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Weight { get; set; }
    public int Warranty { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Stock { get; set; }
}

public class PcDetailsDto : PcListItemDto
{
    public List<PcComponentDto> Components { get; set; } = [];
}

public class PcComponentDto
{
    public int Amount { get; set; }
    public ComponentDto Component { get; set; } = null!;
}

public class ComponentDto
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ComponentManufacturerDto Manufacturer { get; set; } = null!;
    public ComponentTypeDto Type { get; set; } = null!;
}

public class ComponentManufacturerDto
{
    public int Id { get; set; }
    public string Abbreviation { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public DateOnly FoundationDate { get; set; }
}

public class ComponentTypeDto
{
    public int Id { get; set; }
    public string Abbreviation { get; set; } = null!;
    public string Name { get; set; } = null!;
}
