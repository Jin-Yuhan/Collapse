﻿using System;
using System.IO;
using System.Text;

namespace Hi3Helper
{
    public class LoggerBase
    {
        #region Properties
        private StreamWriter _logWriter { get; set; }
        private string _logFolder { get; set; }
        private string _logPath { get; set; }
        private StringBuilder _stringBuilder { get; set; }
        #endregion

        #region Statics
        public static string GetCurrentTime(string format) => DateTime.Now.ToLocalTime().ToString(format);
        #endregion

        public LoggerBase(string logFolder, Encoding logEncoding)
        {
            // Initialize the writer and _stringBuilder
            _stringBuilder = new StringBuilder();
            SetFolderPathAndInitialize(logFolder, logEncoding);
        }

        #region Methods
        public void SetFolderPathAndInitialize(string folderPath, Encoding logEncoding)
        {
            // Set the folder path of the stored log
            _logFolder = folderPath;

            // Check if the directory exist. If not, then create.
            if (!Directory.Exists(_logFolder))
            {
                Directory.CreateDirectory(_logFolder);
            }

            // Try dispose the _logWriter even though it's not initialized.
            // This will be used if the program need to change the log folder to another location.
            _logWriter?.Dispose();

            try
            {
                // Initialize writer and the path of the log file.
                InitializeWriter(false, logEncoding);
            }
            catch
            {
                // If the initialization above fails, then use fallback.
                InitializeWriter(true, logEncoding);
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async void LogWriteLine() { }
        public virtual async void LogWriteLine(string line = null) { }
        public virtual async void LogWriteLine(string line, LogType type) { }
        public virtual async void LogWriteLine(string line, LogType type, bool writeToLog) { }
        public virtual async void LogWrite(string line, LogType type, bool writeToLog, bool fromStart) { }
        public async void WriteLog(string line, LogType type) => _logWriter?.WriteLine(GetLine(line, type, false));
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        #endregion

        #region ProtectedMethods
        /// <summary>
        /// Get the line for displaying or writing into the log based on their type
        /// </summary>
        /// <param name="line">Line for the log you want to return</param>
        /// <param name="type">Type of the log. The type will be added in the return</param>
        /// <param name="isForDisplaying">To indicate if the return will be used for writing into log or just displaying</param>
        /// <returns>Decorated line with timestamp if isForDisplaying is false or colored if isForDisplaying is true</returns>
        protected string GetLine(string line, LogType type, bool isForDisplaying)
        {
            lock (_stringBuilder)
            {
                // Clear the _stringBuilder
                _stringBuilder.Clear();

                // If it's used for display only, then append color string + label.
                // Else, append label + timestamp
                if (isForDisplaying)
                {
                    _stringBuilder.Append(GetColorizedString(type) + GetLabelString(type) + "\u001b[0m");
                }
                else
                {
                    _stringBuilder.Append(GetLabelString(type));

                    // Append timestamp for write log
                    if (type != LogType.NoTag)
                    {
                        _stringBuilder.Append($" [{GetCurrentTime("HH:mm:ss.fff")}]");
                    }
                    else
                    {
                        _stringBuilder.Append(new string(' ', 15));
                    }
                }

                // Append spaces between labels and text
                _stringBuilder.Append("  ");
                _stringBuilder.Append(line);

                return _stringBuilder.ToString();
            }
        }

        protected void DisposeBase() => _logWriter?.Dispose();
        #endregion

        #region PrivateMethods
        private void InitializeWriter(bool isFallback, Encoding logEncoding)
        {
            // Initialize _logPath and get fallback string at the end of the filename if true or none if false.
            string fallbackString = isFallback ? ("-f" + Path.GetFileNameWithoutExtension(Path.GetTempFileName())) : string.Empty;
            string dateString = GetCurrentTime("yyyy-MM-dd");
            _logPath = Path.Combine(_logFolder, $"Log-{dateString + fallbackString}.log");

            // Initialize _logWriter to the given _logPath.
            // This time, the FileStream is getting the FileShare.Write to avoid
            // throw while the same file is being used by more than one instance.
            FileStream fs = new FileStream(_logPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite); // FileMode.OpenOrCreate is append = true in StreamWriter.
            _logWriter = new StreamWriter(fs, logEncoding) { AutoFlush = true };
        }

        private ArgumentException ThrowInvalidType() => new ArgumentException("Type must be Default, Error, Warning, Scheme, Game, NoTag or Empty!");

        /// <summary>
        /// Get the ASCII color in string form.
        /// </summary>
        /// <param name="type">The type of the log</param>
        /// <returns>A string of the ASCII color</returns>
        private string GetColorizedString(LogType type) => type switch
        {
            LogType.Default => "\u001b[32;1m",
            LogType.Error => "\u001b[31;1m",
            LogType.Warning => "\u001b[33;1m",
            LogType.Scheme => "\u001b[34;1m",
            LogType.Game => "\u001b[35;1m",
            _ => string.Empty
        };

        /// <summary>
        /// Get the label string based on log type.
        /// </summary>
        /// <param name="type">The type of the log</param>
        /// <returns>A string of the label based on type</returns>
        /// <exception cref="ArgumentException"></exception>
        private string GetLabelString(LogType type) => type switch
        {
            LogType.Default => "[Info]",
            LogType.Error => "[Erro]",
            LogType.Warning => "[Warn]",
            LogType.Scheme => "[Schm]",
            LogType.Game => "[Game]",
            LogType.NoTag => "      ",
            _ => throw ThrowInvalidType()
        };
        #endregion
    }
}
