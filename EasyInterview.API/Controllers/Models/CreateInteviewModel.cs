﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.Controllers.Models
{
    public class CreateInteviewModel
    {
        public string OwnerEmail { get; set; }
        public DateTime BookedDate { get; set; }
    }
}
