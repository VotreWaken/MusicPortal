using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;
using MusicPortal.Models.GenreModels;
using System;

namespace MusicPortal.Repository.GenreRepository
{
	public class GenreRepository : IGenreRepository
	{
		private readonly UserContext _context;

        public GenreRepository(UserContext context) => _context = context;
        public async Task<List<Genre>> GetGenres()
		{
			return await _context.Genre.ToListAsync();
		}

		public async Task<Genre> GetGenreById(int id)
		{
			return await _context.Genre.FindAsync(id);
		}

		public async Task AddGenre(Genre genre)
		{
			await _context.Genre.AddAsync(genre);
			await _context.SaveChangesAsync();
		}

		public async Task AddGenreAndSongRelation(AudioGenre genre)
		{
			await _context.AudioGenre.AddAsync(genre);
			await _context.SaveChangesAsync();
		}

		public void UpdateGenre(Genre genre)
		{
			_context.Entry(genre).State = EntityState.Modified;
			_context.SaveChanges();
		}

		public async Task DeleteGenre(int id)
		{
			Genre? g = await _context.Genre.FindAsync(id);
			if (g != null)
			{
				_context.Genre.Remove(g);
				await _context.SaveChangesAsync();
			}
		}
	}
}
