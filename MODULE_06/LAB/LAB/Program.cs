using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace LAB
{
    public enum LogLevel
    {
        INFO,
        WARNING,
        ERROR
    }

    public sealed class Logger
    {
        private static readonly Lazy<Logger> _instance =
            new Lazy<Logger>(() => new Logger());

        private LogLevel _currentLogLevel = LogLevel.INFO;
        private string _logFilePath = "log.txt";
        private readonly object _lock = new object();

        private Logger() { }

        public static Logger GetInstance()
        {
            return _instance.Value;
        }

        public void SetLogLevel(LogLevel level)
        {
            _currentLogLevel = level;
        }

        public void SetLogFilePath(string path)
        {
            _logFilePath = path;
        }

        public void Log(string message, LogLevel level)
        {
            if (level < _currentLogLevel) return;

            lock (_lock)
            {
                File.AppendAllText(_logFilePath, $"{DateTime.Now}: [{level}] {message}{Environment.NewLine}");
            }
        }

        public void ReadLogs()
        {
            lock (_lock)
            {
                if (File.Exists(_logFilePath))
                {
                    string logs = File.ReadAllText(_logFilePath);
                    Console.WriteLine(logs);
                }
                else
                {
                    Console.WriteLine("Лог-файл не найден.");
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = Logger.GetInstance();
            logger.SetLogLevel(LogLevel.INFO);
            logger.SetLogFilePath("log.txt");

            var threads = new Thread[5];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    logger.Log("Это информационное сообщение.", LogLevel.INFO);
                    logger.Log("Это предупреждение.", LogLevel.WARNING);
                    logger.Log("Это ошибка.", LogLevel.ERROR);
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            logger.ReadLogs();
        }
    }
}