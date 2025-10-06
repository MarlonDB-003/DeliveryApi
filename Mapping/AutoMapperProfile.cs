
using AutoMapper;
using Delivery.Models;
using Delivery.Dtos.User;
using Delivery.Dtos.Product;
using Delivery.Dtos.Establishment;
using Delivery.Dtos.Order;
using Delivery.Dtos.Address;
using Delivery.Dtos.Delivery;
using Delivery.Dtos.Payment;
using Delivery.Dtos.Review;
using Delivery.Dtos.Category;
using Delivery.Dtos.Coupon;

namespace Delivery.Mapping
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			// Exemplos de mapeamento
			CreateMap<User, UserDto>().ReverseMap();
			CreateMap<Product, ProductDto>()
				.ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType))
				.ReverseMap();
			CreateMap<Product, ProductCreateDto>()
				.ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType))
				.ReverseMap();
			CreateMap<Product, ProductUpdateDto>()
				.ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType))
				.ReverseMap();
			CreateMap<Establishment, EstablishmentDto>().ReverseMap();
			CreateMap<Establishment, EstablishmentResponseDto>().ReverseMap();
			CreateMap<Establishment, EstablishmentDetailResponseDto>().ReverseMap();
			CreateMap<Order, OrderDto>().ReverseMap();
			CreateMap<Address, AddressDto>().ReverseMap();
			CreateMap<DeliveryPerson, DeliveryPersonDto>().ReverseMap();
			CreateMap<Payment, PaymentDto>().ReverseMap();
			CreateMap<Review, ReviewDto>().ReverseMap();
			CreateMap<Category, CategoryDto>().ReverseMap();
			CreateMap<Coupon, CouponDto>().ReverseMap();
			CreateMap<OrderItem, OrderItemDto>().ReverseMap();
		}
	}
}
