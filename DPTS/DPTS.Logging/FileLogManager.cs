using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using DPTS.Logging.Contracts;
using DPTS.Logging.Models;
using Newtonsoft.Json;

namespace DPTS.Logging
{
    public class FileLogManager : ILogManager
    {
        public string ExecutingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public void LogApplicationCalls(EventEntry eventLogEntry)
        {
            WriteLog(eventLogEntry, "log.txt");
            //Writing logs commented for now, Exceptions can be written in diffrent files
        }

        private void WriteLog(EventEntry eventEntry, string fileType)
        {
            try
            {
                using (var txtWriter = File.AppendText(ExecutingAssemblyPath + "\\" + fileType))
                {
                    txtWriter.WriteLine("==================== {0} ====================\n",
                        DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    var result = JsonConvert.SerializeObject(eventEntry);
                    result = result.Replace("{", "").Replace("}", "").Replace(",", " =|= \n");
                    txtWriter.WriteLine("{0}", result);
                    txtWriter.WriteLine("=============================================================\n\n");
                }
            }
            catch (Exception ex)
            {
                WriteLog(new ExceptionEntry {Exception = ex.ToString()}, "exception.txt");
            }
        }
    }
}