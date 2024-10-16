namespace SalesManagementBack.DTO.Responses
{
    public class ActorResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int NumberOfExchanges { get; set; }
        public int ExchangesSize { get; set; }
    }
}
