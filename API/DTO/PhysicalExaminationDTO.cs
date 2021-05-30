using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class PhysicalExaminationDTO
    {
        [Required(ErrorMessage = "Physical examination result is required")] public string Result;
        [Required(ErrorMessage = "Examination code is required")] public string ExaminationCodeAbbreviation;

        [Required(ErrorMessage = "Appointment is required")]
        public int AppointmentId;
    }
}
