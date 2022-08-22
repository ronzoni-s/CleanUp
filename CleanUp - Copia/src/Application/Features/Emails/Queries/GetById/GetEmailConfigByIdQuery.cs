using AutoMapper;
using ErbertPranzi.Application.Interfaces.Repositories;
using ErbertPranzi.Domain.Entities;
using ErbertPranzi.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ErbertPranzi.Application.Features.Emails.Queries.GetById
{
    public class GetEmailConfigByIdQuery : IRequest<Result<GetEmailConfigByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductByIdQueryHandler : IRequestHandler<GetEmailConfigByIdQuery, Result<GetEmailConfigByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetEmailConfigByIdResponse>> Handle(GetEmailConfigByIdQuery query, CancellationToken cancellationToken)
        {
            var email = await _unitOfWork.Repository<EmailConfig>().GetByIdAsync(query.Id);
            var mappedEmail = _mapper.Map<GetEmailConfigByIdResponse>(email);
            return await Result<GetEmailConfigByIdResponse>.SuccessAsync(mappedEmail);
        }
    }
}