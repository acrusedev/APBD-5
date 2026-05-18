using Cwiczenia5.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia5.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Pc> Pcs => Set<Pc>();
    public DbSet<Component> Components => Set<Component>();
    public DbSet<ComponentManufacturer> ComponentManufacturers => Set<ComponentManufacturer>();
    public DbSet<ComponentType> ComponentTypes => Set<ComponentType>();
    public DbSet<PcComponent> PcComponents => Set<PcComponent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pc>(entity =>
        {
            entity.ToTable("PCs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Weight).HasColumnType("float");
            entity.Property(e => e.Warranty).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.Stock).IsRequired();

            entity.HasData(
                new Pc { Id = 1, Name = "Gaming Beast X", Weight = 12.5, Warranty = 36, CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0), Stock = 5 },
                new Pc { Id = 2, Name = "Office Mini Pro", Weight = 4.2, Warranty = 24, CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0), Stock = 12 },
                new Pc { Id = 3, Name = "Creator Studio Z", Weight = 8.9, Warranty = 48, CreatedAt = new DateTime(2026, 3, 20, 11, 15, 0), Stock = 7 }
            );
        });

        modelBuilder.Entity<ComponentManufacturer>(entity =>
        {
            entity.ToTable("ComponentManufacturers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).HasMaxLength(30).IsRequired();
            entity.Property(e => e.FullName).HasMaxLength(300).IsRequired();
            entity.Property(e => e.FoundationDate).HasColumnType("date").IsRequired();

            entity.HasData(
                new ComponentManufacturer { Id = 1, Abbreviation = "AMD", FullName = "Advanced Micro Devices", FoundationDate = new DateOnly(1969, 5, 1) },
                new ComponentManufacturer { Id = 2, Abbreviation = "NV", FullName = "NVIDIA Corporation", FoundationDate = new DateOnly(1993, 4, 5) },
                new ComponentManufacturer { Id = 3, Abbreviation = "COR", FullName = "Corsair Gaming Inc.", FoundationDate = new DateOnly(1994, 1, 1) }
            );
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.ToTable("ComponentTypes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(150).IsRequired();

            entity.HasData(
                new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor" },
                new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
                new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory" }
            );
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.ToTable("Components");
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasMaxLength(10).IsFixedLength().IsRequired();
            entity.Property(e => e.Name).HasMaxLength(300).IsRequired();
            entity.Property(e => e.Description).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(e => e.ComponentManufacturerId).IsRequired();
            entity.Property(e => e.ComponentTypeId).IsRequired();

            entity.HasOne(e => e.Manufacturer)
                .WithMany(e => e.Components)
                .HasForeignKey(e => e.ComponentManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Type)
                .WithMany(e => e.Components)
                .HasForeignKey(e => e.ComponentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasData(
                new Component
                {
                    Code = "CPU0000001",
                    Name = "Ryzen 7 7800X3D",
                    Description = "8-core gaming processor",
                    ComponentManufacturerId = 1,
                    ComponentTypeId = 1
                },
                new Component
                {
                    Code = "GPU0000001",
                    Name = "RTX 4080 Super",
                    Description = "High-end gaming graphics card",
                    ComponentManufacturerId = 2,
                    ComponentTypeId = 2
                },
                new Component
                {
                    Code = "RAM0000001",
                    Name = "Corsair Vengeance DDR5 16GB",
                    Description = "DDR5 RAM module 16GB",
                    ComponentManufacturerId = 3,
                    ComponentTypeId = 3
                }
            );
        });

        modelBuilder.Entity<PcComponent>(entity =>
        {
            entity.ToTable("PCComponents");
            entity.HasKey(e => new { e.PcId, e.ComponentCode });
            entity.Property(e => e.ComponentCode).HasMaxLength(10).IsFixedLength().IsRequired();
            entity.Property(e => e.Amount).IsRequired();

            entity.HasOne(e => e.Pc)
                .WithMany(e => e.PcComponents)
                .HasForeignKey(e => e.PcId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Component)
                .WithMany(e => e.PcComponents)
                .HasForeignKey(e => e.ComponentCode)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasData(
                new PcComponent { PcId = 1, ComponentCode = "CPU0000001", Amount = 1 },
                new PcComponent { PcId = 1, ComponentCode = "GPU0000001", Amount = 1 },
                new PcComponent { PcId = 1, ComponentCode = "RAM0000001", Amount = 2 },
                new PcComponent { PcId = 2, ComponentCode = "CPU0000001", Amount = 1 },
                new PcComponent { PcId = 2, ComponentCode = "RAM0000001", Amount = 2 },
                new PcComponent { PcId = 3, ComponentCode = "CPU0000001", Amount = 1 },
                new PcComponent { PcId = 3, ComponentCode = "GPU0000001", Amount = 1 }
            );
        });
    }
}
