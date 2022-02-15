using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewKaratIk.Models.CustomModels
{
    public class Ozluk
    {
        [Key]
        public int Id { get; set; }
        public string? Tcno { get; set; }
        public string? NufusKayitOrnegi { get; set; }
        public string? AdliSicil { get; set; }
        public string? YerlisimYeri { get; set; }
        public string? ogrenimBlegesi { get; set; }
        public string? KanGrubu { get; set; }
        public string? saglikRaporu { get; set; }
        public string? NufusCuzdanFotok { get; set; }
        public string? kursBelgeleri { get; set; }
        public string? Fotograf { get; set; }
        public string? AskerlikBelgesi { get; set; }
        public string? iskurKayit { get; set; }
        public string? AileBildirimFormu { get; set; }
        public string? isBasvuruFormu { get; set; }
        public string? Muvakatname { get; set; }
        public string? MaasHesapIbanNo { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
