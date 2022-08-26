using fbognini.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Domain.Entities
{
    public class CleanUpUser : AuditableUser<string>
    {
        public CleanUpUser()
            : base()
        {
            Id = Guid.NewGuid().ToString();
        }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? EmailConfirmationDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
