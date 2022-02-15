using System.ComponentModel.DataAnnotations;

namespace NewKaratIk.Dtos
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email boş bırakılamaz")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifreniz büyük ve küçük harfler, sayılar içermelidir.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Lütfen şifrenizi tekrar giriniz")]
        [DataType(DataType.Password)]
        public string? RePassword { get; set; }
        [Required(ErrorMessage = "Lütfen Adınızı  giriniz")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Lütfen e-posta Soyadınızı  giriniz")]
        public string? Surname { get; set; }
        public string? UserName { get; set; }
    }
}
