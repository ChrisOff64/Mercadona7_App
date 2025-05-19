using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Mercadona7_App.Models
{
    public class ProduitViewModel
    {
        public Produit ? Produit { get; set; }

        public IFormFile? Upload { get; set; }

    }
   


}
