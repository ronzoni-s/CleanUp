using CleanUp.Application.Interfaces;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CleanUp.Application.SearchCriterias
{
    public class WorkDaySearchCriteria : SearchCriteria<WorkDay>
    {
        public string UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public override List<Expression<Func<WorkDay, bool>>> ToWhereClause()
        {
            var list = new List<Expression<Func<WorkDay, bool>>>();

            if (!string.IsNullOrEmpty(UserId))
            {
                list.Add(x => x.UserId == UserId);
            }

            if (FromDate != null)
            {
                list.Add(x => x.End >= FromDate);
            }

            if (ToDate != null)
            {
                list.Add(x => x.Start <= ToDate);
            }

            return list;
        }
    }
}
