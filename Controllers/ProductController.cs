using BuianhtuanAssignment.Entites;
using BuianhtuanAssignment.Models.Request;
using BuianhtuanAssignment.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BuianhtuanAssignment.Controllers
{
    public class ProductController : RouteController<ProductController>
    {
        public ProductController(PracticeDbContext practiceDbContext, ILogger<ProductController> logger)
            : base(practiceDbContext, logger) { }

        [HttpGet]
        public IActionResult GetAll([FromQuery] SearchProductModel model)
        {
            var result = _context.Products.Where(p =>
                (p.Name.ToLower().Contains(model.Name.ToLower()) || model.Name == "")
                && (p.Price == model.Price || model.Price == 0)
                && (p.Amount == model.Amount || model.Amount == 0)
                && (p.Description.ToLower().Contains(model.Description.ToLower()) || model.Description == "")
            ).Select(m =>
            new ProductModel()
            {
                Id = m.Id,
                Name = m.Name,
                ExpDate = m.ExpDate,
                Amount= m.Amount,
                Price = m.Price,
                Status = m.Status,
                Description = m.Description,
            }
            );
            return Ok(result);
        }

        [HttpPut("{id:long}/edit")]
        public IActionResult Edit([FromRoute] long id,[FromBody] ProductModel model)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound("Not found Product.");

            product.Name = model.Name;
            product.Description = model.Description;
            product.UpdatedDate = DateTime.Now;
            product.ExpDate = model.ExpDate;
            product.Price = model.Price;
            product.Amount = model.Amount;
            product.Status = model.Status;

            _context.Products.Update(product);
            var eff = _context.SaveChanges();
            return eff > 0 ? Ok("Edit success.") : BadRequest("Edit Failed");
        }

        [HttpPost]
        public IActionResult Add([FromBody] ProductModel model)
        {
            var product = new Product()
            {
                Price= model.Price,
                Name = model.Name,
                Amount= model.Amount,
                CreatedDate= DateTime.Now,
                Description= model.Description,
                ExpDate = model.ExpDate,
                Status= model.Status,
            };
            _context.Products.Add(product);
            var eff = _context.SaveChanges();
            return eff > 0 ? Ok("Create success.") : BadRequest("Create Failed");
        }

        [HttpDelete("{id:long}/delete")]
        public IActionResult Delete([FromRoute] long id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound("Not found Product.");

            _context.Products.Remove(product);
            var eff = _context.SaveChanges();
            return eff > 0 ? Ok("Delete Product success.") : BadRequest("Delete Failed");
        }
    }
}
