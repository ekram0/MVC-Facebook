using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MVC_Facebook.Models
{
    public class Like
    {
        public Like()
        {
            TimeStamp = DateTime.Now;
        }
        public int LikeId { get; set; }
        [ForeignKey("User")]
        public string LikeOwnerID { get; set; }

        //[Key]
        [ForeignKey("Post")]
        public int PostID { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public DateTime TimeStamp { get; private set; }

        //navigational property to retive user's data
        public virtual User LikeOwner { get; set; }
        //navigational property to retive post's data
        public virtual Post Post { get; set; }
    }
}
