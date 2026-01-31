using AutoMapper;
using DomainLayer.Models.ProductModule;
using Shared.DataTtansferObjects.ProductModuleDto;

namespace Service.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dist => dist.BrandName, Options => Options.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dist => dist.TypeName, Options => Options.MapFrom(src => src.ProductType.Name))
                .ForMember(dist => dist.PictureUrl, Options => Options.MapFrom<PictureUrlResolver>());

            CreateMap<ProductType, TypeDto>();
            CreateMap<ProductBrand, BrandDto>();

        }
    }
}
