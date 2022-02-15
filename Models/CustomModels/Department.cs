using System.ComponentModel.DataAnnotations;

namespace NewKaratIk.Models.CustomModels
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Status { get; set; }
        public List<Pozisyon>? PozisyonList { get; set; }
        public List<User>? UsersList { get; set; }
    }
}
