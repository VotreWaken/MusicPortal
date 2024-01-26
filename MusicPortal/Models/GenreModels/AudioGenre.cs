namespace MusicPortal.Models.GenreModels
{
	public class AudioGenre
	{
        public int Id { get; set; }
        public int AudioId { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
