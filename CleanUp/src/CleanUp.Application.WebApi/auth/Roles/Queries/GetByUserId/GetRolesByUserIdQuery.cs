using AutoMapper;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Models;
using CleanUp.Application.Requests;
using CleanUp.Application.WebApi.Roles;
using CleanUp.Application.WebApi.Users;
using fbognini.Core.Exceptions;
using fbognini.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Roles.Queries
{
    public class GetRolesByUserIdQuery : IRequest<List<RoleDto>>
    {
        private string UserId { get; set; }

        public GetRolesByUserIdQuery(string userId)
        {
            UserId = userId;
        }

        public class GetRolesByUserIdQueryHandler : IRequestHandler<GetRolesByUserIdQuery, List<RoleDto>>
        {
            private readonly IUserService userService;
            private readonly IMapper mapper;
            private readonly ICurrentUserService currentUserService;
            private readonly ILogger<GetRolesByUserIdQuery> logger;

            public GetRolesByUserIdQueryHandler(
                IUserService userService
                , IMapper mapper
                , ICurrentUserService currentUserService
                , ILogger<GetRolesByUserIdQuery> logger
                )
            {
                this.userService = userService;
                this.mapper = mapper;
                this.currentUserService = currentUserService;
                this.logger = logger;
            }

            public async Task<List<RoleDto>> Handle(GetRolesByUserIdQuery command, CancellationToken cancellationToken)
            {
                try
                {
                    if (string.IsNullOrEmpty(command.UserId))
                        throw new BadRequestException("Id non valido");

                    var user = await userService.GetById(command.UserId);
                    if (user == null)
                        throw new NotFoundException($"Utente {command.UserId} non trovato");

                    var userRoles = await userService.GetRolesAsync(command.UserId);

                    return mapper.Map<List<RoleDto>>(userRoles);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while getting user roles");
                    throw;
                }
            }
        }
    }
}
