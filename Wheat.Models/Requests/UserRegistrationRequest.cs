using System.ComponentModel.DataAnnotations;

namespace Wheat.Models.Requests
{
    public class UserRegistrationRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [StringLength(5)]
        public string Code { get; set; }
    }
}
