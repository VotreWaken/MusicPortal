using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusicPortal.Models.AccountModels;
using MusicPortal.Repository.AccountRepository;
using System.IO;

namespace MusicPortal.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAccountsRepository _repository;
        public AccountController(IAccountsRepository repository, IWebHostEnvironment appEnvironment)
        {
            _repository = repository;
            _appEnvironment = appEnvironment;
        }

        // 
        [HttpGet]
        public ActionResult SignUp() => View();


		private readonly IWebHostEnvironment _appEnvironment;
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SignUp(SignUp Model, IFormFile ImageAvatar)
		{
			if (!ModelState.IsValid)
				return View(Model);

			if (await _repository.IsLoginExists(Model.Login))
			{
				ModelState.AddModelError("", "This login is taken!");
				return View(Model);
			}

			// Сохранение файла на сервере
			string path = "/images/" + ImageAvatar.FileName;
			using (FileStream filestream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
			{
				await ImageAvatar.CopyToAsync(filestream);
			}

			// Сохранение пути к файлу в таблице ImagePath
			var imageId = await _repository.SaveImagePath(path);

			// Создание пользователя с ImageId
			var user = new User
			{
				Login = Model.Login,
				Email = Model.Email,
				Password = Model.Password,
				ImageId = imageId // Присваиваем Id изображения пользователю
			};

			// Хеширование пароля и другие действия

			user = await _repository.CreateAndHashPassword(user, Model);

			await _repository.AddUserToDb(user);

			return RedirectToAction("SignIn");
		}

		// 
		[HttpGet]
        public ActionResult SignIn() => View();

        // GET: AccountController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignIn Model)
        {
            if ((await _repository.GetAllUsers()).Count == 0)
                return RedirectToAction("SignUp", "Account");
            if (!ModelState.IsValid)
                return View(Model);

            IQueryable<User> users = _repository.GetUsersByLogin(Model);

            if (!users.Any())
            {
                ModelState.AddModelError("", "Incorrect login or password!");
                return View(Model);
            }

            User user = users.First();

            if (await _repository.IsPasswordCorrect(user, Model))
            {
                ModelState.AddModelError("", "Incorrect login or password!");
                return View(Model);
            }
            HttpContext.Session.SetString("Login", user.Login);

            string imagePath = await _repository.GetImagePath(user.ImageId);

            // ViewBag.UserImagePath = imagePath;
            HttpContext.Session.SetString("UserImage", imagePath);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ConfirmUsers()
        {
            var model = new ConfirmUsers();
            model.Users = await _repository.GetAllUsers();

            model.ImagePaths = new Dictionary<int, string>();
            foreach (var user in model.Users)
            {
                if (user.ImageId > 0)
                {
                    string imagePath = await _repository.GetImagePath(user.ImageId);
                    model.ImagePaths.Add(user.Id, imagePath);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> AuthUser(int id)
        {
            var model = new ConfirmUsers();
            model.Users = await _repository.GetAllUsers();
            model.User = await _repository.GetUserById(id);
            model.User.IsAuth = true;
            _repository.UpdateUser(model.User);
            await _repository.SaveChanges();
            model.Id = id;

            model.ImagePaths = new Dictionary<int, string>();
            foreach (var user in model.Users)
            {
                if (user.ImageId > 0)
                {
                    string imagePath = await _repository.GetImagePath(user.ImageId);
                    model.ImagePaths.Add(user.Id, imagePath);
                }
            }

            return View("~/Views/Account/ConfirmUsers.cshtml", model);
        }

        // GET: AccountController
        public async Task<IActionResult> Index(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _repository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Обновление пользователя в базе данных
                    _repository.UpdateUser(user);
                    await _repository.SaveChanges();

                    return RedirectToAction("ConfirmUsers");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _repository.UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(user);
        }

        // GET: AccountController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
			await _repository.RemoveUser(id);
            await _repository.SaveChanges();
			return RedirectToAction("ConfirmUsers");
		}

	}
}
