using AutoMapper;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Models;
using CleanUp.Application.Requests;
using CleanUp.Application.WebApi.Users;
using fbognini.Core.Data.Pagination;
using fbognini.Core.Exceptions;
using fbognini.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Users.Queries
{
    public class GetUsersQuery : IRequest<List<UserDto>>
    {

        public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
        {
            private readonly IUserService userService;
            private readonly IMapper mapper;
            private readonly ILogger<GetUsersQuery> logger;

            public GetUsersQueryHandler(
                IUserService userService
                , IMapper mapper
                , ILogger<GetUsersQuery> logger
                )
            {
                this.userService = userService;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<List<UserDto>> Handle(GetUsersQuery command, CancellationToken cancellationToken)
            {
                try
                {
                    var users = mapper.Map<List<UserDto>>(await userService.GetAll());
                    foreach (var user in users)
                    {
                        var roles = await userService.GetRolesAsync(user.Id);
                        user.Roles = roles.Select(x => x.Name).ToList();
                    }
                    return users;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while getting users");
                    throw;
                }
            }
        }
    }
}
