using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortal.Models.AccountModels;
using MusicPortal.Models.GenreModels;
using MusicPortal.Repository.GenreRepository;

namespace MusicPortal.Controllers
{
	public class GenresController : Controller
	{
		private readonly IGenreRepository _repository;

		public GenresController(IGenreRepository repository)
		{
			this._repository = repository;
		}
		public async Task<IActionResult> Create(GenreModel model)
		{
			await _repository.AddGenre(model.NewGenre);
			return RedirectToAction("Index", "Genres");
		}

		public async Task<IActionResult> Edit(int id)
		{
			Genre genre = await _repository.GetGenreById(id);
			if (genre == null)
			{
				return NotFound();
			}

			return View(genre);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Genre genre)
		{
			_repository.UpdateGenre(genre);

			return RedirectToAction("Index");

		}

		public async Task<IActionResult> Delete(int id)
		{
			await _repository.DeleteGenre(id);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Index()
		{
			GenreModel model = new GenreModel();
			model.Genres = await _repository.GetGenres();

			return View(model);
		}
	}
}
