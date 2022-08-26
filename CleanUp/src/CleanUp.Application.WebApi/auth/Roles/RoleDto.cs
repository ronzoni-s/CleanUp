using CleanUp.Domain.Entities;
using fbognini.Core.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Roles
{
    public class RoleDto : Mappable<RoleDto, CleanUpRole>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
