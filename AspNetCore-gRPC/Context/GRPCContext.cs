using Microsoft.EntityFrameworkCore;
using Models;

namespace Context
{
    public class GRPCContext : DbContext
    {
        public GRPCContext(DbContextOptions<GRPCContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
    }
}
