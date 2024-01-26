using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;
using MusicPortal.Models.AccountModels;
using MusicPortal.Models.GenreModels;
using MusicPortal.Models.SongsModels;

namespace MusicPortal.Repository.SongsRepository
{
	public class SongRepository : ISongRepository
	{
		private readonly UserContext _context;
		//public async Task<List<Song>> GetSongs()
		//{
		//    return await _context.AudioInformation.ToListAsync();
		//}

		public SongRepository(UserContext context)
		{
			this._context = context;
		}

		public async Task AddSong(AudioPath song)
		{
			await _context.AudioPath.AddAsync(song);
			_context.SaveChanges();
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

		public async Task<int> SaveImagePath(string path)
		{
			var imagePath = new ImagePath { Path = path };
			await _context.ImagePath.AddAsync(imagePath);
			await _context.SaveChangesAsync();

			return imagePath.Id;
		}
	}
}
