using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cloud_FinalTwo.Models;

namespace Cloud_FinalTwo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cloud_FinalTwo.Models.Products> Products { get; set; } = default!;
        public DbSet<Cloud_FinalTwo.Models.Cart> Cart { get; set; } = default!;
        public DbSet<Cloud_FinalTwo.Models.Proccessing> Proccessing { get; set; } = default!;
        public DbSet<Cloud_FinalTwo.Models.Orders> Orders { get; set; } = default!;
       
    }
}
