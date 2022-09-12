using AutoMapper;
using CleanUp.Application.Common.Interfaces.Repositorys;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using fbognini.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanUp.Application.WebApi.Events.Commands
{
    public class DeleteEventCommand : IRequest<EventDto>
    {
        private int Id { get; set; }

        public DeleteEventCommand(int id)
        {
            Id = id;
        }

        public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, EventDto>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IMapper mapper;
            private readonly ILogger<DeleteEventCommand> logger;

            public DeleteEventCommandHandler(
                ICleanUpRepositoryAsync repository
                , IMapper mapper
                , ILogger<DeleteEventCommand> logger
                )
            {
                this.repository = repository;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<EventDto> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = await repository.DeleteByIdAsync<Event>(request.Id);
                    await repository.SaveAsync(cancellationToken);

                    return mapper.Map<EventDto>(entity);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while deleting Event");
                    throw;
                }
            }
        }
    }
}
