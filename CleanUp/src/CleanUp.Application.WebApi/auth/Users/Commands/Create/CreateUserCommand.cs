using AutoMapper;
using CleanUp.Application.Common.Interfaces;
using CleanUp.Application.Common.Interfaces.Repositorys;
using CleanUp.Application.Common.Requests;
using CleanUp.Application.WebApi.Users;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using fbognini.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CleanUp.Application.WebApi.Users.Commands
{
    public class CreateUserCommand : RegisterRequest, IRequest<UserDto>
    {
        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
        {
            private readonly IUserService userService;
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IMapper mapper;
            private readonly ILogger<CreateUserCommand> logger;

            public CreateUserCommandHandler(
                IUserService userService
                , ICleanUpRepositoryAsync repository
                , IMapper mapper
                , ILogger<CreateUserCommand> logger
                )
            {
                this.userService = userService;
                this.repository = repository;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await userService.Register(request);

                    return mapper.Map<UserDto>(user);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while registering user");
                    throw;
                }
            }
        }
    }
}
