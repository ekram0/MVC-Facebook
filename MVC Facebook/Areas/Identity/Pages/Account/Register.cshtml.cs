using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MVC_Facebook.Models;
using MVC_Facebook.Models.Repository;

namespace MVC_Facebook.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
   
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<Role> _roleManager;
        private readonly IRepository<User, string> _userRepository;
        public List<string> RolesName { get; set; }

        public RegisterModel(
            UserManager<User> userManager
            ,SignInManager<User> signInManager
            ,ILogger<RegisterModel> logger
            /*,IEmailSender emailSender*/
            ,RoleManager<Role> roleManager,
            IRepository<User,string> userRepository
            )

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
            _roleManager = roleManager;
            _userRepository = userRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [Remote(action: "BirthDateValidation", controller: "Validator")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [EmailAddress]
        [BindProperty]         
        [Remote(action: "IsEmailInUse", controller: "Validator")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.(com)$", ErrorMessage = "Invalid Email format")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            public string RoleName { get; set; } = "Normal User";         

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
           
            [Required]
            [Display(Name = "Gender")]
            public Gender Gender { get; set; }

           


        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            RolesName = _roleManager.Roles.Select(r => r.Name).ToList();
            var userID = _userManager.GetUserId(User);
            ViewData["test"] = _userRepository.GetById(userID);
            //ViewData["Roles"] = new SelectList(RolesName);

        }
        
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
          // _userManager.loc 
            //returnUrl = returnUrl ?? Url.Content("~/");
            //testing 
            returnUrl = returnUrl ?? Url.Content("~/Home");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    UserName = Email,
                    Email = Email,
                    FullName = new FullName(){ FirstName = Input.FirstName, LastName = Input.LastName },
                    Gender = Input.Gender,
                    BirthDate = BirthDate?? DateTime.Now,                   
                    Status = "Single",
                };
                if (user.Gender == Gender.Female)
                {
                    user.Picture = "FemaleDefaultPic.jpg";
                }
                else
                {
                    user.Picture = "MaleDefaultPic.jpg";
                }
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if (!await _roleManager.RoleExistsAsync(Input.RoleName))
                    {
                        await _roleManager.CreateAsync(new Role(Input.RoleName) { Description = $"this is {Input.RoleName}" });
                    }
                    await _userManager.AddToRoleAsync(user, Input.RoleName);


                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    //}
                    //else
                    if (!_signInManager.IsSignedIn(User))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    else
                        return LocalRedirect("~/Admin/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
