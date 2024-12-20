using PatientManagementApi.Repositories.Interfaces;
using PatientManagementApi.Repositories;
using System.Data;

namespace PatientManagementApi.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        public IPatientRepository Patients { get; }
        public IDoctorRepository Doctors { get; }
        public IAppointmentRepository Appointments { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(IDbConnection connection,
                          IPatientRepository patients,
                          IDoctorRepository doctors,
                          IAppointmentRepository appointments,
                          IUserRepository users)
        {
            _connection = connection;
            _connection.Open(); // Open the connection once
            _transaction = _connection.BeginTransaction(); // Start the transaction

            Patients = patients;
            Doctors = doctors;
            Appointments = appointments;
            Users = users;

            // Pass the shared transaction to repositories
            InitializeRepositories();
        }

        private void InitializeRepositories()
        {
            (Patients as PatientRepository)?.SetTransaction(_transaction);
            (Doctors as DoctorRepository)?.SetTransaction(_transaction);
            (Appointments as AppointmentRepository)?.SetTransaction(_transaction);
            (Users as UserRepository)?.SetTransaction(_transaction);
        }

        public void Commit()
        {
            _transaction?.Commit();
            _transaction = _connection.BeginTransaction(); // Start a new transaction for further operations
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction = _connection.BeginTransaction(); // Start a new transaction for further operations
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}
