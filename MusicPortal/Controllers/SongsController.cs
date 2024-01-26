using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicPortal.Models.GenreModels;
using MusicPortal.Models.SongsModels;
using MusicPortal.Repository.AccountRepository;
using MusicPortal.Repository.GenreRepository;
using MusicPortal.Repository.SongsRepository;

namespace MusicPortal.Controllers
{
	public class SongsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		private readonly ISongRepository _repository;

		private readonly IGenreRepository _genreRepository;

		private readonly IAccountsRepository _accountRepository;

		private readonly IWebHostEnvironment _appEnvironment;
		public SongsController(ISongRepository repository,IAccountsRepository accountsRepository , IGenreRepository Genrerepository, IWebHostEnvironment appEnvironment)
		{
			_repository = repository;
			_genreRepository = Genrerepository;
			_accountRepository = accountsRepository;
			_appEnvironment = appEnvironment;
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var genres = await _genreRepository.GetGenres();

			if (genres == null)
			{
				return NotFound();
			}

			ViewBag.AllGenres = genres
				.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.GenreName })
				.ToList();

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(AudioCreation model, IFormFile Path, IFormFile ImagePath)
		{
			var genres = await _genreRepository.GetGenres();
			ViewBag.AllGenres = genres
				.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.GenreName })
				.ToList();

			if (Path != null)
			{
				string audioPath = "/audio/" + Path.FileName;
				using (FileStream filestream = new FileStream(_appEnvironment.WebRootPath + audioPath, FileMode.Create))
				{
					await Path.CopyToAsync(filestream);
				}

				model.Path = audioPath;
			}

			string path = "/images/" + ImagePath.FileName;
			using (FileStream filestream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
			{
				await ImagePath.CopyToAsync(filestream);
			}

			var imageId = await _repository.SaveImagePath(path);

			var song = new AudioPath
			{
				Name = model.Name,
				UserId = _accountRepository.GetUserIdByLogin(model.User.Login),
				ImageId = imageId,
				Path = model.Path
			};

			await _repository.AddSong(song);



			if (model.SelectedGenres != null && model.SelectedGenres.Any())
			{
				foreach (var genreId in model.SelectedGenres)
				{
					var audioGenre = new AudioGenre
					{
						AudioId = song.Id,
						GenreId = genreId
					};
					await _genreRepository.AddGenreAndSongRelation(audioGenre);
				}
			}

			return RedirectToAction("Create");
		}
	}
}
