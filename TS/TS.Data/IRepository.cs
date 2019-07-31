using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TS.Core;

namespace TS.Data
{
    /// <summary>
    /// 仓储
    /// </summary>
    public interface IRepository<T>
        where T : BaseEntity
    {
        /// <summary>
        /// 过滤表达式树，True
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Expression<Func<T, bool>> ExpressionTrue();

        /// <summary>
        /// 过滤表达式树，False
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Expression<Func<T, bool>> ExpressionFalse();

        /// <summary>
        /// 获取分页实体集，正序排列
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLamda"></param>
        /// <param name="orderByLamda"></param>
        /// <returns></returns>
        PagedList<T> GetPagedData<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, TKey>> orderByLamda);

        /// <summary>
        /// 获取分页实体集，倒序排列
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLamda"></param>
        /// <param name="orderByDesLamda"></param>
        /// <returns></returns>
        PagedList<T> GetPagedDataDes<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, TKey>> orderByDesLamda);

        /// <summary>
        /// 根据Id查找实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(int id);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Lamda"></param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> Lamda);

        /// <summary>
        /// 获取实体集
        /// </summary>
        /// <param name="Lamda"></param>
        /// <returns></returns>
        IQueryable<T> GetMany(Expression<Func<T, bool>> Lamda);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        BaseResult Update(T entity);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        BaseResult Update(IEnumerable<T> list);

        /// <summary>
        /// 批量更新实体，不保存到数据库
        /// </summary>
        /// <param name="list"></param>
        void UpdateNoSave(IEnumerable<T> list);

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        BaseResult Add(T entity);

        /// <summary>
        /// 插入实体，不保存到数据库
        /// </summary>
        /// <param name="entity"></param>
        void AddNoSave(T entity);

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        BaseResult Add(IEnumerable<T> list);

        /// <summary>
        /// 批量插入实体，不保存到数据库
        /// </summary>
        /// <param name="list"></param>
        void AddNoSave(IEnumerable<T> list);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        BaseResult Delete(T entity);

        /// <summary>
        /// 删除实体，不保存到数据库
        /// </summary>
        /// <param name="entity"></param>
        void DeleteNosave(T entity);

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        BaseResult Delete(IEnumerable<T> list);

        /// <summary>
        /// 批量删除实体，不保存到数据库
        /// </summary>
        /// <param name="list"></param>
        void DeleteNoSave(IEnumerable<T> list);

        /// <summary>
        /// 根据条件批量删除实体
        /// </summary>
        /// <param name="lamda"></param>
        /// <returns></returns>
        BaseResult Delete(Expression<Func<T, bool>> lamda);

        /// <summary>
        /// 根据条件批量删除实体，不保存到数据库
        /// </summary>
        /// <param name="lamda"></param>
        void DeleteNoSave(Expression<Func<T, bool>> lamda);

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        BaseResult Save();
    }
}
