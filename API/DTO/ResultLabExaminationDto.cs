using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class ResultLabExaminationDto
    {
        [Required(ErrorMessage = "Lab Examination id is required")]
        
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Lab Examination result is required")]
        
        public string Result { get; set; }
    }
}
