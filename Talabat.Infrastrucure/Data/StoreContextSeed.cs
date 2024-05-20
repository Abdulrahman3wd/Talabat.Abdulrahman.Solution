using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastrucure.Data
{
	public class StoreContextSeed
	{
		public async static Task SeedAsync(StoreContext dbContext)
		{


			if (!dbContext.ProductBrands.Any())
			{
				var brandData = File.ReadAllText("../Talabat.Infrastrucure/Data/DataSeeding/brands.json");
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
				if (brands?.Count > 0)
				{
					foreach (var brand in brands)
					{
						dbContext.Set<ProductBrand>().Add(brand);

					}
					await dbContext.SaveChangesAsync();
				}

			}
			if (!dbContext.ProductCategories.Any())
			{
				var CategoryData = File.ReadAllText("../Talabat.Infrastrucure/Data/DataSeeding/categories.json");
				var categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoryData);
				if (categories?.Count > 0)
				{
					foreach (var category in categories)
					{
						dbContext.Set<ProductCategory>().Add(category);

					}
					await dbContext.SaveChangesAsync();
				}

			}
			if (!dbContext.Products.Any())
			{
				var ProductData = File.ReadAllText("../Talabat.Infrastrucure/Data/DataSeeding/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(ProductData);
				if (products?.Count > 0)
				{
					foreach (var product in products)
					{
						dbContext.Set<Product>().Add(product);

					}
					await dbContext.SaveChangesAsync();
				}

			}
			if (!dbContext.DeliveryMethods.Any())
			{
				var deliveryMethodsData = File.ReadAllText("../Talabat.Infrastrucure/Data/DataSeeding/delivery.json");
				var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);
				if (deliveryMethods?.Count > 0)
				{
					foreach (var deliveryMethod in deliveryMethods)
					{
						dbContext.Set<DeliveryMethod>().Add(deliveryMethod);

					}
					await dbContext.SaveChangesAsync();
				}

			}



		}
	}
}
