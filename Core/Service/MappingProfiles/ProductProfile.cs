using AutoMapper;
using DomainLayer.Models;
using Shared.DataTtansferObjects;

namespace Service.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dist => dist.BrandName, Options => Options.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dist => dist.TypeName, Options => Options.MapFrom(src => src.ProductType.Name));

            CreateMap<ProductType, typeDto>();
            CreateMap<ProductBrand, BrandDto>();

        }
    }
}
