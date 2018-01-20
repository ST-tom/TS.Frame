using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using TS.Core.Domain;
using TS.Core.Log;
using TS.Data;

namespace TS.Core.EF
{
    public partial class EFRepository<T, C> 
        where T : BaseEntity
        where C : DbContext
    {
        private readonly C EFContext;
        private DbSet<T> entities;

        public EFRepository()
        {
            EFContext = System.Activator.CreateInstance<C>();
        }

        public virtual Expression<Func<T, bool>> ExpressionTrue
        {
            get
            {
                return DynamicLinqExpressions.True<T>();
            }
        }

        public virtual Expression<Func<T, bool>> ExpressionFalse
        {
            get
            {
                return DynamicLinqExpressions.False<T>();
            }
        }

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        public virtual C Context
        {
            get { return EFContext; }
        }

        /// <summary>
        /// 获取EF对应表
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        /// <summary>
        /// 获取数据库实时数据,不通过上下文
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual DbSet<T> Entities
        {
            get
            {
                if (entities == null)
                    entities = EFContext.Set<T>();
                return entities;
            }
        }

        public virtual PagedList<T> GetPagedData(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamda)
        {
            var list = Entities.Where(whereLamda).OrderBy(e => e.Id);
            return new PagedList<T>(list,pageIndex,pageSize);
        }

        public virtual PagedList<T> GetPagedData<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, TKey>> orderByLamda)
        {
            var list = Entities.Where(whereLamda).OrderBy(orderByLamda);
            return new PagedList<T>(list, pageIndex, pageSize);
        }

