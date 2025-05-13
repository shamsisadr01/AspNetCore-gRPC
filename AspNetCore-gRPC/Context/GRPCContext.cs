
using AspNetCore_gRPC.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore_gRPC.Context
{
    public class GRPCContext : DbContext
    {
        public GRPCContext(DbContextOptions<GRPCContext> options) : base(options)
        {
            
        }

        public DbSet<Product> products;
    }
}
