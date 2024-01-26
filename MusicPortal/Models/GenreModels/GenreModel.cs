using System;

namespace MusicPortal.Models.GenreModels
{
    public class GenreModel
    {
        public ICollection<Genre>? Genres { get; set; }

        public Genre NewGenre { get; set; }
    }
}
