using System.Data;
using PatientManagementApi.Models;
using PatientManagementApi.Repositories.Interfaces;
using PatientManagementApi.Utils;

namespace PatientManagementApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public void AddUser(User user)
        {
            var query = "INSERT INTO [User] (Username, PasswordHash, Role) VALUES (@Username, @PasswordHash, @Role)";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                var parameters = new[]
                {
                CreateParameter("@Username", user.Username),
                CreateParameter("@PasswordHash", EncryptionHelper.Encrypt(user.PasswordHash)),
                CreateParameter("@Role", user.Role)
            };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                command.ExecuteNonQuery();
            }
        }

        public void UpdateUser(User user)
        {
            var query = "UPDATE [User] SET PasswordHash = @PasswordHash, Role = @Role WHERE UserId = @UserId";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                var parameters = new[]
                {
                CreateParameter("@PasswordHash", EncryptionHelper.Encrypt(user.PasswordHash)),
                CreateParameter("@Role", user.Role),
                CreateParameter("@UserId", user.UserId)
            };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                command.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int id)
        {
            var query = "DELETE FROM [User] WHERE UserId = @UserId";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                command.Parameters.Add(CreateParameter("@UserId", id));
                command.ExecuteNonQuery();
            }
        }

        public User GetUserById(int id)
        {
            var query = "SELECT * FROM [User] WHERE UserId = @UserId";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                command.Parameters.Add(CreateParameter("@UserId", id));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserId = (int)reader["UserId"],
                            Username = reader["Username"].ToString(),
                            PasswordHash = EncryptionHelper.Decrypt(reader["PasswordHash"].ToString()),
                            Role = reader["Role"].ToString()
                        };
                    }
                }
            }

            return null;
        }

        public User GetUserByUsername(string username)
        {
            var query = "SELECT * FROM [User] WHERE Username = @Username";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                command.Parameters.Add(CreateParameter("@Username", username));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserId = reader["UserId"] != DBNull.Value ? (int)reader["UserId"] : 0,
                            Username = reader["Username"]?.ToString(),
                            PasswordHash = reader["PasswordHash"]?.ToString(), // Use raw hash, not decryption
                            Role = reader["Role"]?.ToString()
                        };
                    }
                }
            }

            return null; // Return null if no user is found
        }

        public IEnumerable<User> GetAllUsers()
        {
            var query = "SELECT * FROM [User]";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = _transaction;

                using (var reader = command.ExecuteReader())
                {
                    var users = new List<User>();
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserId = (int)reader["UserId"],
                            Username = reader["Username"].ToString(),
                            PasswordHash = EncryptionHelper.Decrypt(reader["PasswordHash"].ToString()),
                            Role = reader["Role"].ToString()
                        });
                    }
                    return users;
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
