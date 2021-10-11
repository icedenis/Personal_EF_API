using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Personal_EF_API.Interfaces;
using NLog;

namespace Personal_EF_API.Services
{
    public class LoggerService : ILoggerService
    {
        // ILogger is from NLogs not from Microsoft
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public void LogDebug(string message)
        {
            logger.Debug(message);
           
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
