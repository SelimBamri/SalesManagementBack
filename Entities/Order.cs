using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesManagementBack.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UnitPrice { get; set; }
        public int NumberOfUnits { get; set; }
        public required DateTime IssueDate { get; set; }
        public int SupplierFk { get; set; }
        [ForeignKey("SupplierFk")]
        public required virtual Supplier Supplier { get; set; }
        public int ProductFk { get; set; }
        [ForeignKey("ProductFk")]
        public required virtual Product Product { get; set; }
    }
}
