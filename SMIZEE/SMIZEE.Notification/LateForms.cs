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
    public static class LateForms
    {
        internal static void ProcessLateSubmissions()
        {
            // Get forms by user Functional Area
            IQueryable<Form> forms = CxForm.GetFormsByFunctionalAreaId(null);
            List<Form> formsList = forms.ToList();


            // for each record in forms 
            // check is exists Foms in the period defined by form Periodicity.Code
            foreach (Form currentForm in formsList)
            {
                switch (currentForm.FormType.FormTypeID)
                {
                    case 1: ProcessLateFinancialExportForm(currentForm);
                        break;
                    case 2: ProcessLateFinancialForm(currentForm);
                        break;
                    case 5: ProcessLateHumanResourceForm(currentForm);
                        break;
                    case 6: ProcessLateHumanResourceQualificationForm(currentForm);
                        break;
                    default: break;
                }
            }
        }
        private static void ProcessLateFinancialExportForm(Form currentForm)
        {
            int? alarmRangeUser = 0;
            int? alarmRangeManager = 0;
            int? alarmRangeExecutive = 0;
            bool userAlert = false; 
            bool managerAlert = false; 
            bool executiveAlert = false;

            IQueryable<FinancialExportForm> forms = CxFinancialExportForm.GetFormsNotApproved(currentForm.FormID);
            var result = forms.ToList();

            // for each form not apporved (formState!=4)
            foreach (FinancialExportForm f1 in result)
            {
                alarmRangeUser = f1.Form.AlarmRangeUser;
                alarmRangeManager = f1.Form.AlarmRangeManager;
                alarmRangeExecutive = f1.Form.AlarmRangeExecutive;
                int lateDays = GetFormLateDays(f1.FormDate, f1.Form.Periodicity.Code);

                if (lateDays >= alarmRangeUser && f1.UserAlertDate == null)
                {
                    Goodies.SendEmail(Properties.Settings.Default.LateNotificationMessage, f1.ProductionUnitID, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    userAlert = true;
                }
                if (lateDays >= alarmRangeManager && f1.ManagerAlertDate == null)
                {
                    Goodies.SendManagerEmail(Properties.Settings.Default.LateNotificationMessage, f1.ProductionUnitID, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    managerAlert = true;
                }
                if (lateDays >= alarmRangeExecutive && f1.ExecutiveAlertDate == null)
                {
                    Goodies.SendExecutiveEmail(Properties.Settings.Default.LateNotificationMessage, f1.ProductionUnitID, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    executiveAlert = true;
                }
                CxFinancialExportForm.UpdateAlertDate(f1.FinancialExportFormID, userAlert, managerAlert, executiveAlert);
            }
        }
        private static void ProcessLateFinancialForm(Form currentForm)
        {
            int? alarmRangeUser = 0;
            int? alarmRangeManager = 0;
            int? alarmRangeExecutive = 0;
            bool userAlert = false;
            bool managerAlert = false;
            bool executiveAlert = false;

            IQueryable<FinancialForm> forms = CxFinancialForm.GetFormsNotApproved(currentForm.FormID);
            var result = forms.ToList();

            // for each form not apporved (formState!=4)
            foreach (FinancialForm f1 in result)
            {
                alarmRangeUser = f1.Form.AlarmRangeUser;
                alarmRangeManager = f1.Form.AlarmRangeManager;
                alarmRangeExecutive = f1.Form.AlarmRangeExecutive;
                int lateDays = GetFormLateDays(f1.FormDate, f1.Form.Periodicity.Code);

                if (lateDays >= alarmRangeUser && f1.UserAlertDate == null)
                {
                    Goodies.SendEmail(Properties.Settings.Default.LateNotificationMessage, f1.ProductionUnitID, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    userAlert = true;
                }
                if (lateDays >= alarmRangeManager && f1.ManagerAlertDate == null)
                {
                    Goodies.SendManagerEmail(Properties.Settings.Default.LateNotificationMessage, f1.ProductionUnitID, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    managerAlert = true;
                }
                if (lateDays >= alarmRangeExecutive && f1.ExecutiveAlertDate == null)
                {
                    Goodies.SendExecutiveEmail(Properties.Settings.Default.LateNotificationMessage, f1.ProductionUnitID, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    executiveAlert = true;
                }
                CxFinancialForm.UpdateAlertDate(f1.FinancialFormID, userAlert, managerAlert, executiveAlert);

            }
        }

        private static void ProcessLateOperationalLicensesForm(Form currentForm)
        {
            int? alarmRangeUser = 0;
            int? alarmRangeManager = 0;
            int? alarmRangeExecutive = 0;
            bool userAlert = false;
            bool managerAlert = false;
            bool executiveAlert = false;

            IQueryable<OperationalLicensesForm> forms = CxOperationalLicensesForm.GetFormsNotApproved(currentForm.FormID);
            var result = forms.ToList();

            // for each form not apporved (formState!=4)
            foreach (OperationalLicensesForm f1 in result)
            {
                alarmRangeUser = f1.Form.AlarmRangeUser;
                alarmRangeManager = f1.Form.AlarmRangeManager;
                alarmRangeExecutive = f1.Form.AlarmRangeExecutive;
                int lateDays = GetFormLateDays(f1.FormDate, f1.Form.Periodicity.Code);

                if (lateDays >= alarmRangeUser && f1.UserAlertDate == null)
                {
                    Goodies.SendEmail(Properties.Settings.Default.LateNotificationMessage, 0, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    userAlert = true;
                }
                if (lateDays >= alarmRangeManager && f1.ManagerAlertDate == null)
                {
                    Goodies.SendManagerEmail(Properties.Settings.Default.LateNotificationMessage, 0, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    managerAlert = true;
                }
                if (lateDays >= alarmRangeExecutive && f1.ExecutiveAlertDate == null)
                {
                    Goodies.SendExecutiveEmail(Properties.Settings.Default.LateNotificationMessage, 0, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    executiveAlert = true;
                }
                CxOperationalLicensesForm.UpdateAlertDate(f1.OperationalLicensesFormID, userAlert, managerAlert, executiveAlert);

            }
        }

        private static void ProcessLateHumanResourceForm(Form currentForm)
        {
            int? alarmRangeUser = 0;
            int? alarmRangeManager = 0;
            int? alarmRangeExecutive = 0;
            bool userAlert = false;
            bool managerAlert = false;
            bool executiveAlert = false;

            IQueryable<HumanResourceForm> forms = CxHumanResourceForm.GetFormsNotApproved(currentForm.FormID);
            var result = forms.ToList();

            // for each form not apporved (formState!=4)
            foreach (HumanResourceForm f1 in result)
            {
                alarmRangeUser = f1.Form.AlarmRangeUser;
                alarmRangeManager = f1.Form.AlarmRangeManager;
                alarmRangeExecutive = f1.Form.AlarmRangeExecutive;
                int lateDays = GetFormLateDays(f1.FormDate, f1.Form.Periodicity.Code);

                if (lateDays >= alarmRangeUser && f1.UserAlertDate == null)
                {
                    Goodies.SendEmail(Properties.Settings.Default.LateNotificationMessage, f1.ProductionUnitID, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    userAlert = true;
                }
                if (lateDays >= alarmRangeManager && f1.ManagerAlertDate == null)
                {
                    Goodies.SendManagerEmail(Properties.Settings.Default.LateNotificationMessage, f1.ProductionUnitID, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    managerAlert = true;
                }
                if (lateDays >= alarmRangeExecutive && f1.ExecutiveAlertDate == null)
                {
                    Goodies.SendExecutiveEmail(Properties.Settings.Default.LateNotificationMessage, f1.ProductionUnitID, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    executiveAlert = true;
                }
                CxHumanResourceForm.UpdateAlertDate(f1.HumanResourceFormID, userAlert, managerAlert, executiveAlert);

            }
        }

        private static void ProcessLateHumanResourceQualificationForm(Form currentForm)
        {
            int? alarmRangeUser = 0;
            int? alarmRangeManager = 0;
            int? alarmRangeExecutive = 0;
            bool userAlert = false;
            bool managerAlert = false;
            bool executiveAlert = false;

            IQueryable<HumanResourceQualificationForm> forms = CxHumanResourceQualificationForm.GetFormsNotApproved(currentForm.FormID);
            var result = forms.ToList();

            // for each form not apporved (formState!=4)
            foreach (HumanResourceQualificationForm f1 in result)
            {
                alarmRangeUser = f1.Form.AlarmRangeUser;
                alarmRangeManager = f1.Form.AlarmRangeManager;
                alarmRangeExecutive = f1.Form.AlarmRangeExecutive;
                int lateDays = GetFormLateDays(f1.FormDate, f1.Form.Periodicity.Code);

                if (lateDays >= alarmRangeUser && f1.UserAlertDate == null)
                {
                    Goodies.SendEmail(Properties.Settings.Default.LateNotificationMessage, f1.ProductionUnitID, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    userAlert = true;
                }
                if (lateDays >= alarmRangeManager && f1.ManagerAlertDate == null)
                {
                    Goodies.SendManagerEmail(Properties.Settings.Default.LateNotificationMessage, f1.ProductionUnitID, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    managerAlert = true;
                }
                if (lateDays >= alarmRangeExecutive && f1.ExecutiveAlertDate == null)
                {
                    Goodies.SendExecutiveEmail(Properties.Settings.Default.LateNotificationMessage, f1.ProductionUnitID, f1.Form.FormType.FunctionalAreaID, f1.Form.Description, f1.PeriodNumber, f1.Form.Periodicity.Code, f1.FormDate);
                    executiveAlert = true;
                }
                CxHumanResourceQualificationForm.UpdateAlertDate(f1.HumanResourceQualificationFormID, userAlert, managerAlert, executiveAlert);

            }
        }

        private static int GetFormLateDays(DateTime formDate, string periodicityCode)
        {
            GregorianCalendar calendar = new GregorianCalendar();
            TimeSpan ts = DateTime.Now.Subtract(formDate);
            return ts.Days;
        }
    }
}
