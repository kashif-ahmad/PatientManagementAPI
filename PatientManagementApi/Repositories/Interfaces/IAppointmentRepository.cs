using PatientManagementApi.Models;

namespace PatientManagementApi.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        IEnumerable<Appointment> GetAllAppointments();
        Appointment GetAppointmentById(int appointmentId);
        void AddAppointment(Appointment appointment);
        void UpdateAppointment(Appointment appointment);
        void DeleteAppointment(int appointmentId);
    }
}
