using System.ComponentModel.DataAnnotations;

namespace RentSomeWheels.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        public required string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Required(ErrorMessage = "The license plate is required.")]
        [RegularExpression(@"^(?:[A-Z]{2}-\d{2}-\d{2}|\d{2}-\d{2}-[A-Z]{2}|\d{2}-[A-Z]{2}-\d{2}|[A-Z]{2}-\d{2}-[A-Z]{2})$", 
        ErrorMessage = "Invalid Portugal license plate format.")]
        public string LicensePlate { get; set; }

        [Required]
        [CustomValidation(typeof(Vehicle), nameof(ValidateYearOfManufacture))]
        public int YearOfManufacture { get; set; }

        [Required]
        public string FuelType { get; set; }

        public bool IsRented { get; set; } = false;

        public static ValidationResult ValidateYearOfManufacture(int yearOfManufacture, ValidationContext context)
        {
            if (yearOfManufacture < 1886)
            {
                return new ValidationResult("Year of manufacture cannot be earlier than the year when the first car was manufactured.");
            }

            if (yearOfManufacture > DateTime.Now.Year)
            {
                return new ValidationResult("Year of manufacture cannot be later than the current year.");
            }

            return ValidationResult.Success;
        }
    }
}