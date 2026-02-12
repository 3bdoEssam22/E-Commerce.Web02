
using System.ComponentModel.DataAnnotations;

namespace Shared.DataTtansferObjects.IdentityDTOs
{
    public class LoginDto
    {
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
    }
}
