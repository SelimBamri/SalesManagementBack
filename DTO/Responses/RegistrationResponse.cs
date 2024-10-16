namespace SalesManagementBack.DTO.Responses
{
    public class RegistrationResponse
    {
        public bool IsSuccessful { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
