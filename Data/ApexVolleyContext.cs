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

    }
}
