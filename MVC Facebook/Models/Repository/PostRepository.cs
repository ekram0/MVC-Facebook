using Microsoft.EntityFrameworkCore;
using MVC_Facebook.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Facebook.Models.Repository
{
    public class PostRepository : IRepository<Post, int>
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext db)
        {
            _context = db;
        }
        public void Add(Post Object)
        {
            _context.Posts.Add(Object);
            _context.SaveChanges();
        }

        public Post Delete(int id)
        {
            Post post = _context.Posts.Find(id);
            post.IsDeleted = true;
            _context.Posts.Update(post);
            _context.SaveChanges();
            var commList = _context.Comments.Where(p => p.PostID == id);
            foreach (var com in commList)
            {
                com.IsDeleted = true;
                _context.Comments.Update(com);

            }
            _context.SaveChanges();
            return post;
        }



        public IQueryable<Post> GetAll()
        {
            _context.Posts.Include(c => c.Comments);
            return _context.Posts;
        }

        public Post GetById(int id)
        {
            Post post = _context.Posts.Find(id);

            return post;
        }

        public Post GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public Post GetByIdWithEagerLoading(int id)
        {
            return _context.Posts.Include(p => p.Comments)
                .Include(p => p.Likes)
                .SingleOrDefault(p => p.ID == id);
        }

        public List<Post> GetByName(int name)
        {
            throw new NotImplementedException();
        }

        public Post GetFriendship(string senderId, string receivedID)
        {
            throw new NotImplementedException();
        }

        public bool IsExisted(int id)
        {
            return _context.Posts.Any(p => p.ID == id);
        }

        public bool IsExisted([AllowNull] int? id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(Post Object)
        {
            _context.Entry(Object).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
