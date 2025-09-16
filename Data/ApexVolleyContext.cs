using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApexVolley.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApexVolley.Data
{
    public class ApexVolleyContext : IdentityDbContext<ApplicationUser>
    {
        public ApexVolleyContext (DbContextOptions<ApexVolleyContext> options)
            : base(options)
        {
        }

        public DbSet<ApexVolley.Models.Player> Player { get; set; } = default!;
        public DbSet<ApexVolley.Models.Match> Match { get; set; } = default!;
        public DbSet<ApexVolley.Models.NewsPost> NewsPost { get; set; } = default!;

        public DbSet<ApexVolley.Models.Product> Products { get; set; }
        public DbSet<ApexVolley.Models.Order> Orders { get; set; }
        public DbSet<ApexVolley.Models.OrderItem> OrderItems { get; set; }

        public DbSet<ApexVolley.Models.CartItem> CartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // molto importante chiamare la base

            // Qui aggiungi le configurazioni
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); // precisione e decimali per il prezzo
        }


    }
}
