using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        // This constructor Will be Used For Creating Object , That will be used to get All Product
        public ProductWithBrandAndCategorySpecifications():base()
        {
            Includes.Add(P=>P.Brand);
            Includes.Add(p => p.Category);
        }

    } 
}
