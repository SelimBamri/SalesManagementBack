namespace SalesManagementBack.DTO.Responses
{
    public class MyProfileResponse
    {
        public required string Email { get; set; }
        public required string Position { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public byte[]? ProfilePhoto { get; set; }
    }
}
