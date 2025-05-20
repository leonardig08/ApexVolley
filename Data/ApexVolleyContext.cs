using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApexVolley.Models;

namespace ApexVolley.Data
{
    public class ApexVolleyContext : DbContext
    {
        public ApexVolleyContext (DbContextOptions<ApexVolleyContext> options)
            : base(options)
        {
        }

        public DbSet<ApexVolley.Models.Match> Match { get; set; } = default!;
    }
}
