using APIsDll.Local.APIs;
using APIsDll.Local.Models;
using IODll;
using LoggersDll.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggersDllTests
{
    [TestClass]
    public class FileLoggerTests
    {
        static string _logFolderPath = Path.Combine(Environment.CurrentDirectory, "Logs");


        // Runs before every test
        [TestInitialize]
        public void InitializeTests()
        {
            Directory.CreateDirectory(_logFolderPath);
        }

        // Runs after every test
        [TestCleanup]
        public void CleanupTests()
        {
            Directory.Delete(_logFolderPath, true);
        }



        [TestMethod]
        public void LogMessage__LogsMessage()
        {
            ILogger fileLogger = new FileLogger(_logFolderPath);
            fileLogger.LogMessage("Hello World");
        }
    }
}
