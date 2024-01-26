using System;
using MusicPortal.Models.GenreModels;
namespace MusicPortal.Repository.GenreRepository
{
	public interface IGenreRepository
	{
		Task<List<Genre>> GetGenres();
		Task<Genre> GetGenreById(int id);
		Task AddGenre(Genre genre);
		Task AddGenreAndSongRelation(AudioGenre genre);
		void UpdateGenre(Genre genre);
		Task DeleteGenre(int id);
	}
}
