
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using Microsoft.Win32;


namespace SF.Expand.Logger
{
    public static class LOGGER
    {

        private const string cBASEAPPNAME = "LOGGER";
        private const string cBASE_NAME = @"http://logger.softfinanca.com/";
        private const string cTRACE_LEVEL = "TRACE_LEVEL";
        private const string cTRACE_FILEPATH = "TRACE_FILEPATH";
        private const string cTRACE_BASEFILENAME = "TRACE_BASEFILENAME";
        private const string cTRACE_FILELOCKDELAY = "TRACE_FILELOCKDELAY";
        private const string cTRACE_FILENAMEFORMATER = "TRACE_FILENAMEFORMATER";
        private const string cBASE_FILEFORMATER = "yyyyMMddHH";
        private const int cBASE_FILELOCKDELAY = 1000;


        private static TextWriter s_Writer = null;
        private static Object s_Lock = new object();


        /// <summary>
        /// </summary>
        [Serializable]
        public enum LOGGEREventID { EXCEPTION = 1, ERROR = 2, WARNING = 3, INFORMATION = 4 };


        /// <summary>
        /// </summary>
        /// <param name="_option"></param>
        /// <returns></returns>
        private static string[] _getMachineInfo(int _option)
        {
            Process _thisProc = null;
            string _procName = null;

            try
            {
                switch (_option)
                {
                    case 1:
                        _procName = Registry.LocalMachine.OpenSubKey(@"hardware\description\system\centralprocessor\0").GetValue("ProcessorNameString").ToString();
                        return new string[] { 
                            "# " + Assembly.GetExecutingAssembly().FullName.ToString(),
                            string.Format("# machine/ name:{0}; CPU(s):[count:{1} type:{2}]; OS:{3}; CLR:{4}",Environment.MachineName,Environment.ProcessorCount,_procName,Environment.OSVersion.VersionString,Environment.Version.ToString())};
                    case 2:
                        _thisProc = Process.GetCurrentProcess();
                        return new string[] { 
                            string.Format("# process/ startTime:{0}; totalProcessorTime:{1} Priority/ ({2});{3}",_thisProc.StartTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),_thisProc.TotalProcessorTime, _thisProc.BasePriority, _thisProc.PriorityClass),
                            string.Format("#          virtual memory allocated:{0}MBytes; physical allocated:{1}MBytes; used memory:{2}MBytes",(_thisProc.VirtualMemorySize64/1024f)/1024f,(_thisProc.WorkingSet64/1024f)/1024f,(_thisProc.PrivateMemorySize64/1024f)/1024f)};
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (_thisProc != null)
                {
                    _thisProc.Dispose();
                }
                _thisProc = null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="loggerEventID"></param>
        /// <param name="appBASEid"></param>
        /// <param name="appMSGtext"></param>
        /// <returns></returns>
        private static string _formatLogMSG(LOGGEREventID loggerEventID, string appBASEid, string[] appMSGtext)
        {
            try
            {
                return string.Format(@"{0} [{1}] [{2}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), loggerEventID.ToString(), Process.GetCurrentProcess().ProcessName) + " " + string.Join(" ", appMSGtext) +
                    (loggerEventID == LOGGEREventID.EXCEPTION ? Environment.NewLine + _getMachineInfo(2)[0] + Environment.NewLine + _getMachineInfo(2)[1] + Environment.NewLine : "");
            }
            catch
            {
                return "... [ unable to format message ] ...";
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="evtLogType"></param>
        /// <param name="evtLogText"></param>
        private static void writeToEventLog(EventLogEntryType evtLogType, string evtLogText)
        {
            try
            {
                if (!EventLog.SourceExists(cBASEAPPNAME, @"."))
                {
                    EventLog.CreateEventSource(new EventSourceCreationData(cBASEAPPNAME, cBASEAPPNAME));
                }
                new EventLog(cBASEAPPNAME, @".", cBASEAPPNAME).WriteEntry(evtLogText, evtLogType);
            }
            catch (Exception ex)
            {
                Debug.Write(_formatLogMSG(LOGGEREventID.EXCEPTION, cBASE_NAME, new string[] { "unable to write on EventLOG", ex.ToString() }));
                Debug.Write(evtLogText);
            }
        }



        /// <summary>
        /// </summary>
        /// <param name="_filelockDelay"></param>
        /// <param name="_baseFilePath"></param>
        /// <param name="_logMessage"></param>
        private static void writeToFile(string fileBaseName, string _logMessage)
        {
            int _filelockDelay = 0;
            string _baseFilePath = null;
            string _fileFormater = null;
            string _filePathName = null;


            try
            {
                if (null == (_fileFormater = ConfigurationManager.AppSettings.Get(cTRACE_FILENAMEFORMATER)))
                {
                    _fileFormater = cBASE_FILEFORMATER;
                }
                if (null == (_filePathName = ConfigurationManager.AppSettings.Get(cTRACE_FILEPATH)))
                {
                    _filePathName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }
                _baseFilePath = Path.Combine(_filePathName, (string.Format(fileBaseName.Trim() + "{0}.LOG", DateTime.Now.ToString(_fileFormater))));
                int.TryParse("0" + ConfigurationManager.AppSettings.Get(cTRACE_FILELOCKDELAY), out _filelockDelay);
            }
            catch (Exception ex)
            {
                Debug.Write(_formatLogMSG(LOGGEREventID.WARNING, cBASE_NAME, new string[] { "unable to write message!", ex.ToString() }));
                Debug.Write(_logMessage); return;
            }

            if (Monitor.TryEnter(s_Lock, (_filelockDelay < cBASE_FILELOCKDELAY ? cBASE_FILELOCKDELAY : _filelockDelay)))
            {
                try
                {
                    lock (s_Lock)
                    {

                        if (s_Writer == null)
                        {
                            if (!File.Exists(_baseFilePath))
                            {
                                s_Writer = File.CreateText(_baseFilePath);
                                string[] _fileHeaders = _getMachineInfo(1);
                                s_Writer.WriteLine(_fileHeaders[0] + Environment.NewLine + _fileHeaders[1] + Environment.NewLine);
                            }
                            else
                            {
                                s_Writer = File.AppendText(_baseFilePath);
                            }
                        }
                        s_Writer.WriteLine(_logMessage);
                        s_Writer.Flush(); s_Writer.Close();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Write(_formatLogMSG(LOGGEREventID.WARNING, cBASE_NAME, new string[] { "unable to write message!", ex.ToString() }));
                    Debug.Write(_logMessage);
                }
                finally
                {
                    s_Writer = null;
                    Monitor.Exit(s_Lock);
                }
            }
            else
            {
                writeToEventLog(EventLogEntryType.Error, _formatLogMSG(LOGGEREventID.EXCEPTION, cBASE_NAME, new string[] { "Timeout; could not obtain lock to log file!" }) + Environment.NewLine + _logMessage);
                Debug.Write(_formatLogMSG(LOGGEREventID.WARNING, cBASE_NAME, new string[] { "Timeout; could not obtain lock to log file!" }));
            }
        }







        /// <summary>
        /// </summary>
        /// <param name="loggerEventID"></param>
        /// <param name="appBASEid"></param>
        /// <param name="appMSGtext"></param>
        public static void Write(LOGGEREventID loggerEventID, string appBASEid, string[] appMSGtext)
        {
            int _DEPLOYTRACELevel = 0;

            try
            {
                if (-9 == (_DEPLOYTRACELevel = int.Parse("0" + ConfigurationManager.AppSettings.Get(cTRACE_LEVEL))))
                {
                    Debug.Write(_formatLogMSG(loggerEventID, appBASEid, appMSGtext));
                    if ((int)loggerEventID != -1) return;
                }
                if ((int)loggerEventID > _DEPLOYTRACELevel)
                {
                    return;
                }
                writeToFile(appBASEid, _formatLogMSG(loggerEventID, appBASEid, appMSGtext));

            }
            catch (Exception ex)
            {
                writeToEventLog(EventLogEntryType.Error, _formatLogMSG(LOGGEREventID.EXCEPTION, cBASE_NAME, new string[] { ex.ToString(), "unable to dump message log!!" }) + Environment.NewLine + _formatLogMSG(loggerEventID, appBASEid, appMSGtext));
                return;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="loggerEventID"></param>
        /// <param name="appBASEid"></param>
        /// <param name="appMSGtext"></param>
        public static void dump(LOGGEREventID loggerEventID, string appBASEid, string[] appMSGtext)
        {
            writeToFile(appBASEid, _formatLogMSG(loggerEventID, appBASEid, appMSGtext));
        }
    }
}