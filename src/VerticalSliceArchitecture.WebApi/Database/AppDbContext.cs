using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VerticalSliceArchitecture.WebApi.Entities;

namespace VerticalSliceArchitecture.WebApi.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
    }
}
