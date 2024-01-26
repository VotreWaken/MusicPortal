using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;
using MusicPortal.Models.AccountModels;
using System.Security.Cryptography;
using System.Text;
using MusicPortal.Models.Hashing;

namespace MusicPortal.Repository.AccountRepository
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly UserContext _context;

        public AccountsRepository(UserContext context) => _context = context;
        public async Task<List<User>> GetAllUsers() => await _context.User.ToListAsync();
        public IQueryable<User> GetUsersByLogin(SignIn user) => _context.User.Where(x => x.Login == user.Login);

        public async Task<bool> IsPasswordCorrect(User user, SignIn model) =>
    await Task.Run(() =>
    {
        Sha256 sha256 = new Sha256();

        string salt = user.Salt;

        string password = sha256.ComputeHash(salt, model.Password);

        return user.Password != password;
    });

        public async Task<bool> IsLoginExists(string? login) =>
    await _context.User.AnyAsync(x => x.Login == login);

        public async Task AddUserToDb(User user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> CreateAndHashPassword(User user, SignUp Model) =>
            await Task.Run(() =>
    {
        Sha256 sha256 = new Sha256();
        string salt = sha256.ComputeSalt();

        user.Password = sha256.ComputeHash(salt, user.Password);
        user.Salt = salt;

        return user;
    });

        public void UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.User
                .SingleOrDefaultAsync(m => m.Id == id);
        }
        public async Task RemoveUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> UserExists(int userId)
        {
            return await _context.User.AnyAsync(u => u.Id == userId);
        }

        public async Task<string> GetImagePath(int imageId)
        {
            var image = await _context.ImagePath.FindAsync(imageId);

            if (image != null)
            {
                return image.Path;
            }

            return "";
        }

		public int GetUserIdByLogin(string user)
        {
			var userEntity = _context.User.FirstOrDefault(x => x.Login == user);

			if (userEntity != null)
			{
				return userEntity.Id;
			}
			return -1;
		}

		public async Task<int> SaveImagePath(string path)
		{
			var imagePath = new ImagePath { Path = path };
			await _context.ImagePath.AddAsync(imagePath);
			await _context.SaveChangesAsync();

			return imagePath.Id;
		}
	}
}
