using LearningCqrs.Contracts;
using LearningCqrs.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningCqrs.Features.Categories;

public class GetCategory
{
    public record GetCategoryQuery(string Name = "") : IRequest<Category[]>;

    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, Category[]>
    {
        private readonly IRepository<Category> _repository;
        public GetCategoryHandler(IRepository<Category> repository) => _repository = repository;
        
        public async Task<Category[]> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Name))
            {
                return await _repository.Context.Categories.Where(e => e.Name.Contains(request.Name)).ToArrayAsync(cancellationToken);
            }

            return await _repository.Context.Categories.ToArrayAsync(cancellationToken);
        }
    }
}