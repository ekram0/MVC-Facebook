using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Facebook.Models
{
    public class Role:IdentityRole
    {
        public Role():base()
        {

        }
        public Role(string roleName):base(roleName)
        {

        }
        public string Description { get; set; }
    }
}
