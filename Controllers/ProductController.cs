using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesManagementBack.Data;
using SalesManagementBack.DTO.Requests;
using SalesManagementBack.DTO.Responses;
using SalesManagementBack.Entities;

namespace SalesManagementBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public ProductController(AppDbContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddProduct([FromBody] NewProductRequest input)
        {
            Product product = new Product()
            {
                Name = input.Name,
                Description = input.Description,
                Reference = input.Reference,
            };
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Product added successfully!"
            });
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, NewProductRequest input)
        {
            Product? product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return BadRequest();
            }
            product.Name = input.Name;
            product.Description = input.Description;
            product.Reference = input.Reference;
            _context.Products.Update(product);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Product updated successfully!"
            });
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product? product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return BadRequest();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Product deleted successfully!"
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .Select(c => new ProductResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Reference = c.Reference,
                    Description = c.Description,
                    NumberOfSoldUnits = c.Invoices.Sum(x => x.NumberOfUnits),
                    NumberOfBoughtUnits = c.Orders.Sum(x => x.NumberOfUnits),
                    TotalCost = c.Orders.Sum(x => x.NumberOfUnits * x.UnitPrice),
                    TotalProfit = c.Invoices.Sum(x => x.NumberOfUnits * x.UnitPrice),
                    Balance = c.Invoices.Sum(x => x.NumberOfUnits * x.UnitPrice) - c.Orders.Sum(x => x.NumberOfUnits * x.UnitPrice)
                })
                .ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            Product? product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return BadRequest();
            }
            return Ok(new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Reference = product.Reference,
                Description = product.Description,
                NumberOfSoldUnits = product.Invoices.Sum(x => x.NumberOfUnits),
                NumberOfBoughtUnits = product.Orders.Sum(x => x.NumberOfUnits),
                TotalCost = product.Orders.Sum(x => x.NumberOfUnits * x.UnitPrice),
                TotalProfit = product.Invoices.Sum(x => x.NumberOfUnits * x.UnitPrice),
                Balance = product.Invoices.Sum(x => x.NumberOfUnits * x.UnitPrice) - product.Orders.Sum(x => x.NumberOfUnits * x.UnitPrice)
            });
        }
    }
}
