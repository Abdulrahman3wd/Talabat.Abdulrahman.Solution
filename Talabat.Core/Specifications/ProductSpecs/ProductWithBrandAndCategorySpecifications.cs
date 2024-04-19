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
        public ProductWithBrandAndCategorySpecifications(ProductSpecificationsParams specParams)
            :base(P=> 
                     (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
                     (!specParams.BrandId.HasValue    || P.BrandId == specParams.BrandId.Value ) && 
                     (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)          
            )
        {
            Includes.Add(P=>P.ProductBrand);
            Includes.Add(p => p.ProductCategory);


            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                 switch (specParams.Sort)
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
            // totalProducts = 18 ~ 20
            // pageSize = 5 
            // pageIndex = 3 

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);


        }
        // This Constructor Will be Used aAn Object , that Will Be used To Get A Specific Product With Id
        public ProductWithBrandAndCategorySpecifications(int id)
            :base(P=>P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(p => p.ProductCategory);
        }

    } 
}
