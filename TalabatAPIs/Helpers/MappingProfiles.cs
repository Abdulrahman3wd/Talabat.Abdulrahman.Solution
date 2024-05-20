using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using OrderAgg= Talabat.Core.Entities.Order_Aggregate;
using TalabatAPIs.DTOs;


namespace TalabatAPIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, O => O.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.Category, O => O.MapFrom(s => s.ProductCategory.Name))
               //.ForMember(P => P.PictureUrl, O=>O.MapFrom(S => $"{configuration["ApiBaseUrl"]}/{S.PictureUrl}"));
               .ForMember(P => P.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<CustomerBasketDto, CustomerBasket>(); 
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<UserAddress, AddressDto>().ReverseMap();
            CreateMap<AddressDto, OrderAgg.Address>();
            CreateMap<OrderAgg.Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d=>d.DeliveryMethodCost,o=>o.MapFrom(s=>s.DeliveryMethod.Cost)) ;
            CreateMap<OrderAgg.OrderItem, OrderItemDto>()
				.ForMember(d => d.ProductId, O => O.MapFrom(s => s.Product.ProductId))
				.ForMember(d => d.ProductName, O => O.MapFrom(s => s.Product.ProductName))
				.ForMember(d => d.PictureUrl, O => O.MapFrom(s => s.Product.PictureUrl))
				.ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());


		}
    }
}
