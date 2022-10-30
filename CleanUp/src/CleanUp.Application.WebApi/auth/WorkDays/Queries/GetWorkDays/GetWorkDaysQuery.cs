using AutoMapper;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Application.SearchCriterias;
using CleanUp.Application.WebApi.WorkDays;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using fbognini.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CleanUp.Application.WebApi.WorkDays.Commands
{
    public class GetWorkDaysQuery : IRequest<List<WorkDayDto>>
    {
        private string UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public void SetId(string id) => UserId = id;

        public class GetWorkDaysQueryHandler : IRequestHandler<GetWorkDaysQuery, List<WorkDayDto>>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IUserService userService;
            private readonly IMapper mapper;
            private readonly ILogger<GetWorkDaysQuery> logger;

            public GetWorkDaysQueryHandler(
                ICleanUpRepositoryAsync repository
                , IUserService userService
                , IMapper mapper
                , ILogger<GetWorkDaysQuery> logger
                )
            {
                this.repository = repository;
                this.userService = userService;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<List<WorkDayDto>> Handle(GetWorkDaysQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var criteria = new WorkDaySearchCriteria
                    {
                        UserId = request.UserId,
                        FromDate = request.From,
                        ToDate = request.To
                    };
                    var entities = await repository.GetAllAsync(criteria);

                    return mapper.Map<List<WorkDayDto>>(entities);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while getting WorkDay");
                    throw;
                }
            }
        }
    }
}
