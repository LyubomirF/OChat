﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Models
{
    public class CreateChatWithModel
    {
        public Guid[] ParticipantsIds { get; set; }    

        public string ChatName { get; set; }
    }
}
