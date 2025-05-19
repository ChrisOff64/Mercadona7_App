using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Mercadona7_App.Models
{
    public class Produit
    {
        public int ProduitID  { get; set; }
        [MaxLength(70)]
        [Display(Name = "Libellé")]
        [Required(ErrorMessage = "Le Libellé est un champ obligatoire")]
        public string? Libelle { get; set; }
        [Display(Name = "Description")]
        public string? Description { get; set; }
        [Display(Name = "Prix")]
        [Required(ErrorMessage = "Le prix est un champ obligatoire")]
        [Column(TypeName = "decimal(18,4)")]
        public Decimal Prix { get; set; }
        public string? Image { get; set; }
        [Display(Name = "Catégorie")]
        [Required(ErrorMessage = "La catégorie est un champ obligatoire")]
        [MaxLength(30)] 
        public string? Categorie { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProduitPromotion>? ProduitPromotions { get; set; }
    }

     

}
