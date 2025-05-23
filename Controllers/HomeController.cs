﻿//using Castle.Core.Internal;
using Azure;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
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
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Policy;
using System.Threading.Tasks;


using static Mercadona7_App.Models.ListePromotionsViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System.Text;

namespace Mercadona7_App.Controllers

{
    [Authorize(Roles = "Administrateur")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController>? _logger;
        public static ApplicationDbContext? _db { get; set; }
        private IProduitDepot _depot;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        List<Produit> ProduitsImport = new List<Produit>
        {
            new Produit{Libelle="Tête de vissage plaquiste Wolfcraft"
            ,Categorie="Outillage"
            ,Description=""
            ,Image="tete-de-vissage-plaquiste-wolfcraft-noir.webp"
            ,Prix=7.9m},
            new Produit{Libelle="Lingot® Piment Jalapeno pour potager Véritable®"
            ,Categorie="Jardin et terrasse"
            ,Description="Période de plantation: Toute l'année\nPériode de récolte: dés 4 à 8 semaines jusqu'à 5 mois"
            ,Image="lingot-pour-potager-d-interieur-veritable-variete-piment-jalapeno-plantation-toute-l-annee.webp"
            ,Prix=7.95m},
            new Produit{Libelle="Pince à cliquet Wolfcraft FZR 60"
            ,Categorie="Outillage"
            ,Description="Pince à cliquet A cliquet"
            ,Image="pince-a-cliquet-wolfcraft-fzr-60-noir-et-vert.webp"
            ,Prix=8.90m},
            new Produit{Libelle="Fil Nylium Etoile pour Coupe-bordure diamètre 3mm X Longueur 15 M"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Image="fil-nylium-etoile-pour-coupe-bordure-diametre-3mm-x-longueur-15-m.webp"
            ,Prix=10.90m},
            new Produit{Libelle="Balai de jardin à poils recourbés + raclette Gardirex"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Image="balai-de-jardin-droit-a-poils-raclette-gardirex.webp"
            ,Prix=17.90m},
            new Produit{Libelle="Flexible de douche VitalioFlex 1750 cm gris Grohe"
            ,Categorie="Salle de bains et WC"
            ,Description=""
            ,Image="flexible-de-douche-vitalioflex-1750-cm-gris-grohe.webp"
            ,Prix=18.90m},
            new Produit{Libelle="Lot de 3 étagères murales cubes Fixy 5Five P. 9 cm en bambou"
            ,Categorie="Rangement - Dressing"
            ,Description="Dimensions taille L : H. 25 cm x L. 25 cm x P. 9 cm\nDimensions taille M : H. 20 cm x L. 20 cm x P. 9 cm\nDimensions taille S : H. 15 cm x L. 15 cm x P. 9 cm"
            ,Image="lot-de-3-etageres-murales-cubes-fixy-5five-p-9-cm-en-bambou.webp"
            ,Prix=19.90m},
            new Produit{Libelle="4 dalles de potager maxi Garantia"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Image="lot-de-4-dalles-potager-maxi-garantia.webp"
            ,Prix=19.90m},
            new Produit{Libelle="Etagère de douche d'angle chromé Hansgrohe Logis Universal"
            ,Categorie="Salle de bains et WC"
            ,Description=""
            ,Image="etagere-de-douche-d-angle-chrome-hansgrohe-logis-universal.webp"
            ,Prix=22.90m},
            new Produit{Libelle="Kit d'entretien pour les outils de coupe Fiskars"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Image="kit-d-entretien-pour-les-outils-de-coupe-fiskars.webp"
            ,Prix=22.99m},
            new Produit{Libelle="Kit déjointage Dremel 568"
            ,Categorie="Outillage"
            ,Description="Kit Joint & mortier Dremel\nGarantie : À vie"
            ,Image="kit-dejointage-pour-outil-multifonction-dremel-568.webp"
            ,Prix=23.90m},
            new Produit{Libelle="Support de perçage avec colonne ronde Wolfcraft"
            ,Categorie="Outillage"
            ,Description="Support de perçage avec colonne ronde Wolfcraft, accessoire idéal pour vos perçages.\nDiamètre de coupe: 43 mm"
            ,Image="support-de-percage-avec-colonne-ronde-wolfcrafti.webp"
            ,Prix=34.95m},
            new Produit{Libelle="Support de meuleuse Wolfcraft Ø115/125 mm"
            ,Categorie="Outillage"
            ,Description=""
            ,Image="support-de-meuleuse-wolfcraft-115-125-mm.webp"
            ,Prix=39.90m},
            new Produit{Libelle="Massette anti-vibration Stanley 1300g"
            ,Categorie="Outillage"
            ,Description=""
            ,Image="massette-anti-vibration-stanley-1300g.webp"
            ,Prix=49.90m},
            new Produit{Libelle="Égouttoir pliable en métal gris clair Brabantia Sinkside taille L"
            ,Categorie="Cuisine"
            ,Description=""
            ,Image="egouttoir-pliable-en-metal-gris-clair-brabantia-sinkside-taille-l.webp"
            ,Prix=59.90m},
            new Produit{Libelle="Grille pour barbecue Weber Spirit II 210 et 310"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Image="grille-pour-barbecue-weber-spirit-ii-210-et-310.webp"
            ,Prix=79.90m},
            new Produit{Libelle="Coupe-bordure EasyGrassCut 18-26 outil seul sans fil"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Image="coupe-bordure-sans-fil-sur-batterie-18-26v-bosch-easygrasscut-vendu-sans-batterie-ni-chargeur-.webp"
            ,Prix=79.90m},
            new Produit{Libelle="Perceuse à percussion Ryobi RPD800K 800W"
            ,Categorie="Outillage"
            ,Description="Puissance 800 W\nFonction(s) Percage, vissage, percussion\n1 mallette et poignée auxiliaire"
            ,Image="perceuse-a-percussion-ryobi-rpd800k-800w.webp"
            ,Prix=79.99m},
            new Produit{Libelle="Guide de perçage portatif et orientable Wolfcraft"
            ,Categorie="Outillage"
            ,Description="Fourni avec: Raccord d'aspiration de poussière, poignée supplémentaire\nDiamètre de coupe: 43 mm"
            ,Image="guide-de-percage-portatif-et-orientable-wolfcraft.webp"
            ,Prix=84.90m},
            new Produit{Libelle="Rouleau à gazon Einhell métal rouge 46L"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Image="rouleau-a-gazon-einhell-metal-rouge-46l.webp"
            ,Prix=89.90m},
            new Produit{Libelle="Abri pour robot tondeuse Black&Decker"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Image="abri-pour-robot-tondeuse-black-decker.webp"
            ,Prix=99.90m},
            new Produit{Libelle="Programmateur d'arrosage 2 voies Claber Dual Logic"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Image="programmateur-d-arrosage-2-voies-claber-dual-logic.webp"
            ,Prix=107.90m},
            new Produit{Libelle="Nettoyeur haute pression Karcher K2 Power Control 110 bar"
            ,Categorie="Jardin et terrasse"
            ,Description=""
            ,Image="nettoyeur-haute-pression-karcher-k2-power-control-110-bar.webp"
            ,Prix=109},
            new Produit{Libelle="Agrafeuse cloueuse Bosch PTK14EDT"
            ,Categorie="Outillage"
            ,Description=""
            ,Image="agrafeuse-cloueuse-bosch-ptk14edt-plastique.webp"
            ,Prix=119},
            new Produit{Libelle="Pulvérisateur à gachette Gloria 0,5L"
            ,Categorie="Jardin et terrasse"
            ,Description="Non transportable à dos\nRéservoir gradué\nnon résistant aux produits chimiques\nPoids net: 0,15 kg"
            ,Image="pulverisateur-a-gachette-gloria-0-5l.webp"
            ,Prix=7.90m},
            new Produit{Libelle="Douchette 1 jet Grohe Vitalio Start 100 coloris chromé"
            ,Categorie="Salle de bains et WC"
            ,Description=""
            ,Image="douchette-1-jet-grohe-vitalio-start-100-coloris-chrome.webp"
            ,Prix=28.55m},
            new Produit{Libelle="Housse de rangement sous vide Compactor Aspilito 145L"
            ,Categorie="Rangement - Dressing"
            ,Description="Polypropylène.\nDimensions : L. 65cm x l. Largeur de produit: 50 cm.\nDimensions du produit (cm) : l.65 x P.50 x H.15\nTaux de compression : 75%\nColoris extérieur : Beige\nMatière extérieure : Polypropylène\nMatière intérieure : Nylon et Polyethylene\nDimensions de l'emballage (cm) : l.50 x P.5 x H.33"
            ,Image="housse-de-rangement-sous-vide-compactor-aspilito-145l.webp"
            ,Prix=34.90m}
        };


        public HomeController(ApplicationDbContext db,IProduitDepot depot)
        {
            _db = db;
            _depot = depot;
        }



        [AllowAnonymous]
           
        public async Task<IActionResult> Index()
        {

            



            //var userAdministrateur = (from user in _db.Users
            //                    join userRole in _db.UserRoles
            //                    on user.Id equals userRole.UserId
            //                    join role in _db.Roles
            //                    on userRole.RoleId equals role.Id
            //                    where role.Name == "Administrateur"
            //                            select user)
            //                    .FirstOrDefault();
            //
            //return View(userAdministrateur);
            return View();
            
        }

        [AllowAnonymous]
        public JsonResult UtilisateurAdmin()


        {



            var userAdministrateur = (from user in _db.Users
                                join userRole in _db.UserRoles
                                on user.Id equals userRole.UserId
                                join role in _db.Roles
                                on userRole.RoleId equals role.Id
                                where role.Name == "Administrateur"
                                        select user)
                                .FirstOrDefault();
            
            //return View(userAdministrateur);

            if (userAdministrateur != null)
                return new JsonResult(new {userAdministrateur.Email});
            else
                return new JsonResult(null);
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

        public async Task<IActionResult> ListePromotions(int produitID = 0)
        {
            return View(await RenvoiPromotionsAsync(produitID));

        }

        public async Task<IActionResult> ImporteProduits()
        {
            await AjouteListeProduits(ProduitsImport);
            return RedirectToAction("Index");
        }
        [HttpGet]
        
        public async Task<IActionResult>ProduitForm(int id)
        {


            ProduitViewModel pvm= new ProduitViewModel();

            //Produit p = _db.Produits.FirstOrDefault(p => p.ProduitID == id);
            pvm.Produit = await _depot.RenvoiProduitAsync(id);


            return View(pvm);
        }
        
        public IActionResult PromotionForm(int ProduitId, int id)
        {
            if (id != 0)
            {
                PromotionViewModel pvm= new PromotionViewModel();
                pvm.Promotion = _db.Promotions.FirstOrDefault(p => p.PromotionID == id);
                pvm.ProduitID = ProduitId;


                return View(pvm);
            }
            else
                return View();


        }
        public async Task<CatalogueViewModel> RenvoiProduitsAsync(string categorie)
        {

            var vm = new CatalogueViewModel(categorie);




            vm.Produits = (List<ProduitCatalogue>)await _depot.ListeProduitsAsync(categorie);



            vm.ListeCategories = _depot.RequeteListeCategories().AsEnumerable().ToList().Select(a =>
                                                      new SelectListItem
                                                      {
                                                          Value = a.Libelle,
                                                          Text = a.Libelle,
                                                          Selected = a.Libelle == categorie
                                                      })
                                                 .ToList();




            return vm;
        }

        public async Task<ListePromotionsViewModel> RenvoiPromotionsAsync(int produitID)
        {
            var vm = new ListePromotionsViewModel(produitID);
            if (produitID !=0)
            {

                Produit ? p = _db.Produits.FirstOrDefault(p => p.ProduitID == produitID);
                if (p != null) vm.Libelle = p.Libelle;
            }
            vm.PromotionsProduit = await _db.Promotions.OrderBy(promotion => promotion.DateDebut).Select(a =>
                                          new ListePromotionsViewModel.PromotionProduit
                                          {
                                              Promotion = a,
                                              Affectee = a.ProduitPromotions.Where(x => x.ProduitID == produitID).Any()
                                          })
                                     .ToListAsync();

            return vm;
        }



        public IActionResult CreationPromotionProduit(int ProduitId)
        {
            PromotionViewModel pvm = new PromotionViewModel();
            pvm.ProduitID = ProduitId;
            pvm.ModeCreation = true;
            //p.ProduitPromotions[0].Produit.ProduitID = null;

            return View("PromotionForm", pvm);

        }
        [HttpPost]
        public async Task<IActionResult> ProduitForm(ProduitViewModel pvm)
        {
            
            if (ModelState.IsValid && pvm.Produit!=null){
                bool resultatOk=false;
                if (pvm.Produit.ProduitID==0)
                    resultatOk = await _depot.AjouteProduitAsync(pvm.Produit);
                else
                    resultatOk = await _depot.ModifieProduitAsync(pvm.Produit);
                //_db.Produits.Update(produit);
                //_db.SaveChanges();
                if (resultatOk) return 
                        RedirectToAction("Catalogue");
            }
            return View();



            //###########################################
            //if (ModelState.IsValid){
            //    _db.Produits.Update(produit);
            //    _db.SaveChanges();
            //    return RedirectToAction("Catalogue");
            //}
            //return View();

        }
        public async Task<IActionResult> EnvoiImage(ProduitViewModel pvm)
        {
            if (pvm.Upload != null && pvm.Upload.Length > 0)
            {

                string nomFichier = await _depot.UpLoadImage(pvm.Upload, pvm.Produit.Libelle);
                if (!string.IsNullOrEmpty(nomFichier))
                {
                    pvm.Produit.Image = nomFichier;
                    
                }
            }
            return View("ProduitForm", pvm);
        }
        public async Task<IActionResult> SupprimerImage(ProduitViewModel pvm)
        {
            pvm.Produit.Image = null;
            return View("ProduitForm", pvm);
        }

        [HttpPost]
        public IActionResult PromotionForm(PromotionViewModel pvm)
        {
            if(ModelState.IsValid) {
                
                _db.Promotions.Update(pvm.Promotion);
                _db.SaveChanges();
                if (pvm.ModeCreation &&  pvm.ProduitID!=0)
                {
                    ProduitPromotion pp=new ProduitPromotion();
                    pp.PromotionID = pvm.Promotion.PromotionID;
                    pp.ProduitID = pvm.ProduitID;
                    
                    _db.ProduitPromotions.Add(pp);
                    _db.SaveChanges();
                }
                
                return RedirectToAction("ListePromotions", new { ProduitID = pvm.ProduitID } );
            }
            return View(pvm);
        }
        [HttpPost]
        public IActionResult SupprimePromotion(PromotionViewModel pvm)
        {
            Promotion ? promo = _db.Promotions.FirstOrDefault(p => p.PromotionID == pvm.Promotion.PromotionID);

            if (promo!=null) { 
                if (promo.ProduitPromotions != null)
                {
                    _db.ProduitPromotions.RemoveRange(promo.ProduitPromotions);
                }

                _db.Promotions.Remove(promo);
                _db.SaveChanges();
			}
			return RedirectToAction("ListePromotions", new { ProduitID = pvm.ProduitID });
        }

        [HttpPost]
        public async Task<IActionResult> SupprimeProduit(Produit pvm)
        {

            await _depot.SupprimeProduitAsync(pvm.ProduitID);

            return RedirectToAction("Catalogue");
        }


        [HttpPost]
        public IActionResult ListePromotions(ListePromotionsViewModel pvm)
        {


            
            var produitPromotion = _db.ProduitPromotions.Where(f => f.ProduitID == pvm.ProduitID);

            if(produitPromotion != null)
                _db.ProduitPromotions.RemoveRange(produitPromotion);
           
           

            foreach (ListePromotionsViewModel.PromotionProduit promotionProduit in pvm.PromotionsProduit)
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

    }
}
