using AutoMapper;
using Blog.Core.Common;

namespace Blog.Core.Features.User
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Create.Command, Models.User>()
                .ForMember(d => d.Password, o => o.MapFrom(s => s.Password.ToMd5()));
            CreateMap<Delete.Command, Models.User>();
            CreateMap<Update.Command, Models.User>()
                .ForMember(d => d.Password, 
                o => o.
                MapFrom(s => string.IsNullOrEmpty(s.Password) ? 
                    s.Password : s.Password.ToMd5()));
        }
    }
}