        public virtual PagedList<T> GetPagedData<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, TKey>> orderByLamda, Expression<Func<T,TKey>> thenOrderByLamada)
        {
            var list = Entities.Where(whereLamda).OrderBy(orderByLamda).ThenBy(thenOrderByLamada);
            return new PagedList<T>(list, pageIndex, pageSize);
        }

        public virtual PagedList<T> GetPageDataDes<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, TKey>> orderByDesLamda)
        {
            var list = Entities.Where(whereLamda).OrderByDescending(orderByDesLamda);
            return new PagedList<T>(list, pageIndex, pageSize);
        }

        public virtual PagedList<T> GetPageDataDes<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, TKey>> orderByDesLamda,Expression<Func<T,TKey>> thenOrderByDesLamda)
        {
            var list = Entities.Where(whereLamda).OrderByDescending(orderByDesLamda).ThenByDescending(thenOrderByDesLamda);
            return new PagedList<T>(list, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamda"></param>
        /// <returns></returns>
        public virtual T Get(Expression<Func<T, bool>> Lamda)
        {
            try
            {
                return Entities.FirstOrDefault(Lamda);
            }
            catch (Exception ex)
            {
                LogHelper.Error("数据库查询实体失败,错误原因:" + ex.Message, ex);
                return null;
            }
        }

        public virtual T GetById(int id)
        {
            try
            {
                return Entities.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                LogHelper.Error("数据库查询实体失败,错误原因:" + ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// 批量获取数据
        /// </summary>
        /// <param name="Lamda"></param>
        /// <returns></returns>
        public virtual IList<T> GetMany(Expression<Func<T, bool>> Lamda)
        {
            try
            {
                return Entities.Where(Lamda).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error("数据库查询实体集失败，错误原因:" + ex.Message);
                return new List<T>();
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual EFResult Update(T entity)
        {
            try
            {
                EFContext.Entry(entity).State = EntityState.Modified;
                EFContext.SaveChanges();
                return new EFResult();
            }
            catch (DbEntityValidationException ex)
            {
                string str = "";
                foreach (var i in ex.EntityValidationErrors)
                {
                    foreach (var j in i.ValidationErrors)
                    {
                        str += j.ErrorMessage + "，";
                    }
                }
                LogHelper.Error("数据库更新实体失败,错误原因：" + str, ex);
            }
            catch (Exception ex)
            {
                LogHelper.Error("数据库更新实体失败,错误原因：" + ex.Message, ex);
            }
            return new EFResult("修改失败");
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual EFResult Update(List<T> list)
        {
            try
            {
                foreach (var entity in list)
                {
                    EFContext.Entry(entity).State = EntityState.Modified;
                }
                EFContext.SaveChanges();
                return new EFResult();
            }
            catch (DbEntityValidationException ex)
            {
                string str = "";
                foreach (var i in ex.EntityValidationErrors)
                {
                    foreach (var j in i.ValidationErrors)
                    {
                        str += j.ErrorMessage + "，";
                    }
                }
                LogHelper.Error("数据库更新实体集失败，错误原因：" + str, ex);
            }
            catch (Exception ex)
            {
                LogHelper.Error("数据库更新实体集失败，错误原因：" + ex.Message, ex);
            }
            return new EFResult("修改失败");
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual EFResult Insert(T entity)
        {
            try
            {
                EFContext.Entry(entity).State = EntityState.Added;
                EFContext.SaveChanges();
                return new EFResult();
            }
            catch (DbEntityValidationException ex)
            {
                string str = "";
                foreach (var i in ex.EntityValidationErrors)
                {
                    foreach (var j in i.ValidationErrors)
                    {
                        str += j.ErrorMessage;
                    }
                }
                LogHelper.Error("新增实体失败，错误原因：" + str, ex);
            }
            catch (Exception ex)
            {
                LogHelper.Error("新增实体失败，错误原因：" + ex.Message, ex);
            }
            return new EFResult("新增失败");
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual EFResult Insert(List<T> list)
        {
            try
            {
                foreach (var entity in list)
                {
                    EFContext.Entry(entity).State = EntityState.Added;
                }
                EFContext.SaveChanges();
                return new EFResult();
            }
            catch (DbEntityValidationException ex)
            {
                string str = "";
                foreach (var i in ex.EntityValidationErrors)
                {
                    foreach (var j in i.ValidationErrors)
                    {
                        str += j.ErrorMessage + "，";
                    }
                }
                LogHelper.Error("新增实体集失败，错误原因：" + str, ex);
            }
            catch (Exception ex)
            {
                LogHelper.Error("新增实体集失败，错误原因：" + ex.Message, ex);
            }
            return new EFResult("新增失败");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual EFResult Delete(T entity)
        {
            try
            {
                EFContext.Entry(entity).State = EntityState.Deleted;
                EFContext.SaveChanges();
                return new EFResult();
            }
            catch (Exception ex)
            {
                LogHelper.Error("删除实体失败，错误原因：" + ex.Message, ex);
            }
            return new EFResult("删除失败");
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual EFResult Delete(List<T> list)
        {
            try
            {
                foreach (var entity in list)
                {
                    EFContext.Entry<T>(entity).State = EntityState.Deleted;
                }
                EFContext.SaveChanges();
                return new EFResult();
            }
            catch (Exception ex)
            {
                LogHelper.Error("删除实体集失败，错误原因：" + ex.Message);
                return new EFResult("删除失败");
            }
        }

        /// <summary>
        /// 根据条件批量删除数据
        /// </summary>
        /// <param name="lamda"></param>
        /// <returns></returns>
        public virtual EFResult Delete(Expression<Func<T, bool>> lamda)
        {
            try
            {
                var list = Entities.Where(lamda);
                foreach (var entity in list)
                {
                    EFContext.Entry<T>(entity).State = EntityState.Deleted;
                }
                EFContext.SaveChanges();
                return new EFResult();
            }
            catch (Exception ex)
            {
                LogHelper.Error("删除实体集失败，错误原因：" + ex.Message);
                return new EFResult("删除失败");
            }
        }

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        public virtual EFResult Save()
        {
            try
            {
                EFContext.SaveChanges();
                return new EFResult();
            }
            catch (DbEntityValidationException ex)
            {
                string str = "";
                foreach (var i in ex.EntityValidationErrors)
                {
                    foreach (var j in i.ValidationErrors)
                    {
                        str += j.ErrorMessage + "，";
                    }
                }
                LogHelper.Error("更新数据库失败，错误原因：" + str, ex);
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新数据库失败，错误原因：" + ex.Message, ex);
            }
            return new EFResult("保存数据失败");
        }
    }
}
