using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class AppointmentDto
    {
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Scheduled date is required")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Pesel number is required")]
        public string PeselNumber { get; set; }

        [Required(ErrorMessage = "Permit number is required")]
        public string PermitNumber { get; set; }
    }
}
