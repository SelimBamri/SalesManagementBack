namespace SalesManagementBack.DTO.Responses
{
    public class DocumentResponse
    {
        public int Id { get; set; }
        public required string ActorName { get; set; }
        public required string ProductName { get; set; }
        public required DateTime IssueDate { get; set; }
        public int NumberOfUnits { get; set; }
        public int UnitPrice { get; set; }
        public int TotalAmount { get; set; }
    }
}
