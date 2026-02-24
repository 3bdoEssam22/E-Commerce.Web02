
using AutoMapper;
using DomainLayer.Models.ProductModule;
using Microsoft.Extensions.Configuration;
using Shared.DataTtansferObjects.ProductModuleDto;

namespace Service.MappingProfiles
{
    class PictureUrlResolver(IConfiguration _configuration) : IValueResolver<Product, ProductDto, string>
    {
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
                return string.Empty;
            else
            {
                var Url = $"{_configuration.GetSection("Urls")["BaseUrl"]}/{source.PictureUrl}";
                return Url;
            }
        }
    }
}
