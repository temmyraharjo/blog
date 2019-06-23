using System.ComponentModel.DataAnnotations;

namespace backend.Data
{
    public class User : Entity
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "First Name can't be empty")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Last Name can't be empty")]
        public string LastName { get; set; }
    }
}