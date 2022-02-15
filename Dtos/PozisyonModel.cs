using NewKaratIk.Models.CustomModels;

namespace NewKaratIk.Dtos
{
    public class PozisyonModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Nitelik>? NitelikList { get; set; }
        public string? GorevTanimi { get; set; }
        public int pozSayisi { get; set; }
        public int? ManagerId { get; set; }
        public int? DepartmentId { get; set; }
        public bool? Status { get; set; }
        public List<int> UserList { get; set; }
        public string? Seviye { get; set; }
    }
}
