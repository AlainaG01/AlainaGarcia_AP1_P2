using AlainaGarcia_AP1_P2.Models;
using Microsoft.EntityFrameworkCore;

namespace AlainaGarcia_AP1_P2.DAL;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

    public DbSet<Combos> Combos { get; set; }
    public DbSet<CombosDetalles> CombosDetalles { get; set; }
    public DbSet<Productos> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Productos>().HasData(new List<Productos>()
        {
            new Productos(){ArticuloId = 1, Descripcion = "Memorio RAM", Costo = 4500, Precio = 5200, Existencia = 25},
            new Productos(){ArticuloId = 2, Descripcion = "Disco Duro", Costo = 4700, Precio = 5450, Existencia = 20},
            new Productos(){ArticuloId = 3, Descripcion = "Teclado", Costo = 999, Precio = 1200, Existencia = 25},
            new Productos(){ArticuloId = 4, Descripcion = "Mouse", Costo = 545, Precio = 850, Existencia = 25}
        });
    }
}
