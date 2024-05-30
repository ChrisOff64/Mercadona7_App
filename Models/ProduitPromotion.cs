namespace Mercadona7_App.Models
{
    public class ProduitPromotion
    {
        public int ProduitID { get; set; }
        public virtual Produit Produit { get; set; }
        public int PromotionID { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
