using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Events.Queries
{
    public class GetEventQueryValidator : AbstractValidator<GetEventQuery>
    {
        public GetEventQueryValidator()
        {
        }
    }
}
