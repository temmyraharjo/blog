using FluentValidation;
using LearningCqrs.Contracts;
using LearningCqrs.Core.Handler;
using LearningCqrs.Core.Swagger;
using LearningCqrs.Data;
using LearningCqrs.Extensions;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace LearningCqrs.Features.Categories;

public class Update
{
    public record UpdateCategoryCommand(string? Name = "", bool? UpdateSlug = null,
        [property: SwaggerIgnore] string? Slug = null) : IRequest<Category>;

    public class UpdateCategoryHandler : UpdateDocumentHandler<UpdateCategoryCommand, Category>
    {
        public UpdateCategoryHandler(IRepository<Category> repository, IMapper mapper,
            IEnumerable<IValidator<UpdateCategoryCommand>> validators) :
            base(repository, mapper, validators)
        {
        }

        public override Task<Category> Handling(Category entity,
            UpdateDocument<UpdateCategoryCommand, Category> request, CancellationToken cancellationToken)
        {
            var updateRequest = new UpdateCategoryCommand();
            request.JsonPatchDocument.ApplyTo(updateRequest);

            if (!string.IsNullOrEmpty(updateRequest.Name) && updateRequest.UpdateSlug.GetValueOrDefault())
            {
                request.JsonPatchDocument.Replace(e => e.Slug, updateRequest.Name.ToUrlSlug());
            }

            return base.Handling(entity, request, cancellationToken);
        }
    }
}