using System.ComponentModel.DataAnnotations;

namespace SalesManagementBack.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required string Reference { get; set; }

    }
}
