using CleanUp.Application.Interfaces;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CleanUp.Application.SearchCriterias
{
    public class EventSearchCriteria : SearchCriteria<Event>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ClassRoomId { get; set; }

        public override List<Expression<Func<Event, bool>>> ToWhereClause()
        {
            var list = new List<Expression<Func<Event, bool>>>();

            if (FromDate.HasValue)
            {
                list.Add(x => x.StartTime.Date >= FromDate.Value.Date);
            }

            if (ToDate.HasValue)
            {
                list.Add(x => x.EndTime.Date <= ToDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(ClassRoomId))
            {
                list.Add(x => x.ClassroomId == ClassRoomId);
            }

            return list;
        }
    }
}
