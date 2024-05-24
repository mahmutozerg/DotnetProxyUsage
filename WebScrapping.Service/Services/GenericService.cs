using System.Linq.Expressions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using Titanium.Web.Proxy.Http.Responses;
using WebScrapping.Core;
using WebScrapping.Core.Models;
using WebScrapping.Core.Repositories;
using WebScrapping.Core.Services;
using WebScrapping.Core.UnitOfWorks;

namespace WebScrapping.Service.Services;

public class GenericService<TEntity>:IGenericService<TEntity> where TEntity: Base
{
    private readonly IGenericRepository<TEntity?> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public GenericService(IGenericRepository<TEntity?> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task RemoveAsync(string id, string updatedBy)
    {

        var entity = await _repository.Where(e => !e.IsDeleted && e.Id == id).SingleOrDefaultAsync();
        ArgumentNullException.ThrowIfNull(entity);

        _repository.Remove(entity,updatedBy);
        
    }

    public async Task AddAsync(TEntity entity, string createdBy)
    {
        var entityExist = await _repository.AnyAsync(e => e.Id == entity.Id);

        if (entityExist)
            throw new Exception(ResponseMessages.AlreadyExist);

        await _repository.AddAsync(entity, createdBy);
        await _unitOfWork.CommitAsync();
        
    }

    public IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression)
    {
        return _repository.Where(expression);
    }

    public async Task UpdateAsync(TEntity? entity, string updatedBy)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _repository.Update(entity,updatedBy);
        
        await _unitOfWork.CommitAsync();
    }

    public async Task<TEntity> GetByIdAsync(string id)
    {
        var entity = await _repository.GetByIdAsync(id);
        
        ArgumentNullException.ThrowIfNull(entity);

        return entity;
    }
}