using AlainaGarcia_AP1_P2.DAL;
using AlainaGarcia_AP1_P2.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Azure.Core.HttpHeader;

namespace AlainaGarcia_AP1_P2.Services;

public class CombosDetallesService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<List<Productos>> Listar(Expression<Func<Productos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Productos
        .AsNoTracking()
        .Where(criterio)
        .ToListAsync();
    }

    public async Task<bool> Eliminar(int detalleId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var detalle = await contexto.CombosDetalles.FindAsync(detalleId);
        if (detalle != null)
        {
            await AfectarCantidad(detalle, false);
            contexto.CombosDetalles.Remove(detalle);
            await contexto.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task AfectarCantidad(CombosDetalles detalles, bool resta)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
       // foreach (var item in detalles)
       // {
            var detalle = await contexto.Productos.SingleOrDefaultAsync(d => d.ProductoId == detalles.ProductoId);
            if (resta)
                detalle.Existencia -= detalles.Cantidad;
            else
                detalle.Existencia += detalles.Cantidad;
        //}
        await contexto.SaveChangesAsync();
    }
}
