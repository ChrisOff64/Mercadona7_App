using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Mercadona7_App.Models
{
    public class PromotionViewModel
    {
        public Promotion Promotion { get; set; }

        public int ProduitID { get; set; }
        public bool ModeCreation { get; set; }

        public PromotionViewModel(string categorieSelectionnee="")
        {
            //CategorieSelectionnee = categorieSelectionnee;
        }
        public PromotionViewModel()
        {
            
        }
    }
   


}
