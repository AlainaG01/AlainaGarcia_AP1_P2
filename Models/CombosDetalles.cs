using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlainaGarcia_AP1_P2.Models;

public class CombosDetalles
{
    [Key]
    public int DetalleId { get; set; }

    [Required]
    [ForeignKey("Combo")]
    public int ComboId { get; set; }
    public Combos? Combo { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    [ForeignKey("Producto")]
    public int ProductoId { get; set; }
    public Productos? Producto { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    [RegularExpression(@"^\d?$", ErrorMessage = "Solo se permiten numeros enteros")]
    public int Cantidad { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten numeros enteros o decimales")]
    public double Costo { get; set; }
}
