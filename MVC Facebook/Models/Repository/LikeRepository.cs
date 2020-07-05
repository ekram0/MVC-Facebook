using Microsoft.EntityFrameworkCore;
using MVC_Facebook.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Facebook.Models.Repository
{
    public class LikeRepository : IRepository<Like, int>
    {
        readonly ApplicationDbContext _context;
        public LikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Like like)
        {
            var obj = _context.Likes.FirstOrDefault(l => l.LikeOwnerID == like.LikeOwnerID && l.PostID == like.PostID);
            if (obj == null)
            {
                _context.Likes.Add(like);
                _context.SaveChanges();
            }
            else
            {
                obj.IsDeleted = like.IsDeleted;
                Update(obj);
            }


        }

        public Like Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Like> GetAll()
        {
            return _context.Likes;
        }

        public Like GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Like GetByIdWithEagerLoading(int id)
        {
            throw new NotImplementedException();
        }

        public List<Like> GetByName(int name)
        {
            throw new NotImplementedException();
        }

        public Like GetFriendship(string senderId, string receivedID)
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

        public void Update(Like Object)
        {
            //var like = _context.Likes.FirstOrDefault(l => l.LikeOwnerID == Object.LikeOwnerID && l.PostID == Object.PostID);
            //like.IsDeleted = Object.IsDeleted;
            _context.Entry(Object).State = EntityState.Modified;
            _context.SaveChanges();
        }
        //public IEnumerable<Like> SearchByPostID(int pID)
        //{
        //    return _context.Likes.Where(l => l.PostID == pID);
        //}
    }
}
