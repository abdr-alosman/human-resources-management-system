using NewKaratIk.Models.CustomModels;

namespace NewKaratIk.Models.ViewModels
{
    public class PozisyonViewModel
    {
        public Pozisyon? Pozisyon { get; set; }
        public Nitelik? Nitelik { get; set; }
        public Department? Department { get; set; }
        public User? User { get; set; }


        public IEnumerable<User>? UserList { get; set; }
        public IEnumerable<Department>? DepartmentList { get; set; }
        public IEnumerable<Pozisyon>? PozisyonListVM { get; set; }
        public IEnumerable<Nitelik>? NitelikListVM { get; set; }
    }
}
