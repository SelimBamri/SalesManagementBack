namespace SalesManagementBack.DTO.Requests
{
    public class NewDocumentRequest
    {
        public int ActorId { get; set; }
        public int ProductId { get; set; }
        public int UnitPrice { get; set; }
        public int NumberOfUnits { get; set; }
        public required DateTime IssueDate { get; set; }
    }
}
