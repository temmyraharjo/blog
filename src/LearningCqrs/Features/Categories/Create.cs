using LearningCqrs.Contracts;
using LearningCqrs.Core;
using LearningCqrs.Core.Handler;
using LearningCqrs.Data;
using LearningCqrs.Extensions;
using MediatR;

namespace LearningCqrs.Features.Categories;

public class Create
{
    public record CreateCategoryCommand(string Name) : IRequest<DocumentCreated>;

    public class CreateCategoryHandler : CreateDocumentHandler<CreateCategoryCommand>
    {
        private readonly IRepository<Category> _repository;

        public CreateCategoryHandler(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public override async Task<DocumentCreated> Handling(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.Name,
                Slug = request.Name.ToUrlSlug()
            };   
            await _repository.CreateAsync(category, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return new DocumentCreated
            {
                Id = category.Id,
                EntityLogicalName = nameof(Category)
            };
        }
    }
}