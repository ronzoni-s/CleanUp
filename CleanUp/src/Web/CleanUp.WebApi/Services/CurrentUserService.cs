using CleanUp.Application.Common.Authorization;
using fbognini.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CleanUp.WebApi.Services
{
    
    public class CurrentUserService : ICurrentUserService
    {
        private ClaimsPrincipal User { get; set; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            //UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "CleanUp.WebApi";
            User = httpContextAccessor.HttpContext?.User;
        }

        //public string UserId => "CleanUp.WebApi";
        //public string UserName => "CleanUp.WebApi";

        //public string UserId => User?.FindFirstValue(CustomClaimTypes.Information.Id) ?? "CleanUp.WebApi";
        public string UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier)/* ?? "CleanUp.WebApi"*/;
        
        public string UserName => User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity.Name;

        public string Email => User?.FindFirstValue(CustomClaimTypes.Information.Email);

        public bool HasClaim(string type, string value) => throw new NotImplementedException();

        public List<string> GetRoles() => throw new NotImplementedException();
    }
}
