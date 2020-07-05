using MVC_Facebook.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace MVC_Facebook.Models.Repository
{
    public class UserRepository : IRepository<User, string>
    {
        readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(User Object)
        {
            _context.Users.Add(Object);
            _context.SaveChanges();

        }

        public List<Post> GetMyPosts(string postOwnerId)
        {
            var user = GetById(postOwnerId);
            var result = user.Posts.ToList();
            return result;
        }

        public User Delete(string id)
        {
            throw new NotImplementedException();
        }
        public IQueryable<User> GetAll()
        {
            return _context.Users;
        }


        public User GetById(string id)
        {
            if (id != null)
                return _context.Users.Find(id);
            return new User();
        }

        public User GetByIdWithEagerLoading(string id)
        {
            if (id != null)
            {
                var user = _context.Users.Find(id);

                //.Include(p => p.Posts).SingleOrDefault(p => p.Id == id);
                _context.Entry(user)
                        .Collection(u => u.Posts)
                        .Query()
                        .Where(p => p.IsDeleted == false)
                        .Load();
                foreach (var post in user.Posts)
                {
                    _context.Entry(post)
                            .Collection(p => p.Comments)
                            .Query()
                            .Where(c => c.IsDeleted == false).Include(c => c.CommentOwner)
                            .Load();
                    _context.Entry(post)
                            .Collection(p => p.Likes)
                            .Query()
                            .Where(l => l.IsDeleted == false).Include(l => l.LikeOwner)
                            .Load();
                }
                var receiver = _context.Entry(user)
                        .Collection(u => u.Friends)
                        .Query()
                        .Where(f => f.State == FriendshipState.Accepted)
                        .Include(f => f.Receiver)
                        .ThenInclude(r => r.Posts);
                receiver.Load();
                foreach (var friendship in receiver)
                {
                    foreach (var post in friendship.Receiver.Posts)
                    {
                        _context.Entry(post)
                                .Collection(p => p.Comments)
                                .Query()
                                .Where(c => c.IsDeleted == false).Include(c => c.CommentOwner)
                                .Load();
                        _context.Entry(post)
                                .Collection(p => p.Likes)
                                .Query()
                                .Where(l => l.IsDeleted == false).Include(l => l.LikeOwner)
                                .Load();
                    }
                }
                var sender = _context.Entry(user)
                        .Collection(u => u.FriendOf)
                        .Query()
                        .Where(f => f.State == FriendshipState.Accepted)
                        .Include(f => f.Sender)
                        .ThenInclude(r => r.Posts);
                sender.Load();
                foreach (var friendship in sender)
                {
                    foreach (var post in friendship.Sender.Posts)
                    {
                        _context.Entry(post)
                                .Collection(p => p.Comments)
                                .Query()
                                .Where(c => c.IsDeleted == false).Include(c => c.CommentOwner)
                                .Load();
                        _context.Entry(post)
                                .Collection(p => p.Likes)
                                .Query()
                                .Where(l => l.IsDeleted == false).Include(l => l.LikeOwner)
                                .Load();
                    }
                }
                return user;
            }
            return new User();
            //return _context.Users.Include(p => p.Posts)
            //                     .Include(u => u.Friends)
            //                        .ThenInclude(f => f.Receiver)
            //                        .ThenInclude(r => r.Posts)
            //                     .Include(u => u.FriendOf)
            //                        .ThenInclude(f => f.Sender)
            //                        .ThenInclude(s => s.Posts)
            //                     .SingleOrDefault(p => p.Id == id)
            //                     ;
        }

        public List<User> GetByName(string name)
        {
            string [] fullName=name.Split(' ');
            List<User> users1;
            if (fullName.Length > 1)
            {
                if (fullName[1] != "")
                {
                   users1 = _context.Users.Where(u => u.FullName.FirstName.Equals(fullName[0]) && u.FullName.LastName.Equals(fullName[1])).ToList();
                    return users1;
                }
            }
            List<User> users = _context.Users.Where(u => u.FullName.FirstName.StartsWith(name)|| u.FullName.LastName.StartsWith(name)).ToList();
            return users;
        }

        public bool IsExisted([AllowNull] int id)
        {
            throw new NotImplementedException();
        }

        public bool IsExisted([AllowNull] string id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(User Object)
        {
            _context.Entry(Object).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public User GetFriendship(string senderId, string receivedID)
        {
            throw new NotImplementedException();
        }

        //public IEnumerable<Like> SearchByPostID(int pID)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
