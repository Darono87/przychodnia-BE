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

        [Required(ErrorMessage = "Doctor ID is required")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Patient ID is required")]
        public int PatientId { get; set; }
    }
}
