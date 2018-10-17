using Microsoft.VisualStudio.TestTools.UnitTesting;
using Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cerberus.Sos.Accounting.Log;

namespace Log.Tests
{
    [TestClass()]
    public class LogHelperTests
    {
        [TestMethod()]
        public void RegisterErrorTest()
        {
            LogHelper.RegisterError("Fatal error nooooooooooo");
        }
    }
}