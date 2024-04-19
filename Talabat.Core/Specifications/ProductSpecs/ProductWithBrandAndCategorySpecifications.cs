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
        public ProductWithBrandAndCategorySpecifications(string sort):base()
        {
            Includes.Add(P=>P.Brand);
            Includes.Add(p => p.Category);


            if (!string.IsNullOrEmpty(sort))
            {
                 switch (sort)
                {
                    case "priceAsc":
                        //OrderBy = P => P.Price; 
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        //OrderByDesc = P => P.Price;
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P=>P.Name);
                        break;
                }
            }
            else
                AddOrderBy(P => P.Name);



        }
        // This Constructor Will be Used aAn Object , that Will Be used To Get A Specific Product With Id
        public ProductWithBrandAndCategorySpecifications(int id)
            :base(P=>P.Id == id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(p => p.Category);
        }

    } 
}
