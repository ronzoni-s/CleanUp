using CleanUp.Application.Common.Interfaces;
using CleanUp.Domain.Entities;
using fbognini.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CleanUp.Application.Users
{
    public class UserSearchCriteria : SearchCriteria<CleanUpUser>
    {
        public string Id { get; set; }

        public override List<Expression<Func<CleanUpUser, bool>>> ToWhereClause()
        {
            var list = new List<Expression<Func<CleanUpUser, bool>>>();

            if (!string.IsNullOrEmpty(Id))
            {
                list.Add(x => x.Id == Id);
            }

            return list;
        }
    }
}
