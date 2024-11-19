using AlainaGarcia_AP1_P2.DAL;
using AlainaGarcia_AP1_P2.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AlainaGarcia_AP1_P2.Services;

public class CombosServices(IDbContextFactory<Contexto> DbFactory)
{
    private async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Combos.AnyAsync(e => e.ComboId == id);
    }

    private async Task<bool> Insertar(Combos combo)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        await AfectarCantidad(combo.CombosDetalle.ToArray(), true);
        contexto.Combos.Add(combo);
        return await contexto.SaveChangesAsync() > 0; ;
    }

    private async Task<bool> Modificar(Combos combo)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var comboOriginal = await contexto.Combos
            .Include(t => t.CombosDetalle)
            .FirstOrDefaultAsync(t => t.ComboId == combo.ComboId);

        if (comboOriginal == null)
            return false;

        await AfectarCantidad(comboOriginal.CombosDetalle.ToArray(), false);

        foreach (var detalleOriginal in comboOriginal.CombosDetalle)
        {
            if (!combo.CombosDetalle.Any(d => d.DetalleId == detalleOriginal.DetalleId))
            {
                contexto.CombosDetalles.Remove(detalleOriginal);
            }
        }

        await AfectarCantidad(combo.CombosDetalle.ToArray(), true);

        contexto.Entry(comboOriginal).CurrentValues.SetValues(combo);

        foreach (var detalle in combo.CombosDetalle)
        {
            var detalleExistente = comboOriginal.CombosDetalle
                .FirstOrDefault(d => d.DetalleId == detalle.DetalleId);

            if (detalleExistente != null)
            {
                contexto.Entry(detalleExistente).CurrentValues.SetValues(detalle);
            }
            else
            {
                comboOriginal.CombosDetalle.Add(detalle);
            }
        }

        return await contexto.SaveChangesAsync() > 0;
    }


    public async Task<bool> Guardar(Combos combo)
    {
        if (!await Existe(combo.ComboId))
        {
            return await Insertar(combo);
        }
        else
        {
            return await Modificar(combo);
        }
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var combo = contexto.Combos.Find(id);
        if (combo == null)
            return false;


        await AfectarCantidad(combo.CombosDetalle.ToArray(), false);
        return await contexto.Combos
            .Include(m => m.CombosDetalle)
            .Where(m => m.ComboId == combo.ComboId)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<Combos> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Combos
        .Include(m => m.CombosDetalle)
        .AsNoTracking()
        .FirstOrDefaultAsync(m => m.ComboId == id);
    }

    public async Task<List<Combos>> Listar(Expression<Func<Combos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Combos
        .Include(m => m.CombosDetalle)
        .AsNoTracking()
        .Where(criterio)
        .ToListAsync();
    }

    public async Task<Combos> BuscarConDetalle(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Combos
        .Include(m => m.CombosDetalle)
        .ThenInclude(md => md.Producto)
        .FirstOrDefaultAsync(m => m.ComboId == id);
    }

    public async Task AfectarCantidad(CombosDetalles[] detalles, bool resta)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        foreach (var item in detalles)
        {
            var detalle = await contexto.Productos.SingleOrDefaultAsync(d => d.ProductoId == item.ProductoId);
            if (resta)
                detalle.Existencia -= item.Cantidad;
            else
                detalle.Existencia += item.Cantidad;
        }
        await contexto.SaveChangesAsync();
    }

    public async Task<bool> ExisteModelo(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Combos.AnyAsync(m => m.ComboId == id);
    }
}