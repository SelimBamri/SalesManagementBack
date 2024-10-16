using System.ComponentModel.DataAnnotations;

namespace SalesManagementBack.DTO.Requests
{
    public class NewActorRequest
    {
        [Required]
        public required string Name { get; set; }
    }
}
