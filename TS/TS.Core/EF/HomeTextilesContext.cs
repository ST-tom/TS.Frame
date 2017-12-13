using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Core.EF
{
    public  class HomeTextilesContext : DbContext
    {
        static HomeTextilesContext()
        {
            Database.SetInitializer<HomeTextilesContext>(new CreateDatabaseIfNotExists<HomeTextilesContext>());
            using (var context = new HomeTextilesContext())
            {
                context.Database.Initialize(true);
            }
        }

        public HomeTextilesContext()
            : base("XingXingHomeTextilesContext")
        {

        }
    }
}
