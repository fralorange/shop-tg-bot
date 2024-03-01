using AutoMapper;
using FreelanceBotBase.Contracts.Product;

using ProductEntity = FreelanceBotBase.Domain.Product.ProductRecord;

namespace FreelanceBotBase.Infrastructure.ComponentRegistrar.Mappers.Product
{
    /// <summary>
    /// Product mapper profile.
    /// </summary>
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductEntity, ProductDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
                .ReverseMap();
        }
    }
}
