using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;
using MusicPortal.Models.GenreModels;
using MusicPortal.Models.SongsModels;

namespace MusicPortal.Repository.HomeRepository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly UserContext _context;

        public HomeRepository(UserContext context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<AudioGenre>> GetAudioGenresAsync()
        {
            return await _context.AudioGenre.ToListAsync();
        }

        public async Task<IEnumerable<AudioPath>> GetSongsByGenreAsync(string genreName)
        {
            return await _context.AudioGenre
                .Where(ag => ag.Genre.GenreName == genreName)
                .Select(ag => ag.Audio)
                .ToListAsync();
        }
    }
}
