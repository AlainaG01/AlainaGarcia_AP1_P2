﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlainaGarcia_AP1_P2.Models;

public class Combos
{
    [Key]
    public int ComboId { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Campo obligatorio")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public double Precio { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public bool Vendido { get; set; } = false;

    [ForeignKey("ComboId")]
    public ICollection<CombosDetalles> CombosDetalle { get; set; } = new List<CombosDetalles>();

}
