using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Facebook.Models.Repository
{
    public interface IRepository<T, Tkey>
    {
        IQueryable<T> GetAll();

        T GetById(Tkey id);
        void Add(T Object);
        List<T> GetByName(Tkey name);
        void Update(T Object);
        T Delete(Tkey id);
        bool IsExisted([AllowNull]Tkey id);
        void SaveChanges();

        T GetByIdWithEagerLoading(Tkey id);

        T GetFriendship(string senderId, string receivedID);

        //public IEnumerable<Like> SearchByPostID(int pID);


    }

}
