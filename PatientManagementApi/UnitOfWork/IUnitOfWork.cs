using PatientManagementApi.Repositories.Interfaces;
using System;

namespace PatientManagementApi.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patients { get; }
        IDoctorRepository Doctors { get; }
        IAppointmentRepository Appointments { get; }
        IUserRepository Users { get; }
        void Commit();
        void Rollback();
    }
}
