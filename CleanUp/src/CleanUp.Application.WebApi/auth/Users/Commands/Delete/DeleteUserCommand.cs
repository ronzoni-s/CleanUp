using AutoMapper;
using CleanUp.Application.Common.Interfaces;
using CleanUp.Application.Common.Interfaces.Repositorys;
using CleanUp.Application.WebApi.Users;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using fbognini.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CleanUp.Application.WebApi.Users.Commands
{
    public class DeleteUserCommand : IRequest<UserDto>
    {
        private string Id { get; set; }

        public DeleteUserCommand(string id)
        {
            Id = id;
        }

        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserDto>
        {
            private readonly IUserService userService;
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IMapper mapper;
            private readonly ILogger<DeleteUserCommand> logger;

            public DeleteUserCommandHandler(
                IUserService userService
                , ICleanUpRepositoryAsync repository
                , IMapper mapper
                , ILogger<DeleteUserCommand> logger
                )
            {
                this.userService = userService;
                this.repository = repository;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<UserDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    if (string.IsNullOrEmpty(request.Id))
                        throw new BadRequestException("Id non valido");

                    var user = await userService.DeleteById(request.Id);

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
