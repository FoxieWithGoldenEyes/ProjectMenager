using APIsDll.Local.APIs;
using APIsDll.Local.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggersDll.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void LogMessage(LogMessage message)
        {
            Console.WriteLine(message.GetAllMessage());
        }

        public void LogMessage(string message) => LogMessage(new LogMessage(message));
    }
}
