using System.ComponentModel.DataAnnotations;

namespace AuthanticateAndAuthorizeFlow.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
