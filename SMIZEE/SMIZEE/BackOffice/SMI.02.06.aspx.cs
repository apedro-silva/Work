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
    public partial class SMI_02_06 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options);
                SetPageDescription(Resources.MainMenu.IM_02_06);
                BigPanel.DefaultButton = "OptionsBtn";
                Options.Focus();
            }
        }


        public IQueryable<Sector> GetSectors(int pageNumber, int? SectorId, string SmallDescription, string description)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<Sector> query = db.Sectors;

            query = query.Where(p => (SmallDescription == null | (p.SmallDescription.StartsWith(SmallDescription))) & (description == null | (p.Description == description)) & (SectorId == null | (p.SectorID == SectorId)))
                .OrderBy(p => p.SectorID ).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;

        }
        protected void OnOptions(object sender, EventArgs e)
        {
            OptionsPanel.Enabled = false;
            SearchPanel.Visible = true;
            SearchPanelButtons.Visible = true;
            btnConfirm.CommandName = Options.SelectedValue;
            BigPanel.DefaultButton = "btnSearch";
            SectorIdSearchInput.Focus();
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
            Response.Redirect("SMI.02.06.aspx");
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

                Label SectorId = currentRow.FindControl("SectorIdLabel") as Label;
                IdInput.Text = Server.HtmlDecode(SectorId.Text).Trim();

                Label SectorDescription = currentRow.FindControl("SectorDescriptionLabel") as Label;
                DescriptionInput.Text = Server.HtmlDecode(SectorDescription.Text).Trim();
                                                                              
                Label SectorSmallDescription = currentRow.FindControl("SectorSmallDescriptionLabel") as Label;
                SmallDescriptionInput.Text = Server.HtmlDecode(SectorSmallDescription.Text).Trim();

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
            // Get page Code
            BindEntities(int.Parse(ddlPaging.SelectedValue));

            PagingPanel.Visible = true;
        }
        protected void OnConfirm(object sender, EventArgs e)
        {
            string operationResult = "Successo";
            string optionSelected = null;
            string SmallDescription = Server.HtmlDecode(SmallDescriptionInput.Text).Trim();
            string description = Server.HtmlDecode(DescriptionInput.Text).Trim();
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
                    case "CRE": CreateRecord(SmallDescription,description); 
                                break;
                    case "DEL": DeleteRecord(int.Parse(IdInput.Text)); 
                                break;
                    case "UPD": UpdateRecord(int.Parse(IdInput.Text), SmallDescription, description); 
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
                AuditAction.Create(Page.User.Identity.Name, "SMI.02.06", GetOperation(optionSelected), operationResult);
            }

        }

        private void UpdateRecord(int code,string SmallDescription, string description)
        {
            CxSector.UpdateRecord(code,SmallDescription, description);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordUpdateOK);
        }

        private void DeleteRecord(int code)
        {
            CxSector.DeleteRecord(code);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordDeleteOK);

        }

        private void CreateRecord(string SmallDescription,string description)
        {
            CxSector.CreateRecord(SmallDescription,description);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordCreateOK);
        }

        private void BindEntities(int pageNumber)
        {
            string description = SectorDescriptionSearchInput.Text == "" ? null : SectorDescriptionSearchInput.Text;
            string SmallDescription = SectorSmallDescriptionSearchInput.Text == "" ? null : SectorSmallDescriptionSearchInput.Text;

            int? SectorId = null;
            if (SectorIdSearchInput.Text != "")
                SectorId = int.Parse(SectorIdSearchInput.Text);


            IQueryable<Sector> Sectors = GetSectors(pageNumber, SectorId, SmallDescription, description);
            List<Sector> listEntities = Sectors.ToList();

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
    }
}