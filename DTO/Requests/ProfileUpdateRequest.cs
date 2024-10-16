namespace SalesManagementBack.DTO.Requests
{
    public class ProfileUpdateRequest
    {
        public string? Email { get; set; }
        public string? Position { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePhoto { get; set; }
    }
}
