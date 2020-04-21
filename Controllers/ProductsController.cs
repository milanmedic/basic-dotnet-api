using System;
using Microsoft.AspNetCore.Mvc;
using petstore.Data;
using System.Collections.Generic;
using System.Linq;
using petstore.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace petstore.Controllers
{
  [ApiController]
  [Route("/api/products")]
  public class ProductsController : ControllerBase
  {
    private readonly ContosoPetsContext _context;
    public ProductsController(ContosoPetsContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public ActionResult<List<Product>> GetAll() => _context.Products.ToList();

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
      var product = await _context.Products.FindAsync(id);
      if (product == null)
      {
        return NotFound();
      }

      return Ok(product);
    }
    /*
        Because the controller is annotated with the [ApiController] attribute, it's implied that the product parameter will be found in the request body.
    */
    [HttpPost]
    public async Task<ActionResult<Product>> Create([FromBody] Product product)
    {
      await _context.Products.AddAsync(product);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
      /*
        The product was added to the database.
        The product is included in the response body in the media type as defined in the Accept HTTP request header (JSON by default).
        The first parameter in the CreatedAtAction method call represents an action name. The nameof keyword is used to avoid hard-coding the action name. CreatedAtAction uses the action name to generate a Location HTTP response header with a URL to the newly created product.
      */
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] Product product)
    {
      if (id != product.Id)
      {
        return BadRequest();
      }

      try
      {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return NotFound(); //the product wasn't found in the database
      //The product was updated in the database.
      /*
        Responds only to the HTTP PUT verb, as denoted by the [HttpPut] attribute.

        Returns IActionResult because the ActionResult return type isn't known until runtime. The BadRequest and NoContent methods return BadRequestResult and NoContentResult types, respectively.

        Requires that the id value is included in the URL segment after products/.

        Updates the Name and Price properties of the product. The following code instructs EF Core to mark all of the Product entity's properties as modified:
         _context.Entry(product).State = EntityState.Modified;
      */
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var product = await _context.Products.FindAsync(id);

      if (product == null)
      {
        return NotFound();
      }

      try
      {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return NotFound(); //product not found      
      /*
        Responds only to the HTTP DELETE verb, as denoted by the [HttpDelete] attribute.
        Requires that id is included in the URL path.
        Queries the database for a product matching the provided id parameter.
      */
    }
  }
}