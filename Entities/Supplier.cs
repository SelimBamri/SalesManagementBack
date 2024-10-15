using System.ComponentModel.DataAnnotations;

namespace SalesManagementBack.Entities
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}
