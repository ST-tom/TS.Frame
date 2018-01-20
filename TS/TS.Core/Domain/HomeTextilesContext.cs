using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Core.Domain
{
    public  class HomeTextilesContext : DbContext
    {
        static HomeTextilesContext()
        {
            //不通过EF建立数据库
            //Database.SetInitializer<HomeTextilesContext>(new CreateDatabaseIfNotExists<HomeTextilesContext>());
            //using (var context = new HomeTextilesContext())
            //{
            //    context.Database.Initialize(false);
            //}
        }

        public HomeTextilesContext()
            : base("XingXingHomeTextilesContext")
        {

        }
    }
}
