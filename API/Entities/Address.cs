using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(160)]
        public string Country { get; set; }

        [MaxLength(20)]
        public string PostalCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string City { get; set; }

        [Required]
        [MaxLength(200)]
        public string Street { get; set; }

        [Required]
        [MaxLength(20)]
        public string BuildingNumber { get; set; }
    }
}
