using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.Security.Entities
{
    public class Result
    {
        public Result(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
