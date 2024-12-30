using System.ComponentModel.DataAnnotations;

namespace RentSomeWheels.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The phone number is required.")]
        [Phone]
        [RegularExpression(@"^(?:2\d{8}|9[01236]\d{7})$", 
        ErrorMessage = "Invalid Portugal phone number format.")]
        public string PhoneNumber { get; set; }

        [Required]
        public string DrivingLicense { get; set; }
    }
}