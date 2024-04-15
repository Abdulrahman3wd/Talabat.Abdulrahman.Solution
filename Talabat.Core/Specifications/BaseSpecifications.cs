using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {

        public Expression<Func<T, bool>> Criteria { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get; set; } = null;
        public BaseSpecifications()
        {
            
           // Criteria =  null;
        }
        public BaseSpecifications(Expression<Func<T, bool>> criteriaEpression)
        {
            Criteria = criteriaEpression; // P=> P.Id == 10
            
        }
    }
}
