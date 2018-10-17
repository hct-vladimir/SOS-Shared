using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Cerberus.Sos.Accounting.Security.Entities
{
    public class Role : IRole<int>
    {
        public int Id { get; }
        public string Name { get; set; }
    }
}
