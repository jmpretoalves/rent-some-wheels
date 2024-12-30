using System;
using System.ComponentModel.DataAnnotations;

namespace RentSomeWheels.Models
{
    public class RentalContract
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [Required]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [CustomValidation(typeof(RentalContract), nameof(ValidateStartDate))]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [CustomValidation(typeof(RentalContract), nameof(ValidateEndDate))]
        public DateTime EndDate { get; set; }

        [Required]
        public int InitialMileage { get; set; }

        public static ValidationResult ValidateStartDate(DateTime startDate, ValidationContext context)
        {
            if (startDate < DateTime.Today)
            {
                return new ValidationResult("Start date cannot be earlier than the current date.");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateEndDate(DateTime endDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as RentalContract;
            if (instance != null && endDate <= instance.StartDate)
            {
                return new ValidationResult("End date must be later than the start date.");
            }
            return ValidationResult.Success;
        }
    }
}