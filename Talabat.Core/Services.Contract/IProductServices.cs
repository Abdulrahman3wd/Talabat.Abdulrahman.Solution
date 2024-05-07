using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.Core.Services.Contract
{
	public interface IProductServices
	{
		Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecificationsParams specParams);

		Task<Product?> GetProductAsync(int productId);
		Task<int> GetCountAsync(ProductSpecificationsParams specParams);
		Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
		Task<IReadOnlyList<ProductCategory>> GetCatigoriesAsync();
	}
}
