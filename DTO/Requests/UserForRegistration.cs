using System.ComponentModel.DataAnnotations;

namespace SalesManagementBack.DTO.Requests
{
    public class UserForRegistration
    {
        [Required(ErrorMessage = "First name is required")]
        public required string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public required string LastName { get; set; }
        public string? ProfilePhoto { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
}
