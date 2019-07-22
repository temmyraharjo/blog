using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Core.Models
{
    public class User : Entity
    {
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}