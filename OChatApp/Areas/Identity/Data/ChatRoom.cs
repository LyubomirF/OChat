using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Areas.Identity.Data
{
    public class ChatRoom
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<OChatAppUser> Users { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
