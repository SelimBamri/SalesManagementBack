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
    public class InvoiveController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public InvoiveController(AppDbContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddInvoice([FromBody] NewDocumentRequest input)
        {
            Client? c = await _context.Clients.FindAsync(input.ActorId);
            if (c == null) {
                return BadRequest();
            }
            Product? p = await _context.Products.FindAsync(input.ProductId);
            if (p == null) { 
                return BadRequest();
            }
            Invoice invoice = new Invoice()
            {
                Client = c,
                Product = p,
                UnitPrice = input.UnitPrice,
                NumberOfUnits = input.NumberOfUnits,
                IssueDate = input.IssueDate,
            };
            _context.Invoices.Add(invoice);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Invoice added successfully!"
            });
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            Invoice? invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return BadRequest();
            }
            _context.Invoices.Remove(invoice);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Invoice deleted successfully!"
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetInvoices()
        {
            var invocices = await _context.Invoices
                .Select(c => new DocumentResponse
                {
                    Id = c.Id,
                    IssueDate = c.IssueDate,
                    ProductName = c.Product.Name,
                    ActorName = c.Client.Name,
                    UnitPrice = c.UnitPrice,
                    NumberOfUnits = c.NumberOfUnits,
                    TotalAmount = c.UnitPrice * c.NumberOfUnits
                })
                .ToListAsync();
            return Ok(invocices);
        }
    }
}
