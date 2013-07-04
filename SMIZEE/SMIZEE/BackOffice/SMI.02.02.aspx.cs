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
    public partial class SMI_02_02 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options);
                SetPageDescription(Resources.MainMenu.IM_02_02);
                BigPanel.DefaultButton = "OptionsBtn";
                Options.Focus();
            }
        }

        public IQueryable<Country> GetCountries(int pageNumber, int? countryId, string description)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<Country> query = db.Countries;

            query = query.Where(p => (description == null | (p.Description.StartsWith(description))) & (countryId == null | (p.CountryID == countryId)))
                .OrderBy(p => p.CountryID).Skip(numberOfObjectsPerPage * (pageNumber-1)).Take(numberOfObjectsPerPage+1);

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

                DescriptionInput.Text = "";
                SmallDescriptionInput.Text = "";
                IsoInput.Text = "";
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
            Response.Redirect("SMI.02.02.aspx");
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

                Label companyName = currentRow.FindControl("CompanyNameLabel") as Label;
                DescriptionInput.Text = Server.HtmlDecode(companyName.Text).Trim();

                Label smallDescription = currentRow.FindControl("SmallDescriptionLabel") as Label;
                SmallDescriptionInput.Text = Server.HtmlDecode(smallDescription.Text).Trim();

                Label isoCode = currentRow.FindControl("IsoCodeLabel") as Label;
                IsoInput.Text = Server.HtmlDecode(isoCode.Text).Trim();

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
            string smallDescription = Server.HtmlDecode(SmallDescriptionInput.Text).Trim();
            string isoCode = Server.HtmlDecode(IsoInput.Text).Trim();
            CleanMessage(MessagePanel, ErrorPanel);

            if (DescriptionInput.Text == "")
            {
                ShowError(ErrorPanel, Resources.Resource.mMandatoryCountryName);
                return;
            }
            if (SmallDescriptionInput.Text == "")
            {
                ShowError(ErrorPanel, Resources.Resource.mMandatoryCountryLabelName);
                return;
            }
            if (IsoInput.Text == "")
            {
                ShowError(ErrorPanel, Resources.Resource.mMandatoryCountryIsoCode);
                return;
            }

            try
            {
                optionSelected = btnConfirm.CommandName;
                switch (optionSelected)
                {
                    case "CRE": CreateRecord(smallDescription, description, isoCode); 
                                break;
                    case "DEL": DeleteRecord(int.Parse(IdInput.Text)); 
                                break;
                    case "UPD": UpdateRecord(int.Parse(IdInput.Text), smallDescription, description, isoCode); 
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
                AuditAction.Create(Page.User.Identity.Name, "SMI.02.02", GetOperation(optionSelected), operationResult);
            }

        }

        private void UpdateRecord(int countryId, string smallDescription, string description, string isoCode)
        {
            CxCountry.UpdateRecord(countryId, smallDescription, description, isoCode);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordUpdateOK);
        }

        private void DeleteRecord(int code)
        {
            CxCountry.DeleteRecord(code);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordDeleteOK);

        }

        private void CreateRecord(string smallDescription, string description, string isoCode)
        {
            CxCountry.CreateRecord(smallDescription, description, isoCode);
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordCreateOK);
        }

        private void BindEntities(int pageNumber)
        {
            string description = CompanyDescriptionSearchInput.Text == "" ? null : CompanyDescriptionSearchInput.Text;

            int? countryId = null;
            if (CompanyIdSearchInput.Text != "")
                countryId = int.Parse(CompanyIdSearchInput.Text);

            IQueryable<Country> countries = GetCountries(pageNumber, countryId, description);
            List<Country> listEntities = countries.ToList();

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

                ListPanel.Visible = true;
                DetailPanel.Visible = false;
                BackPanel.Visible = false;
                ConfirmButtonPanel.Visible = false;
            }


        }
    }
}