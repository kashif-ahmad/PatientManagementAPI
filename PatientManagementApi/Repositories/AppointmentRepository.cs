using PatientManagementApi.Models;
using PatientManagementApi.Repositories.Interfaces;
using System.Data;

namespace PatientManagementApi.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        public AppointmentRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public void AddAppointment(Appointment appointment)
        {
            var query = "INSERT INTO Appointment (PatientId, DoctorId, AppointmentDate, Reason, Status, Diagnosis) VALUES (@PatientId, @DoctorId, @AppointmentDate, @Reason, @Status, @Diagnosis)";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                var parameters = new[]
                {
                CreateParameter("@PatientId", appointment.PatientId),
                CreateParameter("@DoctorId", appointment.DoctorId),
                CreateParameter("@AppointmentDate", appointment.AppointmentDate),
                CreateParameter("@Reason", appointment.Reason),
                CreateParameter("@Status", appointment.Status),
                CreateParameter("@Diagnosis", appointment.Diagnosis)
            };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                command.ExecuteNonQuery();
            }
        }

        public void UpdateAppointment(Appointment appointment)
        {
            var query = "UPDATE Appointment SET PatientId = @PatientId, DoctorId = @DoctorId, AppointmentDate = @AppointmentDate, Reason = @Reason, Status = @Status, Diagnosis = @Diagnosis WHERE AppointmentId = @AppointmentId";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                var parameters = new[]
                {
                CreateParameter("@PatientId", appointment.PatientId),
                CreateParameter("@DoctorId", appointment.DoctorId),
                CreateParameter("@AppointmentDate", appointment.AppointmentDate),
                CreateParameter("@Reason", appointment.Reason),
                CreateParameter("@Status", appointment.Status),
                CreateParameter("@Diagnosis", appointment.Diagnosis),
                CreateParameter("@AppointmentId", appointment.AppointmentId)
            };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                command.ExecuteNonQuery();
            }
        }

        public void DeleteAppointment(int id)
        {
            var query = "DELETE FROM Appointment WHERE AppointmentId = @AppointmentId";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                command.Parameters.Add(CreateParameter("@AppointmentId", id));
                command.ExecuteNonQuery();
            }
        }

        public Appointment GetAppointmentById(int id)
        {
            var query = "SELECT * FROM Appointment WHERE AppointmentId = @AppointmentId";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                command.Parameters.Add(CreateParameter("@AppointmentId", id));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Appointment
                        {
                            AppointmentId = (int)reader["AppointmentId"],
                            PatientId = (int)reader["PatientId"],
                            DoctorId = (int)reader["DoctorId"],
                            AppointmentDate = (DateTime)reader["AppointmentDate"],
                            Reason = reader["Reason"].ToString(),
                            Status = reader["Status"].ToString(),
                            Diagnosis = reader["Diagnosis"].ToString()
                        };
                    }
                }
            }

            return null;
        }

        public IEnumerable<Appointment> GetAllAppointments()
        {
            var query = "SELECT * FROM Appointment";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                using (var reader = command.ExecuteReader())
                {
                    var appointments = new List<Appointment>();
                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            AppointmentId = (int)reader["AppointmentId"],
                            PatientId = (int)reader["PatientId"],
                            DoctorId = (int)reader["DoctorId"],
                            AppointmentDate = (DateTime)reader["AppointmentDate"],
                            Reason = reader["Reason"].ToString(),
                            Status = reader["Status"].ToString(),
                            Diagnosis = reader["Diagnosis"].ToString()
                        });
                    }
                    return appointments;
                }
            }
        }

        private IDbDataParameter CreateParameter(string name, object value)
        {
            var param = _connection.CreateCommand().CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            return param;
        }
    }
}