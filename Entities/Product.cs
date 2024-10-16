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
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();


    }
}
