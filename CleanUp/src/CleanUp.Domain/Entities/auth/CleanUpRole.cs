using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Domain.Entities
{
    public class CleanUpRole : IdentityRole
    {
        public CleanUpRole(): base() { }
        public CleanUpRole(string roleName) : base(roleName)
        {

        }
    }
}
