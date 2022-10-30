using AutoMapper;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Application;
using MediatR;
using Microsoft.Extensions.Logging;
using CleanUp.Application.SearchCriterias;

namespace CleanUp.Application.WebApi.Events.Queries
{
    public class GetEventsQuery : IRequest<List<EventDto>>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ClassroomId { get; set; }

        public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, List<EventDto>>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IMapper mapper;
            private readonly ILogger<GetEventsQuery> logger;

            public GetEventsQueryHandler(
                ICleanUpRepositoryAsync repository
                , IMapper mapper
                , ILogger<GetEventsQuery> logger
                )
            {
                this.repository = repository;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<List<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var criteria = new EventSearchCriteria
                    {
                        FromDate = request.FromDate,
                        ToDate = request.ToDate,
                        ClassRoomId = request.ClassroomId,
                    };

                    var events = await repository.GetAllAsync(criteria, cancellationToken);

                    return mapper.Map<List<EventDto>>(events);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while getting Events");
                    throw;
                }
            }
        }
    }
}
