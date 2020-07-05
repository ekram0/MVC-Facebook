using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Facebook.Models
{
    public class Post
    {
        public Post()
        {
            TimeStamp = DateTime.Now;
        }
        public int ID { get; set; }
        public DateTime TimeStamp { get; private set; }
        public string Body { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("User")]
        public string PostOwnerID { get; set; }

        public virtual User PostOwner { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
        public virtual IEnumerable<Like> Likes { get; set; }

    }
}
