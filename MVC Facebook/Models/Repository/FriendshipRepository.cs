using Microsoft.EntityFrameworkCore;
using MVC_Facebook.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Facebook.Models.Repository
{
    public class FriendshipRepository : IRepository<Friendship, int>
    {
        readonly ApplicationDbContext _context;

        public FriendshipRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Friendship friendship)
        {
            _context.Friendships.Add(friendship);
            _context.SaveChanges();

        }

        public Friendship Delete(int id)
        {
            var deletedFriend = _context.Friendships.Find(id);
            _context.Friendships.Remove(deletedFriend);
            _context.SaveChanges();
            return deletedFriend;
        }

        public IQueryable<Friendship> GetAll()
            => _context.Friendships;

        public Friendship GetById(int id)
            => _context.Friendships.Find(id);

        public Friendship GetByIdWithEagerLoading(int id)
        {
            return _context.Friendships.Include(p => p.Receiver).ThenInclude(o => new { o.FullName, o.Id })
                                       .Include(i => i.Sender).ThenInclude(u => new { u.FullName, u.Id })
                                       .FirstOrDefault(y => y.FriendshipId == id);
        }

        public List<Friendship> GetByName(int id)
            => _context.Friendships.Where(p => p.FriendshipId == id).Select(o => o).ToList();

        public bool IsExisted([AllowNull] int id)
            => id == default ? false : true;

        public void SaveChanges()
            => _context.SaveChanges();


        public void Update(Friendship Object)
        {
            _context.Entry<Friendship>(Object).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Friendship GetFriendship(string user1, string user2)
            => _context.Friendships.FirstOrDefault(p => (p.ReceiverID == user2 && p.SenderID == user1)
                                                    || (p.SenderID == user2 && p.ReceiverID == user1));


    }
}

