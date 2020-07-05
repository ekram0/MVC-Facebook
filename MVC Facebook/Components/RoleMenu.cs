using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MVC_Facebook.Models.Enums;
using MVC_Facebook.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Facebook.Components
{
    public class RoleMenu : ViewComponent
    {
        private readonly RoleRepository roleRepository;
        private readonly IHtmlHelper htmlHelper;

        public RoleMenu(RoleRepository roleRepository, IHtmlHelper htmlHelper)
            => (this.roleRepository,this.htmlHelper) = (roleRepository, htmlHelper);


        public IViewComponentResult Invoke()
            => View(roleRepository.GetAll().OrderBy(p => p.Name).Select(R => R.Name).ToList());

    }
}
