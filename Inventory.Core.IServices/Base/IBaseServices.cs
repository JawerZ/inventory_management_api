using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core.IServices
{
    public interface IBaseServices<TEntity>where TEntity:class
    {
        Task<List<TEntity>> GetList();
        Task<List<TEntity>> GetListBy(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetModelById(Expression<Func<TEntity, bool>> expression);
        Task<bool> Insert(TEntity model);
        Task<bool> InsertRange(List<TEntity> models);
        Task<int> Del(TEntity model);
        Task<int> DelRange(List<TEntity> models);
        Task<int> Update(TEntity model);
        Task<int> Update(List<TEntity> models);
        void RollBackChanges();
    }
}
