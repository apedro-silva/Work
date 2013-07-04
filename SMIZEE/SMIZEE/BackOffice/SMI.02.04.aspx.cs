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
    public partial class SMI_02_04 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options);
                SetPageDescription(Resources.MainMenu.IM_02_04);
                BigPanel.DefaultButton = "OptionsBtn";
                Options.Focus();
            }
        }


        public IQueryable<Area> GetAreas(int pageNumber, int? AreaId, string SmallDescription, string description)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<Area> query = db.Areas;

            query = query.Where(p => (SmallDescription == null | (p.SmallDescription.StartsWith(SmallDescription))) & (description == null | (p.Description == description)) & (AreaId == null | (p.AreaID == AreaId)))
                .OrderBy(p => p.AreaID ).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;

        }
        protected void OnOptions(object sender, EventArgs e)
        {
            OptionsPanel.Enabled = false;
            SearchPanel.Visible = true;
            SearchPanelButtons.Visible = true;
            btnConfirm.CommandName = Options.SelectedValue;
            BigPanel.DefaultButton = "btnSearch";
            AreaIdSearchInput.Focus();
        }
        protected void OnQuery(object sender, EventArgs e)
        {
            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                string optionSelected = btnConfirm.CommandName;

                BindEntities(1);
                DescriptionInput.Text = "";
                SmallDescriptionInput.Text = "";

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
            Response.Redirect("SMI.02.04.aspx");
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

                Label AreaId = currentRow.FindControl("AreaIdLabel") as Label;
                IdInput.Text = Server.HtmlDecode(AreaId.Text).Trim();

                Label AreaDescription = currentRow.FindControl("AreaDescriptionLabel") as Label;
                DescriptionInput.Text = Server.HtmlDecode(AreaDescription.Text).Trim();
                                                                              
                Label AreaSmallDescription = currentRow.FindControl("AreaSmallDescriptionLabel") as Label;
                SmallDescriptionInput.Text = Server.HtmlDecode(AreaSmallDescription.Text).Trim();

                Label Hectares = currentRow.FindControl("HectaresLabel") as Label;
                HectaresInput.Text = Server.HtmlDecode(Hectares.Text).Trim();

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
            string SmallDescription = Server.HtmlDecode(SmallDescriptionInput.Text).Trim();
            string description = Server.HtmlDecode(DescriptionInput.Text).Trim();
            int? hectares = GetIntValueFromInput(HectaresInput.Text);

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
                    case "CRE": CreateRecord(SmallDescription, description, hectares); 
                                break;
                    case "DEL": DeleteRecord(int.Parse(IdInput.Text)); 
                                break;
                    case "UPD": UpdateRecord(int.Parse(IdInput.Text), SmallDescription, description, hectares); 
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
                AuditAction.Create(Page.User.Identity.Name, "SMI.02.04", GetOperation(optionSelected), operationResult);
            }

        }

        private void UpdateRecord(int code, string SmallDescription, string description, int? hectares)
        {

            CxArea.UpdateRecord(code, SmallDescription, description, hectares);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordUpdateOK);
        }

        private void DeleteRecord(int code)
        {
            CxArea.DeleteRecord(code);
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordDeleteOK);

        }

        private void CreateRecord(string SmallDescription, string description, int? hectares)
        {
            CxArea.CreateRecord(SmallDescription, description, hectares);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordCreateOK);
        }

        private void BindEntities(int pageNumber)
        {
            string description = AreaDescriptionSearchInput.Text == "" ? null : AreaDescriptionSearchInput.Text;
            string SmallDescription = AreaSmallDescriptionSearchInput.Text == "" ? null : AreaSmallDescriptionSearchInput.Text;

            int? AreaId = null;
            if (AreaIdSearchInput.Text != "")
                AreaId = int.Parse(AreaIdSearchInput.Text);

            IQueryable<Area> Areas = GetAreas(pageNumber, AreaId, SmallDescription, description);
            List<Area> listEntities = Areas.ToList();

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

        protected void OnGridView1RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow x = e.Row;

            if (x.RowType != DataControlRowType.DataRow)
                return;

            Label hectaresLabel = x.FindControl("HectaresLabel") as Label;
            int? hectares = GetIntValueFromInput(hectaresLabel.Text);
            if (hectares != null)
            {
                int val = (int)hectares;
                hectaresLabel.Text = val.ToString("###,###");
            }

        }
    }
}