using APIsDll.Local.APIs;
using APIsDll.Local.Models;
using IODll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggersDll.Loggers
{
    public class FileLogger : ILogger
    {
        public FileLogger(string LogDir) 
        {
            string logFileName = DateOnly.FromDateTime(DateTime.Now).ToString();
            string extenstion = ".log";
            string path = Path.Combine(LogDir, logFileName + extenstion);

            LogFile = new AppFile<string>(path, AppFile<string>.FileType.LinedText);
        }
        public FileLogger(AppFile<string> logFile)
        {
            LogFile = logFile;
        }


        public AppFile<string> LogFile { get; private set; }


        public void LogMessage(LogMessage message)
        {
            LogFile.Add(message.GetAllMessage());
        }

        public void LogMessage(string message) => LogMessage(new LogMessage(message));
    }
}
