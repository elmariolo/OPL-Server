using System;
using System.IO;
using System.Reflection;
using Utilities;

namespace OplServer.Interface
{
    public class LogWriter
    {
        private FileStream m_logFile;
        private DateTime? m_logFileDate;
        private readonly string m_logsDirectoryPath;
        private readonly object m_syncLock = new object();

        public LogWriter()
        {
            string assemblyDirectory = GetAssemblyDirectory();
            m_logsDirectoryPath = assemblyDirectory + @"Logs\";
        }

        public LogWriter(string logsDirectoryPath)
        {
            m_logsDirectoryPath = logsDirectoryPath;
        }

        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        private void OpenLogFile()
        {
            if (m_logFileDate.HasValue && m_logFileDate.Value != DateTime.Today && m_logFile != null)
            {
                m_logFile.Close();
                m_logFile = null;
                m_logFileDate = null;
            }

            if (m_logFileDate == null)
            {
                m_logFileDate = DateTime.Today;
                string logFilePath = string.Format("{0}{1}-{2}-{3}.log", m_logsDirectoryPath, DateTime.Now.Year,
                    DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"));
                try
                {
                    if (!Directory.Exists(m_logsDirectoryPath)) Directory.CreateDirectory(m_logsDirectoryPath);
                    m_logFile = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read, 0x1000,
                        FileOptions.WriteThrough);
                }
                catch
                {
                }
            }
        }

        public void CloseLogFile()
        {
            if (m_logFile != null)
                lock (m_syncLock)
                {
                    m_logFile.Close();
                    m_logFile = null;
                }
        }

        public void WriteLine(string value, params object[] args)
        {
            WriteLine(string.Format(value, args));
        }

        public void WriteLine(string value)
        {
            lock (m_syncLock)
            {
                OpenLogFile();
                if (m_logFile != null)
                {
                    var writer = new StreamWriter(m_logFile);
                    writer.WriteLine(value);
                    writer.Flush();
                }
            }
        }

        public void OnLogEntryAdded(object sender, LogEntry entry)
        {
            if (entry.Severity != Severity.Trace)
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                WriteLine("{0} {1} [{2}] {3}", entry.Severity.ToString().PadRight(12), timestamp, entry.Source,
                    entry.Message);
            }
        }

        public static string GetAssemblyDirectory()
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null) assembly = Assembly.GetExecutingAssembly();
            string assemblyDirectory = Path.GetDirectoryName(assembly.Location);
            if (!assemblyDirectory.EndsWith(@"\")) assemblyDirectory += @"\";
            return assemblyDirectory;
        }
    }
}