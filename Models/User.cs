using System.ComponentModel.DataAnnotations;

namespace AddressBookApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public List<Contact> Contacts { get; set; }
    }
}
