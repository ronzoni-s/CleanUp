using CleanUp.Domain.Entities;
using fbognini.Core.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Users
{
    public class UserDto : Mappable<UserDto, CleanUpUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime? RegistrationDate { get; set; }
        public DateTime? EmailConfirmationDate { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public List<string> Roles { get; set; }
    }
}
