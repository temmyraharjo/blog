using FluentValidation;
using LearningCqrs.Contracts;
using LearningCqrs.Core;
using LearningCqrs.Core.Handler;
using LearningCqrs.Core.Swagger;
using LearningCqrs.Data;
using LearningCqrs.Extensions;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace LearningCqrs.Features.Posts;

public class Update
{
    public record UpdatePostCommand(string? Title = null, string Body = null,
        [property: Lookup(typeof(Category))] Guid[]? CategoryIds = null, Status? Status = null,
        PostType? PostType = null, DateTime? PublishedAt = null, bool? UpdateSlug = false,
        [property: SwaggerIgnore] string Slug = "") : IRequest<Post>;

    public class UpdatePostHandler : UpdateDocumentHandler<UpdatePostCommand, Post>
    {
        public UpdatePostHandler(IRepository<Post> repository, IMapper mapper,
            IEnumerable<IValidator<UpdatePostCommand>> validators) : base(repository, mapper, validators)
        {
        }

        public override Task<Post> Handling(Post entity, UpdateDocument<UpdatePostCommand, Post> request, CancellationToken cancellationToken)
        {
            var updatePost = new UpdatePostCommand();
            request.JsonPatchDocument.ApplyTo(updatePost);

            if (!string.IsNullOrEmpty(updatePost.Title) && updatePost.UpdateSlug.GetValueOrDefault())
            {
                request.JsonPatchDocument.Add(e => e.Slug, updatePost.Title.ToUrlSlug());
            }

            if (updatePost.CategoryIds != null && updatePost.CategoryIds.Any())
            {
                if (entity.PostCategories != null && entity.PostCategories.Any())
                {
                    entity.PostCategories.Clear();
                }
                
                entity.PostCategories = updatePost.CategoryIds.Select(categoryId => new PostCategory
                {
                    CategoryId = categoryId,
                    Post = entity
                }).ToArray();
            }
            
            return base.Handling(entity, request, cancellationToken);
        }
    }
}