using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewKaratIk.Models.CustomModels
{
    public class interview
    {
        public interview()
        {
            isDone = false;
        }
        [Key]
        public int Id { get; set; }
        public DateTime interviewDate { get; set; }
        public bool? isDone { get; set; }

        public string? Subject { get; set; }
        public string? ilaveNot { get; set; }
        public string? Yer { get; set; }
        public ICollection<InterviewUser>? InterviewUsers { get; set; }
        public int AdayId { get; set; }
        [ForeignKey("AdayId")]
        public virtual Aday? Aday { get; set; }
    }
}
