using System;

using Crunchy.Services.Interfaces;

namespace Crunchy.Services {
    public class DevLoggerService : ILoggerService {

        public void DevInfo(string message) {
            Write("DEV", message);
        }

        public void Error(string message) {
            Write("ERR", message);
        }

        public void Info(string message) {
            Write("INF", message);
        }

        public void Client(string message) {
            Write("CLI", message);
        }

        public void Write(string msgType, string message) {
            System.Console.WriteLine("*** [" + msgType + "] [" + TimeNow() + "]: " + message);
        }

        public static string TimeNow() {
            return DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}