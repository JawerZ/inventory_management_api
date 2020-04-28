using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace Inventory.Core.IRepository.Base
{
    public interface IBaseRepository<TEntity>where TEntity: class
    {
        Task<List<TEntity>> GetList();
        Task<List<TEntity>> GetListBy(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetModelById(Expression<Func<TEntity, bool>>expression);
        Task<bool> Insert(TEntity model);
        Task<bool> InsertRange(List<TEntity> models);
        Task<int> Del(TEntity model);
        Task<int> DelRange(List<TEntity> models);
        Task<int> Update(TEntity model);
        Task<int> Update(List<TEntity> models);
        void RollBackChanges();
    }
}
