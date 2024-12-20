using PatientManagementApi.Models;

namespace PatientManagementApi.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        IEnumerable<Doctor> GetAllDoctors();
        Doctor GetDoctorById(int doctorId);
        void AddDoctor(Doctor doctor);
        void UpdateDoctor(Doctor doctor);
        void DeleteDoctor(int doctorId);
    }
}
