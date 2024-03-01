using AutoMapper;
using FreelanceBotBase.Contracts.User;
using UserEntity = FreelanceBotBase.Domain.User.User;

namespace FreelanceBotBase.Infrastructure.ComponentRegistrar.Mappers.User
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserEntity, UserDto>(MemberList.None)
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole))
                .ForMember(dest => dest.DeliveryPointId, opt => opt.MapFrom(src => src.DeliveryPointId));
        }
    }
}
