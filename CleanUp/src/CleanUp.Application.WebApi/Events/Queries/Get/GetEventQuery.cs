using AutoMapper;
using CleanUp.Application.Common.Interfaces;
using CleanUp.Application.Common.Interfaces.Repositorys;
using CleanUp.Application.Common.Models;
using CleanUp.Application.Common.Requests;
using CleanUp.Application.WebApi.Events;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using fbognini.Core.Exceptions;
using fbognini.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Events.Queries
{
    public class GetEventQuery : IRequest<EventDto>
    {
        private int Id { get; set; }

        public GetEventQuery(int id)
        {
            Id = id;
        }

        public class GetEventQueryHandler : IRequestHandler<GetEventQuery, EventDto>
        {
            private readonly ICleanUpRepositoryAsync repository;
            private readonly IMapper mapper;
            private readonly ILogger<GetEventQuery> logger;

            public GetEventQueryHandler(
                ICleanUpRepositoryAsync repository
                , IMapper mapper
                , ILogger<GetEventQuery> logger
                )
            {
                this.repository = repository;
                this.mapper = mapper;
                this.logger = logger;
            }

            public async Task<EventDto> Handle(GetEventQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var args = new SelectArgs<Event>();
                    args.Includes.Add(x => x.Classroom);

                    var entity = await repository.GetByIdAsync(request.Id, args);
                    if (entity == null)
                        throw new NotFoundException(nameof(EventDto), request.Id);

                    return mapper.Map<EventDto>(entity);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while getting Event");
                    throw;
                }
            }
        }
    }
}
