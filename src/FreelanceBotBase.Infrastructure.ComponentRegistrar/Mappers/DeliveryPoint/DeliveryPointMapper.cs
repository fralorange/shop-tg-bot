using AutoMapper;
using FreelanceBotBase.Contracts.DeliveryPoint;

using DeliveryPointEntity = FreelanceBotBase.Domain.DeliveryPoint.DeliveryPoint;

namespace FreelanceBotBase.Infrastructure.ComponentRegistrar.Mappers.DeliveryPoint
{
    public class DeliveryPointMapper : Profile
    {
        public DeliveryPointMapper()
        {
            CreateMap<DeliveryPointEntity, DeliveryPointDto>(MemberList.None)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId));
        }
    }
}
