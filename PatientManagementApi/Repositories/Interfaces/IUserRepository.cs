using PatientManagementApi.Models;

namespace PatientManagementApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);
        void AddUser(User user);
    }
}
