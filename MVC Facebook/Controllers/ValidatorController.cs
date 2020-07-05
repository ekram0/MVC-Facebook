using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_Facebook.Models;
using MVC_Facebook.Models.Repository;
using System.Net.Mail;
using SendGrid.Helpers.Mail;

namespace MVC_Facebook.Controllers
{
    public class ValidatorController : Controller
    {
        private readonly IRepository<User, string> UserRepository;

        public ValidatorController( IRepository<User, string> UserRepository)
            => ( this.UserRepository) = ( UserRepository);

        public IActionResult BirthDateValidation(DateTime BirthDate)
        {
            if (DateTime.Now.Year - BirthDate.Year >= 18) {
                if (DateTime.Now.Year - BirthDate.Year >= 90)
                    return Json("Ensure your age");
                return Json(true);
            }
            return Json("Your Age Must be greater than 18");
        }
        public IActionResult IsEmailInUse(string Email)
        {
            var user = UserRepository.GetAll();
            User EUser = user.FirstOrDefault(o => o.Email == Email);
            if (EUser == null)
            {             
                return Json(true);
            }
            else
            {
                return Json($"Email {Email} is already in use.");
            }
        }
    }
}