using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using SMIZEE.Models;
using SF.Expand.Logger;
using SF.Expand.Notification;

namespace SMIZEE.Notification
{
    public static class Goodies
    {
        private const string cMODULE_NAME = "SMIZEE.Notification.Goodies";
        private const string cBASE_NAME = @"http://sf.expand.com/";

        public static void SendEmail(string notificationMessage, int productionUnitId, int functionalAreaId, string formDescription, int formPeriod, string periodicityCode, DateTime formDate)
        {
            string userEmail = null;
            string logText = null;
            string baseParams = Properties.Settings.Default.NotificationParams;
            string notificationServiceTypeName = Properties.Settings.Default.NotificationTypeName;
            string subject = Properties.Settings.Default.NotificationSubject;
            string messageBody = null;

            try
            {
                // get all users of passed Production Unit
                // send an email to each user
                IQueryable<UserProductionUnit> usersList = CxUserProductionUnit.GetListByProductionUnit(productionUnitId);
                foreach (UserProductionUnit currentUser in usersList.ToList())
                {
                    User user = CxUser.GetUserById(currentUser.UserID);

                    // if user on passed Functional Area send an email
                    if (!user.IsLockedOut && (user.FunctionalAreaID == functionalAreaId))
                    {
                        Company userCompany = CxCompany.GetCompany(user.CompanyID);

                        userEmail = user.Email;
                        messageBody = string.Format(notificationMessage, user.Username, userCompany.CompanyName, formDescription, GetPeriodMessage(formPeriod, periodicityCode, formDate));
                        logText = string.Format("sending an email to user {0} with form : {1} and message {2}", userEmail, formDescription, messageBody);
                        LOGGER.Write(LOGGER.LOGGEREventID.INFORMATION, cMODULE_NAME, new string[] { cBASE_NAME, logText });

                        NotificationService.Send(baseParams, notificationServiceTypeName, userEmail, subject, messageBody);
                    }
                }
            }
            catch (Exception exp)
            {
                LOGGER.Write(LOGGER.LOGGEREventID.EXCEPTION, cMODULE_NAME, new string[] { cBASE_NAME, exp.Message });
            }
        }
        public static void SendManagerEmail(string notificationMessage, int productionUnitId, int functionalAreaId, string formDescription, int formPeriod, string periodicityCode, DateTime formDate)
        {
            string userEmail = null;
            string logText = null;
            string baseParams = Properties.Settings.Default.NotificationParams;
            string notificationServiceTypeName = Properties.Settings.Default.NotificationTypeName;
            string subject = Properties.Settings.Default.NotificationSubject;
            string messageBody = null;

            try
            {
                // get all users of passed Production Unit
                // send an email to each user
                IQueryable<UserProductionUnit> usersList = CxUserProductionUnit.GetListByProductionUnit(productionUnitId);
                foreach (UserProductionUnit currentUser in usersList.ToList())
                {
                    User user = CxUser.GetUserById(currentUser.UserID);

                    // if user is Manager send an email
                    if ((bool)user.IsManager && !user.IsLockedOut)
                    {
                        Company userCompany = CxCompany.GetCompany(user.CompanyID);

                        userEmail = user.Email;
                        messageBody = string.Format(notificationMessage, user.Username, userCompany.CompanyName, formDescription, GetPeriodMessage(formPeriod, periodicityCode, formDate));
                        logText = string.Format("sending an email to manager {0} with form : {1} and message {2}", userEmail, formDescription, messageBody);
                        LOGGER.Write(LOGGER.LOGGEREventID.INFORMATION, cMODULE_NAME, new string[] { cBASE_NAME, logText });

                        NotificationService.Send(baseParams, notificationServiceTypeName, userEmail, subject, messageBody);
                    }
                }
            }
            catch (Exception exp)
            {
                LOGGER.Write(LOGGER.LOGGEREventID.EXCEPTION, cMODULE_NAME, new string[] { cBASE_NAME, exp.Message });
            }
        }
        public static void SendExecutiveEmail(string notificationMessage, int productionUnitId, int functionalAreaId, string formDescription, int formPeriod, string periodicityCode, DateTime formDate)
        {
            string userEmail = null;
            string logText = null;
            string baseParams = Properties.Settings.Default.NotificationParams;
            string notificationServiceTypeName = Properties.Settings.Default.NotificationTypeName;
            string subject = Properties.Settings.Default.NotificationSubject;
            string messageBody = null;

            try
            {
                // get all users of passed Production Unit
                // send an email to each user
                IQueryable<UserProductionUnit> usersList = CxUserProductionUnit.GetListByProductionUnit(productionUnitId);
                foreach (UserProductionUnit currentUser in usersList.ToList())
                {
                    User user = CxUser.GetUserById(currentUser.UserID);

                    // if user is Manager send an email
                    if ((bool)user.IsExecutive && !user.IsLockedOut)
                    {
                        userEmail = user.Email;
                        messageBody = string.Format(notificationMessage, formDescription, GetPeriodMessage(formPeriod, periodicityCode, formDate));
                        logText = string.Format("sending an email to executive {0} with form : {1} and message {2}", userEmail, formDescription, messageBody);
                        LOGGER.Write(LOGGER.LOGGEREventID.INFORMATION, cMODULE_NAME, new string[] { cBASE_NAME, logText });

                        NotificationService.Send(baseParams, notificationServiceTypeName, userEmail, subject, messageBody);
                    }
                }
            }
            catch (Exception exp)
            {
                LOGGER.Write(LOGGER.LOGGEREventID.EXCEPTION, cMODULE_NAME, new string[] { cBASE_NAME, exp.Message });
            }
        }

