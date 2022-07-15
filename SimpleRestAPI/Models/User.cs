using System.ComponentModel.DataAnnotations;

namespace SimpleRestAPI.Models
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public override string ToString()
        {
            return $"USer: Id: {Id}, Name: {Name}, Password: {Password}";
        }
    }
}
