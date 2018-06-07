using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TS.Core;

namespace TS.Data
{
    public partial class EFRepository<C>
        where C : DbContext
    {
        private readonly C context;

        public EFRepository()
        {
            context = System.Activator.CreateInstance<C>();
        }

        /// <summary>
        /// 实体集数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IQueryable<T> Table<T>()
            where T : BaseEntity
        {
            return context.Set<T>();
        }

        /// <summary>
        /// 实体集数据源,实时从数据库查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IQueryable<T> TableNoTracking<T>()
            where T : BaseEntity
        {
            return context.Set<T>().AsNoTracking();
        }

        /// <summary>
        /// 表达式树，起始为True
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Expression<Func<T, bool>> ExpressionTrue<T>()
        {
            return DynamicLinqExpressions.True<T>();
        }

        /// <summary>
        /// 表达式树，起始为False，即非
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Expression<Func<T, bool>> ExpressionFalse<T>()
        {
            return DynamicLinqExpressions.False<T>();
        }

        /// <summary>
        /// 获取分页实体集，正序排列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLamda">过滤条件</param>
        /// <param name="orderByLamda">排序字段</param>
        /// <returns></returns>
        public virtual PagedList<T> GetPagedData<T,TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, TKey>> orderByLamda)
            where T : BaseEntity
        {
            var list = context.Set<T>().Where(whereLamda).OrderBy(orderByLamda);
            return new PagedList<T>(list, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取分页实体集，倒序排列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLamda">过滤条件</param>
        /// <param name="orderByDesLamda">排序字段</param>
        /// <returns></returns>
        public virtual PagedList<T> GetPageDataDes<T,TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, TKey>> orderByDesLamda)
            where T : BaseEntity
        {
            var list = context.Set<T>().Where(whereLamda).OrderByDescending(orderByDesLamda);
            return new PagedList<T>(list, pageIndex, pageSize);
        }

        /// <summary>
        /// 根据Id查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById<T>(int id)
            where T : BaseEntity
        {
            return context.Set<T>().Find(id);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamda"></param>
        /// <returns></returns>
        public virtual T Get<T>(Expression<Func<T, bool>> Lamda)
            where T : BaseEntity
        {
            return context.Set<T>().FirstOrDefault(Lamda);
        }

        /// <summary>
        /// 批量获取实体
        /// </summary>
        /// <param name="Lamda"></param>
        /// <returns></returns>
        public virtual IQueryable<T> GetMany<T>(Expression<Func<T, bool>> Lamda)
            where T : BaseEntity
        {
            return context.Set<T>().Where(Lamda);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Update<T>(T entity)
            where T : BaseEntity
        {
            try
            {
                context.Entry(entity).State = EntityState.Modified;
                return context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// 更新实体，不保存到数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public virtual void UpdateNoSave<T>(T entity)
            where T : BaseEntity
        {
            context.Entry(entity).State = EntityState.Modified;
        }


        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual int Update<T>(IEnumerable<T> list)
            where T : BaseEntity
        {
            try
            {
                foreach (var entity in list)
                {
                    context.Entry(entity).State = EntityState.Modified;
                }
                return context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// 批量更新实体，不保存到数据库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual void UpdateNoSave<T>(IEnumerable<T> list)
            where T : BaseEntity
        {
            foreach (var entity in list)
            {
                context.Entry(entity).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Insert<T>(T entity)
            where T : BaseEntity
        {
            try
            {
                context.Entry(entity).State = EntityState.Added;
                return context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// 插入实体，不保存到数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void InsertNoSave<T>(T entity)
            where T : BaseEntity
        {
            context.Entry(entity).State = EntityState.Added;
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual int Insert<T>(IEnumerable<T> list)
            where T :BaseEntity
        {
            try
            {
                foreach (var entity in list)
                {
                    context.Entry(entity).State = EntityState.Added;
                }
                return context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// 批量插入实体，不保存到数据库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual void InsertNoSave<T>(IEnumerable<T> list)
            where T : BaseEntity
        {
            foreach (var entity in list)
            {
                context.Entry(entity).State = EntityState.Added;
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Delete<T>(T entity)
             where T : BaseEntity
        {
            try
            {
                context.Entry(entity).State = EntityState.Deleted;
                return context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// 删除实体，不保存到数据库
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void DeleteNosave<T>(T entity)
             where T : BaseEntity
        {
            context.Entry(entity).State = EntityState.Deleted;
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual int Delete<T>(IEnumerable<T> list)
            where T : BaseEntity
        {
            try
            {
                foreach (var entity in list)
                {
                    context.Entry<T>(entity).State = EntityState.Deleted;
                }
                return context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// 批量删除实体，不保存到数据库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual void DeleteNoSave<T>(IEnumerable<T> list)
            where T : BaseEntity
        {
            foreach (var entity in list)
            {
                context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        /// <summary>
        /// 根据条件批量删除实体
        /// </summary>
        /// <param name="lamda"></param>
        /// <returns></returns>
        public virtual int Delete<T>(Expression<Func<T, bool>> lamda)
            where T : BaseEntity
        {
            try
            {
                var list = context.Set<T>().Where(lamda);
                foreach (var entity in list)
                {
                    context.Entry<T>(entity).State = EntityState.Deleted;
                }
                return context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// 根据条件批量删除实体，不保存到数据库
        /// </summary>
        /// <param name="lamda"></param>
        /// <returns></returns>
        public virtual void DeleteNoSave<T>(Expression<Func<T, bool>> lamda)
            where T : BaseEntity
        {
            var list = context.Set<T>().Where(lamda);
            foreach (var entity in list)
            {
                context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        public virtual int Save()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// 获取完整错误说明
        /// </summary>
        /// <param name="exc">Exception</param>
        /// <returns>Error</returns>
        protected string GetFullErrorText(DbEntityValidationException exc)
        {
            var msg = string.Empty;
            foreach (var validationErrors in exc.EntityValidationErrors)
                foreach (var error in validationErrors.ValidationErrors)
                    msg += string.Format("字段名称: {0} 错误原因: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
            return msg;
        }
    }
}
