using MusicPortal.Models.GenreModels;
using MusicPortal.Models.SongsModels;

namespace MusicPortal.Repository.SongsRepository
{
    public interface ISongRepository
    {
        // Task<List<AudioInformation>> GetSongs();
        // Task<AudioInformation> GetSongById(int id);
        Task AddSong(AudioPath song);
		Task<int> SaveImagePath(string path);
		Task<string> GetImagePath(int imageId);
		// void UpdateSong(AudioInformation song);
		// Task DeleteSong(int id);
	}
}
