using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewKaratIk.Models.CustomModels
{
    public class Aday
    {
        [Key]
        public int Id { get; set; }
        public string? NameSurname { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public string? CV { get; set; }
        public string? Status { get; set; }
        public enum StatusEnum
        {

            [Display(Name = "Değerlendiriliyor")]
            Wait = 5
        }
        public string? IpAdres { get; set; }
        public bool KvkkOnayi { get; set; }
        public List<interview>? interviewList { get; set; }

        public int? PozisyonId { get; set; }
        [ForeignKey("PozisyonId")]
        public virtual Pozisyon? Pozisyon { get; set; }
    }
}
