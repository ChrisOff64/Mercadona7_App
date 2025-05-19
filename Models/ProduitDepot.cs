using Mercadona7_App.Controllers;
using Mercadona7_App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using NuGet.Protocol.Core.Types;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;
using System.Text;

namespace Mercadona7_App.Models
{
    public class Categorie
    {
        public string? Libelle { get; set; }
    }
    public class ProduitDepot : IProduitDepot
    {

        private readonly ApplicationDbContext _context;
        private OptionsStorageBlobs _options;

        public ProduitDepot(ApplicationDbContext context, IOptions<OptionsStorageBlobs> options)
        {
            _context = context;
            _options = options.Value;
        }

        bool IProduitDepot.AjouteProduit(Produit produitACreer)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProduitCatalogue>> ListeProduitsAsync(string categorie = "")
        {

            if (_context.Produits == null)
            {
                return null;
            }

            List<ProduitCatalogue> listeProduitsCatalogue = new List<ProduitCatalogue>();



            

            listeProduitsCatalogue = await _context.Produits
                     .GroupJoin(_context.ProduitPromotions
                                    .GroupJoin(_context.Promotions, produitPromotion => produitPromotion.PromotionID, promotion => promotion.PromotionID,
                                        (produitPromotion,promotion ) =>
                                        //new { produitPromotion.ProduitID, promotion.Remise, promotion.DateDebut, promotion.DateFin })
                                        new { produitPromotion, promotion })
                                    .SelectMany(
                                        g => g.promotion.DefaultIfEmpty(),
                                        (pp, p) =>
                                           new
                                           {
                                               pp.produitPromotion.ProduitID,
                                               p.Remise,
                                               p.DateDebut,
                                               p.DateFin
                                           })
                                        .Where(pp => pp.DateDebut <= DateTime.Now.Date && pp.DateFin >= DateTime.Now.Date)
                                        .GroupBy(pp => pp.ProduitID)
                                        .Select(pgrp => new
                                        {
                                            ProduitID = pgrp.Key,
                                            Remise = pgrp.Max(p => p.Remise)
                                        }), promotion => promotion.ProduitID, produit => produit.ProduitID,
                          (produit,promotionEnVigueur ) =>
                          new { produit , promotionEnVigueur })
                     .SelectMany(
                        g => g.promotionEnVigueur.DefaultIfEmpty(),
                        (prod, promo) =>
                           new ProduitCatalogue
                        { Produit=prod.produit,RemiseEnVigueur = (promo == null? 0 : promo.Remise)
                           })
                        //new ProduitCatalogue { ProduitID = produit.ProduitID, Categorie = produit.Categorie, RemiseEnVigueur = promotion.Remise })
                     .ToListAsync();






            if (!string.IsNullOrWhiteSpace(categorie))
            {

            
                listeProduitsCatalogue= listeProduitsCatalogue.Where(produit => produit.Produit!=null && produit.Produit.Categorie == categorie).ToList();
                

            }


            if (listeProduitsCatalogue != null && _options.NomConteneurImage != null) { 
                BlobContainerClient containerClient = await RenvoieCloudBlobContainer(_options.NomConteneurImage);

                BlobClient blobClient;
                BlobSasBuilder blobSasBuilder;


                foreach (ProduitCatalogue p in listeProduitsCatalogue) {
                    if (!string.IsNullOrWhiteSpace(p.Produit.Image))
                    {
                        blobClient = containerClient.GetBlobClient(p.Produit.Image);
                        blobSasBuilder = new BlobSasBuilder()
                        {
                            BlobContainerName = _options.NomConteneurImage,
                            //BlobName = blobItem.Name,
                            BlobName = p.Produit.Image,
                            ExpiresOn = DateTime.UtcNow.AddMinutes(5),//Let SAS token expire after 5 minutes.
                            Protocol = SasProtocol.Https
                        };
                        blobSasBuilder.SetPermissions(BlobSasPermissions.Read);
                        p.Produit.Image = blobClient.GenerateSasUri(blobSasBuilder).AbsoluteUri;
                    }
                }
            }
            return listeProduitsCatalogue;
        }

        public async Task<IEnumerable<Categorie>> ListeCategoriesAsync()
        {

            return await RequeteListeCategories().ToListAsync();
            //return await _context.Produits.GroupBy(produit => produit.Categorie)
            //                               .OrderBy(produit => produit.Key)
            //                               .Select(produit =>
            //                                           new Categorie
            //                                           {
            //                                               Libelle = produit.Key,
            //                                           })
            //                               .ToListAsync();
        }

        public IQueryable<Categorie> RequeteListeCategories()
        {
            return _context.Produits.GroupBy(produit => produit.Categorie)
                                           .OrderBy(produit => produit.Key)
                                           .Select(produit =>
                                                       new Categorie
                                                       {
                                                           Libelle = produit.Key,
                                                       });
        }

        private async Task<BlobContainerClient> RenvoieCloudBlobContainer(string NomConteneurImage)
        {
            BlobServiceClient serviceClient = new BlobServiceClient(_options.ChaineConnexion);
            BlobContainerClient conteneurClient = serviceClient.GetBlobContainerClient(NomConteneurImage);
            await conteneurClient.CreateIfNotExistsAsync();
            return conteneurClient;
        }
        public async Task<Produit> RenvoiProduitAsync(int id)
        {
            if (_context.Produits == null)
            {
                return null;
            }
            var produit = await _context.Produits.FindAsync(id);
            if (produit != null &&  !string.IsNullOrWhiteSpace(produit.Image) && _options.NomConteneurImage != null)
            {
                BlobContainerClient conteneurClient = await RenvoieCloudBlobContainer(_options.NomConteneurImage);

                BlobClient blobClient;
                BlobSasBuilder blobSasBuilder;


                
                blobClient = conteneurClient.GetBlobClient(produit.Image);
                blobSasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = _options.NomConteneurImage,
                    BlobName = produit.Image,
                    ExpiresOn = DateTime.UtcNow.AddMinutes(5),//Let SAS token expire after 5 minutes.
                    Protocol = SasProtocol.Https
                };
                blobSasBuilder.SetPermissions(BlobSasPermissions.Read);


                produit.Image = blobClient.GenerateSasUri(blobSasBuilder).AbsoluteUri;
            }
            return produit;

            
        }


