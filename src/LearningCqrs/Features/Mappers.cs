using AutoMapper;
using LearningCqrs.Data;
using LearningCqrs.Features.Users;

namespace LearningCqrs.Features;

public class Mapper : Profile, Contracts.IMapper
{
    public Mapper()
    {
        CreateMap<User, Update.UpdateUserCommand>();
        CreateMap<Update.UpdateUserCommand, User>();
    }
}