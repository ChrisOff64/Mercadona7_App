//using Castle.Core.Internal;
using Mercadona7_App.Data;
using Mercadona7_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static Mercadona7_App.Models.PromotionsViewModel;

namespace Mercadona7_App.Controllers

{
    [Authorize(Roles = "Administrateur")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        List<Produit> Produits = new List<Produit>
        {
            new Produit{Libelle="Tête de vissage plaquiste Wolfcraft"
            ,Categorie="Outillage"
            ,Description=""
            ,Prix=7.9m},
            new Produit{Libelle="Lingot® Piment Jalapeno pour potager Véritable®"
            ,Categorie="Jardin et terrasse"
            ,Description="Période de plantation: Toute l'année\nPériode de récolte: dés 4 à 8 semaines jusqu'à 5 mois"
            ,Prix=7.95m},
            new Produit{Libelle="Pince à cliquet Wolfcraft FZR 60"
            ,Categorie="Outillage"
            ,Description="Pince à cliquet A cliquet"
            ,Prix=8.90m},
            new Produit{Libelle="Fil Nylium Etoile pour Coupe-bordure diamètre 3mm X Longueur 15 M"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Prix=10.90m},
            new Produit{Libelle="Balai de jardin à poils recourbés + raclette Gardirex"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Prix=17.90m},
            new Produit{Libelle="Flexible de douche VitalioFlex 1750 cm gris Grohe"
            ,Categorie="Salle de bains et WC"
            ,Description=""
            ,Prix=18.90m},
            new Produit{Libelle="Lot de 3 étagères murales cubes Fixy 5Five P. 9 cm en bambou"
            ,Categorie="Rangement - Dressing"
            ,Description="Dimensions taille L : H. 25 cm x L. 25 cm x P. 9 cm\nDimensions taille M : H. 20 cm x L. 20 cm x P. 9 cm\nDimensions taille S : H. 15 cm x L. 15 cm x P. 9 cm"
            ,Prix=19.90m},
            new Produit{Libelle="4 dalles de potager maxi Garantia"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Prix=19.90m},
            new Produit{Libelle="Etagère de douche d'angle chromé Hansgrohe Logis Universal"
            ,Categorie="Salle de bains et WC"
            ,Description=""
            ,Prix=22.90m},
            new Produit{Libelle="Kit d'entretien pour les outils de coupe Fiskars"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Prix=22.99m},
            new Produit{Libelle="Kit déjointage Dremel 568"
            ,Categorie="Outillage"
            ,Description="Kit Joint & mortier Dremel\nGarantie : À vie"
            ,Prix=23.90m},
            new Produit{Libelle="Support de perçage avec colonne ronde Wolfcraft"
            ,Categorie="Outillage"
            ,Description="Support de perçage avec colonne ronde Wolfcraft, accessoire idéal pour vos perçages.\nDiamètre de coupe: 43 mm"
            ,Prix=34.95m},
            new Produit{Libelle="Support de meuleuse Wolfcraft Ø115/125 mm"
            ,Categorie="Outillage"
            ,Description=""
            ,Prix=39.90m},
            new Produit{Libelle="Massette anti-vibration Stanley 1300g"
            ,Categorie="Outillage"
            ,Description=""
            ,Prix=49.90m},
            new Produit{Libelle="Égouttoir pliable en métal gris clair Brabantia Sinkside taille L"
            ,Categorie="Cuisine"
            ,Description=""
            ,Prix=59.90m},
            new Produit{Libelle="Grille pour barbecue Weber Spirit II 210 et 310"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Prix=79.90m},
            new Produit{Libelle="Coupe-bordure EasyGrassCut 18-26 outil seul sans fil"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Prix=79.90m},
            new Produit{Libelle="Perceuse à percussion Ryobi RPD800K 800W"
            ,Categorie="Outillage"
            ,Description="Puissance 800 W\nFonction(s) Percage, vissage, percussion\n1 mallette et poignée auxiliaire"
            ,Prix=79.99m},
            new Produit{Libelle="Guide de perçage portatif et orientable Wolfcraft"
            ,Categorie="Outillage"
            ,Description="Fourni avec: Raccord d'aspiration de poussière, poignée supplémentaire\nDiamètre de coupe: 43 mm"
            ,Prix=84.90m},
            new Produit{Libelle="Rouleau à gazon Einhell métal rouge 46L"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Prix=89.90m},
            new Produit{Libelle="Abri pour robot tondeuse Black&Decker"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Prix=99.90m},
            new Produit{Libelle="Programmateur d'arrosage 2 voies Claber Dual Logic"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Prix=107.90m},
            new Produit{Libelle="Nettoyeur haute pression Karcher K2 Power Control 110 bar"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Prix=109},
            new Produit{Libelle="Agrafeuse cloueuse Bosch PTK14EDT"
            ,Categorie="Outillage"
            ,Description=""
            ,Prix=119},
            new Produit{Libelle="Pulvérisateur à gachette Gloria 0,5L"
            ,Categorie="Jardin et terrasse"
            ,Description="Non transportable à dos\nRéservoir gradué\nnon résistant aux produits chimiques\nPoids net: 0,15 kg"
            ,Prix=7.90m},
            new Produit{Libelle="Douchette 1 jet Grohe Vitalio Start 100 coloris chromé"
            ,Categorie="Salle de bains et WC"
            ,Description=""
            ,Prix=28.55m},
            new Produit{Libelle="Housse de rangement sous vide Compactor Aspilito 145L"
            ,Categorie="Rangement - Dressing"
            ,Description="Polypropylène.\nDimensions : L. 65cm x l. Largeur de produit: 50 cm.\nDimensions du produit (cm) : l.65 x P.50 x H.15\nTaux de compression : 75%\nColoris extérieur : Beige\nMatière extérieure : Polypropylène\nMatière intérieure : Nylon et Polyethylene\nDimensions de l'emballage (cm) : l.50 x P.5 x H.33"
            ,Prix=34.90m}
        };

        public static ApplicationDbContext _db { get; set; }
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        [AllowAnonymous]
           
        public IActionResult Index()
        {

            var userAdministrateur = (from user in _db.Users
                                join userRole in _db.UserRoles
                                on user.Id equals userRole.UserId
                                join role in _db.Roles
                                on userRole.RoleId equals role.Id
                                where role.Name == "Administrateur"
                                        select user)
                                .FirstOrDefault();
            
            return View(userAdministrateur);
            //return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Catalogue(string id)
        {
            return View(await RenvoiProduitsAsync(id));
        }
        
        public async Task<IActionResult> ListePromotions(int id=0)
        {
            return View(await RenvoiPromotionsAsync(id));

        }

        public async Task<IActionResult> ImporteProduits()
        {
            await AjouteListeProduits(Produits);
            return RedirectToAction("Index");
        }
        [HttpGet]
        
        public IActionResult ProduitForm(int id)
        {
            Produit p = _db.Produits.FirstOrDefault(p => p.ProduitID == id);
            return View(p);
        }
        
        public IActionResult PromotionForm(int id)
        {
            if (id != 0)
            {
                Promotion p = _db.Promotions.FirstOrDefault(p => p.PromotionID == id);
                return View(p);
            }
            else
                return View();


        }
        [HttpPost]
        public IActionResult ProduitForm(Produit produit)
        {
            if (ModelState.IsValid)
            {
                _db.Produits.Update(produit);
                _db.SaveChanges();
                return RedirectToAction("Catalogue");
            }
            return View();

        }
        [HttpPost]
        public IActionResult PromotionForm(Promotion promotion)
        {
            if(ModelState.IsValid) {

                _db.Promotions.Update(promotion);
                _db.SaveChanges();
                return RedirectToAction("ListePromotions");
            }
            return View();
        }
        [HttpPost]
        public IActionResult ListePromotions(PromotionsViewModel pvm)
        {


            
            var produitPromotion = _db.ProduitPromotions.Where(f => f.ProduitID == pvm.ProduitID);

            if(produitPromotion != null)
                _db.ProduitPromotions.RemoveRange(produitPromotion);
           
           

            foreach (PromotionsViewModel.PromotionProduit promotionProduit in pvm.PromotionsProduit)
            {

                if (promotionProduit.Affectee)
                {
                    
                    _db.ProduitPromotions.Add(new ProduitPromotion { ProduitID = pvm.ProduitID, PromotionID = promotionProduit.Promotion.PromotionID });

                }
            }
            _db.SaveChanges();
            return RedirectToAction("Catalogue");
        }


        public async Task AjouteListeProduits(List<Produit> produits)
        {
            foreach (Produit p in produits)
            {
                _db.Produits.Add(p);
            }
            await _db.SaveChangesAsync();
        }

        //public async Task<List<Produit>> RenvoiProduitsAsync(string categorie)
        public async Task<CatalogueViewModel> RenvoiProduitsAsync(string categorie)
        {

            var vm = new CatalogueViewModel(categorie);
            
            if (string.IsNullOrWhiteSpace(categorie))
                vm.Produits = await _db.Produits.ToListAsync();
            else
                vm.Produits = await _db.Produits.Where(x => x.Categorie == categorie).ToListAsync();


            vm.ListeCategories = _db.Produits.GroupBy(produit => produit.Categorie)
                                                .OrderBy(produit => produit.Key)
                                                .Select(a =>
                                                      new SelectListItem
                                                      {
                                                          Value = a.Key,
                                                          Text = a.Key,
                                                          Selected = a.Key== categorie
                                                      })
                                                 .ToList();

            //vm.Categories = groupByCategorieQuery.ToList();
            return vm;
        }

        public async Task<PromotionsViewModel> RenvoiPromotionsAsync(int produitID)
        {
            var vm = new PromotionsViewModel(produitID);
            vm.PromotionsProduit = await _db.Promotions.OrderBy(promotion=> promotion.DateDebut).Select(a =>
                                          new PromotionsViewModel.PromotionProduit
                                          {
                                              Promotion = a,
                                              Affectee = !a.ProduitPromotions.Where(x=>x.ProduitID==produitID).Any()
                                          })
                                     .ToListAsync();

            return vm;
        }



    }
}
