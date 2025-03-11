using AutoMapper;
using rp_api.DTO;
using rp_api.Model;

namespace rp_api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
            CreateMap<RoleRequest, Role>();
            CreateMap<Role, RoleResponse>();
            CreateMap<RoleUpdateRequest, Role>();
            CreateMap<CompleteRoleRequest, Role>();
            CreateMap<LoveMessage,  LoveMessageResponse>();
            CreateMap<ReviewRequest, Review>();
            CreateMap<Review, ReviewResponse>();
        }
    }
}
