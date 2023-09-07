using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace APIsDll.Local.Models
{
    public class LogMessage
    {
        public enum MessageType
        {
            Tract, Info, Warn, Error, Critical
        }

        public LogMessage(string message)
            : this(MessageType.Info, message) { }
        public LogMessage(MessageType messageType, string message)
        {
            this.Type = messageType;
            this.OnlyMessage = message;
            this.TimeStamp = TimeOnly.FromDateTime(DateTime.Now);
        }


        public MessageType Type { get; }
        public string OnlyMessage { get; }
        public TimeOnly TimeStamp { get; }


        public string GetAllMessage() => ToString();


        public override string ToString()
        {
            return $"[{TimeStamp}] {Type}: {OnlyMessage}";
        }
    }
}
