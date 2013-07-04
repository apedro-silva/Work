using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SMIZEE.Models;
using SMIZEE.CodeFirstMembership;

namespace SMIZEE.Forms
{
    public partial class SMI_03_08 : BasePage
    {
        private string currentOption = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options, true, false, false, false, false, false, false);
                //SetTransactionOptions(Options);
                SetPageDescription(Resources.MainMenu.IM_03_08);
                currentOption = Options.SelectedValue;

                string pu = Request.QueryString["PU"];
                string fefid = Request.QueryString["FID"];
                string stateid = Request.QueryString["ST"];
                if (fefid != null)
                {
                    CompanyFormID.Value = fefid;
                    currentOption = "UPD";
                    OptionsPanel.Visible = false;
                    SearchPanel.Visible = false;
                    DetailPanel.Visible = true;
                    ConfirmButtonPanel.Visible = true;

                    HumanResourceSDZEEForm entity = CxHumanResourceSDZEEForm.GetFormById(int.Parse(fefid));
                    FormMessageLiteral.Text = GetPeriodMessage(Resources.Resource.lForm2Message, entity.PeriodNumber, entity.Form.Periodicity.Code, entity.FormDate);
                    if (stateid == "3")
                    {
                        ApprovePanel.Visible = true;
                        ConfirmButtonPanel.Visible = false;
                        FormMessageLiteral.Text = "";
                    }

                    FillDateLiteral.Text = DateTime.Now.ToShortDateString();
                    ShowCurrentForm(int.Parse(fefid));
                }
            }
            else
            {
                currentOption = Options.SelectedValue;
            }

        }

        protected void OnOptions(object sender, EventArgs e)
        {
            OptionsPanel.Enabled = false;

            if (!currentOption.Equals("CRE"))
            {
                SearchPanel.Visible = true;
                SearchPanelButtons.Visible = true;
            }
            else
            {
                FillDateLiteral.Text = DateTime.Now.ToShortDateString();
                DetailPanel.Visible = true;
                ConfirmButtonPanel.Visible = true;
            }
            BindYears(ddlYear);
            BindPeriodicity(ddlPeriodicity);


        }

        protected void OnQuery(object sender, EventArgs e)
        {
            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                BindEntities(1);

            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }
        protected void OnCancel(object sender, EventArgs e)
        {
            Response.Redirect("SMI.03.08.aspx");
        }

        protected void OnGridView1SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow currentRow = GridView1.SelectedRow;
                CleanMessage(MessagePanel, ErrorPanel);
                if (currentRow == null)
                    return;

                DetailPanel.Visible = true;
                DetailPanel.Enabled = true;
                ConfirmButtonPanel.Visible = true;
                BackPanel.Visible = false;

                FillDateLiteral.Text = DateTime.Now.ToShortDateString();

                HiddenField periodNumber = currentRow.FindControl("PeriodNumberHidden") as HiddenField;
                HiddenField periodicityCode = currentRow.FindControl("PeriodicityCodeHidden") as HiddenField;

                HiddenField financialFormIdHidden = currentRow.FindControl("FinancialFormIdHidden") as HiddenField;
                int financialFormId = int.Parse(financialFormIdHidden.Value);
                CompanyFormID.Value = financialFormIdHidden.Value;

                HiddenField formDateHidden = currentRow.FindControl("FormDateHidden") as HiddenField;
                DateTime formDate = DateTime.Parse(formDateHidden.Value);

                FormMessageLiteral.Text = GetPeriodMessage(Resources.Resource.lForm2Message, int.Parse(periodNumber.Value), periodicityCode.Value, formDate);
                ShowCurrentForm(financialFormId);

            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }
        }
        private void ShowCurrentForm(int formId)
        {
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.HumanResourceSDZEEForms where c.HumanResourceSDZEEFormID == formId select c).First();
                if (entity != null)
                {
                    ActiveEmployeesInput.Text = GetStringValue(entity.ActiveEmployees);
                    EmployeesLeftInput.Text = GetStringValue(entity.EmployeesLeft);
                    EmployeesHiredInput.Text = GetStringValue(entity.EmployeesHired);
                    TotalTrainingHoursInput.Text = GetStringValue(entity.TotalTrainingHours);
                }
            }

            switch (currentOption)
            {

                case "QRY": ConfirmButtonPanel.Visible = false;
                            DetailPanel.Enabled = false;
                            BackPanel.Visible = true;
                            break;
                case "DEL": DetailPanel.Enabled = false;
                    break;
                case "UPD": break;
                case "CRE": break;
            }

        }

        protected void OnPagingSelectedIndexChanged(object sender, EventArgs e)
        {
            // Get page Code
            BindEntities(int.Parse(ddlPaging.SelectedValue));
            PagingPanel.Visible = true;

        }
        protected void OnSubmit(object sender, EventArgs e)
        {
            UpdateEvent(3);
        }

        protected void OnUpdate(object sender, EventArgs e)
        {
            UpdateEvent(2);
        }
        protected void OnApproveForm(object sender, EventArgs e)
        {
            UpdateEvent(4);
        }
        protected void OnRejectForm(object sender, EventArgs e)
        {
            UpdateEvent(5);
        }

        protected void UpdateEvent(int formState)
        {
            string operationResult = "Successo";
            int financialExportFormID = int.Parse(CompanyFormID.Value);

            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                UpdateRecord(formState, financialExportFormID);
                BackPanel.Visible = true;
                ConfirmButtonPanel.Visible = false;
                DetailPanel.Visible = true;
                DetailPanel.Enabled = false;
                ApprovePanel.Visible = false;
                if (formState == 2)
                    ShowInfo(MessagePanel, Resources.Resource.mUpdateOK);
                else if (formState == 3)
                    ShowInfo(MessagePanel, Resources.Resource.mSubmitedOK);
                else if (formState == 4)
                    ShowInfo(MessagePanel, Resources.Resource.mApprovedOK);
                else if (formState == 5)
                    ShowInfo(MessagePanel, Resources.Resource.mRejectedOK);
            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
                operationResult = exp.Message;
            }
            finally
            {
                AuditAction.Create(Page.User.Identity.Name, "SMI.03.08", GetOperation("UPD"), operationResult);
            }

        }

        private void UpdateRecord(int formState, int financialFormId)
        {
            int? activeEmployees = GetIntValueFromInput(ActiveEmployeesInput.Text);
            int? employeesLeft = GetIntValueFromInput(EmployeesLeftInput.Text);
            int? employeesHired = GetIntValueFromInput(EmployeesHiredInput.Text);
            int? totalTrainingHours = GetIntValueFromInput(TotalTrainingHoursInput.Text);

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.HumanResourceSDZEEForms where c.HumanResourceSDZEEFormID == financialFormId select c).First();
                if (entity != null)
                {
                    entity.ActiveEmployees = activeEmployees;
                    entity.EmployeesLeft = employeesLeft;
                    entity.EmployeesHired = employeesHired;
                    entity.TotalTrainingHours = totalTrainingHours;

                    if (formState <= 3)
                    {
                        entity.SubmitDate = DateTime.Now;
                        entity.SubmitUserName = User.Identity.Name;
                    }
                    else
                    {
                        entity.ApprovalUserName = User.Identity.Name;
                        entity.ApprovalDate = DateTime.Now;
                    }
                    entity.StateID = formState;
                    db.SaveChanges();
                }
            }
        }

        private void DeleteRecord(int code)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                //db.Database.ExecuteSqlCommand("delete from FinancialExportForms where FinancialExportFormId={0}", code);
                var entity = (from c in db.HumanResourceSDZEEForms where c.HumanResourceSDZEEFormID == code select c).First();
                if (entity != null)
                {
                    db.HumanResourceSDZEEForms.Remove(entity);
                    db.SaveChanges();
                }
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mDeleteOK);

        }

        private void CreateRecord(int formId, int productionUnitId)
        {
            int? activeEmployees = GetIntValueFromInput(ActiveEmployeesInput.Text);
            int? employeesLeft = GetIntValueFromInput(EmployeesLeftInput.Text);
            int? employeesHired = GetIntValueFromInput(EmployeesHiredInput.Text);
            int? totalTrainingHours = GetIntValueFromInput(TotalTrainingHoursInput.Text);

            using (var db = new Models.SmizeeContext())
            {
                HumanResourceSDZEEForm entity = new HumanResourceSDZEEForm();
                entity.StateID = 1;
                entity.FormID = formId;
                entity.ActiveEmployees = activeEmployees;
                entity.EmployeesLeft = employeesLeft;
                entity.EmployeesHired = employeesHired;
                entity.TotalTrainingHours = totalTrainingHours;

                entity.SubmitDate = DateTime.Now;

                entity.SubmitUserName = User.Identity.Name;
                db.HumanResourceSDZEEForms.Add(entity);
                db.SaveChanges();
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mCreateOK);
        }


        private void BindEntities(int pageNumber)
        {
            int year = int.Parse(ddlYear.SelectedValue);
            int periodicity = int.Parse(ddlPeriodicity.SelectedValue);
            User user = WebSecurity.GetUserInfo(Page.User.Identity.Name);

            IQueryable<HumanResourceSDZEEForm> forms = CxHumanResourceSDZEEForm.GetForms(pageNumber, year, periodicity, user.FunctionalAreaID);

            var results = from f in forms
                          select new
                          {
                              FinancialFormId = f.HumanResourceSDZEEFormID,
                              FormDescription = f.Form.Description,
                              PeriodicityDescription = f.Form.Periodicity.Description,
                              PeriodNumber = f.PeriodNumber,
                              PeriodicityCode = f.Form.Periodicity.Code,
                              FormDate = f.FormDate,
                              FormState = f.State.Description,
                          };


            var listEntities = results.ToList();

            GridView1.DataSource = listEntities.Take(10);
            GridView1.DataBind();

            //int pageSize = PageRecordsInput.Text == "" ? GridView1.PageSize : int.Parse(PageRecordsInput.Text);
            int pageSize = GridView1.PageSize;

            if (listEntities.Count == 0)
            {
                ShowInfo(MessagePanel, Resources.Resource.mNoRecordsFound);
                ListPanel.Visible = false;
                PagingPanel.Visible = false;
            }
            else
            {
                if (listEntities.Count > pageSize)
                {
                    SetPages(ddlPaging, pageNumber + 1);
                    PagingPanel.Visible = true;
                    ddlPaging.SelectedIndex = pageNumber - 1;
                }
                else
                    PagingPanel.Visible = false;

                DetailPanel.Visible = false;
                BackPanel.Visible = false;
                ConfirmButtonPanel.Visible = false;
                ListPanel.Visible = true;
            }
        }

        protected void OnFormsInQueueRowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow x = e.Row;

            if (x.RowType != DataControlRowType.DataRow)
                return;

            HiddenField periodicityCode = x.FindControl("PeriodicityCodeHidden") as HiddenField;
            HiddenField formDate = x.FindControl("FormDateHidden") as HiddenField;
            HiddenField periodNumber = x.FindControl("PeriodNumberHidden") as HiddenField;
            Label periodicityLabel = x.FindControl("PeriodicityDescriptionLabel") as Label;

            string periodicityText = GetPeriodMessage("{0} {1} {2}", int.Parse(periodNumber.Value), periodicityCode.Value, DateTime.Parse(formDate.Value));
            periodicityLabel.Text = periodicityText;

        }

    }
}