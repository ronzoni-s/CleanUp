using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Classrooms.Queries
{
    public class GetClassroomsQueryValidator : AbstractValidator<GetClassroomsQuery>
    {
        public GetClassroomsQueryValidator()
        {
        }
    }
}
