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
    public class CreateWorkDayCommand : IRequest<WorkDayDto>
    {
        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public class CreateWorkDayCommandHandler : IRequestHandler<CreateWorkDayCommand, WorkDayDto>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IUserService userService;
            private readonly IMapper mapper;
            private readonly ILogger<CreateWorkDayCommand> logger;

            public CreateWorkDayCommandHandler(
                ICleanUpRepositoryAsync repository
                , IUserService userService
                , IMapper mapper
                , ILogger<CreateWorkDayCommand> logger
                )
            {
                this.repository = repository;
                this.userService = userService;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<WorkDayDto> Handle(CreateWorkDayCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await userService.GetById(request.UserId);
                    if (user == null)
                        throw new NotFoundException("Utente non trovato");

                    var workDay = new WorkDay() 
                    {
                        UserId = request.UserId,
                        Start = request.Start,
                        End = request.End
                    };

                    repository.Create(workDay);
                    await repository.SaveAsync(cancellationToken);

                    return mapper.Map<WorkDayDto>(workDay);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while creating WorkDay");
                    throw;
                }
            }
        }
    }
}
