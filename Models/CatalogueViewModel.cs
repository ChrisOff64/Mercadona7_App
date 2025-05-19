using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Mercadona7_App.Models
{
    public class CatalogueViewModel
    {
            public List<ProduitCatalogue> Produits { get; set; }
            public IEnumerable<SelectListItem> ListeCategories { get; set; }
        
            public string CategorieSelectionnee { get; set; }
         public CatalogueViewModel(string categorieSelectionnee="")
        {
            CategorieSelectionnee = categorieSelectionnee;
        }
        public CatalogueViewModel()
        {
            
        }
    }
   


}
