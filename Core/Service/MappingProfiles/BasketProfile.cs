
using AutoMapper;
using DomainLayer.Models.BasketModule;
using Shared.DataTtansferObjects.BasketModuleDto;

namespace Service.MappingProfiles
{
    internal class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
        }
    }
}
