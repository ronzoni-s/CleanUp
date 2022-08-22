﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ErbertPranzi.Application.Requests.Identity;
using ErbertPranzi.Application.Responses.Identity;
using ErbertPranzi.Shared.Wrapper;

namespace ErbertPranzi.Client.Infrastructure.Managers.Identity.RoleClaims
{
    public interface IRoleClaimManager : IManager
    {
        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync();

        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId);

        Task<IResult<string>> SaveAsync(RoleClaimRequest role);

        Task<IResult<string>> DeleteAsync(string id);
    }
}