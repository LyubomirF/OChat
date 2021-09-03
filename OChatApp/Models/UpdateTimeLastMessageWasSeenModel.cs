using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Models
{
    public class UpdateTimeLastMessageWasSeenModel
    {
        public Guid UserId { get; set; }

        public Guid ChatId { get; set; }

        public DateTime TimeLastMessageWasSeen { get; set; }
    }
}
