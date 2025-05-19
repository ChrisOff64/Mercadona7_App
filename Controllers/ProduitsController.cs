using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mercadona7_App.Data;
using Mercadona7_App.Models;
using Microsoft.AspNetCore.Authorization;

namespace Mercadona7_App.Controllers
{


    //pattern: "{controller=Home}/{action=Index}/{id?}"
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrateur")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private IProduitDepot _depot;

     

        //public ProduitsController(ApplicationDbContext context) :
        //    this(new ProduitDepot(context))
        //{ }
        public ProduitsController(IProduitDepot depot)
        {
            _depot = depot;
        }

        // GET: api/Produits
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProduits(string categorie = "")
        {
            var produits = await _depot.ListeProduitsAsync(categorie);
            if (produits == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(produits);
            }

        }

        [HttpGet]
        [Route("ListeCategories")]
        [Route("[controller]/[action]")]
        public async Task<ActionResult<IEnumerable<Categorie>>> ListeCategories()
        {

            var categories = await _depot.ListeCategoriesAsync();
            if (categories == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(categories);
            }


        }

        




        // GET: api/Produits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produit>> GetProduit(int id)
        {
            var produit = await _depot.RenvoiProduitAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(produit);
            }

        }

        // PUT: api/Produits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduit(int id, Produit produit)
        {
            if (id != produit.ProduitID)
            {
                return BadRequest();
            }
            
            bool resultat= await _depot.ModifieProduitAsync(produit);
            if (!resultat)
                return NotFound();

            return NoContent();
        }

        // POST: api/Produits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produit>> PostProduit(Produit produit)
        {
            bool resultat = await _depot.AjouteProduitAsync(produit);
            if (!resultat)
                return Problem("Entity set 'ApplicationDbContext.Produits'  is null.");
            
              return CreatedAtAction("GetProduit", new { id = produit.ProduitID }, produit);
            
        }

        // DELETE: api/Produits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduit(int id)
        {

            bool resultat = await _depot.SupprimeProduitAsync(id);
            if (!resultat)
                return NotFound();
            return NoContent();
        }

        
    }
}
