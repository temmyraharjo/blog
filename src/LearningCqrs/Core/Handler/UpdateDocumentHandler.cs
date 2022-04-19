using LearningCqrs.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using IMapper = AutoMapper.IMapper;

namespace LearningCqrs.Core.Handler;

public abstract class UpdateDocumentHandler<TInput, TEntity> : IRequestHandler<UpdateDocument<TInput>, Unit>
    where TInput : class, IRequest<Unit>
    where TEntity : class, IEntity
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMapper _mapper;

    public UpdateDocumentHandler(IRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<Unit> Handle(UpdateDocument<TInput> request, CancellationToken cancellationToken)
    {
        return await Handling(request, cancellationToken);
    }

    public async virtual Task<Unit> Handling(UpdateDocument<TInput> request, CancellationToken cancellationToken)
    {
        var entity = await _repository.Context.Set<TEntity>()
            .SingleAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        var initialDto = _mapper.Map<TInput>(entity);
        request.JsonPatchDocument.ApplyTo(initialDto);
        _mapper.Map(initialDto, entity);
        
        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}