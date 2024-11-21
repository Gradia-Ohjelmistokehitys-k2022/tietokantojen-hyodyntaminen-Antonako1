using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace teht1.Models
{
    internal class UserModel
    {
        [Table("User")]
        public class User
        {
            [Key]
            [Column("MemberId")]
            public int Id { get; set; }

            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Address { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Email { get; set; }
            public DateTime? RegistrationDate { get; set; }
        }
    }
}