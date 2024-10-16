namespace SalesManagementBack.DTO.Responses
{
    public class AuthenticationResponse
    {
        public bool IsAuthenticated { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
    }
}
