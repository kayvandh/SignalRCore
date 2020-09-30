using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SignalR.Chat.Mvc.Models
{
    public class ChatUser : IdentityUser
    {
        public virtual ICollection<Message> Messages { get; set; }

    }
}