using CleanUp.Application.Interfaces;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CleanUp.Application.SearchCriterias
{
    public class ClassroomSearchCriteria : SearchCriteria<Classroom>
    {

        public override List<Expression<Func<Classroom, bool>>> ToWhereClause()
        {
            var list = new List<Expression<Func<Classroom, bool>>>();

            return list;
        }
    }
}
