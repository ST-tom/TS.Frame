using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Data
{
    public class EFResult
    {
        public bool Result { get; set; }
        public string Errmsg { get; set; }

        public EFResult()
        {
            this.Result = true;
            this.Errmsg = "";

        }

        public EFResult(string errmsg)
        {
            this.Result = false;
            this.Errmsg = errmsg;
        }
    }
}
