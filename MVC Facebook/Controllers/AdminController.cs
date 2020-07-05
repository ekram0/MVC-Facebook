using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Facebook.Models;
using MVC_Facebook.Models.Repository;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC_Facebook.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        readonly IRepository<Role, string> RoleRepository;
        private readonly IRepository<User, string> UserRepository;
        private readonly UserManager<User> UserManager;

        public AdminController(IRepository<Role, string> RoleRepos, IRepository<User, string> UserRepository, UserManager<User> UserManager)
            => (RoleRepository, this.UserRepository, this.UserManager) = (RoleRepos, UserRepository, UserManager);

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddNewRole()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewRole(Role role)
        {
            if (ModelState.IsValid && role.Name != null && role.Description != null)
            {
                var Roles = RoleRepository.GetAll().ToList();
                bool ISduplicated = Roles.Any(r => r.Name == role.Name);
                ViewBag.duplicate = "duplicated";

                if (!ISduplicated)
                {
                    ViewBag.duplicate = "Not";

                    Role role1 = new Role();
                    role1.Name = role.Name;
                    role1.Description = role.Description;
                    role1.NormalizedName = role.Name.ToUpper();
                    RoleRepository.Add(role1);
                    return View("Index");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> EditRole(string Id, string RoleName)
        {
            var user = UserRepository.GetById(Id);
            var UserRoles = await UserManager.GetRolesAsync(user);
            await UserManager.RemoveFromRolesAsync(user, UserRoles);
            await UserManager.AddToRoleAsync(user, RoleName);
            return Json("");
        }

        public JsonResult GetUserBySearch(string SearchName)
            => Json(UserRepository.GetByName(SearchName).FirstOrDefault());

        public JsonResult GetAllUser()
            => Json(UserRepository.GetAll());


        public IActionResult GetAllBySearchName(string FullName)
        {
            var CurrentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (FullName != default)
            {
                var allUsers = UserRepository.GetByName(FullName).ToList();
                if (allUsers != default)
                {
                    allUsers.Remove(allUsers.Find(p => p.Id == CurrentUserID));
                    return PartialView(allUsers);
                }
                else
                    return PartialView();
            }
            else
                return PartialView(UserRepository.GetAll().ToList());
        }

        [HttpPost]
        public IActionResult ToggleBlockUser(string id)
        {
            var user = UserRepository.GetById(id);
            user.IsBlocked = !user.IsBlocked;
            UserRepository.Update(user);
            return Ok();
        }

    }
}
