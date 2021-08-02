using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Models
{
    public class CreateChatWithModel
    {
        public string InitiatorId { get; set; }

        public string TargetId { get; set; }

        public string ChatName { get; set; }
    }
}
