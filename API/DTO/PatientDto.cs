using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class PatientDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Pesel number is required")]
        public string PeselNumber { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Cityr is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Building number is required")]
        public string BuildingNumber { get; set; }
    }
}
