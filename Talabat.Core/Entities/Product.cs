using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; } // Foregin key Column => ProductCategory
        public ProductCategory ProductCategory { get; set; } // Navigational Proerty [one] 
        public int BrandId { get; set; } // Foregin key Column => ProductBrand

        public ProductBrand ProductBrand { get; set; } // Navigational Proerty [one] 

    }
}
