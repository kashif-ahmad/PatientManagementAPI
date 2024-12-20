using System.ComponentModel.DataAnnotations;

namespace PatientManagementApi.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Range(0, 150, ErrorMessage = "Age must be between 0 and 150.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Contact Number is required.")]
        [Phone(ErrorMessage = "Invalid Contact Number.")]
        public string ContactNumber { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; }
    }
}