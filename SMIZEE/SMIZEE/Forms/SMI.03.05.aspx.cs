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
    public partial class SMI_03_05 : BasePage
    {
        private string currentOption = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options, true, false, false, false, false, false, false);
                SetPageDescription(Resources.MainMenu.IM_03_05);
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
                    HumanResourceForm entity = CxHumanResourceForm.GetFormById(int.Parse(fefid));
                    FormMessageLiteral.Text = GetPeriodMessage(Resources.Resource.lForm2Message, entity.PeriodNumber, entity.Form.Periodicity.Code, entity.FormDate);
                    if (stateid == "3")
                    {
                        DetailPanel.Enabled = false;
                        ApprovePanel.Visible = true;
                        ConfirmButtonPanel.Visible = false;
                        FormMessageLiteral.Text = "";
                    }

                    ProductionUnitLiteral.Text = pu;
                    FillDateLiteral.Text = DateTime.Now.ToShortDateString();
                    ShowCurrentForm(int.Parse(fefid));
                }
            }
            else
            {
                currentOption = Options.SelectedValue;
                User user = WebSecurity.GetUserInfo(Page.User.Identity.Name);
                int? companyId = user.CompanyID;

                if (companyId != null && companyId != 0)
                {
                    ddlCompany.SelectedValue = companyId.ToString();
                    ddlCompany.Enabled = false;
                }
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
                DetailPanel.Visible = true;
                ConfirmButtonPanel.Visible = true;
            }
            BindCompanies(ddlCompany);
            int companyId = int.Parse(ddlCompany.SelectedValue);
            BindProductionUnits(ddlProductionUnit, companyId);
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
            Response.Redirect("SMI.03.05.aspx");
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

                Label productionUnitDescription = currentRow.FindControl("ProductionUnitDescriptionLabel") as Label;
                ProductionUnitLiteral.Text = productionUnitDescription.Text;
                FillDateLiteral.Text = DateTime.Now.ToShortDateString();

                HiddenField periodNumber = currentRow.FindControl("PeriodNumberHidden") as HiddenField;
                HiddenField periodicityCode = currentRow.FindControl("PeriodicityCodeHidden") as HiddenField;
                HiddenField formDateHidden = currentRow.FindControl("FormDateHidden") as HiddenField;
                DateTime formDate = DateTime.Parse(formDateHidden.Value);
                FormMessageLiteral.Text = GetPeriodMessage(Resources.Resource.lForm2Message, int.Parse(periodNumber.Value), periodicityCode.Value, formDate);

                Label HumanResourceFormId = currentRow.FindControl("HumanResourceFormIDLabel") as Label;
                CompanyFormID.Value = HumanResourceFormId.Text;
                int formId = int.Parse(CompanyFormID.Value);

                ShowCurrentForm(formId);
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
                var entity = (from c in db.HumanResourceForms where c.HumanResourceFormID == formId select c).First();
                if (entity != null)
                {
                    NrTotalHorasFormacaoInput.Text = GetStringValue(entity.TrainingTotalHours);
                    NrDiasUteisPeriodoInput.Text = GetStringValue(entity.WorkingDaysInPeriod);
                    NrDiasAusenciaTrabalhoInput.Text = GetStringValue(entity.WorkMissingDays);
                    NrSaidasInput.Text = GetStringValue(entity.EmployeesLeft);
                    NrColaboradoresAdmitidosInput.Text = GetStringValue(entity.EmployeesHired);
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

        private void UpdateEvent(int formState)
        {
            string operationResult = "Successo";
            string optionSelected = "UPD";
            int HumanResourceFormId = 0;

            int? nrTotalHorasFormacao = GetIntValueFromInput(NrTotalHorasFormacaoInput.Text);
            int? nrDiasAusenciaTrabalho = GetIntValueFromInput(NrDiasAusenciaTrabalhoInput.Text);
            int? nrDiasUteisPeriodo = GetIntValueFromInput(NrDiasUteisPeriodoInput.Text);
            int? nrColaboradoresAdmitidos = GetIntValueFromInput(NrColaboradoresAdmitidosInput.Text);
            int? nrSaidas = GetIntValueFromInput(NrSaidasInput.Text);

            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                HumanResourceFormId = int.Parse(CompanyFormID.Value);
                UpdateRecord(HumanResourceFormId, formState, nrTotalHorasFormacao, nrDiasAusenciaTrabalho, nrDiasUteisPeriodo, nrColaboradoresAdmitidos, nrSaidas);
                BackPanel.Visible = true;
                ConfirmButtonPanel.Visible = false;
                DetailPanel.Visible = true;
                DetailPanel.Enabled= false;
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
                AuditAction.Create(Page.User.Identity.Name, "SMI.03.05", GetOperation(optionSelected), operationResult);
            }

        }

        private void UpdateRecord(int HumanResourceFormId, int formState, int? nrTotalHorasFormacao, int? nrDiasAusenciaTrabalho, int? nrDiasUteisPeriodo, int? nrColaboradoresAdmitidos, int? nrSaidas)
        {

             //Query the database for the rows to be updated.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.HumanResourceForms where c.HumanResourceFormID == HumanResourceFormId select c).First();
                if (entity != null)
                {
                    entity.HumanResourceFormID = HumanResourceFormId;
                    entity.EmployeesHired = nrColaboradoresAdmitidos;
                    entity.EmployeesLeft = nrSaidas;
                    entity.TrainingTotalHours = nrTotalHorasFormacao;
                    entity.WorkingDaysInPeriod = nrDiasUteisPeriodo;
                    entity.WorkMissingDays = nrDiasAusenciaTrabalho;
                    entity.StateID = formState;
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
                    db.SaveChanges();
                }
            }
        }

        private void DeleteRecord(int code)
        {
            //Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                //db.Database.ExecuteSqlCommand("delete from HumanResourceForms where HumanResourceFormId={0}", code);
                var entity = (from c in db.HumanResourceForms where c.HumanResourceFormID == code select c).First();
                if (entity != null)
                {
                    db.HumanResourceForms.Remove(entity);
                    db.SaveChanges();
                }
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mDeleteOK);

        }

        private void CreateRecord(int formId)
        {
            int? nrTotalHorasFormacao = GetIntValueFromInput(NrTotalHorasFormacaoInput.Text);
            int? nrDiasAusenciaTrabalho = GetIntValueFromInput(NrDiasAusenciaTrabalhoInput.Text);
            int? nrDiasUteisPeriodo = GetIntValueFromInput(NrDiasUteisPeriodoInput.Text);
            int? nrColaboradoresAdmitidos = GetIntValueFromInput(NrColaboradoresAdmitidosInput.Text);
            int? nrSaidas = GetIntValueFromInput(NrSaidasInput.Text);

            using (var db = new Models.SmizeeContext())
            {
                HumanResourceForm entity = new HumanResourceForm();
                entity.StateID = 1;
                entity.FormID = formId;
                entity.EmployeesHired = nrColaboradoresAdmitidos;
                entity.EmployeesLeft = nrSaidas;
                entity.TrainingTotalHours = nrTotalHorasFormacao;
                entity.WorkingDaysInPeriod = nrDiasUteisPeriodo;
                entity.WorkMissingDays = nrDiasAusenciaTrabalho;
                entity.SubmitDate = DateTime.Now;
                entity.ApprovalDate = DateTime.Parse("01/01/2000 00:00:00");
                entity.SubmitUserName = User.Identity.Name;

                db.HumanResourceForms.Add(entity);
                db.SaveChanges();
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mCreateOK);
        }


        private void BindEntities(int pageNumber)
        {
            int companyId = int.Parse(ddlCompany.SelectedValue);
            int productionUnitId = int.Parse(ddlProductionUnit.SelectedValue);

            IQueryable<HumanResourceForm> forms = CxHumanResourceForm.GetForms(pageNumber, companyId, productionUnitId);
            var results = from f in forms
                          select new
                          {
                              HumanResourceFormID = f.HumanResourceFormID,
                              FormDescription = f.Form.Description,
                              Company = f.ProductionUnit.Company.CompanyName,
                              ProductionUnitDescription = f.ProductionUnit.Description,
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