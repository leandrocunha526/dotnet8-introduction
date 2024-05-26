using System.Security.Cryptography;
using dotnet8_introduction.Data;
using dotnet8_introduction.Entities;
using dotnet8_introduction.Helpers;

namespace dotnet8_introduction.Services
{
    public class UserService : IUserService
    {
        private DataContext _context;
        public UserService(DataContext context)
        {
            _context = context;
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            // hmac is a hash-based message authentication code
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(storedHash));
            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(storedSalt));

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }
            }

            return true;
        }
        public User Authenticate(string username, string password)
        {
            User authenticatedUser = null;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var user = _context?.Users.SingleOrDefault(x => x.Username == username);

                // check if username exists and password is correct
                if (user != null && VerifyPasswordHash(password, user.Password, user.PasswordSalt))
                {
                    authenticatedUser = user;
                }
            }

            return authenticatedUser;
        }

        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context?.Users.Any(x => x.Username == user.Username) == true)
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.Password = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context?.Users.Add(user);
            _context?.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            var user = _context?.Users.Find(id);
            if (user != null)
            {
                _context?.Users.Remove(user);
                _context?.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _context!.Users;
        }

        public User GetById(int id)
        {
            return _context?.Users.Find(id);
        }

        public void Update(User user, string password)
        {
            var userToUpdate = _context?.Users.Find(user.Id);

            if (userToUpdate == null)
                throw new AppException("User not found");

            if (user.Username != userToUpdate.Username)
            {
                // username has changed so check if the new username is already taken
                if (_context?.Users.Any(x => x.Username == user.Username) == true)
                    throw new AppException("Username " + user.Username + " is already taken");
            }

            // update user properties
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Username = user.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                userToUpdate.Password = passwordHash;
                userToUpdate.PasswordSalt = passwordSalt;
            }

            _context?.Users.Update(userToUpdate);
            _context?.SaveChanges();
        }
    }
}
