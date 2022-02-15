using System.ComponentModel.DataAnnotations;

namespace NewKaratIk.Dtos
{
    public class ResetPasswordModel
    {
        [Required]
        public string? Token { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre ")]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre tekrarı")]
        [Compare("Password")]
        public string? RePassword { get; set; }

    }
}
