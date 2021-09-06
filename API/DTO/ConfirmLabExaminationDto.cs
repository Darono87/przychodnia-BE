using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class ConfirmLabExaminationDto
    {
        [Required(ErrorMessage = "Lab Examination id is required")]
        public int Id { get; set; }
        
        public string ManagerRemarks { get; set; }
    }
}
