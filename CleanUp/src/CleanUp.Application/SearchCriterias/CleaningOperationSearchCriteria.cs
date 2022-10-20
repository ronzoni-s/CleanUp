using CleanUp.Application.Interfaces;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CleanUp.Application.SearchCriterias
{
    public class CleaningOperationSearchCriteria : SearchCriteria<CleaningOperation>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string UserId { get; set; }

        public override List<Expression<Func<CleaningOperation, bool>>> ToWhereClause()
        {
            var list = new List<Expression<Func<CleaningOperation, bool>>>();

            if (FromDate.HasValue)
            {
                list.Add(x => x.Start.Date >= FromDate.Value.Date);
            }

            if (ToDate.HasValue)
            {
                list.Add(x => x.Start.Date <= ToDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(UserId))
            {
                list.Add(x => x.UserId == UserId);
            }

            return list;
        }
    }
}
