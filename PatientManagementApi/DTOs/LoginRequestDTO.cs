using System.ComponentModel.DataAnnotations;

namespace PatientManagementApi.DTOs
{
    public class LoginRequestDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