        public async Task<bool> ModifieProduitAsync(Produit produit)
        {

            int id = produit.ProduitID;
            if (!String.IsNullOrEmpty(produit.Image))
            {
                Uri uriImage = new Uri(produit.Image);
                if (uriImage.Segments.Length > 0)
                {
                    produit.Image=uriImage.Segments[uriImage.Segments.Length-1];
                }
                
            }

            

            _context.Entry(produit).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProduitExiste(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
            
        }



        public async Task<bool> AjouteProduitAsync(Produit produit)
        {
            if (_context.Produits == null)
            {
                return false;
            }
            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<string> UpLoadImage(IFormFile Upload,string libelle)
        {
            var imagesUrl = _options.ApiUrl;
            if (_options.NomConteneurImage != null)
            {

                BlobContainerClient conteneurClient = await RenvoieCloudBlobContainer(_options.NomConteneurImage);
                string nomFichier = Upload.FileName;
                if (!string.IsNullOrEmpty(libelle))
                {

                    StringBuilder sbNomFichier = new StringBuilder();
                    var arrayText = libelle.Normalize(NormalizationForm.FormD).ToCharArray();
                    foreach (char letter in arrayText)
                    {
                        if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                            sbNomFichier.Append(letter);
                    }
                    nomFichier= sbNomFichier.ToString().ToLower();
                    nomFichier = nomFichier.Replace("  "," ").Replace(" ","-");
                    nomFichier = string.Join("", nomFichier.Split(Path.GetInvalidFileNameChars()));
                    nomFichier += Path.GetExtension(Upload.FileName);
                }

                BlobClient blobClient = conteneurClient.GetBlobClient(nomFichier);

                BlobHttpHeaders httpHeaders = new BlobHttpHeaders()
                {
                    ContentType = Upload.ContentType
                };

                await blobClient.UploadAsync(Upload.OpenReadStream(), httpHeaders);

                //blobClient = conteneurClient.GetBlobClient(produit.Image);

                
                BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = _options.NomConteneurImage,
                    BlobName = nomFichier,
                    ExpiresOn = DateTime.UtcNow.AddMinutes(5),//Let SAS token expire after 5 minutes.
                    Protocol = SasProtocol.Https
                };
                blobSasBuilder.SetPermissions(BlobSasPermissions.Read);


                return blobClient.GenerateSasUri(blobSasBuilder).AbsoluteUri;

            }
            return "";
        }
        


        public async Task<bool> SupprimeProduitAsync(int id)
        {
            
            
            if (_context.Produits == null)
            {
                return false;
            }
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return false;
            }


            if (produit.ProduitPromotions != null)
            {
                _context.ProduitPromotions.RemoveRange(produit.ProduitPromotions);
            }



            if (!string.IsNullOrWhiteSpace(produit.Image) && _options.NomConteneurImage != null)
            { 
                var produitMemeImage = _context.Produits.Where(f => f.Image == "" && f.ProduitID != id);
                if (produitMemeImage == null)
                {
                    BlobContainerClient conteneurClient = await RenvoieCloudBlobContainer(_options.NomConteneurImage);
                    conteneurClient.GetBlobClient(produit.Image).DeleteIfExists();

                    //BlobServiceClient blobServiceClient = new BlobServiceClient("StorageConnectionString");
                    //BlobContainerClient cont = blobServiceClient.GetBlobContainerClient("containerName");
                    //cont.GetBlobClient(produit.Image).DeleteIfExists();
                }
            }
            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();
            

            return true;
        }

            //IEnumerable<Produit> IProduitDepot.ListeProduits(string categorie)
            //{
            //    throw new NotImplementedException();
            //}


            //public bool AjouteProduit(Produit produitACreer)
            //{
            //
            //    if (_context.Produits == null)
            //    {
            //        return Problem("Entity set 'ApplicationDbContext.Produits'  is null.");
            //    }
            //    _context.Produits.Add(produitACreer);
            //    await _context.SaveChangesAsync();
            //
            //    return CreatedAtAction("GetProduit", new { id = produitACreer.ProduitID }, produitACreer);
            //
            //
            //    //try
            //    //{
            //    //    _entities.AddToProductSet(produitACreer);
            //    //    _entities.SaveChanges();
            //    //    return true;
            //    //}
            //    //catch
            //    //{
            //    //    return false;
            //    //}
            //}

            private bool ProduitExiste(int id)
        {
            return (_context.Produits?.Any(e => e.ProduitID == id)).GetValueOrDefault();
            
        }
    }

    public interface IProduitDepot
    {
        bool AjouteProduit(Produit produitACreer);
        Task<IEnumerable<ProduitCatalogue>> ListeProduitsAsync(string categorie = "");
        Task<IEnumerable<Categorie>> ListeCategoriesAsync();
        Task<Produit> RenvoiProduitAsync(int id);
        Task <bool> ModifieProduitAsync(Produit produit);
        Task<bool> AjouteProduitAsync(Produit produit);
        Task<string> UpLoadImage(IFormFile Upload, string libelle);
        Task<bool> SupprimeProduitAsync(int id);
        IQueryable<Categorie> RequeteListeCategories();
    }

}