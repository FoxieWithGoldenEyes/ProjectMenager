using APIsDll.Local.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIsDll.Local.APIs
{
    public interface ILogger
    {
        void LogMessage(LogMessage message);
        void LogMessage(string message);
    }
}
