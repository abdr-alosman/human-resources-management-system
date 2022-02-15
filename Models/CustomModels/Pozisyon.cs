using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewKaratIk.Models.CustomModels
{
    public class Pozisyon
    {
        public Pozisyon()
        {
            Status = true;
        }
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Nitelik>? NitelikList { get; set; }
        public string? GorevTanimi { get; set; }
        public int pozSayisi { get; set; }
        public int? ManagerId { get; set; }
        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }
        public bool? Status { get; set; }
        public List<User>? UserList { get; set; }
        public string? Seviye { get; set; }
        public enum SeviyeEnum
        {
            [Display(Name = "Director")]
            UD = 0,
            [Display(Name = "Manager")]
            Sef = 1,
            [Display(Name = "Executive")]
            Orta = 2,
            [Display(Name = "Expert")]
            uzman = 3,
            [Display(Name = "Assistant")]
            Asistan = 4
        }
    }
}
