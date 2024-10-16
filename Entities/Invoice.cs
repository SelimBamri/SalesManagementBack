using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesManagementBack.Entities
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public int UnitPrice { get; set; }
        public int NumberOfUnits { get; set; }  
        public required DateTime IssueDate { get; set; }
        public int ClientFk { get; set; }
        [ForeignKey("ClientFk")]
        public required virtual Client Client { get; set; }

        public int ProductFk { get; set; }
        [ForeignKey("ProductFk")]
        public required virtual Product Product { get; set; }
    }
}
