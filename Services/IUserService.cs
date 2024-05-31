using dotnet8_introduction.Entities;

namespace dotnet8_introduction.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password);
        void Delete(int id);
    }
}
