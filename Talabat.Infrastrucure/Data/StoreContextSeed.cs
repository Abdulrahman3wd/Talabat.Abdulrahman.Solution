using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Infrastrucure.Data
{
    public class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext dbContext)
        {
            if (dbContext.ProductBrands.Count() == 0)
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
            if(dbContext.ProductCategories.Count() == 0)
            {
                var CategoryData = File.ReadAllText("../Talabat.Infrastrucure/Data/DataSeeding/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductBrand>>(CategoryData);
                if (categories?.Count > 0)
                {
                    foreach (var category in categories)
                    {
                        dbContext.Set<ProductBrand>().Add(category);

                    }
                    await dbContext.SaveChangesAsync();
                }

            }
            if (dbContext.Products.Count() == 0)
            {
                var ProductData = File.ReadAllText("../Talabat.Infrastrucure/Data/DataSeeding/products.json");
                var products = JsonSerializer.Deserialize<List<ProductBrand>>(ProductData);
                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        dbContext.Set<ProductBrand>().Add(product);

                    }
                    await dbContext.SaveChangesAsync();
                }

            }


        }
    }
}
