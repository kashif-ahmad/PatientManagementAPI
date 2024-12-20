using System.ComponentModel.DataAnnotations;

namespace PatientManagementApi.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "PatientId is required.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "DoctorId is required.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Appointment Date is required.")]
        public DateTime AppointmentDate { get; set; }

        [StringLength(255, ErrorMessage = "Reason cannot exceed 255 characters.")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        [StringLength(255, ErrorMessage = "Diagnosis cannot exceed 255 characters.")]
        public string Diagnosis { get; set; }
    }
}