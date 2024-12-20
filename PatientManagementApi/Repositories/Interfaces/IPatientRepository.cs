using PatientManagementApi.Models;

namespace PatientManagementApi.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        IEnumerable<Patient> GetAllPatients();
        Patient GetPatientById(int patientId);
        void AddPatient(Patient patient);
        void UpdatePatient(Patient patient);
        void DeletePatient(int patientId);
    }
}
