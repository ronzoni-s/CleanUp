﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.Requests
{
    public class RefreshLoginRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
