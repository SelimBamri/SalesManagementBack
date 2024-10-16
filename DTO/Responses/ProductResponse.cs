using System.ComponentModel.DataAnnotations;

namespace SalesManagementBack.DTO.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Reference { get; set; }
        public int NumberOfSoldUnits { get; set; }
        public int NumberOfBoughtUnits { get; set; }
        public int TotalCost { get; set; }
        public int TotalProfit { get; set; }
        public int Balance{ get; set; }
    }
}
