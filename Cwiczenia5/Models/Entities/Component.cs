namespace Cwiczenia5.Models.Entities;

public class Component
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int ComponentManufacturerId { get; set; }
    public int ComponentTypeId { get; set; }

    public ComponentManufacturer Manufacturer { get; set; } = null!;
    public ComponentType Type { get; set; } = null!;
    public ICollection<PcComponent> PcComponents { get; set; } = new List<PcComponent>();
}
