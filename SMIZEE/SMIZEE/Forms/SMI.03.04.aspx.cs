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
    public partial class SMI_03_04 : BasePage
    {
        private string currentOption = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options, true, false, false, false, false, false, false);
                //SetTransactionOptions(Options);
                SetPageDescription(Resources.MainMenu.IM_03_04);
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
                    OperationalForm entity = CxOperationalForm.GetFormById(int.Parse(fefid));
                    FormMessageLiteral.Text = GetPeriodMessage(Resources.Resource.lForm2Message, entity.PeriodNumber, entity.Form.Periodicity.Code, entity.FormDate);
                    if (stateid == "3")
                    {
                        DetailPanel.Enabled = false;
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
            Response.Redirect("SMI.03.04.aspx");
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
                HiddenField formDateHidden = currentRow.FindControl("FormDateHidden") as HiddenField;
                DateTime formDate = DateTime.Parse(formDateHidden.Value);
                FormMessageLiteral.Text = GetPeriodMessage(Resources.Resource.lForm2Message, int.Parse(periodNumber.Value), periodicityCode.Value, formDate);

                Label operationalFormId = currentRow.FindControl("OperationalFormIDLabel") as Label;
                CompanyFormID.Value = operationalFormId.Text;
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
                var entity = (from c in db.OperationalForms where c.OperationalFormID == formId select c).First();
                if (entity != null)
                {
                    NumeroTotalUnidadesProdutivasInput.Text = GetDddlSelectedValue(entity.ProductionUnitsTotalNumber);
                    NumeroHectaresEspacoUtilizadoInput.Text = GetStringValue(entity.UsedSpace);
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
            int operationalFormId = 0;

            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                operationalFormId = int.Parse(CompanyFormID.Value); 
                UpdateRecord(operationalFormId, formState);
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
                AuditAction.Create(Page.User.Identity.Name, "SMI.03.04", GetOperation(optionSelected), operationResult);
            }

        }

        private void UpdateRecord(int operationalFormId, int formState)
        {
            int? numeroHectaresEspacoUtilizado = GetIntValueFromInput(NumeroHectaresEspacoUtilizadoInput.Text);
            int? numeroTotalUnidadesProdutivas = GetIntValueFromInput(NumeroTotalUnidadesProdutivasInput.Text);

            int? numeroHectaresQuadrante1 = GetIntValueFromInput(NumeroHectaresEspacoUtilizadoInput.Text);
            int? numeroHectaresQuadrante2 = GetIntValueFromInput(NumeroHectaresEspacoUtilizadoInput.Text);
            int? numeroHectaresQuadrante3 = GetIntValueFromInput(NumeroHectaresEspacoUtilizadoInput.Text);
            int? numeroHectaresQuadrante4 = GetIntValueFromInput(NumeroHectaresEspacoUtilizadoInput.Text);

            CxOperationalForm.UpdateRecord(operationalFormId, formState, User.Identity.Name, 
                                            numeroHectaresQuadrante1, numeroHectaresQuadrante2, 
                                            numeroHectaresQuadrante3, numeroHectaresQuadrante4,
                                            numeroHectaresEspacoUtilizado, numeroTotalUnidadesProdutivas);

        }

        private void DeleteRecord(int code)
        {
            CxOperationalForm.DeleteRecord(code);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mDeleteOK);

        }

        private void CreateRecord(int formId)
        {
            int? numeroHectaresQuadrante1 = GetIntValueFromInput(NumeroHectaresEspacoUtilizadoInput.Text);
            int? numeroHectaresQuadrante2 = GetIntValueFromInput(NumeroHectaresEspacoUtilizadoInput.Text);
            int? numeroHectaresQuadrante3 = GetIntValueFromInput(NumeroHectaresEspacoUtilizadoInput.Text);
            int? numeroHectaresQuadrante4 = GetIntValueFromInput(NumeroHectaresEspacoUtilizadoInput.Text);
            int? numeroHectaresEspacoUtilizado = GetIntValueFromInput(NumeroHectaresEspacoUtilizadoInput.Text);
            int? numeroTotalUnidadesProdutivas = GetIntValueFromInput(NumeroTotalUnidadesProdutivasInput.Text);

            CxOperationalForm.CreateRecord(formId, User.Identity.Name,
                                            numeroHectaresQuadrante1, numeroHectaresQuadrante2,
                                            numeroHectaresQuadrante3, numeroHectaresQuadrante4,
                                            numeroHectaresEspacoUtilizado, numeroTotalUnidadesProdutivas);

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

            IQueryable<OperationalForm> forms = CxOperationalForm.GetForms(pageNumber, year, periodicity, user.FunctionalAreaID);
            var results = from f in forms
                          select new
                          {
                              OperationalFormId = f.OperationalFormID,
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