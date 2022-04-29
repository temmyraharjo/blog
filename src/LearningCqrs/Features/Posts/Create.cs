using LearningCqrs.Contracts;
using LearningCqrs.Core;
using LearningCqrs.Data;
using LearningCqrs.Extensions;
using MediatR;

namespace LearningCqrs.Features.Posts;

public class Create
{
    public record CreatePostCommand(string Title, string Body,
        Status? Status, DateTime? PublishedAt = null,
        PostType? PostType = PostType.Blog,
        [property: Lookup(typeof(Category))] Guid[]? CategoryIds = null) : IRequest<DocumentCreated>;

    public class CreatePostHandler : Core.Handler.Create.CreateDocumentHandler<CreatePostCommand>
    {
        private readonly IRepository<Post> _repository;

        public CreatePostHandler(IRepository<Post> repository)
        {
            _repository = repository;
        }

        public override async Task<DocumentCreated> Handling(CreatePostCommand request,
            CancellationToken cancellationToken)
        {
            ICollection<Category>? categories = request.CategoryIds?.Select(id =>
            {
                var result = _repository.Context.Categories.Find(id);
                return result;
            }).Where(e => e != null).ToArray()!;

            var post = new Post
            {
                Title = request.Title,
                Slug = request.Title.ToUrlSlug(),
                Status = request.Status ?? Status.Draft,
                Body = request.Body,
                PublishedAt = request.PublishedAt,
                PostType = request.PostType ?? PostType.Blog
            };

            if (request.CategoryIds != null && request.CategoryIds.Any())
                post.PostCategories = request.CategoryIds.Select(categoryId => new PostCategory
                {
                    CategoryId = categoryId,
                    Post = post
                }).ToArray();
            await _repository.CreateAsync(post, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return new DocumentCreated
            {
                Id = post.Id,
                EntityLogicalName = nameof(Post)
            };
        }
    }
}