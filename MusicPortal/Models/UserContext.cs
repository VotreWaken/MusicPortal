using Microsoft.EntityFrameworkCore;
using MusicPortal.Models.AccountModels;
using MusicPortal.Models.GenreModels;
using MusicPortal.Models.Hashing;
using MusicPortal.Models.SongsModels;

namespace MusicPortal.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> User { get; set; }
		public DbSet<Genre> Genre { get; set; }
		public DbSet<ImagePath> ImagePath { get; set; }
		public DbSet<AudioPath> AudioPath { get; set; }
		public DbSet<AudioGenre> AudioGenre { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<AudioGenre>()
	            .HasKey(ag => new { ag.AudioId, ag.GenreId });

			base.OnModelCreating(modelBuilder);
        }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            if (Database.EnsureCreated())
            {
                User user = new User()
                {
                    Login = "admin",
                    Password = "admin",
                    Email = "admin@gmail.com",
                    IsAdmin = true
                };
                Sha256 sha256 = new Sha256();
                sha256.ComputeSalt();
                user.Password = sha256.ComputeHash(sha256.ComputeSalt(), user.Password);

                //Genre newGenre = new Genre
                //{
                //    GenreName = "Pop"
                //};

                // Добавляем жанр в DbSet и сохраняем изменения в базе данных
                // _context.Genres.Add(newGenre);
                // await _context.SaveChangesAsync();
            }
        }
    }
}
