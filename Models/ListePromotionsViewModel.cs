using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Mercadona7_App.Models
{
    public class ListePromotionsViewModel
    {
        public class PromotionProduit
        {
            public Promotion Promotion { get; set; }
            public bool Affectee { get; set; }
        }

        public List<PromotionProduit> PromotionsProduit { get; set; }
      
        public int ProduitID { get; set; }
        public string ? Libelle { get; set; }

        public ListePromotionsViewModel(int produitID = 0)
        {
            ProduitID = produitID;
            
        }
        public ListePromotionsViewModel()
        {
            
        }
    }
   


}
