using System.ComponentModel.DataAnnotations;

namespace NewKaratIk.Dtos
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
