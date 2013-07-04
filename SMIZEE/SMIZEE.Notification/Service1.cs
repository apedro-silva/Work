using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SF.Expand.Logger;

namespace SMIZEE.Notification
{
    public partial class SMIZEENotification : ServiceBase
    {
        private const string cMODULE_NAME = "SMIZEE.Notification";
        private const string cBASE_NAME = @"http://sf.expand.com/";
        Timer NewFormsTimer = new Timer();
        Timer LateFormsTimer = new Timer();

        public SMIZEENotification()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LOGGER.Write(LOGGER.LOGGEREventID.INFORMATION, cMODULE_NAME, new string[] { cBASE_NAME, "Starting..." });
            double newFormsAlarmInterval = 0;
            double lateFormsAlarmInterval = 0;

            System.Threading.Thread.Sleep(20000);

            try
            {
                newFormsAlarmInterval = Properties.Settings.Default.NewFormsAlarmInterval;
                lateFormsAlarmInterval = Properties.Settings.Default.LateFormsAlarmInterval;

                Forms.ProcessUserProductionUnitsForms(Properties.Settings.Default.ReferenceStartDate);
                LateForms.ProcessLateSubmissions();
            }
            catch (Exception exp)
            {
                LOGGER.Write(LOGGER.LOGGEREventID.EXCEPTION, cMODULE_NAME, new string[] { cBASE_NAME, exp.Message });
            }

            NewFormsTimer.Elapsed += NewFormsTimer_Elapsed;
            NewFormsTimer.Enabled = true;
            NewFormsTimer.Interval = newFormsAlarmInterval  * 60 * 60 * 1000;
            NewFormsTimer.Start();

            LateFormsTimer.Elapsed += LateFormsTimer_Elapsed;
            LateFormsTimer.Enabled = true;
            LateFormsTimer.Interval = lateFormsAlarmInterval * 60 * 60 * 1000;
            LateFormsTimer.Start();

        }

        void NewFormsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LOGGER.Write(LOGGER.LOGGEREventID.INFORMATION, cMODULE_NAME, new string[] { cBASE_NAME, "Processing new form notifications!" });
            NewFormsTimer.Stop();

            try
            {
                Forms.ProcessUserProductionUnitsForms(Properties.Settings.Default.ReferenceStartDate);
            }
            catch (Exception exp)
            {
                LOGGER.Write(LOGGER.LOGGEREventID.EXCEPTION, cMODULE_NAME, new string[] { cBASE_NAME, exp.Message });
            }
            finally
            {
                NewFormsTimer.Start();
            }
        }

        void LateFormsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LOGGER.Write(LOGGER.LOGGEREventID.INFORMATION, cMODULE_NAME, new string[] { cBASE_NAME, "Processing alerts notifications!" });
            LateFormsTimer.Stop();

            try
            {
                LateForms.ProcessLateSubmissions();
            }
            catch (Exception exp)
            {
                LOGGER.Write(LOGGER.LOGGEREventID.EXCEPTION, cMODULE_NAME, new string[] { cBASE_NAME, exp.Message });
            }
            finally
            {
                LateFormsTimer.Start();
            }
        }

        protected override void OnStop()
        {
            NewFormsTimer.Stop();
            LateFormsTimer.Stop();
            LOGGER.Write(LOGGER.LOGGEREventID.INFORMATION, cMODULE_NAME, new string[] { cBASE_NAME, "Stopped!" });
        }

    }
}
