using System.ComponentModel.DataAnnotations;

namespace SalesManagementBack.Entities
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    }
}
