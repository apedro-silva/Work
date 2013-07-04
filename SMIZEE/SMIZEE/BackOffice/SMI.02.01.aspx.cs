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
    public partial class SMI_02_01 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options);
                SetPageDescription(Resources.MainMenu.IM_02_01);
                BigPanel.DefaultButton = "OptionsBtn";
                Options.Focus();
            }
        }

        public IQueryable<Company> GetCompanies(int pageNumber, int? companyId, string companyName)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<Company> query = db.Companies;

            query = query.Where(p => (companyName == null | (p.CompanyName.StartsWith(companyName))) & (companyId == null | (p.CompanyID == companyId)))
                .OrderBy(p => p.CompanyID).Skip(numberOfObjectsPerPage * (pageNumber-1)).Take(numberOfObjectsPerPage+1);

            return query;

        }
        protected void OnOptions(object sender, EventArgs e)
        {
            OptionsPanel.Enabled = false;
            SearchPanel.Visible = true;
            SearchPanelButtons.Visible = true;
            btnConfirm.CommandName = Options.SelectedValue;
            BigPanel.DefaultButton = "btnSearch";
            CompanyIdSearchInput.Focus();

        }
        protected void OnQuery(object sender, EventArgs e)
        {
            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                string optionSelected = btnConfirm.CommandName;

                BigPanel.DefaultButton = "btnConfirm";

                DescriptionInput.Text = "";
                BindEntities(1);
                switch (optionSelected)
                {
                    case "CRE": DetailPanel.Visible = true;
                                DetailPanel.Enabled = true;
                                BackPanel.Visible = false;
                                ConfirmButtonPanel.Visible = true;
                                IdInput.Text = "Auto";
                                IdInput.Enabled = false;
                                break;
                    case "QRY": BigPanel.DefaultButton = "btnBack";
                                break;
                    case "DEL": break;
                    case "UPD": ;
                        break;
                }
                CompanyNameInput.Focus();

            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }
        protected void OnCancel(object sender, EventArgs e)
        {
            Response.Redirect("SMI.02.01.aspx");
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

                Label companyId = currentRow.FindControl("CompanyIdLabel") as Label;
                IdInput.Text = Server.HtmlDecode(companyId.Text).Trim();

                Label description = currentRow.FindControl("DescriptionLabel") as Label;
                DescriptionInput.Text = Server.HtmlDecode(description.Text).Trim();

                Label companyName = currentRow.FindControl("CompanyNameLabel") as Label;
                CompanyNameInput.Text = Server.HtmlDecode(companyName.Text).Trim();

                switch (optionSelected)
                {

                    case "QRY": ConfirmButtonPanel.Visible = false;
                                DetailPanel.Enabled = false;
                                BackPanel.Visible = true;
                                break;
                    case "DEL": DetailPanel.Enabled = false;
                                break;
                    case "UPD": IdInput.Enabled = false;
                                break;
                    case "CRE": IdInput.Text = "Auto"; 
                                break;
                }

            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }
        protected void OnPagingSelectedIndexChanged(object sender, EventArgs e)
        {
            string optionSelected = btnConfirm.CommandName;
            
            // Get page Code
            BindEntities(int.Parse(ddlPaging.SelectedValue));
            PagingPanel.Visible = true;

        }
        protected void OnConfirm(object sender, EventArgs e)
        {
            string operationResult = "Successo";
            string optionSelected = null;
            string description = Server.HtmlDecode(DescriptionInput.Text).Trim();
            string companyName = Server.HtmlDecode(CompanyNameInput.Text).Trim();

            CleanMessage(MessagePanel, ErrorPanel);

            if (CompanyNameInput.Text == "")
            {
                ShowError(ErrorPanel, Resources.Resource.mMandatoryCompanyName);
                return;
            }
            if (DescriptionInput.Text == "")
            {
                ShowError(ErrorPanel, Resources.Resource.mMandatoryCompanyDescription);
                return;
            }
            try
            {
                optionSelected = btnConfirm.CommandName;
                switch (optionSelected)
                {
                    case "CRE": CreateRecord(companyName, description); 
                                break;
                    case "DEL": DeleteRecord(int.Parse(IdInput.Text)); 
                                break;
                    case "UPD": UpdateRecord(int.Parse(IdInput.Text), companyName, description); 
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
                AuditAction.Create(Page.User.Identity.Name, "SMI.02.01", GetOperation(optionSelected), operationResult);
            }

        }

        private void UpdateRecord(int code, string companyName, string description)
        {

            CxCompany.UpdateRecord(code, companyName, description);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordUpdateOK);
        }

        private void DeleteRecord(int code)
        {
            CxCompany.DeleteRecord(code);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordDeleteOK);

        }

        private void CreateRecord(string companyName, string description)
        {
            CxCompany.CreateRecord(companyName, description);
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordCreateOK);
        }

        private void BindEntities(int pageNumber)
        {
            string companyName = CompanyDescriptionSearchInput.Text == "" ? null : CompanyDescriptionSearchInput.Text;

            int? companyId = null;
            if (CompanyIdSearchInput.Text != "")
                companyId = int.Parse(CompanyIdSearchInput.Text);

            IQueryable<Company> companies = GetCompanies(pageNumber, companyId, companyName);
            List<Company> listEntities = companies.ToList();

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
    }
}