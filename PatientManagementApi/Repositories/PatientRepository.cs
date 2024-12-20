using System.Data;
using System.Net;
using PatientManagementApi.Models;
using PatientManagementApi.Repositories.Interfaces;
using PatientManagementApi.Utils;

namespace PatientManagementApi.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        public PatientRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public void AddPatient(Patient patient)
        {
            var query = "INSERT INTO Patient (Name, Age, Gender, ContactNumber, Address) VALUES (@Name, @Age, @Gender, @ContactNumber, @Address)";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                var parameters = new[]
                {
                CreateParameter("@Name", EncryptionHelper.Encrypt(patient.Name)),
                CreateParameter("@Age", patient.Age),
                CreateParameter("@Gender", patient.Gender),
                CreateParameter("@ContactNumber", EncryptionHelper.Encrypt(patient.ContactNumber)),
                CreateParameter("@Address", EncryptionHelper.Encrypt(patient.Address))
            };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                command.ExecuteNonQuery();
            }
        }

        public void UpdatePatient(Patient patient)
        {
            var query = "UPDATE Patient SET Name = @Name, Age = @Age, Gender = @Gender, ContactNumber = @ContactNumber, Address = @Address WHERE PatientId = @PatientId";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                var parameters = new[]
                {
                CreateParameter("@Name", EncryptionHelper.Encrypt(patient.Name)),
                CreateParameter("@Age", patient.Age),
                CreateParameter("@Gender", patient.Gender),
                CreateParameter("@ContactNumber", EncryptionHelper.Encrypt(patient.ContactNumber)),
                CreateParameter("@Address", EncryptionHelper.Encrypt(patient.Address)),
                CreateParameter("@PatientId", patient.PatientId)
            };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                command.ExecuteNonQuery();
            }
        }

        public void DeletePatient(int id)
        {
            var query = "DELETE FROM Patient WHERE PatientId = @PatientId";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                command.Parameters.Add(CreateParameter("@PatientId", id));
                command.ExecuteNonQuery();
            }
        }

        public Patient GetPatientById(int id)
        {
            var query = "SELECT * FROM Patient WHERE PatientId = @PatientId";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                command.Parameters.Add(CreateParameter("@PatientId", id));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Patient
                        {
                            PatientId = (int)reader["PatientId"],
                            Name = EncryptionHelper.Decrypt(reader["Name"].ToString()),
                            Age = (int)reader["Age"],
                            Gender = reader["Gender"].ToString(),
                            ContactNumber = EncryptionHelper.Decrypt(reader["ContactNumber"].ToString()),
                            Address = EncryptionHelper.Decrypt(reader["Address"].ToString())
                        };
                    }
                }
            }

            return null;
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            Console.WriteLine("Entering GetAllPatients method...");

            var query = "SELECT * FROM Patient";

            using (var command = _connection.CreateCommand())
            {
                Console.WriteLine("Query executed, processing results...");

                command.CommandText = query;
                command.Transaction = _transaction;

                using (var reader = command.ExecuteReader())
                {
                    var patients = new List<Patient>();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Processing PatientId: {reader["PatientId"]}");

                        try
                        {
                            var decryptedName = EncryptionHelper.Decrypt(reader["Name"].ToString());
                            Console.WriteLine($"Decrypted Name: {decryptedName}");

                            var decryptedContact = EncryptionHelper.Decrypt(reader["ContactNumber"].ToString());
                            Console.WriteLine($"Decrypted Contact: {decryptedContact}");

                            var decryptedAddress = EncryptionHelper.Decrypt(reader["Address"].ToString());
                            Console.WriteLine($"Decrypted Address: {decryptedAddress}");

                            patients.Add(new Patient
                            {
                                PatientId = (int)reader["PatientId"],
                                Name = EncryptionHelper.Decrypt(reader["Name"].ToString()),

                                Age = (int)reader["Age"],
                                Gender = reader["Gender"].ToString(),
                                ContactNumber = EncryptionHelper.Decrypt(reader["ContactNumber"].ToString()),
                                Address = EncryptionHelper.Decrypt(reader["Address"].ToString())
                            });

                            Console.WriteLine("Decryption succeeded for this patient.");
                        }
                        catch (Exception ex)
                        {
                            // Log decryption failure for this patient
                            Console.WriteLine($"Decryption failed: {ex.Message}");
                            throw; // Rethrow to allow the error to propagate
                        }
                    }
                    return patients;
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