using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoelsaCommon.Models
{
    public class Contact : IEntity
    {
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column(Order = 2, TypeName = "varchar(75)")]
        public string FirstName { get; set; }

        [Required]
        [Column(Order = 3, TypeName = "varchar(75)")]
        public string LastName { get; set; }

        [Required]
        [Column(Order = 4, TypeName = "nvarchar(75)")]
        public string Company { get; set; }

        [Required]
        [Column(Order = 5, TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [Required]
        [Column(Order = 6, TypeName = "nvarchar(10)")] 
        public string PhoneNumber { get; set; }

        public Contact(){}

        public Contact(string firstName, string lastName, string company, string email, string phoneNumber)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Company = company;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }
    }
}
