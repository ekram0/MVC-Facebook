using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Facebook.Data;
using MVC_Facebook.Models;
using System.Security.Claims;
using MVC_Facebook.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;

namespace MVC_Facebook.Controllers
{

    [Authorize(Roles = "Normal User")]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly IRepository<User, string> userRepository;
        private readonly IRepository<Friendship, int> friendRepository;
        private readonly ApplicationDbContext context;
        public ProfileController(IRepository<User, string> repository, IRepository<Friendship, int> friendRepos, ApplicationDbContext c)
        {
            userRepository = repository;
            friendRepository = friendRepos;
            context = c;
        }

        [Route("{id}")]
        public IActionResult Index(string id)
        {

            var user = userRepository.GetByIdWithEagerLoading(id);
            var LoggedInUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var LoggedInUser = userRepository.GetByIdWithEagerLoading(LoggedInUserId);
            ViewBag.CurrentUser = LoggedInUserId;
            ///
            //Trying to not use context below!
            ///
            if (user.Id != LoggedInUserId)
            {
                var SenderLoggedIn = context.Friendships.SingleOrDefault(f =>
                                                    (f.ReceiverID == user.Id && f.SenderID == LoggedInUserId));
                var ReciverLoggedIn = context.Friendships.SingleOrDefault(f =>
                                                    (f.SenderID == user.Id && f.ReceiverID == LoggedInUserId));

                if (SenderLoggedIn != null)//pending friends
                {
                    var enumValueString = Enum.GetName(typeof(FriendshipState), SenderLoggedIn.State);
                    ViewBag.Reciver = SenderLoggedIn.ReceiverID;
                    ViewBag.friendstate = enumValueString;
                }
                else if (ReciverLoggedIn != null)//confirm delete
                {
                    var enumValueString = Enum.GetName(typeof(FriendshipState), ReciverLoggedIn.State);
                    ViewBag.Sender = ReciverLoggedIn.SenderID;
                    ViewBag.friendstate = enumValueString;
                }
                else
                {
                    ViewBag.friendstate = "Removed";//msh friends
                    ViewBag.u = user.Id;
                }
            }

            return View(user);
        }



        public IActionResult AddFriend(string id)
        {
            var CurrentUser = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Recvier = userRepository.GetById(id);
            var friendship = friendRepository.GetFriendship(CurrentUser, id);
            if (friendship != null)
            {
                friendRepository.Delete(friendship.FriendshipId);
            }
            friendship = new Friendship();
            friendship.SenderID = CurrentUser;
            friendship.ReceiverID = Recvier.Id;
            friendship.State = FriendshipState.Pending;
            friendRepository.Add(friendship);

            return RedirectToAction("Index", "Profile", new { id = Recvier.Id });
        }


    }
}