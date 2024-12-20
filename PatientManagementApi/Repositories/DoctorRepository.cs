using System.Data;
using PatientManagementApi.Models;
using PatientManagementApi.Repositories.Interfaces;
using PatientManagementApi.Utils;

namespace PatientManagementApi.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        public DoctorRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public void AddDoctor(Doctor doctor)
        {
            var query = "INSERT INTO Doctor (Name, Specialization, Phone, Email) VALUES (@Name, @Specialization, @Phone, @Email)";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                var parameters = new[]
                {
                CreateParameter("@Name", doctor.Name),
                CreateParameter("@Specialization", doctor.Specialization),
                CreateParameter("@Phone", EncryptionHelper.Encrypt(doctor.Phone)),
                CreateParameter("@Email", EncryptionHelper.Encrypt(doctor.Email))
            };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                command.ExecuteNonQuery();
            }
        }

        public void UpdateDoctor(Doctor doctor)
        {
            var query = "UPDATE Doctor SET Name = @Name, Specialization = @Specialization, Phone = @Phone, Email = @Email WHERE DoctorId = @DoctorId";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                var parameters = new[]
                {
                CreateParameter("@Name", EncryptionHelper.Encrypt(doctor.Name)),
                CreateParameter("@Specialization", doctor.Specialization),
                CreateParameter("@Phone", EncryptionHelper.Encrypt(doctor.Phone)),
                CreateParameter("@Email", EncryptionHelper.Encrypt(doctor.Email)),
                CreateParameter("@DoctorId", doctor.DoctorId)
            };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                command.ExecuteNonQuery();
            }
        }

        public void DeleteDoctor(int id)
        {
            var query = "DELETE FROM Doctor WHERE DoctorId = @DoctorId";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                command.Parameters.Add(CreateParameter("@DoctorId", id));
                command.ExecuteNonQuery();
            }
        }

        public Doctor GetDoctorById(int id)
        {
            var query = "SELECT * FROM Doctor WHERE DoctorId = @DoctorId";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                command.Parameters.Add(CreateParameter("@DoctorId", id));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Doctor
                        {
                            DoctorId = (int)reader["DoctorId"],
                            Name = reader["Name"].ToString(),
                            Specialization = reader["Specialization"].ToString(),
                            Phone = EncryptionHelper.Decrypt(reader["Phone"].ToString()),
                            Email = EncryptionHelper.Decrypt(reader["Email"].ToString())
                        };
                    }
                }
            }

            return null;
        }

        public IEnumerable<Doctor> GetAllDoctors()
        {
            var query = "SELECT * FROM Doctor";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                using (var reader = command.ExecuteReader())
                {
                    var doctors = new List<Doctor>();
                    while (reader.Read())
                    {
                        // Log encrypted values before decryption
                        Console.WriteLine($"Encrypted Phone: {reader["Phone"]}");
                        Console.WriteLine($"Encrypted Email: {reader["Email"]}");

                        try
                        {
                            doctors.Add(new Doctor
                            {
                                DoctorId = (int)reader["DoctorId"],
                                Name = reader["Name"].ToString(),
                                Specialization = reader["Specialization"].ToString(),
                                Phone = EncryptionHelper.Decrypt(reader["Phone"].ToString()),
                                Email = EncryptionHelper.Decrypt(reader["Email"].ToString())
                            });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Decryption failed: {ex.Message}");
                            throw; // Rethrow to propagate the error
                        }
                    }
                    return doctors;
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