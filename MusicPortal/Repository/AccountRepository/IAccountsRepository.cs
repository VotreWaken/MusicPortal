using MusicPortal.Models.AccountModels;

namespace MusicPortal.Repository.AccountRepository
{
    public interface IAccountsRepository
    {
        Task<List<User>> GetAllUsers();

        IQueryable<User> GetUsersByLogin(SignIn user);
        public int GetUserIdByLogin(string user);

		Task<bool> IsPasswordCorrect(User user, SignIn model);

        Task<bool> IsLoginExists(string? login);

        Task<bool> UserExists(int userId);

		Task RemoveUser(int id);

		Task AddUserToDb(User user);

        Task<User> CreateAndHashPassword(User user, SignUp Model);

        Task<User> GetUserById(int id);

        Task<int> SaveImagePath(string path);

        Task<string> GetImagePath(int imageId);

        void UpdateUser(User user);

        Task SaveChanges();

        Task<User> GetByIdAsync(int id);
    }
}
