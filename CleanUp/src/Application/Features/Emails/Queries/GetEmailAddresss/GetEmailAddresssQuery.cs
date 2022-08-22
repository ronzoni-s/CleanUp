using AutoMapper;
using CleanUp.Application.Interfaces.Repositories;
using CleanUp.Domain.Entities;
using CleanUp.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CleanUp.Application.Features.Emails.Queries.GetById
{
    public class GetEmailAddresssQuery : IRequest<Result<List<string>>>
    {

    }

    internal class GetEmailAddresssQueryHandler : IRequestHandler<GetEmailAddresssQuery, Result<List<string>>>
    {
        private readonly IParameterRepository _parameterRepository;
        private readonly IMapper _mapper;

        public GetEmailAddresssQueryHandler(IParameterRepository parameterRepository, IMapper mapper)
        {
            _parameterRepository = parameterRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<string>>> Handle(GetEmailAddresssQuery query, CancellationToken cancellationToken)
        {
            List<string> email = await _parameterRepository.GetRecipientEmailAddresses();
            return await Result<List<string>>.SuccessAsync(email);
        }
    }
}