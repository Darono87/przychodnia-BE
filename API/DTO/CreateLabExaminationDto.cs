using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class CreateLabExaminationDto
    {
        [Required(ErrorMessage = "Appointment is required")]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Examination code is required")]
        public int ExaminationCodeId { get; set; }

        public string DoctorRemarks { get; set; }
    }
}