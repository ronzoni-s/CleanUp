﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.Common.Requests
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }
}
