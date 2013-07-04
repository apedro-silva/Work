using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMIZEE.Models;
using SF.Expand.Logger;
using SF.Expand.Notification;

namespace SMIZEE.Notification
{
    public class Forms
    {
        private const string cMODULE_NAME = "SMIZEE.Notification.Forms";
        private const string cBASE_NAME = @"http://sf.expand.com/";
        private static DateTime _ReferenceDate;

        public static void ProcessUserProductionUnitsForms(DateTime referenceDate)
        {
            int productionUnitId;
            int? companyId = null;
            int? functionalAreaId = null;
            Guid? userId = null;

            _ReferenceDate = referenceDate;

            // Get list of user production units
            IQueryable<UserProductionUnit> userProductionUnits = CxUserProductionUnit.GetListByUserId(userId);
            List<UserProductionUnit> userProductionUnitsLis = userProductionUnits.ToList(); ;

            foreach (UserProductionUnit upu in userProductionUnitsLis)
            {
                productionUnitId = upu.ProductionUnitID;
                functionalAreaId = upu.User.FunctionalAreaID;
                // Select forms for this production unit that are not created
                GetFormsInQueue(companyId, productionUnitId, functionalAreaId);
            }
        }

        private void GetZEEFormsInQueue(int? functionalAreaId)
        {
            // for each record in forms 
            // get forms by Functional Area
            IQueryable<Form> forms = CxForm.GetFormsByFunctionalAreaId(functionalAreaId);
            List<Form> formsList = forms.ToList();


            foreach (Form currentForm in formsList)
            {
                switch (currentForm.FormType.FormTypeID)
                {
                    case 3: GetOperationalLicensesFormInQueue(currentForm);
                        break;
                    case 4: GetOperationalFormInQueue(currentForm);
                        break;
                    case 7: GetZEEFinancialFormInQueue(currentForm);
                        break;
                    case 8: GetZEEHumanResourceFormInQueue(currentForm);
                        break;
                    default: break;
                }
            }
        }

        private static void GetFormsInQueue(int? companyId, int productionUnitId, int? functionalAreaId)
        {
            // Get forms by user Functional Area
            IQueryable<Form> forms = CxForm.GetFormsByFunctionalAreaId(functionalAreaId);
            List<Form> formsList = forms.ToList();


            // for each record in forms 
            // check is exists Foms in the period defined by form Periodicity.Code
            foreach (Form currentForm in formsList)
            {
                switch (currentForm.FormType.FormTypeID)
                {
                    case 1: GetFinancialExportFormInQueue(currentForm, productionUnitId);
                        break;
                    case 2: GetFinancialFormInQueue(currentForm, productionUnitId);
                        break;
                    case 5: GetHumanResourceFormInQueue(currentForm, productionUnitId);
                        break;
                    case 6: GetHumanResourceQualificationFormInQueue(currentForm, productionUnitId);
                        break;
                    default: break;
                }
            }
        }
        private static void GetFinancialExportFormInQueue(Form currentForm, int productionUnitId)
        {
            CreateNewForm(_ReferenceDate, currentForm, productionUnitId, CxFinancialExportForm.CreateForm, CxFinancialExportForm.CheckFormsByFormDate);
        }
        private static void GetFinancialFormInQueue(Form currentForm, int productionUnitId)
        {
            CreateNewForm(_ReferenceDate, currentForm, productionUnitId, CxFinancialForm.CreateForm, CxFinancialForm.CheckFormsByFormDate);
        }

        private static void GetOperationalLicensesFormInQueue(Form currentForm)
        {
            CreateNewForm(_ReferenceDate, currentForm, 0, CxOperationalLicensesForm.CreateForm, CxOperationalLicensesForm.CheckFormsByFormDate);
        }
        private static void GetOperationalFormInQueue(Form currentForm)
        {
            CreateNewForm(_ReferenceDate, currentForm, 0, CxOperationalLicensesForm.CreateForm, CxOperationalForm.CheckFormsByFormDate);
        }

