using MusicPortal.Models.GenreModels;
using MusicPortal.Models.SongsModels;

namespace MusicPortal.Repository.HomeRepository
{
    public interface IHomeRepository
    {
        Task<IEnumerable<AudioPath>> GetSongsByGenreAsync(string genreName);

        Task<IEnumerable<string>> GetImagePathsForSongsAsync(IEnumerable<int> imageIds);
    }
}
