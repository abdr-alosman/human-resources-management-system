using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewKaratIk.Models.CustomModels
{
    public class AdayOnaylama
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int? AdayId { get; set; }
        [ForeignKey("AdayId")]
        public virtual Aday? Aday { get; set; }
        public string? aciklama { get; set; }
        public string? Onay { get; set; }
        public enum OnayEnum
        {
            [Display(Name = "Onaylıyorum")]
            poz = 1,

            [Display(Name = "Onaylamıyorum")]
            Neg = 0
        }
    }
}
