using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewKaratIk.Models.CustomModels
{
    public class Nitelik
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Score { get; set; }
        public int? PozisyonId { get; set; }
        [ForeignKey("PozisyonId")]
        public virtual Pozisyon? Pozisyon { get; set; }
    }
}
