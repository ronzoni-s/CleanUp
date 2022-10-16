using AutoMapper;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanUp.Application.WebApi.Events.Commands
{
    public class RescheduleCommand : IRequest<string>
    {
        public DateTime Date { get; set; }

        public class RescheduleCommandHandler : IRequestHandler<RescheduleCommand, string>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IMapper mapper;
            private readonly ILogger<RescheduleCommand> logger;
            private readonly ISchedulerService service;

            public RescheduleCommandHandler(
                ICleanUpRepositoryAsync repository
                , IMapper mapper
                , ILogger<RescheduleCommand> logger
                , ISchedulerService service
                )
            {
                this.repository = repository;
                this.mapper = mapper;
                this.logger = logger;
                this.service = service;
            }

            public async Task<string> Handle(RescheduleCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    return await service.Reschedule(request.Date);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
