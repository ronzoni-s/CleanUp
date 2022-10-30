using AutoMapper;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Application.WebApi.WorkDays;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using fbognini.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CleanUp.Application.WebApi.WorkDays.Commands
{
    public class UpdateWorkDayCommand : IRequest<WorkDayDto>
    {
        private int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public void SetId(int id) => Id = id;

        public class UpdateWorkDayCommandHandler : IRequestHandler<UpdateWorkDayCommand, WorkDayDto>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IMapper mapper;
            private readonly ILogger<UpdateWorkDayCommand> logger;

            public UpdateWorkDayCommandHandler(
                ICleanUpRepositoryAsync repository
                , IMapper mapper
                , ILogger<UpdateWorkDayCommand> logger
                )
            {
                this.repository = repository;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<WorkDayDto> Handle(UpdateWorkDayCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var workday = await repository.GetByIdAsync<WorkDay>(request.Id);
                    if (workday == null)
                        throw new NotFoundException("Orario lavorativo non trovato");

                    workday.Start = request.Start;
                    workday.End = request.End;

                    repository.Update(workday);
                    await repository.SaveAsync(cancellationToken);

                    return mapper.Map<WorkDayDto>(workday);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while updating WorkDay");
                    throw;
                }
            }
        }
    }
}
