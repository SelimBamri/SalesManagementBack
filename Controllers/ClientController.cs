using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesManagementBack.Data;
using SalesManagementBack.DTO.Requests;
using SalesManagementBack.DTO.Responses;
using SalesManagementBack.Entities;
using System.Security.Claims;

namespace SalesManagementBack.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public ClientController(AppDbContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddClient( [FromBody] NewActorRequest input)
        {
            Client client = new Client()
            {
                Name = input.Name,
            };
            _context.Clients.Add(client);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Client added successfully!"
            });             
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateClient(int id, NewActorRequest input)
        {
            Client? client = await _context.Clients.FindAsync(id);
            if(client == null)
            {
                return BadRequest();
            }
            client.Name = input.Name;
            _context.Clients.Update(client);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Client updated successfully!"
            });
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteClient(int id)
        {
            Client? client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return BadRequest();
            }
            _context.Clients.Remove(client);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Client deleted successfully!"
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _context.Clients
                .Include(c => c.Invoices)
                .Select(c => new ActorResponse
                {
                    Name = c.Name,
                    NumberOfExchanges = c.Invoices.Count(),
                    ExchangesSize = c.Invoices.Sum(i => i.UnitPrice * i.NumberOfUnits)
                })
                .ToListAsync();
            return Ok(clients);
        }
    }
}
