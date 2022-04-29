using AutoMapper;
using LearningCqrs.Data;

namespace LearningCqrs.Features;

public class Mapper : Profile, Contracts.IMapper
{
    public Mapper()
    {
        CreateMap<User, Users.Update.UpdateUserCommand>();
        CreateMap<Users.Update.UpdateUserCommand, User>();

        CreateMap<Category, Categories.Update.UpdateCategoryCommand>();
        CreateMap<Categories.Update.UpdateCategoryCommand, Category>();


        CreateMap<Post, Posts.Update.UpdatePostCommand>();
        CreateMap<Posts.Update.UpdatePostCommand, Post>();
    }
}