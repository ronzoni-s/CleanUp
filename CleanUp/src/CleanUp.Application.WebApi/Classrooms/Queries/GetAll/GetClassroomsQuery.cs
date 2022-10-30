using AutoMapper;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Application;
using MediatR;
using Microsoft.Extensions.Logging;
using CleanUp.Application.SearchCriterias;

namespace CleanUp.Application.WebApi.Classrooms.Queries
{
    public class GetClassroomsQuery : IRequest<List<ClassroomDto>>
    {

        public class GetClassroomsQueryHandler : IRequestHandler<GetClassroomsQuery, List<ClassroomDto>>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IMapper mapper;
            private readonly ILogger<GetClassroomsQuery> logger;

            public GetClassroomsQueryHandler(
                ICleanUpRepositoryAsync repository
                , IMapper mapper
                , ILogger<GetClassroomsQuery> logger
                )
            {
                this.repository = repository;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<List<ClassroomDto>> Handle(GetClassroomsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var criteria = new ClassroomSearchCriteria();

                    var classrooms = await repository.GetAllAsync(criteria, cancellationToken);

                    return mapper.Map<List<ClassroomDto>>(classrooms);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while getting Classrooms");
                    throw;
                }
            }
        }
    }
}
