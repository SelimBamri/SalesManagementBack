using Microsoft.EntityFrameworkCore;

namespace SalesManagementBack.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options): base(options) { }
    }
}
