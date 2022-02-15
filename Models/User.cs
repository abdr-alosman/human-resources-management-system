using Microsoft.AspNetCore.Identity;
using NewKaratIk.Models.CustomModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewKaratIk.Models
{
    public class User: IdentityUser<int>
    {
        public User()
        {
            Status = false;
            IsAdmin = false;
            IsOzluk = false;
        }

        [Column(TypeName = "nvarchar(50)")]
        public string? Name { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Surname { get; set; }
        public bool? Status { get; set; }
        public bool? IsAdmin { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? NameSurname { get; set; }
        public int? FirstLevelManager { get; set; }
        public int? DepartmentId { get; set; }
        public bool? IsOzluk { get; set; }
        public Ozluk? Ozluk { get; set; }
        public int? PozisyonId { get; set; }
        [ForeignKey("PozisyonId")]
        public virtual Pozisyon? Pozisyon { get; set; }
    }
}

