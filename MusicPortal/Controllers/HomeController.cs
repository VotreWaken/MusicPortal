using Microsoft.AspNetCore.Mvc;
using MusicPortal.Models;
using MusicPortal.Models.GenreModels;
using MusicPortal.Models.HomeModels;
using MusicPortal.Repository.GenreRepository;
using MusicPortal.Repository.HomeRepository;
using System.Diagnostics;

namespace MusicPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenreRepository _repository;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IGenreRepository repository, IHomeRepository homeRepository)
        {
            _logger = logger;
            this._repository = repository;
            this._homeRepository = homeRepository;
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "Account");
        }


        //public async Task<IActionResult> Index()
        //{
        //    var songs = await _repository.GetSongs();
        //    return View(songs);
        //}
        public async Task<IActionResult> Index()
        {
            var genres = await _repository.GetGenres();
            var genreSongs = new List<HomeAudioGenreModel>();

            foreach (var genre in genres)
            {
                var songs = await _homeRepository.GetSongsByGenreAsync(genre.GenreName);

                var imagePaths = await _homeRepository.GetImagePathsForSongsAsync(songs.Select(song => song.ImageId));

                var genreSongModel = new HomeAudioGenreModel
                {
                    Genre = genre,
                    Songs = songs,
                    ImagePaths = imagePaths
                };
                genreSongs.Add(genreSongModel);
            }

            var viewModel = new List<HomeAudioGenreModel>(genreSongs);
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
