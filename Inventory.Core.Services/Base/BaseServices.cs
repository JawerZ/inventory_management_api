/*
 * 功能：服务层基础搭建
 * 日期：2020-04-27
 * 开发人员：曾佳炜
 * 重大变更：
 */
using Inventory.Core.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using Inventory.Core.IRepository.Base;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Inventory.Core.Services.Base
{
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity: class, new()
    {
        public IBaseRepository<TEntity> BaseDal;
        #region 查询
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> GetList()
        {
            return await BaseDal.GetList();
        }
        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> GetListBy(Expression<Func<TEntity, bool>> expression)
        {
            return await BaseDal.GetListBy(expression);
        }
        /// <summary>
        ///通过id查询单个数据
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns>一个数据实体</returns>
        public async Task<TEntity> GetModelById(Expression<Func<TEntity, bool>> expression)
        {
            return await BaseDal.GetModelById(expression);
        }
        #endregion 查询
        #region 增加
        /// <summary>
        /// 单个增加数据
        /// </summary>
        /// <param name="model">单个数据实体</param>
        /// <returns></returns>
        public async Task<bool> Insert(TEntity model)
        {
            return await BaseDal.Insert(model);
        }
        /// <summary>
        /// 批量增加数据
        /// </summary>
        /// <param name="models">数据实体列表</param>
        /// <returns></returns>
        public async Task<bool> InsertRange(List<TEntity> models)
        {
            return await BaseDal.InsertRange(models);
        }
        #endregion 增加
        #region 删除
        /// <summary>
        /// 单个删除数据
        /// </summary>
        /// <param name="model">单个数据实体</param>
        /// <returns></returns>
        public async Task<int> Del(TEntity model)
        {
            return await BaseDal.Del(model);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="models">数据实体列表</param>
        /// <returns></returns>
        public async Task<int> DelRange(List<TEntity> models)
        {
            return await BaseDal.DelRange(models);
        }
        #endregion 删除
        #region 修改
        public async Task<int> Update(TEntity model)
        {
            return await BaseDal.Update(model);
        }
        public async Task<int> Update(List<TEntity> models)
        {
            return await BaseDal.Update(models);
        }
        #endregion 修改
        /// <summary>
        /// 回滚
        /// </summary>
        public void RollBackChanges()
        {
            BaseDal.RollBackChanges();
        }
    }
}
