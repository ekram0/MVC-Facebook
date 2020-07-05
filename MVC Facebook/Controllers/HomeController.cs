using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_Facebook.Models;
using MVC_Facebook.Models.Repository;

namespace MVC_Facebook.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Post, int> _postRepository;
        private readonly IRepository<User, string> _userRepository;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger
            , IRepository<Post, int> postRepository
            , IRepository<User, string> userRepository
            , UserManager<User> userManager
            )
        {
            _logger = logger;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {

            var userId = _userManager.GetUserId(User);
            var user = _userRepository.GetById(userId);
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
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
