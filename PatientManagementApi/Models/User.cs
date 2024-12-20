using System.ComponentModel.DataAnnotations;

namespace PatientManagementApi.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "PasswordHash is required.")]
        [StringLength(200, ErrorMessage = "PasswordHash cannot exceed 200 characters.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        public string Role { get; set; } // Admin, Doctor, Patient
    }
}