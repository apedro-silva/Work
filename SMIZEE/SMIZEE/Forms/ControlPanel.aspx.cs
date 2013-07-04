using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SMIZEE.Models;
using SMIZEE.CodeFirstMembership;

namespace SMIZEE.Forms
{
    public partial class ControlPanel : BasePage
    {
        private static DateTime _ReferenceDate = DateTime.Parse(ConfigurationManager.AppSettings["ReferenceStartDate"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageDescription(Resources.Resource.lControlPanel);

                BindCompanies(ddlCompany);
                BindFormStates(ddlFormState);
                User user = WebSecurity.GetUserInfo(Page.User.Identity.Name);
                int? companyId = user.CompanyID;

                ddlCompany.SelectedValue = companyId.ToString();

                if (companyId != null && companyId != 0)
                {
                    ddlCompany.Enabled = false;
                    ProcessUserProductionUnitsForms(user);
                }
                else
                {
                    GetZEEFormsInQueue(user.FunctionalAreaID);
                    BindZEEUserForms(user);
                }

                AuditAction.Create(Page.User.Identity.Name, "ControlPanel", GetOperation("QRY"), "Successo");
            }


        }

        protected void OnGridView1SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow currentRow = gvFormsInQueue.SelectedRow;
                CleanMessage(MessagePanel, ErrorPanel);
                if (currentRow == null)
                    return;

                HiddenField formPage = currentRow.FindControl("FormPageHidden") as HiddenField;
                HiddenField formStateId = currentRow.FindControl("FormStateIdHidden") as HiddenField;
                Label companyFormId = currentRow.FindControl("CompanyFormIDLabel") as Label;
                Label productionUnit = currentRow.FindControl("ProductionUnitLabel") as Label;

