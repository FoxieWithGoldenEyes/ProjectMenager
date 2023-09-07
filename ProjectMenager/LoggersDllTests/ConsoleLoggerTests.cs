using APIsDll.Local.APIs;
using APIsDll.Local.Models;
using LoggersDll.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggersDllTests
{
    [TestClass]
    public class ConsoleLoggerTests
    {
        [TestMethod]
        public void LogMessage__DoesNotThorwException()
        {
            ConsoleLogger logger = new ConsoleLogger();
            LogMessage logMessage = new LogMessage("Hello World");
            logger.LogMessage(logMessage);
        }
    }
}
