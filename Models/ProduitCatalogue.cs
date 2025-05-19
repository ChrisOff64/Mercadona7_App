using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercadona7_App.Models
{
    public class ProduitCatalogue
    {
        //public int ProduitID  { get; set; }
        //public string? Libelle { get; set; }
        //public string? Description { get; set; }
        //public Decimal Prix { get; set; }
        //public string? Image { get; set; }
        //public string? Categorie { get; set; }
        public Produit? Produit { get; set; }
        public float? RemiseEnVigueur { get; set; }
    }



}