        public static string GetPeriodMessage(int periodNumber, string periodicityCode, DateTime formDate)
        {
            string periodMessage = "";

            switch (periodicityCode)
            {
                case "D": periodMessage = string.Format("{0}{1} de {2}", periodNumber, "º dia", formDate.Year); break;
                case "W": periodMessage = string.Format("{0}{1} de {2}", periodNumber, "º semana", formDate.Year); break;
                case "M": periodMessage = string.Format("{0}{1} de {2}", "", GetMonthDescription(periodNumber), formDate.Year); break;
                case "Q": periodMessage = string.Format("{0}{1} de {2}", periodNumber, "º trimestre", formDate.Year); break;
                case "S": periodMessage = string.Format("{0}{1} de {2}", periodNumber, "º semestre", formDate.Year); break;
                case "Y": periodMessage = string.Format("{0}{1} de {2}", "", " ano ", formDate.Year); break;
            }

            return periodMessage;
        }
        public static string GetMonthDescription(int month)
        {
            string monthDescription = "";
            switch (month)
            {
                case 1: monthDescription = "Janeiro"; break;
                case 2: monthDescription = "Fevereiro"; break;
                case 3: monthDescription = "Março"; break;
                case 4: monthDescription = "Abril"; break;
                case 5: monthDescription = "Maio"; break;
                case 6: monthDescription = "Junho"; break;
                case 7: monthDescription = "Julho"; break;
                case 8: monthDescription = "Agosto"; break;
                case 9: monthDescription = "Setembro"; break;
                case 10: monthDescription = "Outubro"; break;
                case 11: monthDescription = "Novembro"; break;
                case 12: monthDescription = "Dezembro"; break;

            }
            return monthDescription;
        }

        public static int GetPeriodNumber(string periodicityCode)
        {
            int periodNumber = 0;

            GregorianCalendar calendar = new GregorianCalendar();

            switch (periodicityCode)
            {
                case "D": periodNumber = DateTime.Now.DayOfYear; break;
                case "W": periodNumber = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday); break;
                case "M": periodNumber = DateTime.Now.Month; break;
                case "Q": periodNumber = DateTime.Now.Month / 4 + 1; break;
                case "S": periodNumber = DateTime.Now.Month / 2 + 1; break;
                case "Y": periodNumber = DateTime.Now.Year; break;
            }
            return periodNumber;
        }




    }
}
