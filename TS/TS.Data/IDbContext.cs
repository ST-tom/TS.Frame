using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Core;

namespace TS.Data
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() 
            where TEntity : BaseEntity;

        int SaveChanges();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) 
            where TEntity : BaseEntity;
    }
}
