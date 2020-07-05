using MVC_Facebook.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Facebook.Models.Repository
{
    public class CommentRepository : IRepository<Comment, int>
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Comment Object)
        {
            _context.Comments.Add(Object);
            _context.SaveChanges();
        }

        public Comment Delete(int id)
        {
            Comment comment = _context.Comments.Find(id);
            comment.IsDeleted = true;
            _context.Comments.Update(comment);
            _context.SaveChanges();
            return comment;
        }


        public IQueryable<Comment> GetAll()
        {
            return _context.Comments;
        }

        public Comment GetById(int id)
        {
            Comment comment = _context.Comments.Find(id);
            return comment;
        }

        public Comment GetByIdWithEagerLoading(int id)
        {
            throw new NotImplementedException();
        }

        public List<Comment> GetByName(int name)
        {
            throw new NotImplementedException();
        }

        public Comment GetFriendship(string senderId, string receivedID)
        {
            throw new NotImplementedException();
        }

        public bool IsExisted([AllowNull] int id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(Comment Object)
        {
            throw new NotImplementedException();
        }
    }
}
