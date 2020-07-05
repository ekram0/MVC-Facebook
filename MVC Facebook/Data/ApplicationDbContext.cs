using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC_Facebook.Models;

namespace MVC_Facebook.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Friendship>().HasOne(f => f.Sender).WithMany(ff => ff.Friends);
            builder.Entity<Friendship>().HasOne(f => f.Receiver).WithMany(ff => ff.FriendOf);
            builder.Entity<User>(table =>
            {

                ///for complexType (fullname) which is user entity
                table.OwnsOne(
                    u => u.FullName,
                    fullname =>
            {
                fullname.Property(f => f.FirstName).HasColumnName("FirstName");
                fullname.Property(f => f.LastName).HasColumnName("LastName");
            });
            });

            builder.Entity<User>(table =>
            {
                ///for complexType (address) which is user entity
                table.OwnsOne(
                    u => u.Address,
                    address =>
                    {
                        address.Property(a => a.City).HasColumnName("City");
                        address.Property(a => a.Country).HasColumnName("Country");
                        address.Property(a => a.Zipcode).HasColumnName("Zipcode");
                    });

            });




            base.OnModelCreating(builder);
        }

    }
}
