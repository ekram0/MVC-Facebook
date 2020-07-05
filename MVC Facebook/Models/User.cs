using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Facebook.Models
{
    //[Table("Users")]
    public class User : IdentityUser
    {
        public User(string userName) : base(userName)
        {
            TimeStamp = DateTime.Now;
        }
        public User() : base()
        {
            TimeStamp = DateTime.Now;
        }

        [Required]
        public FullName FullName { get; set; }
        public Address Address { get; set; }
        public string Bio { get; set; }
        public string Status { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public DateTime TimeStamp { get; private set; }
        public bool IsBlocked { get; set; }
        public string Picture { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Friendship> Friends { get; set; }//Accepted /Confirmed friends By Receiver(Actual friend List)
        public virtual ICollection<Friendship> FriendOf { get; set; }//Pending friends/Incoming Requests //friend Request List


    }
}
