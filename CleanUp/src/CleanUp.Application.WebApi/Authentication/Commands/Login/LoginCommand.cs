using AutoMapper;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Models;
using CleanUp.Application.Requests;
using fbognini.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Authentication.Commands
{
    public class LoginCommand : LoginRequest, IRequest<LoginResponse>
    {
        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
        {
            private readonly IAuthenticationService authenticationService;
            private readonly IMapper mapper;
            private readonly ICurrentUserService currentUserService;
            private readonly ILogger<LoginCommand> logger;

            public LoginCommandHandler(
                IAuthenticationService authenticationService
                , IMapper mapper
                , ICurrentUserService currentUserService
                , ILogger<LoginCommand> logger
                )
            {
                this.authenticationService = authenticationService;
                this.mapper = mapper;
                this.currentUserService = currentUserService;
                this.logger = logger;
            }

            public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    return await authenticationService.LoginAsync(command);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while login user");
                    throw;
                }
            }
        }
    }
}
