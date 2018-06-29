using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Data
{
    public class BaseResult
    {
        public BaseResult()
        {
            Result = true;
        }

        public BaseResult(string msg)
        {
            Result = false;
            Errmsg = msg;
        }

        public BaseResult(bool result,string code)
        {
            Result = result;
            Code = code;
        }

        public BaseResult(bool result,string code,string msg)
        {
            Result = result;
            Code = code;
            Errmsg = msg;
        }

        public bool Result { get; set; }
        public string Code { get; set; }
        public string Errmsg { get; set; }
    }
}
