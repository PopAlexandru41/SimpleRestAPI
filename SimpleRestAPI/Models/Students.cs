using System.ComponentModel.DataAnnotations;

namespace SimpleRestAPI.Models
{
    public class Students
    {
        [Key] 
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Range(0,10)]
        public int FinalNote { get; set; }
        [StringLength(50)]
        public string Observation { get; set; }

        public override string ToString()
        {
            return $"Student: Id: {Id}, Name: {Name}, Age: {Age}, FinalNote: {FinalNote}, Observation: {Observation}";
        }
    }
}
