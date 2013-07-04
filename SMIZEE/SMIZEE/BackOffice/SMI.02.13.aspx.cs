using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMIZEE.Models;

namespace SMIZEE.Account
{
    public partial class SMI_02_13 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options);
                SetPageDescription(Resources.MainMenu.IM_02_13);
                BigPanel.DefaultButton = "OptionsBtn";
                Options.Focus();
            }

        }

        public IQueryable<Form> GetForms(int pageNumber, int? FormId, string Description)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<Form> query = db.Forms;

            query = query.Where(p => (Description == null | (p.Description.StartsWith(Description))) & (FormId == null | (p.FormID == FormId)))
                .OrderBy(p => p.FormID).Skip(numberOfObjectsPerPage * (pageNumber-1)).Take(numberOfObjectsPerPage+1);

            return query;

        }
        protected void OnOptions(object sender, EventArgs e)
        {
            OptionsPanel.Enabled = false;
            SearchPanel.Visible = true;
            SearchPanelButtons.Visible = true;
            btnConfirm.CommandName = Options.SelectedValue;
            BindPeriodicity(ddlPeriodicity);
            BindFormTypes();
            BigPanel.DefaultButton = "btnSearch";
            FormIdSearchInput.Focus();
        }
        protected void OnQuery(object sender, EventArgs e)
        {
            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                string optionSelected = btnConfirm.CommandName;

                BindEntities(1);

                if (optionSelected == "CRE")
                {
                    DetailPanel.Visible = true;
                    ConfirmButtonPanel.Visible = true;
                    IdInput.Text = "Auto";
                    IdInput.Enabled = false;
                }
                BigPanel.DefaultButton = "btnConfirm";
                DescriptionInput.Focus();
            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }
        protected void OnCancel(object sender, EventArgs e)
        {
            Response.Redirect("SMI.02.13.aspx");
        }
        protected void OnGridView1SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string optionSelected = btnConfirm.CommandName;
                GridViewRow currentRow = GridView1.SelectedRow;
                CleanMessage(MessagePanel, ErrorPanel);
                if (currentRow == null)
                    return;

                DetailPanel.Visible = true;
                DetailPanel.Enabled = true;
                ConfirmButtonPanel.Visible = true;
                BackPanel.Visible = false;

                Label FormId = currentRow.FindControl("FormIdLabel") as Label;
                IdInput.Text = Server.HtmlDecode(FormId.Text).Trim();

                Label Description = currentRow.FindControl("DescriptionLabel") as Label;
                DescriptionInput.Text = Server.HtmlDecode(Description.Text).Trim();

                Label smallDescription = currentRow.FindControl("SmallDescriptionLabel") as Label;
                SmallDescriptionInput.Text = Server.HtmlDecode(smallDescription.Text).Trim();

                HiddenField periodicityIdLabel = currentRow.FindControl("PeriodicityIdLabel") as HiddenField;
                ddlPeriodicity.SelectedValue = Server.HtmlDecode(periodicityIdLabel.Value).Trim();

                HiddenField formTypeLabel = currentRow.FindControl("FormTypeLabel") as HiddenField;
                ddlFormType.SelectedValue = Server.HtmlDecode(formTypeLabel.Value).Trim();

                HiddenField alarmUserRangeHidden = currentRow.FindControl("AlarmUserRangeHidden") as HiddenField;
                SliderAlarmUser.Text = alarmUserRangeHidden.Value;

                HiddenField alarmManagerRangeHidden = currentRow.FindControl("AlarmManagerRangeHidden") as HiddenField;
                SliderAlarmManager.Text = alarmManagerRangeHidden.Value;

                HiddenField alarmExecutiveRangeHidden = currentRow.FindControl("AlarmExecutiveRangeHidden") as HiddenField;
                SliderAlarmExecutive.Text = alarmExecutiveRangeHidden.Value;

                switch (optionSelected)
                {

                    case "QRY": ConfirmButtonPanel.Visible = false;
                                DetailPanel.Enabled = false;
                                BackPanel.Visible = true;
                                break;
                    case "DEL": DetailPanel.Enabled = false;
                                break;
                    case "UPD": IdInput.Enabled = false; 
                                SliderExtenderUser.Enabled=true;
                                SliderExtenderManager.Enabled=true;
                                SliderExtenderExecutive.Enabled=true;
                                break;
                    case "CRE": IdInput.Text = "Auto"; 
                                SliderExtenderUser.Enabled = true; 
                                SliderExtenderManager.Enabled=true;
                                SliderExtenderExecutive.Enabled=true;
                                break;
                }
                BigPanel.DefaultButton = "btnConfirm";
                DescriptionInput.Focus();

            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }
        protected void OnPagingSelectedIndexChanged(object sender, EventArgs e)
        {
            // Get page Code
            BindEntities(int.Parse(ddlPaging.SelectedValue));

            PagingPanel.Visible = true;
        }
        protected void OnConfirm(object sender, EventArgs e)
        {
            string operationResult = "Successo";
            string optionSelected = null;
            string description = Server.HtmlDecode(DescriptionInput.Text).Trim();
            string smallDescription = Server.HtmlDecode(SmallDescriptionInput.Text).Trim();
            int periodicityId = int.Parse(ddlPeriodicity.SelectedValue);
            int formTypeId = int.Parse(ddlFormType.SelectedValue);

            CleanMessage(MessagePanel, ErrorPanel);

            if (DescriptionInput.Text == "")
            {
                ShowError(ErrorPanel, Resources.Resource.mMandatoryDescription);
                return;
            }
            if (SmallDescriptionInput.Text == "")
            {
                ShowError(ErrorPanel, Resources.Resource.mMandatorySmallDescription);
                return;
            }

            try
            {
                optionSelected = btnConfirm.CommandName;
                switch (optionSelected)
                {
                    case "CRE": CreateRecord(description, smallDescription, periodicityId, formTypeId); 
                                break;
                    case "DEL": DeleteRecord(int.Parse(IdInput.Text)); 
                                break;
                    case "UPD": UpdateRecord(int.Parse(IdInput.Text), description, smallDescription, periodicityId, formTypeId); 
                                break;
                    case "QRY": break;
                }
                BindEntities(1);
            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
                operationResult = exp.Message;
            }
            finally
            {
                AuditAction.Create(Page.User.Identity.Name, "SMI.02.13", GetOperation(optionSelected), operationResult);
            }

        }

        private void UpdateRecord(int code, string description, string smallDescription, int periodicityId, int formTypeId)
        {

            if (periodicityId == 0)
                throw new Exception(Resources.Resource.mPeriodicityMandatory);

            CxForm.UpdateRecord(code, description, smallDescription, periodicityId, formTypeId, int.Parse(SliderAlarmUser.Text), int.Parse(SliderAlarmManager.Text), int.Parse(SliderAlarmExecutive.Text));

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordUpdateOK);
        }

        private void DeleteRecord(int code)
        {
            CxForm.DeleteRecord(code);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordDeleteOK);

        }

        private void CreateRecord(string description, string smallDescription, int periodicityId, int formTypeId)
        {
            if (periodicityId == 0)
                throw new Exception(Resources.Resource.mPeriodicityMandatory);

            CxForm.CreateRecord(description, smallDescription, periodicityId, formTypeId, int.Parse(SliderAlarmUser.Text), int.Parse(SliderAlarmManager.Text), int.Parse(SliderAlarmExecutive.Text));

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordCreateOK);
        }

        private void BindEntities(int pageNumber)
        {
            string Description = FormDescriptionSearchInput.Text == "" ? null : FormDescriptionSearchInput.Text;

            int? FormId = null;
            if (FormIdSearchInput.Text != "")
                FormId = int.Parse(FormIdSearchInput.Text);

            IQueryable<Form> Forms = GetForms(pageNumber, FormId, Description);

            var results = from p in Forms
                          select new
                          {
                              FormId=p.FormID,
                              Description = p.Description,
                              SmallDescription = p.SmallDescription,
                              PeriodicityDescription = p.Periodicity.Description,
                              PeriodicityId = p.Periodicity.PeriodicityID,
                              FormTypeId = p.FormTypeID,
                              AlarmRangeUser = p.AlarmRangeUser,
                              AlarmRangeManager = p.AlarmRangeManager,
                              AlarmRangeExecutive = p.AlarmRangeExecutive
                          };


            var listEntities = results.ToList();
            GridView1.DataSource = listEntities.Take(10);
            GridView1.DataBind();

            //int pageSize = PageRecordsInput.Text == "" ? GridView1.PageSize : int.Parse(PageRecordsInput.Text);
            int pageSize = GridView1.PageSize;

            if (listEntities.Count> pageSize)
                SetPages(ddlPaging, pageNumber + 1);
            else
                SetPages(ddlPaging, pageNumber);

            ddlPaging.SelectedIndex = pageNumber - 1;

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

                ListPanel.Visible = true;
                DetailPanel.Visible = false;
                BackPanel.Visible = false;
                ConfirmButtonPanel.Visible = false;
            }
        }
        private void BindFormTypes()
        {
            var db = new Models.SmizeeContext();
            IQueryable<FormType> formTypes = db.FormTypes;
            List<FormType> listEntities = formTypes.ToList();

            ddlFormType.DataSource = listEntities;
            ddlFormType.DataTextField = "Description";
            ddlFormType.DataValueField = "FormTypeID";
            ddlFormType.DataBind();
            ddlFormType.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));
        }

        protected void OnPeriodicitySelectedIndexChanged(object sender, EventArgs e)
        {
            int periodicity = int.Parse(ddlPeriodicity.SelectedItem.Value);
            int minimumPeriod = 0;
            int maximumPeriod = 0;

            switch (periodicity)
            {
                case 1: //Diário
                        minimumPeriod = 1;
                        maximumPeriod = 1;
                        break;
                case 2: //Semanal
                        minimumPeriod = 1;
                        maximumPeriod = 7;
                        break;
                case 3: //Mensal
                        minimumPeriod = 1;
                        maximumPeriod = 30;
                        break;
                case 4: //Trimestral
                        minimumPeriod = 1;
                        maximumPeriod = 90;
                        break;
                case 5: //Semestral
                        minimumPeriod = 1;
                        maximumPeriod = 120;
                        break;
                case 6: //Anual
                        minimumPeriod = 1;
                        maximumPeriod = 365;
                        break;
                default: break;
            }
            SliderExtenderUser.Minimum = minimumPeriod;
            SliderExtenderUser.Maximum = maximumPeriod;
            
            SliderExtenderManager.Minimum = minimumPeriod;
            SliderExtenderManager.Maximum = maximumPeriod;

            SliderExtenderExecutive.Minimum = minimumPeriod;
            SliderExtenderExecutive.Maximum = maximumPeriod;
        }
    }
}