        private static void GetHumanResourceFormInQueue(Form currentForm, int productionUnitId)
        {
            CreateNewForm(_ReferenceDate, currentForm, productionUnitId, CxHumanResourceForm.CreateForm, CxHumanResourceForm.CheckFormsByFormDate);
        }

        private static void GetHumanResourceQualificationFormInQueue(Form currentForm, int productionUnitId)
        {
            CreateNewForm(_ReferenceDate, currentForm, productionUnitId, CxHumanResourceQualificationForm.CreateForm, CxHumanResourceQualificationForm.CheckFormsByFormDate);
        }

        private void GetZEEFinancialFormInQueue(Form currentForm)
        {
            CreateNewForm(_ReferenceDate, currentForm, 0, CxFinancialSDZEEForm.CreateForm, CxFinancialSDZEEForm.CheckFormsByFormDate);
        }
        private void GetZEEHumanResourceFormInQueue(Form currentForm)
        {
            CreateNewForm(_ReferenceDate, currentForm, 0, CxHumanResourceSDZEEForm.CreateForm, CxHumanResourceSDZEEForm.CheckFormsByFormDate);
        }

        private static void CreateNewForm(DateTime referenceDate, Form currentForm, int productionUnitId, Func<int, int, int, DateTime, int> createFormMethod, Func<int, int, DateTime, bool> checkFormMethod)
        {
            GregorianCalendar calendar = new GregorianCalendar();
            int formID = currentForm.FormID;
            string periodicityCode = currentForm.Periodicity.Code;

            // Check Form by FormDate=currentDate
            //bool formExists = Cx???Form.CheckFormsByFormDate(formID, productionUnitId, referenceDate);
            bool formExists = checkFormMethod(formID, productionUnitId, referenceDate);

            // If not exists create one
            if (!formExists)
            {
                int periodNumber = GetPeriodNumber(periodicityCode, referenceDate);
                int fid = createFormMethod(formID, productionUnitId, periodNumber, referenceDate);
                Goodies.SendEmail(Properties.Settings.Default.NotificationMessage, productionUnitId, currentForm.FormType.FunctionalAreaID, currentForm.Description, periodNumber, currentForm.Periodicity.Code, referenceDate);
            }

            if (periodicityCode == "D")
            {
                referenceDate = referenceDate.AddDays(1);
            }
            if (periodicityCode == "W")
            {
                //int weekOfYear = calendar.GetWeekOfYear(referenceDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                referenceDate = referenceDate.AddDays(7);
            }
            if (periodicityCode == "M")
            {
                referenceDate = referenceDate.AddMonths(1);
            }
            if (periodicityCode == "Q")
            {
                referenceDate = referenceDate.AddMonths(3);
            }
            if (periodicityCode == "S")
            {
                referenceDate = referenceDate.AddMonths(6);
            }
            if (periodicityCode == "Y")
            {
                referenceDate = referenceDate.AddYears(1);
            }

            if (referenceDate.CompareTo(DateTime.Today) >= 0)
                return;

            //recursively run this method
            CreateNewForm(referenceDate, currentForm, productionUnitId, createFormMethod, checkFormMethod);
        }
        private static int GetPeriodNumber(string periodicityCode, DateTime referenceDate)
        {
            int periodNumber = 0;

            GregorianCalendar calendar = new GregorianCalendar();

            switch (periodicityCode)
            {
                case "D": periodNumber = referenceDate.DayOfYear; break;
                case "W": periodNumber = calendar.GetWeekOfYear(referenceDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday); break;
                case "M": periodNumber = referenceDate.Month; break;
                case "Q": periodNumber = (referenceDate.Month-1) / 3 + 1; break;
                case "S": periodNumber = referenceDate.Month / 2 + 1; break;
                case "Y": periodNumber = referenceDate.Year; break;
            }
            return periodNumber;
        }

    }
}
