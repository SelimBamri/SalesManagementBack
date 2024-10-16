using System.ComponentModel.DataAnnotations;

namespace SalesManagementBack.DTO.Requests
{
    public class NewProductRequest
    {
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required string Reference { get; set; }
    }
}
