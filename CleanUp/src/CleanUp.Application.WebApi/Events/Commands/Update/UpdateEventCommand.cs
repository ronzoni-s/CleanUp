using AutoMapper;
using CleanUp.Application.Common.Interfaces.Repositorys;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using fbognini.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanUp.Application.WebApi.Events.Commands
{
    public class UpdateEventCommand : IRequest<EventDto>
    {
        private int Id { get; set; }

        public string ClassroomId { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }

        public void SetId(int id) => Id = id;

        public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, EventDto>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IMapper mapper;
            private readonly ILogger<UpdateEventCommand> logger;

            public UpdateEventCommandHandler(
                ICleanUpRepositoryAsync repository
                , IMapper mapper
                , ILogger<UpdateEventCommand> logger
                )
            {
                this.repository = repository;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<EventDto> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = await repository.GetByIdAsync<Event>(request.Id);
                    if (entity == null)
                        throw new NotFoundException(nameof(Event), request.Id);

                    entity.ClassroomId = request.ClassroomId;
                    entity.Name = request.Name;
                    entity.Teacher = request.Teacher;
                    entity.StartTime = request.StartTime;
                    entity.EndTime = request.EndTime;
                    entity.IsActive = request.IsActive;

                    repository.Update(entity);
                    
                    await repository.SaveAsync(cancellationToken);

                    return mapper.Map<EventDto>(entity);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while updating Event");
                    throw;
                }
            }
        }
    }
}
