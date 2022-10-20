using AutoMapper;
using CleanUp.Application;
using CleanUp.Application.Authorization;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Application.SearchCriterias;
using fbognini.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanUp.Application.WebApi.CleaningOperations.Queries
{
    public class GetCleaningOperationsQuery : IRequest<List<CleaningOperationDto>>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public class GetCleaningOperationsQueryHandler : IRequestHandler<GetCleaningOperationsQuery, List<CleaningOperationDto>>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IUserService userService;
            private readonly ICurrentUserService currentUserService;
            private readonly IMapper mapper;
            private readonly ILogger<GetCleaningOperationsQuery> logger;

            public GetCleaningOperationsQueryHandler(
                ICleanUpRepositoryAsync repository
                , IUserService userService
                , ICurrentUserService currentUserService
                , IMapper mapper
                , ILogger<GetCleaningOperationsQuery> logger
                )
            {
                this.repository = repository;
                this.userService = userService;
                this.currentUserService = currentUserService;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<List<CleaningOperationDto>> Handle(GetCleaningOperationsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var criteria = new CleaningOperationSearchCriteria
                    {
                        FromDate = request.FromDate,
                        ToDate = request.ToDate,
                    };

                    if (await userService.IsInRole(currentUserService.UserId, RoleConstants.OperatorRole))
                    {
                        criteria.UserId = currentUserService.UserId;
                    }

                    var cleaningOperations = await repository.GetAllAsync(criteria, cancellationToken);

                    return mapper.Map<List<CleaningOperationDto>>(cleaningOperations);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while getting CleaningOperations");
                    throw;
                }
            }
        }
    }
}
