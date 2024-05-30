using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Mercadona7_App.Models
{
    public class PromotionsViewModel
    {
        public class PromotionProduit
        {
            public Promotion Promotion { get; set; }
            public bool Affectee { get; set; }
        }

        public List<PromotionProduit> PromotionsProduit { get; set; }
      
        public int ProduitID { get; set; }
       
        public PromotionsViewModel(int produitID = 0)
        {
            ProduitID = produitID;
        }
        public PromotionsViewModel()
        {
            
        }
    }
   


}
