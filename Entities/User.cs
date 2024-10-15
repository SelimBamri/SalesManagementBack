using Microsoft.AspNetCore.Identity;

namespace SalesManagementBack.Entities
{
    public class User : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Position { get; set; }
        public byte[]? ProfilePhoto { get; set; }
    }
}
