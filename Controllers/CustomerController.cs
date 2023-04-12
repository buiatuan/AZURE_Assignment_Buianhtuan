using BuianhtuanAssignment.Entites;
using BuianhtuanAssignment.Models.Request;
using BuianhtuanAssignment.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace BuianhtuanAssignment.Controllers
{
    public class CustomerController : RouteController<CustomerController>
    {
        public CustomerController(PracticeDbContext practiceDbContext, ILogger<CustomerController> logger)
            : base(practiceDbContext, logger) { }

        [HttpGet]
        public IActionResult GetAll([FromQuery] SearchCustomerModel model)
        {
            var result = _context.Customers.Where(c =>
                (c.Name.ToLower().Contains(model.Name.ToLower()) || model.Name == "")
                && (c.Age == model.Age || model.Age == 0)
                && (c.Gender == model.Gender || model.Gender == "")
                && (c.Address.ToLower().Contains(model.Address.ToLower()) || model.Address == "")
            ).Select(m =>
            new CustomerModel()
            {
                Id = m.Id,
                Name = m.Name,
                Address = m.Address,
                Status = m.Status,
                Age = m.Age,
                Debit = m.Debit,
                Description = m.Description,
                Gender = m.Gender,
                Username = m.Username
            }
            );
            return Ok(result);
        }

        [HttpGet("{id:long}/detail")]
        public IActionResult Details(long id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return BadRequest("Not found customer");
            var order = from or in _context.Orders
                        join pr in _context.Products on or.ProductId equals pr.Id
                        where or.CustomerId == id
                        select new Product
                        {
                            Id = pr.Id,
                            Name = pr.Name,
                            Amount = or.Amount,
                            Price = or.Price,
                            Status = pr.Status,
                            Description = pr.Description,
                            ExpDate = pr.ExpDate,
                        };
            ;
            return Ok(new CustomerDetailModel()
            {
                Id = customer.Id,
                Name = customer.Name,
                Age = customer.Age,
                Gender = customer.Gender,
                Description = customer.Description,
                Debit = customer.Debit,
                Username = customer.Username,
                Status = customer.Status,
                Products = order.ToList(),
            });
        }

        [HttpPut("{id:long}/edit")]
        public IActionResult Edit([FromRoute] long id, [FromBody] EditCustomerModel model)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound("Not found customer.");

            customer.Name = model.Name;
            customer.Address = model.Address;
            customer.Age = model.Age;
            customer.Status = model.Status;
            customer.Description = model.Description;
            customer.Gender = model.Gender;
            customer.UpdatedDate = DateTime.Now;

            _context.Customers.Update(customer);
            var eff = _context.SaveChanges();
            return eff > 0 ? Ok("Create success.") : BadRequest("Create Failed");
        }

        [HttpPost]
        public IActionResult Add([FromBody] CustomerModel model)
        {
            var customer = new Customer()
            {
                Name = model.Name,
                Age = model.Age,
                Address = model.Address,
                Username = model.Username,
                Status = model.Status,
                Gender = model.Gender,
                Description = model.Description,
                Debit = model.Debit,
            };
            _context.Customers.Add(customer);
            var eff = _context.SaveChanges();
            return eff > 0 ? Ok("Create success.") : BadRequest("Create Failed");
        }

        [HttpDelete("{id:long}/delete")]
        public IActionResult Delete([FromRoute] long id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound("Not found customer.");

            _context.Customers.Remove(customer);
            var eff = _context.SaveChanges();
            return eff > 0 ? Ok("Delete customer success.") : BadRequest("Delete Failed");
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderModel model)
        {
            var dataCustomer = _context.Customers.Find(model.CustomerId);
            if (dataCustomer == null) return NotFound("Not found customer.");

            var dataProducr = _context.Products.Find(model.ProductId);
            if (dataProducr == null) return NotFound("Not found product.");

            var dataOrder = _context.Orders.Find(model.ProductId, model.CustomerId);
            Order newOrder = new Order();
            if (dataOrder != null)
            {
                newOrder.ProductId = model.ProductId;
                newOrder.CustomerId = model.CustomerId;
                newOrder.Price = model.Price;
                newOrder.CreatedDate = DateTime.Now;
                newOrder.Amount = model.Amount + dataOrder.Amount;
                _context.Orders.Update(newOrder);
            }
            else
            {

                newOrder.ProductId = model.ProductId;
                newOrder.CustomerId = model.CustomerId;
                newOrder.Price = model.Price;
                newOrder.CreatedDate = DateTime.Now;
                newOrder.Amount = model.Amount;
                _context.Orders.Add(newOrder);
            }


            var eff = _context.SaveChanges();
            return eff > 0 ? Ok("Order success.") : BadRequest("Order failed.");
        }
    }
}
