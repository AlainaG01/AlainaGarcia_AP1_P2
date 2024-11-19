using System.ComponentModel.DataAnnotations;

namespace AlainaGarcia_AP1_P2.Models;

public class Productos
{
    [Key]
    public int ArticuloId { get; set; }

    [Required(ErrorMessage ="Campo obligatorio")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public double Costo { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public double Precio { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public int Existencia { get; set; }
}
