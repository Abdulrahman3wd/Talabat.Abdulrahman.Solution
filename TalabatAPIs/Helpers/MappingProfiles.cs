using AutoMapper;
using Talabat.Core.Entities;
using TalabatAPIs.DTOs;

namespace TalabatAPIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, O => O.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, O => O.MapFrom(s => s.Category.Name));
        }
    }
}
