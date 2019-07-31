using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TS.Core;
using TS.Data;
using TS.Data.Helper;

namespace TS.Data
{
    public partial class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        private IDbContext Context { get; set; }
        private DbSet<T> _entities;

        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = Entities;
                return _entities;
            }
        }

        /// <summary>
        /// 数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IQueryable<T> Table { get { return Entities; } }

        /// <summary>
        /// 实时数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IQueryable<T> TableNoTracking { get { return Entities.AsNoTracking(); } }

        /// <summary>
        /// 表达式树，起始为True
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Expression<Func<T, bool>> ExpressionTrue()
        {
            return DynamicLinqExpressions.True<T>();
        }

        /// <summary>
        /// 表达式树，起始为False，即非
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Expression<Func<T, bool>> ExpressionFalse()
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
        public virtual PagedList<T> GetPagedData<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, TKey>> orderByLamda)
        {
            var list = Entities.Where(whereLamda).OrderBy(orderByLamda);
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
        public virtual PagedList<T> GetPagedDataDes<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, TKey>> orderByDesLamda)
        {
            var list = Entities.Where(whereLamda).OrderByDescending(orderByDesLamda);
            return new PagedList<T>(list, pageIndex, pageSize);
        }

        /// <summary>
        /// 根据Id查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(int id)
        {
            return Entities.Find(id);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLamda"></param>
        /// <returns></returns>
        public virtual T Get(Expression<Func<T, bool>> lamda)
        {
            return Entities.FirstOrDefault(lamda);
        }

        /// <summary>
        /// 获取实体集
        /// </summary>
        /// <param name="Lamda"></param>
        /// <returns></returns>
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> lamda)
        {
            return Entities.Where(lamda);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual BaseResult Update(T entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Modified;
                Context.SaveChanges();
                return new BaseResult();
            }
            catch (DbEntityValidationException dbEx)
            {
                LogHelper.Error(GetFullErrorText(dbEx), dbEx);
                return new BaseResult("更新失败");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
                return new BaseResult("更新失败");
            }
        }

        /// <summary>
        /// 更新实体，不保存到数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public virtual void UpdateNoSave(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual BaseResult Update(IEnumerable<T> list)
        {
            try
            {
                foreach (var entity in list)
                {
                    Context.Entry(entity).State = EntityState.Modified;
                }
                Context.SaveChanges();
                return new BaseResult();
            }
            catch (DbEntityValidationException dbEx)
            {
                LogHelper.Error(GetFullErrorText(dbEx), dbEx);
                return new BaseResult("更新失败");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
                return new BaseResult("更新失败");
            }
        }

        /// <summary>
        /// 批量更新实体，不保存到数据库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual void UpdateNoSave(IEnumerable<T> list)
        {
            foreach (var entity in list)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual BaseResult Add(T entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Added;
                Context.SaveChanges();
                return new BaseResult();
            }
            catch (DbEntityValidationException dbEx)
            {
                LogHelper.Error(GetFullErrorText(dbEx), dbEx);
                return new BaseResult("新增失败");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
                return new BaseResult("新增失败");
            }
        }

        /// <summary>
        /// 插入实体，不保存到数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void AddNoSave(T entity)
        {
            Context.Entry(entity).State = EntityState.Added;
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual BaseResult Add(IEnumerable<T> list)
        {
            try
            {
                foreach (var entity in list)
                {
                    Context.Entry(entity).State = EntityState.Added;
                }
                Context.SaveChanges();
                return new BaseResult();
            }
            catch (DbEntityValidationException dbEx)
            {
                LogHelper.Error(GetFullErrorText(dbEx), dbEx);
                return new BaseResult("新增失败");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
                return new BaseResult("新增失败");
            }
        }

        /// <summary>
        /// 批量插入实体，不保存到数据库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual void AddNoSave(IEnumerable<T> list)
        {
            foreach (var entity in list)
            {
                Context.Entry(entity).State = EntityState.Added;
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual BaseResult Delete(T entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Deleted;
                Context.SaveChanges();
                return new BaseResult();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
                return new BaseResult("删除失败");
            }
        }

        /// <summary>
        /// 删除实体，不保存到数据库
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void DeleteNosave(T entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual BaseResult Delete(IEnumerable<T> list)
        {
            try
            {
                foreach (var entity in list)
                {
                    Context.Entry<T>(entity).State = EntityState.Deleted;
                }
                Context.SaveChanges();
                return new BaseResult();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
                return new BaseResult("删除失败");
            }
        }

        /// <summary>
        /// 批量删除实体，不保存到数据库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual void DeleteNoSave(IEnumerable<T> list)
        {
            foreach (var entity in list)
            {
                Context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        /// <summary>
        /// 根据条件批量删除实体
        /// </summary>
        /// <param name="lamda"></param>
        /// <returns></returns>
        public virtual BaseResult Delete(Expression<Func<T, bool>> lamda)
        {
            try
            {
                var list = Entities.Where(lamda);
                foreach (var entity in list)
                {
                    Context.Entry<T>(entity).State = EntityState.Deleted;
                }
                Context.SaveChanges();
                return new BaseResult();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
                return new BaseResult("删除失败");
            }
        }

        /// <summary>
        /// 根据条件批量删除实体，不保存到数据库
        /// </summary>
        /// <param name="lamda"></param>
        /// <returns></returns>
        public virtual void DeleteNoSave(Expression<Func<T, bool>> lamda)
        {
            var list = Entities.Where(lamda);
            foreach (var entity in list)
            {
                Context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        public virtual BaseResult Save()
        {
            try
            {
                Context.SaveChanges();
                return new BaseResult();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
                return new BaseResult("保存失败");
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
