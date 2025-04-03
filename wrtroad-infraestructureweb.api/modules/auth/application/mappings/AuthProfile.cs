using AutoMapper;
using wrtroad_infraestructureweb.api.core.domain.entities;
using wrtroad_infraestructureweb.api.webapi.models.request;

namespace wrtroad_infraestructureweb.api.modules.auth.application.mappings
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterRequestModel, UserEntity>();
            CreateMap<UserEntity, RegisterRequestModel>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
