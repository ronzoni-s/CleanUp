using AutoMapper;
using CleanUp.Application.Common.Interfaces.Repositorys;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using fbognini.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanUp.Application.WebApi.Events.Commands
{
    public class CreateEventCommand : IRequest<EventDto>
    {
        public string ClassroomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDto>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IMapper mapper;
            private readonly ILogger<CreateEventCommand> logger;

            public CreateEventCommandHandler(
                ICleanUpRepositoryAsync repository
                , IMapper mapper
                , ILogger<CreateEventCommand> logger
                )
            {
                this.repository = repository;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = new Event 
                    {
                        ClassroomId = request.ClassroomId,
                        StartTime = request.StartTime,
                        EndTime = request.EndTime,
                    };
                    await repository.CreateAsync(entity, cancellationToken);

                    await repository.SaveAsync(cancellationToken);

                    return mapper.Map<EventDto>(entity);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while creating Event");
                    throw;
                }
            }
        }
    }
}
