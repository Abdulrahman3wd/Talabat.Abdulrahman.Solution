﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Infrastrucure
{
    internal static class SpecificationEvaluator<TEntity> where TEntity: BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecifications<TEntity> spec) 
        {
            var query = inputQuery;
            if(spec.Criteria is not null) // P=> P.Id == 1
                query = query.Where(spec.Criteria);

            // query = _dbContext.Set<Product>().Where(p=> P.Id ==1)
            // Includes 
            // 1. P => P.Brand 
            // 2. P => P.Category 

             spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            // _dbContext.Set<Product>().Where(P=> P.Id == 1).Include(P=>P.Brand)
            return query;
        }

    }
}
