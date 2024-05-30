using Mercadona7_App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mercadona7_App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<ProduitPromotion> ProduitPromotions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProduitPromotion>().HasKey(x => new { x.ProduitID, x.PromotionID });

            base.OnModelCreating(modelBuilder);
        }

    }
}
