using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using MVC_Facebook.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVC_Facebook.Models;
using MVC_Facebook.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MVC_Facebook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Ekram Change: change to AddDbContextPool
            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultUI();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.TryAddScoped<IRepository<User, string>, UserRepository>();
            services.TryAddScoped<IRepository<Role, string>, RoleRepository>();
            services.TryAddScoped<IRepository<Comment, int>, CommentRepository>();
            services.TryAddScoped<IRepository<Like, int>, LikeRepository>();
            services.TryAddScoped<IRepository<Post, int>, PostRepository>();
            services.TryAddScoped<IRepository<Friendship, int>, FriendshipRepository>();
            //services.AddMvc().AddRazorPagesOptions(options => {
            //    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/MyLogin";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            
        }
    }
}
