using System.ComponentModel.DataAnnotations;

namespace NewKaratIk.Dtos
{
    public class DepartmenModel
    {
        public int? Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public bool Status { get; set; }
    }
}
