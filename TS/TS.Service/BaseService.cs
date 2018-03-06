using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data;

namespace TS.Service
{
    public class BaseService
    {
        public readonly EFRepository<TSContext> EFRepository;
        public BaseService()
        {
            EFRepository = new EFRepository<TSContext>();
        }
    }
}
