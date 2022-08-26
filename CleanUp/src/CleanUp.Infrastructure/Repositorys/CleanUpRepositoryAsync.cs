using CleanUp.Application.Common.Interfaces.Repositorys;
using CleanUp.Application.Common.Interfaces;
using CleanUp.Infrastructure.Persistance;
using fbognini.Application.DbContexts;
using fbognini.Core.Data;
using fbognini.Core.Entities;
using fbognini.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqKit;
using fbognini.Core.Data.Pagination;
using AutoMapper;
using EFCore.BulkExtensions;
using System.Data.Common;
using fbognini.Infrastructure.Repositorys;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Collections;

namespace CleanUp.Infrastructure.Repositorys
{
    public class CleanUpRepositoryAsync : RepositoryAsync<CleanUpDbContext>, ICleanUpRepositoryAsync
    {
        private readonly CleanUpDbContext dbContext;
        private readonly ILogger<CleanUpRepositoryAsync> logger;
        private Hashtable _repositorys;

        public CleanUpRepositoryAsync(CleanUpDbContext dbContext, IMapper mapper, IDistributedCache distributedCache, ILogger<CleanUpRepositoryAsync> logger) : base(dbContext, distributedCache, mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        private T CreateInstanceRepository<T>()
        {
            if (_repositorys == null)
                _repositorys = new Hashtable();

            var key = typeof(T).Name;
            if (!_repositorys.ContainsKey(key))
            {
                var instance = Activator.CreateInstance(typeof(T), this, logger);
                _repositorys.Add(key, instance);
            }

            return (T)_repositorys[key];
        }

        #region Massive Operations

        //public void MassiveInsert<T>(List<T> entitys, BulkConfig bulkConfig = null) where T : class
        //{
        //    dbContext.MassiveInsert(entitys, bulkConfig);
        //}

        //public void MassiveUpdate<T>(List<T> entitys) where T : class
        //{
        //    dbContext.MassiveUpdate(entitys);
        //}

        #endregion
    }
}
