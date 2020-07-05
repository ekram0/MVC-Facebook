using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Facebook.Models
{
    public class Comment
    {
        public Comment()
        {
            TimeStamp = DateTime.Now;
        }
        public int ID { get; set; }
        /// <summary>
        /// for table fk
        /// </summary>
        [ForeignKey("User")]
        public string CommentOwnerID { get; set; }
        [ForeignKey("Post")]
        public int PostID { get; set; }
        public string Body { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime TimeStamp { get; private set; }
        //navigational property to retrive user's data
        public virtual User CommentOwner { get; set; }
        //navigational property to retrive post's data
        public virtual Post Post { get; set; }
    }
}
