using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MVC_Facebook.Models;
using MVC_Facebook.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MVC_Facebook.Controllers
{

    [Authorize(Roles = "Normal User")]

    public class UserController : Controller
    {
        private readonly IRepository<User, string> repository;
        private readonly IWebHostEnvironment _appEnvironment;

        private readonly IRepository<Friendship, int> friendRepository;
        private readonly UserManager<User> UserManager;


        public UserController(IRepository<User, string> _repository, UserManager<User> UserManager, IRepository<Friendship, int> friendRepository, IWebHostEnvironment appEnvironment)
        {
            this.friendRepository = friendRepository;
            repository = _repository;
            this.UserManager = UserManager;
            this._appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UserInfo()
        {
            string CurrentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = repository.GetByIdWithEagerLoading(CurrentUserID);
            return PartialView(user);
        }
        [HttpGet]
        public IActionResult EditInfo()
        {
            string CurrentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User currentUser = repository.GetByIdWithEagerLoading(CurrentUserID);
            return PartialView(currentUser);
        }
        [HttpPost]
        public JsonResult EditInfo(string ID,string Bio,string Status)//User u)
        {
            //var user = repository.GetByIdWithEagerLoading(u.Id);
            //user.Bio = u.Bio;
            //user.BirthDate = u.BirthDate;
            //user.Status = u.Status;
            var user = repository.GetByIdWithEagerLoading(ID);
            user.Bio = Bio;
            //user.BirthDate = BirthDate;//new System.DateTime(long.Parse(BirthDate));
            user.Status = Status;

            repository.Update(user);
            // return Json("");
            return Json("");//Url.Action("Index", new RouteValueDictionary(
                               // new { controller = "Profile", action = "Index", id = user.Id })));
            //return RedirectToAction("Index", new RouteValueDictionary(
            //                    new { controller = "Profile", action = "Index", id = user.Id }));

        }

        [HttpGet]
        public IActionResult IndexUser()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> getName(string search)
        {           
            List<User> users = repository.GetByName(search);
            string CurrentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User currentUser = users.Find(p => p.Id == CurrentUserID);
            users.Remove(currentUser);
            List<User> normalUsers = new List<User>();
            for(int i=0;i<users.Count;i++)
            {
                if(!await UserManager.IsInRoleAsync(users[i], "Admin"))
                {
                    normalUsers.Add(users[i]);
                }
            }         
            return Json(normalUsers);
        }

        

        public IActionResult profile(string id)
        {
            User user = repository.GetById(id);
            return View(user);
        }



        //public IActionResult AddFriendRequest(string ReceiverID)
        //{
        //    if (ReceiverID == default) return View();

        //    Friendship friendship = new Friendship()
        //    {
        //        SenderID = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
        //        ReceiverID = repository.GetById(ReceiverID).Id,
        //        State = FriendshipState.Pending
        //    };

        //    friendRepository.Add(friendship);
        //    return View("UserProfile", friendship.Receiver);

        //}


        public IActionResult FriendList(string ID)
        {
            var CurrentUserLogin = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = repository.GetByIdWithEagerLoading(CurrentUserLogin);
            var result = new List<User>();
            var ff = user.Friends;
            foreach (var item in ff)
                result.Add(item.Receiver);
            var fo = user.FriendOf;
            foreach (var item in fo)
                result.Add(item.Sender);

            return PartialView(result);
        }

        public IActionResult FriendRequestList()
        {
            var CurrentUserLogin = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var requests = friendRepository.GetAll().Where(p => p.ReceiverID == CurrentUserLogin
                                                           && p.State == FriendshipState.Pending);
            var senderList = requests.Select(p => p.Sender);
            return PartialView("FriendRequestList", senderList.ToList());
        }


        [HttpPost]
        public JsonResult RespondToFriendRequest(string senderID, string friendshipStateResponse)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var requestSender = friendRepository.GetFriendship(senderID, currentUserId);

            switch (friendshipStateResponse)
            {
                case "Accepted":
                    requestSender.State = FriendshipState.Accepted;
                    break;
                case "Removed":
                    requestSender.State = FriendshipState.Removed;
                    break;
                default:
                    break;
            }
            friendRepository.Update(requestSender);

            return Json(requestSender);
            //return View("UserProfile", repository.GetAll().Select(p => p.Friends.Select(q => q.Sender.FullName)));

        }
        [HttpPost]
        public async Task< JsonResult> ChangeProfile()
        {
            IFormFileCollection files = Request.Form.Files;
            string CurrentUserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User currentUser = repository.GetAll().FirstOrDefault(p => p.Id == CurrentUserID);
            string imgExt = Path.GetExtension(files[0].FileName);
            if (imgExt == ".jpg" || imgExt == ".PNG" || imgExt == ".png" || imgExt == ".JPG")
            {
                var newName = $"{CurrentUserID}_{files[0].FileName}";
                var saveImg = Path.Combine(_appEnvironment.WebRootPath, "images", newName);
                var stream = new FileStream(saveImg, FileMode.Create);
                await files[0].CopyToAsync(stream);
               
                currentUser.Picture = newName;
                repository.Update(currentUser);

                return Json(newName);
            }
            return Json("");
        }


    }
}