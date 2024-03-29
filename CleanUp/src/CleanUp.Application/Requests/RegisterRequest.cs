﻿using System.Runtime.Serialization;

namespace CleanUp.Application.Requests
{
    public partial class RegisterRequest 
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string Password { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool IsAdmin { get; set; }
    }
}
