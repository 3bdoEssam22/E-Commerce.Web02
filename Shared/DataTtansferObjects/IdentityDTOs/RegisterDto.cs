
using System.ComponentModel.DataAnnotations;

namespace Shared.DataTtansferObjects.IdentityDTOs
{
    public class RegisterDto
    {
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = default!;
    }
}
