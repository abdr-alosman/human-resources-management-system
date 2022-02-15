using System.ComponentModel.DataAnnotations;

namespace NewKaratIk.Models.CustomModels
{
    public class InterviewUser
    {
        [Key]
        public int Id { get; set; }
        public string? IsTeknik { get; set; }
        public enum IsTeknikEnum
        {
            [Display(Name = "Hayır")]
            hayir = 0,
            [Display(Name = "Evet")]
            evet = 1,

        }
        public int? UserId { get; set; }
        public User? User { get; set; }

        public int? interviewId { get; set; }
        public interview? Interview { get; set; }
    }
}
