using System.ComponentModel.DataAnnotations;

namespace AddressBookApi.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
