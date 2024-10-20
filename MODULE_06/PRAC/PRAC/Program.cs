using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace PRAC
{
    public enum LogLevel
    {
        INFO,
        WARNING,
        ERROR
    }

    public class Logger
    {
        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
        private static readonly object _lock = new object();
        private LogLevel _currentLogLevel;
        private string _logFilePath;

        private Logger()
        {
            LoadConfiguration();
            _currentLogLevel = LogLevel.INFO;
        }

        public static Logger GetInstance()
        {
            return _instance.Value;
        }

        public void Log(string message, LogLevel level)
        {
            if (level >= _currentLogLevel)
            {
                lock (_lock)
                {
                    using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                    {
                        writer.WriteLine($"{DateTime.Now}: [{level}] {message}");
                    }
                }
            }
        }

        public void SetLogLevel(LogLevel level)
        {
            lock (_lock)
            {
                _currentLogLevel = level;
            }
        }

        private void LoadConfiguration()
        {
            string configFilePath = "loggerConfig.json";

            if (File.Exists(configFilePath))
            {
                var config = JsonConvert.DeserializeObject<LoggerConfig>(File.ReadAllText(configFilePath));
                _logFilePath = config.LogFilePath;
                _currentLogLevel = config.LogLevel;
            }
            else
            {
                _logFilePath = "logs.txt";
                _currentLogLevel = LogLevel.INFO;
            }
        }

        private class LoggerConfig
        {
            public string LogFilePath { get; set; }
            public LogLevel LogLevel { get; set; }
        }
    }

    public class LogReader
    {
        private string _logFilePath;

        public LogReader(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void ReadLogs(LogLevel? filterLevel = null)
        {
            using (StreamReader reader = new StreamReader(_logFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (filterLevel == null || line.Contains($"[{filterLevel}]"))
                    {
                        Console.WriteLine(line);
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                int threadId = i;
                Thread thread = new Thread(() =>
                {
                    Logger logger = Logger.GetInstance();
                    logger.Log($"Message from thread {threadId}", (LogLevel)(threadId % 3));
                });
                thread.Start();
            }

            Thread.Sleep(1000);
            Logger.GetInstance().SetLogLevel(LogLevel.WARNING);

            LogReader logReader = new LogReader("logs.txt");
            logReader.ReadLogs();
        }
    }
}