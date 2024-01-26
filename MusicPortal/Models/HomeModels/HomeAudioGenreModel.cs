using MusicPortal.Models.GenreModels;
using MusicPortal.Models.SongsModels;

namespace MusicPortal.Models.HomeModels
{
    public class HomeAudioGenreModel
    {
        public Genre Genre { get; set; }
        public IEnumerable<AudioPath> Songs { get; set; }
    }
}
