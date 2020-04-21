using System.ComponentModel.DataAnnotations;

namespace petstore.Models
{
  public class Product
  {
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    [Range(minimum: 0.01, maximum: (double)decimal.MaxValue)]
    public decimal Price { get; set; }
  }
}