using AutoMapper;
using CleanUp.Application.Common.Requests;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Application.Requests;
using CleanUp.Application.WebApi.Users;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using fbognini.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CleanUp.Application.WebApi.Users.Commands
{
    public class UpdateUserCommand : UpdateUserRequest, IRequest<UserDto>
    {
        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
        {
            private readonly IUserService userService;
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IMapper mapper;
            private readonly ILogger<UpdateUserCommand> logger;

            public UpdateUserCommandHandler(
                IUserService userService
                , ICleanUpRepositoryAsync repository
                , IMapper mapper
                , ILogger<UpdateUserCommand> logger
                )
            {
                this.userService = userService;
                this.repository = repository;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    if (string.IsNullOrEmpty(request.Id))
                        throw new BadRequestException("Id non valido");

                    var user = await userService.Update(request);

                    return mapper.Map<UserDto>(user);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while updating user");
                    throw;
                }
            }
        }
    }
}
