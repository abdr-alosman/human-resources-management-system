using NewKaratIk.Models.CustomModels;

namespace NewKaratIk.Models.ViewModels
{
    public class OzlukUserVM
    {
        public Ozluk? Ozluk { get; set; }
        public User User { get; set; }
        public Pozisyon? Pozisyon { get; set; }
        public IEnumerable<Ozluk>? OzlukListVM { get; set; }
        public IEnumerable<Pozisyon>? PozisyonListVM { get; set; }
        public IEnumerable<User> UserListVM { get; set; }
    }
}
