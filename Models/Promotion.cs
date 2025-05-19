using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mercadona7_App.Models
{
    public class Promotion
    {
    public int PromotionID { get; set; }
    [DataType(DataType.Date)]
    [Display(Name = "Date de début")]
    [Required(ErrorMessage = "La date de début est un champ obligatoire")]
    
    public DateTime ? DateDebut { get; set; }
    [DataType(DataType.Date)]
    [Display(Name = "Date de fin")]
    [Required(ErrorMessage = "La date de fin est un champ obligatoire")]
    public DateTime ? DateFin { get; set; }
    [Required(ErrorMessage = "La remise est un champ obligatoire")]
    [Range(0.01, 100.00)]
    [Display(Name = "Remise (en %)")]
    public float ? Remise { get; set; }
    public virtual ICollection<ProduitPromotion> ?ProduitPromotions { get; set; }
    }
}
