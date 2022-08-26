using CleanUp.Application.Common.Authorization;
using CleanUp.Application.Common.Models;
using CleanUp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanUp.Infrastructure.Helpers
{
    public static class ClaimsHelper
    {
        public static void GetAllPermissions(this List<RoleClaimDto> allPermissions)
        {
            var modules = typeof(Permissions).GetNestedTypes();

            foreach (var module in modules)
            {
                var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                foreach (FieldInfo fi in fields)
                {
                    var propertyValue = fi.GetValue(null);

                    if (propertyValue is not null)
                        allPermissions.Add(new RoleClaimDto { Value = propertyValue.ToString(), Type = CustomClaimTypes.Permission, Group = module.Name });
                    //TODO - take descriptions from description attribute
                }
            }

        }

        public static async Task<IdentityResult> AddPermissionClaim(this RoleManager<CleanUpRole> roleManager, CleanUpRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == CustomClaimTypes.Permission && a.Value == permission))
            {
                return await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
            }

            return IdentityResult.Failed();
        }
    }
}