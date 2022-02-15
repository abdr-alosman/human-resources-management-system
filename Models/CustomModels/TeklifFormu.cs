using System.ComponentModel.DataAnnotations;

namespace NewKaratIk.Models.CustomModels
{
    public class TeklifFormu
    {
        [Key]
        public int Id { get; set; }
        public string? IlaveNot { get; set; }
        public int? AdayId { get; set; }
        public string? Subject { get; set; }
        public string? TeklifFile { get; set; }
        public string? imzaliTeklifFile { get; set; }
        public DateTime? TeklifDate { get; set; }
        public string? Status { get; set; }
        public enum StatusEnum
        {

            [Display(Name = "Reddedildi")]
            Neg = 0,
            [Display(Name = "Kabul edildi")]
            Poz = 1,
            [Display(Name = "Beklemede")]
            Bek = 2,
        }
    }
}
