using AutoMapper;

namespace backend.Features.User
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Create.Command, Data.User>();
        }
    }
}
