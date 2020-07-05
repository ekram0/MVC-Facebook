using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Facebook.Data;
using MVC_Facebook.Models;
using MVC_Facebook.Models.Repository;

namespace MVC_Facebook.Controllers
{
    [Authorize (Roles ="Normal User")]
    public class PostsController : Controller
    {
        private readonly IRepository<Post, int> _postRepository;
        private readonly IRepository<User, string> _userRepository;
        private readonly IRepository<Like, int> _likeRepository;
        private readonly UserManager<User> _userManager;

        public PostsController(IRepository<Post, int> postRepository, UserManager<User> userManager, IRepository<User, string> userRepository, IRepository<Like, int> likeRepository)
        {
            _postRepository = postRepository;
            _userManager = userManager;
            _userRepository = userRepository;
            _likeRepository = likeRepository;
        }
        [HttpGet]
        public IActionResult ProfilePosts(string id)
        {
            var user = _userRepository.GetByIdWithEagerLoading(id);
            var ListOfPosts = user.Posts.OrderByDescending(p => p.TimeStamp).ToList();
            return PartialView("PostsList", ListOfPosts);
            
        }
        
        // GET: Posts
        public IActionResult NewsFeed()
        {
            var userId = _userManager.GetUserId(User);
            var user = _userRepository.GetByIdWithEagerLoading(userId);
            var result = user.Posts.ToList();

            foreach (var item in user.Friends)
            {
                var tempPost = item.Receiver.Posts.Where(p => p.IsDeleted == false).ToList();
                if (!(tempPost is null))
                    result.AddRange(tempPost);
            }
            foreach (var item in user.FriendOf)
            {
                var tempPost = item.Sender.Posts.Where(p => p.IsDeleted == false).ToList();
                if (!(tempPost is null))
                    result.AddRange(tempPost);
            }
            var r = result.OrderByDescending(p => p.TimeStamp);
            return PartialView("PostsList", r);
        }

        [HttpPost]
        public IActionResult PostLike(int PostID, string LikeOwnerID, bool isDeleted)//[Bind("PostID,LikeOwnerID,IsDeleted")]Like like)
        {
            // if((LikeRepository)_likeRepository.IsExisted)
            _likeRepository.Add(new Like() { PostID = PostID, LikeOwnerID = LikeOwnerID, IsDeleted = isDeleted });
            return RedirectToAction("Index", "Home");
        }
        //GET: /Posts/ShowLikeOwners/PostID
        [HttpGet]
        public IActionResult ShowLikeOwners(int PostID)
        {
            //Step1: get all likes list by postID
            var likesList = _likeRepository.GetAll().Where(l => l.PostID == PostID && l.IsDeleted == false);
            //Step2: get all users by likeOwnerID in each item in like list derived from prev step
            var likeOwners = from User in _userRepository.GetAll()
                             from Like in likesList
                             where User.Id == Like.LikeOwnerID
                             select User;
            foreach (var item in likeOwners)
            {
                Debug.WriteLine(item.FullName);
            }
            //try getting user's(SignedInUser) friends
            var currentUserID = _userManager.GetUserId(User);
            Debug.WriteLine(currentUserID);
            var user = _userRepository.GetByIdWithEagerLoading(currentUserID);
            ViewData["User"] = user;
            var allFriends = user.Friends.Select(f => f.Receiver).Union(user.FriendOf.Select(f => f.Sender));
            //var result = user.Friends.Where(f=>f.State!=FriendshipState.Pending).Select(f => f.Receiver).Union(user.FriendOf.Where(f => f.State != FriendshipState.Pending).Select(f => f.Sender));
            //var pendingFriends = user.Friends.Where(f => f.State == FriendshipState.Pending).Select(f => f.Receiver).Union(user.FriendOf.Where(f => f.State == FriendshipState.Pending).Select(f => f.Sender));
            //var friendshipState = user.Friends.Select(f => f.State).Union(user.FriendOf.Select(f => f.State));
            //var result = user.Friends.Select(f => new { User = f.Receiver, State = f.State }).Union(user.FriendOf.Select(f => new { User = f.Sender, State = f.State }));

            //var userFriends = user.Friends.ToList();//frndz who received my request
            //var userFriendsOf = user.FriendOf.ToList();//frndz who sended me a request
            Debug.WriteLine(((User)ViewBag.User).FullName);

            //var friends = user.Friends.Union(user.FriendOf);

            //var userFriendsInfo = from Users in _userRepository.GetAll()
            //                  from friend in userFriends
            //                //  from friendOf in user.FriendOf
            //                  where Users.Id == friend.ReceiverID
            //                 // && Users.Id == friendOf.SenderID
            //                  && friend.State != FriendshipState.Removed
            //                 // && friendOf.State != FriendshipState.Removed
            //                  select Users;
            //By Comparing likeOwners with friendsInfo if not found display addfriend button          
            foreach (var item in allFriends)
            {
                Debug.WriteLine(item.FullName);
                
            }
           
           ViewData["AllFriends"] = allFriends;
           

            //Step3:return user model to the partial view
            return PartialView(likeOwners.ToList());
        }
        // GET: Posts
        public IActionResult Index()
        {
            //var applicationDbContext = _context.Posts.Include(p => p.PostOwner);
            return View(_postRepository.GetAll().ToList());
        }

        public IActionResult Home()
        {
            return View();
        }

        // GET: Posts/Details/5
        public IActionResult Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var post =  _context.Posts
            //    .Include(p => p.PostOwner)
            //    .FirstOrDefault(m => m.ID == id);
            Post post = _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            //ViewData["PostOwnerID"] = new SelectList(_context.Users, "Id", "Id");
            return PartialView();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Create(/*[Bind("ID,Body,IsDeleted,PostOwnerID")] Post post*/string Body)
        {
            Post post = new Post();
            post.PostOwnerID = _userManager.GetUserId(User);
            post.Body = Body;
            _postRepository.Add(post);
            return Json("");
        }

        // GET: Posts/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }
            /*ViewData["PostOwnerID"] = new SelectList(_context.Users, "Id", "Id", post.PostOwnerID);*/
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,Body,IsDeleted,PostOwnerID")] Post post)
        {
            if (id != post.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _postRepository.Update(post);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_postRepository.IsExisted(post.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            /* ViewData["PostOwnerID"] = new SelectList(_context.Users, "Id", "Id", post.PostOwnerID);*/
            return View(post);
        }

        // GET: Posts/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*  var post = _context.Posts
                  .Include(p => p.PostOwner)
                  .FirstOrDefault(m => m.ID == id);*/
            var post = _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }
            _postRepository.Delete(id);
            return Ok();
        }

        //// POST: Posts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    /*var post = _context.Posts.Find(id);
        //    _context.Posts.Remove(post);
        //    _context.SaveChanges();*/
        //    var post = _postRepository.Delete(id);
        //    return RedirectToAction(nameof(Index));
        //}


    }
}
