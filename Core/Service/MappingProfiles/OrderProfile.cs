
using AutoMapper;
using DomainLayer.Models.OrderModule;
using Shared.DataTtansferObjects.IdentityDTOs;
using Shared.DataTtansferObjects.OrderDTOs;

namespace Service.MappingProfiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDto, OrderAddress>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>().ForMember(D => D.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(D => D.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(D => D.PictureUrl, O => O.MapFrom<OrderPictureUrlResolver>());
            CreateMap<DeliveryMethod, DeliveryMethodDto>();

        }
    }
}
