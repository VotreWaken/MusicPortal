using Microsoft.AspNetCore.Mvc;
using MusicPortal.Models;
using MusicPortal.Repository.GenreRepository;
using System.Diagnostics;

namespace MusicPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            // this._repository = repository;
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
            return View();
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
