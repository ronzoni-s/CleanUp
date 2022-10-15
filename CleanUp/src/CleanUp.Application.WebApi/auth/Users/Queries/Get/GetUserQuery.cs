using AutoMapper;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Models;
using CleanUp.Application.Requests;
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

namespace CleanUp.Application.WebApi.Users.Queries
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public string Id { get; set; }

        public GetUserQuery(string id)
        {
            Id = id;
        }

        public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
        {
            private readonly IUserService userService;
            private readonly IMapper mapper;
            private readonly ICurrentUserService currentUserService;
            private readonly ILogger<GetUserQuery> logger;

            public GetUserQueryHandler(
                IUserService userService
                , IMapper mapper
                , ICurrentUserService currentUserService
                , ILogger<GetUserQuery> logger
                )
            {
                this.userService = userService;
                this.mapper = mapper;
                this.currentUserService = currentUserService;
                this.logger = logger;
            }

            public async Task<UserDto> Handle(GetUserQuery command, CancellationToken cancellationToken)
            {
                try
                {
                    if (string.IsNullOrEmpty(command.Id))
                        throw new BadRequestException("Id non valido");

                    var user = await userService.GetById(command.Id);
                    if (user == null)
                        throw new NotFoundException($"Utente {command.Id} non trovato");

                    return mapper.Map<UserDto>(user);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while getting user");
                    throw;
                }
            }
        }
    }
}
