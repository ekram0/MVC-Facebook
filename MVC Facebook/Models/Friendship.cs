using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Facebook.Models
{
    public class Friendship
    {
        public int FriendshipId { get; set; }
        [ForeignKey("User")]
        public string SenderID { get; set; }
        [ForeignKey("User")]
        public string ReceiverID { get; set; }
        public FriendshipState State { get; set; }
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }
}
