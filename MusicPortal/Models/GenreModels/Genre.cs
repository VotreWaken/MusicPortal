namespace MusicPortal.Models.GenreModels
{
	public class Genre
	{
		public int Id { get; set; }
		public string GenreName { get; set; }
		public ICollection<AudioGenre> AudioGenres { get; set; }
	}
}
