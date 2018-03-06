using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TS.Core.Domain.Orders;

namespace TS.Data
{
    public  class TSContext : DbContext
    {
        static TSContext()
        {
            //不通过EF建立数据库
            //Database.SetInitializer<HomeTextilesContext>(new CreateDatabaseIfNotExists<HomeTextilesContext>());
            //using (var context = new HomeTextilesContext())
            //{
            //    context.Database.Initialize(false);
            //}
        }

        public TSContext()
            : base("TSContext")
        {

        }

        //public virtual DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(TSEntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
