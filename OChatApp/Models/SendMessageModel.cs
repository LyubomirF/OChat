﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Models
{
    public class SendMessageModel
    {
        public Guid SenderId { get; set; }

        public Guid ChatId { get; set; }

        public String Message { get; set; }
    }
}
