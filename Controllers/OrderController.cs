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
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public OrderController(AppDbContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddOrder([FromBody] NewDocumentRequest input)
        {
            Supplier? s = await _context.Suppliers.FindAsync(input.ActorId);
            if (s == null)
            {
                return BadRequest();
            }
            Product? p = await _context.Products.FindAsync(input.ProductId);
            if (p == null)
            {
                return BadRequest();
            }
            Order order = new Order()
            {
                Supplier = s,
                Product = p,
                UnitPrice = input.UnitPrice,
                NumberOfUnits = input.NumberOfUnits,
                IssueDate = input.IssueDate,
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Order added successfully!"
            });
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            Order? order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return BadRequest();
            }
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Order deleted successfully!"
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrders()
        {
            var invocices = await _context.Orders
                .Select(c => new DocumentResponse
                {
                    Id = c.Id,
                    IssueDate = c.IssueDate,
                    ProductName = c.Product.Name,
                    ActorName = c.Supplier.Name,
                    UnitPrice = c.UnitPrice,
                    NumberOfUnits = c.NumberOfUnits,
                    TotalAmount = c.UnitPrice * c.NumberOfUnits
                })
                .ToListAsync();
            return Ok(invocices);
        }
    }
}
