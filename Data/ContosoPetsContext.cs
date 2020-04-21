using Microsoft.EntityFrameworkCore;
using petstore.Models;

namespace petstore.Data
{
  public class ContosoPetsContext : DbContext
  {
    public ContosoPetsContext(DbContextOptions<ContosoPetsContext> options) : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }
  }
}