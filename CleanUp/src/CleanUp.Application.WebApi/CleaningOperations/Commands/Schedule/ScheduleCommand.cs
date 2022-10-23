using AutoMapper;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanUp.Application.WebApi.CleaningOperations.Commands
{
    public class ScheduleCommand : IRequest<Unit>
    {
        public DateTime Date { get; set; }

        public class ScheduleCommandHandler : IRequestHandler<ScheduleCommand, Unit>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IMapper mapper;
            private readonly ILogger<ScheduleCommand> logger;
            private readonly ISchedulerService service;

            public ScheduleCommandHandler(
                ICleanUpRepositoryAsync repository
                , IMapper mapper
                , ILogger<ScheduleCommand> logger
                , ISchedulerService service
                )
            {
                this.repository = repository;
                this.mapper = mapper;
                this.logger = logger;
                this.service = service;
            }

            public async Task<Unit> Handle(ScheduleCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    await service.Reschedule(request.Date);
                    return Unit.Value;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while scheduling cleaning operations");
                    throw;
                }
            }
        }
    }
}
