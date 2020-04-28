/*
 * 功能：仓储层基础搭建
 * 日期：2020-04-27
 * 开发人员：曾佳炜
 * 重大变更：
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Inventory.Core.IRepository.Base;
using Inventory.Core.Model;
using Inventory.Core.Model.Models;
using Inventory.Core.Model.EF_CORE;
using System.Threading.Tasks;

namespace Inventory.Core.Repository.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where   TEntity : class,new()
    {
        private InventoryContext _db;
        private readonly DbSet<TEntity> _dbSet;
        internal InventoryContext Db
        {
            get { return _db; }
            private set { _db = value; }
        }
        public BaseRepository(IBaseContext mydbcontext)
        {
             this._db = mydbcontext as InventoryContext;
             this._dbSet = _db.Set<TEntity>();
         }

    #region  查询
    ///<summary>
    ///根据条件查询
    /// </summary>
    /// <param name="expression">查询表达式</param>
    /// <returns></returns>
    public async Task<List<TEntity>> GetListBy(Expression<Func<TEntity,bool>>expression)
        {
            return await _db.Set<TEntity>().Where(expression).AsNoTracking().ToListAsync();
        }
        public async Task<TEntity> GetModelById(Expression<Func<TEntity, bool>> expression)
        {
            return await _db.Set<TEntity>().Where(expression).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<List<TEntity>> GetList()
        {
            return await _db.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        #endregion 查询
        #region 增加
        ///<summary>
        ///单个增加
        /// </summary>
        /// <param name="model">单个实体</param>
        /// <returns></returns>
        public async Task<bool> Insert(TEntity model)
        {
            _db.Set<TEntity>().Add(model);
            return await _db.SaveChangesAsync() > 0;
        }
        ///<summary>
        ///批量增加
        /// </summary>
        /// <param name="models">实体列表</param>
        /// <returns></returns>
        public async Task<bool> InsertRange(List<TEntity> models)
        {
            _db.Set<List<TEntity>>().AddRange(models);
            return await _db.SaveChangesAsync() == models.Count;
        }
        #endregion 增加
        #region 删除
        ///<summary>
        ///单个删除
        /// </summary>
        /// <param name="model">单个实体</param>
        /// <returns></returns>
        public async Task<int> Del(TEntity model)
        {
            _db.Set<TEntity>().Attach(model);
            _db.Set<TEntity>().Remove(model);
            return await _db.SaveChangesAsync(); 
        }
        ///<summary>
        ///批量删除
        /// </summary>
        /// <param name="models">实体列表</param>
        /// <returns></returns>
        public async Task<int> DelRange(List<TEntity> models)
        {
            _db.Set<List<TEntity>>().AttachRange(models);
            _db.Set<List<TEntity>>().RemoveRange(models);
            return await _db.SaveChangesAsync();
        }
        #endregion 删除
        #region 修改
        ///<summary>
        ///单个修改
        /// </summary>
        /// <param name="model">单个实体</param>
        /// <returns></returns>
        public async Task<int> Update(TEntity model)
        {
            _db.Entry(model).State = EntityState.Modified;
            return await _db.SaveChangesAsync();
        }
        ///<summary>
        ///批量修改
        /// </summary>
        /// <param name="models">实体列表</param>
        /// <returns></returns>
        public async Task<int> Update(List<TEntity> models)
        {
            _db.Entry(models).State = EntityState.Modified;
            return await _db.SaveChangesAsync();
        }
        #endregion 修改
        #region 其他
        ///<summary>
        ///回滚
        /// </summary>
        public void RollBackChanges()
        {
            var items = _db.ChangeTracker.Entries().ToList();
            items.ForEach(o => o.State = EntityState.Unchanged);
        }
        #endregion 其他
    }
}
