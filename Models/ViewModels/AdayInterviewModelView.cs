using NewKaratIk.Models.CustomModels;

namespace NewKaratIk.Models.ViewModels
{
    public class AdayInterviewModelView
    {
        public Aday Aday { get; set; }
        public interview Interview { get; set; }
        public User? User { get; set; }
        public Pozisyon Pozisyon { get; set; }
        public Nitelik? Nitelik { get; set; }
        public TeklifFormu TeklifFormu { get; set; }
        public MulakatDegerlendirme? MulakatDegerlendirme { get; set; }
        public AdayOnaylama AdayOnaylama { get; set; }
        public Department? Department { get; set; }


        public IEnumerable<Aday> AdayListVM { get; set; }
        public IEnumerable<TeklifFormu> TeklifFormListVM { get; set; }
        public IEnumerable<AdayOnaylama> AdayOnaylamaListVM { get; set; }
        public IEnumerable<Department>? DepartmentList { get; set; }
        public IEnumerable<User>? UserList { get; set; }
        public IEnumerable<InterviewUser> InterviewUserListVM { get; set; }
        public IEnumerable<MulakatDegerlendirme> MulakatDegerlendirmeListVM { get; set; }
        public IEnumerable<Nitelik>? NitelikListVM { get; set; }
        public IEnumerable<interview> InterviewListVM { get; set; }
        public IEnumerable<Pozisyon> PozisyonListVM { get; set; }
    }
}
