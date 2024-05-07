using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.Application.ProductServices
{
	public class ProductService : IProductServices
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductService(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}
		public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecificationsParams specParams)
		{
			var sepc = new ProductWithBrandAndCategorySpecifications(specParams);

			var products = await  _unitOfWork.Repository<Product>().GetAllWithSpecAsync(sepc);
			return products;
		}
		public async Task<Product?> GetProductAsync(int productId)
		{
			var sepc = new ProductWithBrandAndCategorySpecifications(productId);
			var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(sepc);
			return product;
		}

		public async Task<int> GetCountAsync(ProductSpecificationsParams specParams)
		{

			var countSpec = new ProductWithFiltrationForCountSpec(specParams);

			var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);
			return count;
		}
		public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
			=> await _unitOfWork.Repository<ProductBrand>().GetAllAsync();


		public async Task<IReadOnlyList<ProductCategory>> GetCatigoriesAsync()
            => await _unitOfWork.Repository<ProductCategory>().GetAllAsync();




	}
}
