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
    public class SupplierController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public SupplierController(AppDbContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddSupplier([FromBody] NewActorRequest input)
        {
            Supplier supplier = new Supplier()
            {
                Name = input.Name,
            };
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Supplier added successfully!"
            });
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateSupplier(int id, NewActorRequest input)
        {
            Supplier? supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return BadRequest();
            }
            supplier.Name = input.Name;
            _context.Suppliers.Update(supplier);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Supplier updated successfully!"
            });
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            Supplier? supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return BadRequest();
            }
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Supplier deleted successfully!"
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSuppliers()
        {
            var suppliers = await _context.Suppliers
                .Include(c => c.Orders)
                .Select(c => new ActorResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    NumberOfExchanges = c.Orders.Count(),
                    ExchangesSize = c.Orders.Sum(i => i.UnitPrice * i.NumberOfUnits)
                })
                .ToListAsync();
            return Ok(suppliers);
        }
    }
}
