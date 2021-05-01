using System;
using API.Entities;

namespace API.DTO
{
    public class AppointmentDTO
    {
        
        public string Description { get; set; }
        
        public DateTime ScheduledDate { get; set; }
        
        public string PeselNumber { get; set; }

        public string PermitNumber { get; set; }
        
    }
}
