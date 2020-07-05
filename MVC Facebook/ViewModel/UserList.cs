using MVC_Facebook.Data;
using MVC_Facebook.Models;
using MVC_Facebook.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Facebook.ViewModel
{
    public class UserList 
    {
        private readonly ApplicationDbContext applicationDb;
        private readonly RoleRepository roleRepository;


        public string SearchTerm { get; set; }
        public IEnumerable<User> Users { get; set; }

        public UserList(ApplicationDbContext applicationDb,User user ,RoleRepository roleRepository)
        {
            this.applicationDb = applicationDb;
            this.roleRepository = roleRepository;
        }

       


    }
}
