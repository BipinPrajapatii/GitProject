using System.ComponentModel.DataAnnotations;

namespace ProjectApplication.Entities
{
    public class Employee
    {
        [Key] // Primary Key
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Department { get; set; }

        public string Gender { get; set; }

        public bool IsActive { get; set; }
    }
}