                string requestPage = string.Format("{0}?PU={1}&FID={2}&ST={3}", formPage.Value, productionUnit.Text, companyFormId.Text, formStateId.Value);
                Response.Redirect(requestPage);
            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }
        }

        private void ProcessUserProductionUnitsForms(User user)
        {
            int productionUnitId;
            int? companyId = null;
            int? functionalAreaId = null;
            Guid? userId = null;

            companyId = int.Parse(ddlCompany.SelectedValue);

            // Get current user company
            if (user != null)
            {
                companyId = user.CompanyID;
                functionalAreaId = user.FunctionalAreaID;
                userId = user.UserId;
            }

            // Get list of user production units
            IQueryable<UserProductionUnit> userProductionUnits = CxUserProductionUnit.GetListByUserId(userId);
            List<UserProductionUnit> userProductionUnitsLis = userProductionUnits.ToList(); ;

            foreach (UserProductionUnit upu in userProductionUnitsLis)
            {
                productionUnitId = upu.ProductionUnitID;

                // Select forms for this production unit that are not created
                GetFormsInQueue(companyId, productionUnitId, functionalAreaId);
            }
            BindUserForms(userId, companyId, functionalAreaId);

        }

        private void GetFormsInQueue(int? companyId, int productionUnitId, int? functionalAreaId)
        {
            var db = new Models.SmizeeContext();

            // for each record in forms 
            // get forms by Functional Area
            IQueryable<Form> forms = from f in db.Forms where (functionalAreaId==0 | f.FormType.FunctionalAreaID == functionalAreaId) select f;
            List<Form> formsList = forms.ToList();


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

        private void BindUserForms(Guid? userId, int? companyId, int? functionalAreaId)
        {
            SmizeeContext db = new Models.SmizeeContext();
            string userName = Page.User.Identity.Name;

            if (functionalAreaId == 0)
                functionalAreaId = null;

            CleanMessage(MessagePanel, ErrorPanel);

            var upu = from f in CxUserProductionUnit.GetProductionUnitsByUserId(db, userId) select f;

            var fefInQueueX = from f in CxFinancialExportForm.GetUserFormsInQueue(db, userName, companyId, null, functionalAreaId) select f;
            var fefInQueue = from f in fefInQueueX
                             join y1 in upu on f.ProductionUnitID equals y1.ProductionUnitID
                             select new
                            {
                                FormId = f.FormID,
                                CompanyFormID = f.FinancialExportFormID,
                                FormDescription = f.Form.Description,
                                Periodicity = f.Form.Periodicity.Description,
                                ProductionUnit = f.ProductionUnit.Description,
                                FormPage = f.Form.FormType.FormPage,
                                FormStateId = f.StateID,
                                FormState = f.State.Description,
                                FormDate = f.FormDate,
                                PeriodicityCode = f.Form.Periodicity.Code,
                                PeriodNumber = f.PeriodNumber
                            };


            var ffInQueueX = from f in CxFinancialForm.GetUserFormsInQueue(db, userName, companyId, null, functionalAreaId) select f;
            var ffInQueue = from f in ffInQueueX
                             join y1 in upu on f.ProductionUnitID equals y1.ProductionUnitID
                            select new
                           {
                               FormId = f.FormID,
                               CompanyFormID = f.FinancialFormID,
                               FormDescription = f.Form.Description,
                               Periodicity = f.Form.Periodicity.Description,
                               ProductionUnit = f.ProductionUnit.Description,
                               FormPage = f.Form.FormType.FormPage,
                               FormStateId = f.StateID,
                               FormState = f.State.Description,
                                FormDate = f.FormDate,
                                PeriodicityCode = f.Form.Periodicity.Code,
                                PeriodNumber = f.PeriodNumber
                           };

            var hrfInQueueX = from f in CxHumanResourceForm.GetUserFormsInQueue(db, userName, companyId, null, functionalAreaId) select f;
            var hrfInQueue = from f in hrfInQueueX
                             join y1 in upu on f.ProductionUnitID equals y1.ProductionUnitID
                             select new
                            {
                                FormId = f.FormID,
                                CompanyFormID = f.HumanResourceFormID,
                                FormDescription = f.Form.Description,
                                Periodicity = f.Form.Periodicity.Description,
                                ProductionUnit = f.ProductionUnit.Description,
                                FormPage = f.Form.FormType.FormPage,
                                FormStateId = f.StateID,
                                FormState = f.State.Description,
                                FormDate = f.FormDate,
                                PeriodicityCode = f.Form.Periodicity.Code,
                                PeriodNumber = f.PeriodNumber

                            };

            var x = from f in CxHumanResourceQualificationForm.GetUserFormsInQueue(db, userName, companyId, null, functionalAreaId) select f;
            var hrqfInQueue = from f in x join y1 in upu on f.ProductionUnitID equals y1.ProductionUnitID
                              select new
                             {
                                 FormId = f.FormID,
                                 CompanyFormID = f.HumanResourceQualificationFormID,
                                 FormDescription = f.Form.Description,
                                 Periodicity = f.Form.Periodicity.Description,
                                 ProductionUnit = f.ProductionUnit.Description,
                                 FormPage = f.Form.FormType.FormPage,
                                 FormStateId = f.StateID,
                                 FormState = f.State.Description,
                                 FormDate = f.FormDate,
                                 PeriodicityCode = f.Form.Periodicity.Code,
                                 PeriodNumber = f.PeriodNumber

                             };

            var fInQueue = fefInQueue.Union(ffInQueue);
            fInQueue = fInQueue.Union(hrfInQueue);
            fInQueue = fInQueue.Union(hrqfInQueue);

            int formStateId = int.Parse(ddlFormState.SelectedValue);

            var listEntities = fInQueue.Where(f => formStateId == 0 | f.FormStateId == formStateId).ToList();
            gvFormsInQueue.DataSource = listEntities;
            gvFormsInQueue.DataBind();

            if (listEntities.Count == 0)
            {
                ShowInfo(MessagePanel, Resources.Resource.mNoRecordsFound);
                ListPanel.Visible = false;
            }
            else
                ListPanel.Visible = true;
        }

        private void BindZEEUserForms(User user)
        {
            SmizeeContext db = new Models.SmizeeContext();
            string userName = Page.User.Identity.Name;
            Guid? userId = user.UserId;
            int? functionalAreaId = user.FunctionalAreaID;

            CleanMessage(MessagePanel, ErrorPanel);

            var ffInQueueX = from f in CxFinancialSDZEEForm.GetUserFormsInQueue(db, userName, functionalAreaId) select f;
            var ffInQueue = from f in ffInQueueX
                            select new
                            {
                                //FormId = f.FormID,
                                CompanyFormID = f.FinancialSDZEEFormID,
                                FormDescription = f.Form.Description,
                                Periodicity = f.Form.Periodicity.Description,
                                ProductionUnit = "ZEE",
                                FormPage = f.Form.FormType.FormPage,
                                FormStateId = f.StateID,
                                FormState = f.State.Description,
                                FormDate = f.FormDate,
                                PeriodicityCode = f.Form.Periodicity.Code,
                                PeriodNumber = f.PeriodNumber
                            };


            var olfInQueueX = from f in CxOperationalLicensesForm.GetUserFormsInQueue(db, userName, functionalAreaId) select f;
            var olfInQueue = from f in olfInQueueX
                            select new
                             {
                                 //FormId = f.FormID,
                                 CompanyFormID = f.OperationalLicensesFormID,
                                 FormDescription = f.Form.Description,
                                 Periodicity = f.Form.Periodicity.Description,
                                 ProductionUnit = "ZEE",
                                 FormPage = f.Form.FormType.FormPage,
                                 FormStateId = f.StateID,
                                 FormState = f.State.Description,
                                 FormDate = f.FormDate,
                                 PeriodicityCode = f.Form.Periodicity.Code,
                                 PeriodNumber = f.PeriodNumber
                             };

            var ofInQueueX = from f in CxOperationalForm.GetUserFormsInQueue(db, userName, functionalAreaId) select f;
            var ofInQueue = from f in ofInQueueX
                            select new
                             {
                                 //FormId = f.FormID,
                                 CompanyFormID = f.OperationalFormID,
                                 FormDescription = f.Form.Description,
                                 Periodicity = f.Form.Periodicity.Description,
                                 ProductionUnit = "ZEE",
                                 FormPage = f.Form.FormType.FormPage,
                                 FormStateId = f.StateID,
                                 FormState = f.State.Description,
                                FormDate = f.FormDate,
                                PeriodicityCode = f.Form.Periodicity.Code,
                                PeriodNumber = f.PeriodNumber
                             };

            var hrfInQueueX = from f in CxHumanResourceSDZEEForm.GetUserFormsInQueue(db, userName, functionalAreaId) select f;
            var hrfInQueue = from f in hrfInQueueX
                             select new
                             {
                                 //FormId = f.FormID,
                                 CompanyFormID = f.HumanResourceSDZEEFormID,
                                 FormDescription = f.Form.Description,
                                 Periodicity = f.Form.Periodicity.Description,
                                 ProductionUnit = "ZEE",
                                 FormPage = f.Form.FormType.FormPage,
                                 FormStateId = f.StateID,
                                 FormState = f.State.Description,
                                FormDate = f.FormDate,
                                PeriodicityCode = f.Form.Periodicity.Code,
                                PeriodNumber = f.PeriodNumber
                             };


            var f0X = from f in CxFinancialExportForm.GetFormsForApproval(db, userName, 0, null, functionalAreaId) 
                             select new
                             {
                                 CompanyFormID = f.FinancialExportFormID,
                                 FormDescription = f.Form.Description,
                                 Periodicity = f.Form.Periodicity.Description,
                                 ProductionUnit = f.ProductionUnit.Description,
                                 FormPage = f.Form.FormType.FormPage,
                                 FormStateId = f.StateID,
                                 FormState = f.State.Description,
                                 FormDate = f.FormDate,
                                 PeriodicityCode = f.Form.Periodicity.Code,
                                 PeriodNumber = f.PeriodNumber
                             };

            var f1X = from f in CxFinancialForm.GetFormsForApproval(db, userName, 0, null, functionalAreaId) 
                            select new
                            {
                                CompanyFormID = f.FinancialFormID,
                                FormDescription = f.Form.Description,
                                Periodicity = f.Form.Periodicity.Description,
                                ProductionUnit = f.ProductionUnit.Description,
                                FormPage = f.Form.FormType.FormPage,
                                FormStateId = f.StateID,
                                FormState = f.State.Description,
                                FormDate = f.FormDate,
                                PeriodicityCode = f.Form.Periodicity.Code,
                                PeriodNumber = f.PeriodNumber
                            };

            var f3X = from f in CxHumanResourceForm.GetFormsForApproval(db, userName, 0, null, functionalAreaId) 
                             select new
                             {
                                 CompanyFormID = f.HumanResourceFormID,
                                 FormDescription = f.Form.Description,
                                 Periodicity = f.Form.Periodicity.Description,
                                 ProductionUnit = f.ProductionUnit.Description,
                                 FormPage = f.Form.FormType.FormPage,
                                 FormStateId = f.StateID,
                                 FormState = f.State.Description,
                                 FormDate = f.FormDate,
                                 PeriodicityCode = f.Form.Periodicity.Code,
                                 PeriodNumber = f.PeriodNumber

                             };

            var f4X = from f in CxHumanResourceQualificationForm.GetFormsForApproval(db, userName, 0, null, functionalAreaId) 
                              select new
                              {
                                  CompanyFormID = f.HumanResourceQualificationFormID,
                                  FormDescription = f.Form.Description,
                                  Periodicity = f.Form.Periodicity.Description,
                                  ProductionUnit = f.ProductionUnit.Description,
                                  FormPage = f.Form.FormType.FormPage,
                                  FormStateId = f.StateID,
                                  FormState = f.State.Description,
                                  FormDate = f.FormDate,
                                  PeriodicityCode = f.Form.Periodicity.Code,
                                  PeriodNumber = f.PeriodNumber

                              };

            var fInQueue = ffInQueue.Union(ofInQueue);
            fInQueue = fInQueue.Union(olfInQueue);
            fInQueue = fInQueue.Union(hrfInQueue);
            fInQueue = fInQueue.Union(f0X);
            fInQueue = fInQueue.Union(f1X);
            fInQueue = fInQueue.Union(f3X);
            fInQueue = fInQueue.Union(f4X);

            int formStateId = int.Parse(ddlFormState.SelectedValue);
            var listEntities = fInQueue.Where(f => formStateId == 0 | f.FormStateId == formStateId).ToList();
            gvFormsInQueue.DataSource = listEntities;
            gvFormsInQueue.DataBind();

            if (listEntities.Count == 0)
            {
                ShowInfo(MessagePanel, Resources.Resource.mNoRecordsFound);
                ListPanel.Visible = false;
            }
            else
                ListPanel.Visible = true;
        }

        private void GetFinancialExportFormInQueue(Form currentForm, int productionUnitId)
        {
            DateTime referenceDate = new DateTime(2012, 10, 01);

            CreateNewForm(_ReferenceDate, currentForm.FormID, productionUnitId, currentForm.Periodicity.Code, CxFinancialExportForm.CreateForm, CxFinancialExportForm.CheckFormsByFormDate);
        }
        private void GetFinancialFormInQueue(Form currentForm, int productionUnitId)
        {
            CreateNewForm(_ReferenceDate, currentForm.FormID, productionUnitId, currentForm.Periodicity.Code, CxFinancialForm.CreateForm, CxFinancialForm.CheckFormsByFormDate);
        }

        private void GetOperationalLicensesFormInQueue(Form currentForm)
        {
            CreateNewForm(_ReferenceDate, currentForm.FormID, 0, currentForm.Periodicity.Code, CxOperationalLicensesForm.CreateForm, CxOperationalLicensesForm.CheckFormsByFormDate);
        }
        private void GetOperationalFormInQueue(Form currentForm)
        {
            CreateNewForm(_ReferenceDate, currentForm.FormID, 0, currentForm.Periodicity.Code, CxOperationalForm.CreateForm, CxOperationalForm.CheckFormsByFormDate);
        }

        private void GetHumanResourceFormInQueue(Form currentForm, int productionUnitId)
        {
            CreateNewForm(_ReferenceDate, currentForm.FormID, productionUnitId, currentForm.Periodicity.Code, CxHumanResourceForm.CreateForm, CxHumanResourceForm.CheckFormsByFormDate);
        }

        private void GetHumanResourceQualificationFormInQueue(Form currentForm, int productionUnitId)
        {
            CreateNewForm(_ReferenceDate, currentForm.FormID, productionUnitId, currentForm.Periodicity.Code, CxHumanResourceQualificationForm.CreateForm, CxHumanResourceQualificationForm.CheckFormsByFormDate);
        }
        private void GetZEEFinancialFormInQueue(Form currentForm)
        {
            CreateNewForm(_ReferenceDate, currentForm.FormID, 0, currentForm.Periodicity.Code, CxFinancialSDZEEForm.CreateForm, CxFinancialSDZEEForm.CheckFormsByFormDate);
        }
        private void GetZEEHumanResourceFormInQueue(Form currentForm)
        {
            CreateNewForm(_ReferenceDate, currentForm.FormID, 0, currentForm.Periodicity.Code, CxHumanResourceSDZEEForm.CreateForm, CxHumanResourceSDZEEForm.CheckFormsByFormDate);
        }

        private int GetPeriodNumber(string periodicityCode, DateTime referenceDate)
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


        private void CreateNewForm(DateTime referenceDate, int formID, int productionUnitId, string periodicityCode, Func<int, int, int, DateTime, int> createFormMethod, Func<int, int, DateTime, bool> checkFormMethod)
        {
            GregorianCalendar calendar = new GregorianCalendar();

            // Check Form by FormDate=currentDate
            //bool formExists = Cx???Form.CheckFormsByFormDate(formID, productionUnitId, referenceDate);
            bool formExists = checkFormMethod(formID, productionUnitId, referenceDate);

            // If not exists create one
            if (!formExists)
            {
                int periodNumber = GetPeriodNumber(periodicityCode, referenceDate);
                createFormMethod(formID, productionUnitId, periodNumber, referenceDate);
            }

            if (periodicityCode == "D")
            {
                referenceDate = referenceDate.AddDays(1);
            }
            if (periodicityCode == "W")
            {
                //if (calendar.GetWeekOfYear(formDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday) == calendar.GetWeekOfYear(referenceDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                //    formExists = true;
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
            CreateNewForm(referenceDate, formID, productionUnitId, periodicityCode, createFormMethod, checkFormMethod);
        }

        public bool RunTheMethod(Action myMethodName)
        {
            myMethodName();
            return true;
        }

        protected void OnFormStateSelectedIndexChanged(object sender, EventArgs e)
        {
            int companyId = int.Parse(ddlCompany.SelectedValue);
            User user = WebSecurity.GetUserInfo(Page.User.Identity.Name);
            if (companyId == 0)
            {
                BindZEEUserForms(user);
                gvFormsInQueue.Columns[0].Visible = true;
            }
            else
            {
                BindUserForms(null, companyId, null);
                gvFormsInQueue.Columns[0].Visible = false;
                gvFormsInQueue.Columns[3].Visible = true;
            }
        }
        protected void OnCompanySelectedIndexChanged(object sender, EventArgs e)
        {
            int companyId = int.Parse(ddlCompany.SelectedValue);
            User user = WebSecurity.GetUserInfo(Page.User.Identity.Name);
            if (companyId == 0)
            {
                BindZEEUserForms(user);
                gvFormsInQueue.Columns[0].Visible = true;
            }
            else
            {
                BindUserForms(null, companyId, null);
                gvFormsInQueue.Columns[0].Visible = false;
                gvFormsInQueue.Columns[3].Visible = true;
            }
        }

        protected void OnFormsInQueueRowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow x = e.Row;

            if (x.RowType != DataControlRowType.DataRow)
                return;

            HiddenField periodicityCode = x.FindControl("PeriodicityCodeHidden") as HiddenField;
            HiddenField formDate = x.FindControl("FormDateHidden") as HiddenField;
            HiddenField formStateId = x.FindControl("FormStateIdHidden") as HiddenField;
            HiddenField periodNumber = x.FindControl("PeriodNumberHidden") as HiddenField;
            Label periodicityLabel = x.FindControl("PeriodicityLabel") as Label;

            string periodicityText= GetPeriodMessage("{0} {1} {2}", int.Parse(periodNumber.Value), periodicityCode.Value, DateTime.Parse(formDate.Value));
            periodicityLabel.Text = periodicityText;

            // Submited - for approval
            if (formStateId.Value == "3")
            {
                LinkButton command = x.Controls[0].Controls[0] as LinkButton;
                command.Text = Resources.Resource.lApproveForm;
            }

        }
    }
}