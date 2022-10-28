using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.WebApi.Sdk.Requests.User
{
    public class RegisterUserRequest
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
