using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Facebook.Data;
using MVC_Facebook.Models;
using MVC_Facebook.Models.Enums;
using MVC_Facebook.Models.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace MVC_Facebook.Models.Repository
{
    public class RoleRepository : IRepository<Role, string>
    {

        readonly ApplicationDbContext context;

        public RoleRepository(ApplicationDbContext c ) => context = c;

        public void Add(Role role)
        {
            context.Roles.Add(role);
            context.SaveChanges();
        }

        public Role Delete(string id)
        {
            var DeletedUser = context.Roles.Find(id);
            context.Roles.Remove(DeletedUser);
            context.SaveChanges();
            return DeletedUser;
        }

        public IQueryable<Role> GetAll()
        {
            return context.Roles;
        }

        public Role GetById(string id)
            => context.Roles.Find(id);


        public Role GetByIdWithEagerLoading(string id)
        {
            throw new NotImplementedException();
        }

        public List<Role> GetByName(string name)
        => context.Roles.Where(p => p.Name == name).ToList();

        public Role GetFriendship(string senderId, string receivedID)
        {
            throw new NotImplementedException();
        }

        public bool IsExisted([AllowNull] string id)
            => context.Roles.Any(p => p.Id == id);

        

        public void SaveChanges() => context.SaveChanges();
        
        public void Update(Role Object)
        {
            context.Entry(Object).State = EntityState.Modified;
            context.SaveChanges();

        }

    }
}