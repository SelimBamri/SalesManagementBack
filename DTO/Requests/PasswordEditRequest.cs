namespace SalesManagementBack.DTO.Requests
{
    public class PasswordEditRequest
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
