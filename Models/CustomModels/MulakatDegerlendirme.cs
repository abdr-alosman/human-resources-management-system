using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewKaratIk.Models.CustomModels
{
    public class MulakatDegerlendirme
    {
        [Key]
        public int Id { get; set; }
        public int? mulakId { get; set; }
        [ForeignKey("mulakId")]
        public virtual interview? Interview { get; set; }
        public double? Puan { get; set; }
        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual User? User { get; set; }
        public string? aciklama { get; set; }
        public int? AdayId { get; set; }
        [ForeignKey("AdayId")]
        public virtual Aday? Aday { get; set; }
    }
}
