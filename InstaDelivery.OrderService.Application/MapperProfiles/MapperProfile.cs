using AutoMapper;
using InstaDelivery.OrderService.Application.Dto;
using InstaDelivery.OrderService.Domain.Entities;
using Newtonsoft.Json;

namespace InstaDelivery.OrderService.Application.MapperProfiles;

public partial class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(
            dest => dest.DeliveryAddress,
            opt => opt.MapFrom(src => string.IsNullOrEmpty(src.DeliveryAddress) ? default : JsonConvert.DeserializeObject<OrderAddressDto>(src.DeliveryAddress)))
            
            .ForMember(
            dest => dest.Items,
            opt => opt.MapFrom(src => src.Items));

        CreateMap<OrderDto, Order>()
            .ForMember(
            dest => dest.DeliveryAddress,
            opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.DeliveryAddress)))

            .ForMember(
            dest => dest.Items,
            opt => opt.MapFrom(src => src.Items));

        CreateMap<OrderItemDto, OrderItem>();

        CreateMap<OrderItem, OrderItemDto>();

        CreateMap<CreateOrderDto, Order>()
             .ForMember(
            dest => dest.DeliveryAddress,
            opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.DeliveryAddress)))
             .ForMember(
            dest => dest.Items,
            opt => opt.MapFrom(src => src.Items));

        CreateMap<CreateOrderItemDto, OrderItem>()
            .ForMember(dest => dest.Subtotal, src => src.Ignore());
    }
}
