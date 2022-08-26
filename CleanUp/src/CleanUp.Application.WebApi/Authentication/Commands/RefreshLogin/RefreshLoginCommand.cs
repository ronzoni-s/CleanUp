using AutoMapper;
using CleanUp.Application.Common.Interfaces;
using CleanUp.Application.Common.Models;
using CleanUp.Application.Common.Requests;
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
    public class RefreshLoginCommand : RefreshLoginRequest, IRequest<LoginResponse>
    {
        public class RefreshLoginCommandHandler : IRequestHandler<RefreshLoginCommand, LoginResponse>
        {
            private readonly IAuthenticationService authenticationService;
            private readonly IMapper mapper;
            private readonly ICurrentUserService currentUserService;
            private readonly ILogger<RefreshLoginCommand> logger;

            public RefreshLoginCommandHandler(
                IAuthenticationService authenticationService
                , IMapper mapper
                , ICurrentUserService currentUserService
                , ILogger<RefreshLoginCommand> logger
                )
            {
                this.authenticationService = authenticationService;
                this.mapper = mapper;
                this.currentUserService = currentUserService;
                this.logger = logger;
            }

            public async Task<LoginResponse> Handle(RefreshLoginCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    return await authenticationService.GetRefreshTokenAsync(command);
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
