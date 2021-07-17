using System;
using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTO
{
    public class ExaminationCodeDto
    {

        [Required(ErrorMessage = "Examination Type is required")]
        public ExaminationType Type { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Abbreviation is required")]
        [MaxLength(30)]
         public string Abbreviation { get; set; }
    }
}
