using System.ComponentModel.DataAnnotations;

namespace AddressBookApi.DTOs
{
    public class ContactDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
