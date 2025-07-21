using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prog7311_PartTwo.Models;

namespace Prog7311_PartTwo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
           public DbSet<FarmerProfileModel> FarmerProfiles { get; set; }

            public DbSet<ProductsModel> Products { get; set; }
              

    }
}