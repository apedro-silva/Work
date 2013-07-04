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
    public partial class SMI_02_14 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options);
                SetPageDescription(Resources.MainMenu.IM_02_14);
                BigPanel.DefaultButton = "OptionsBtn";
                Options.Focus();
            }
        }


        public IQueryable<Timeline> GetTimelines(int pageNumber, int? TimelineId, string SmallDescription, string description)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<Timeline> query = db.Timelines;

            query = query.Where(p => (SmallDescription == null | (p.SmallDescription.StartsWith(SmallDescription))) & (description == null | (p.Description == description)) & (TimelineId == null | (p.TimelineID == TimelineId)))
                .OrderBy(p => p.TimelineID ).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;

        }
        protected void OnOptions(object sender, EventArgs e)
        {
            OptionsPanel.Enabled = false;
            SearchPanel.Visible = true;
            SearchPanelButtons.Visible = true;
            btnConfirm.CommandName = Options.SelectedValue;
            BigPanel.DefaultButton = "btnSearch";
            TimelineIdSearchInput.Focus();
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
            Response.Redirect("SMI.02.14.aspx");
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

                Label TimelineId = currentRow.FindControl("TimelineIdLabel") as Label;
                IdInput.Text = Server.HtmlDecode(TimelineId.Text).Trim();

                Label TimelineDescription = currentRow.FindControl("TimelineDescriptionLabel") as Label;
                DescriptionInput.Text = Server.HtmlDecode(TimelineDescription.Text).Trim();
                                                                              
                Label TimelineSmallDescription = currentRow.FindControl("TimelineSmallDescriptionLabel") as Label;
                SmallDescriptionInput.Text = Server.HtmlDecode(TimelineSmallDescription.Text).Trim();

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

            DetailPanel.Visible = false;
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;

            switch (optionSelected)
            {
                case "CRE": DetailPanel.Visible = true;
                            DetailPanel.Enabled = true;
                            BackPanel.Visible = false;
                            ConfirmButtonPanel.Visible = true;
                            break;
                case "QRY": break;
                case "DEL": break;
                case "UPD": ;
                            break;
            }
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
                AuditAction.Create(Page.User.Identity.Name, "SMI.02.14", GetOperation(optionSelected), operationResult);
            }

        }

        private void UpdateRecord(int code,string SmallDescription, string description)
        {
            CxTimeline.UpdateRecord(code,SmallDescription, description);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordUpdateOK);
        }

        private void DeleteRecord(int code)
        {
            CxTimeline.DeleteRecord(code);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordDeleteOK);

        }

        private void CreateRecord(string SmallDescription,string description)
        {
            CxTimeline.CreateRecord(SmallDescription,description);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordCreateOK);
        }

        private void BindEntities(int pageNumber)
        {
            string description = TimelineDescriptionSearchInput.Text == "" ? null : TimelineDescriptionSearchInput.Text;
            string SmallDescription = TimelineSmallDescriptionSearchInput.Text == "" ? null : TimelineSmallDescriptionSearchInput.Text;

            int? TimelineId = null;
            if (TimelineIdSearchInput.Text != "")
                TimelineId = int.Parse(TimelineIdSearchInput.Text);


            IQueryable<Timeline> Timelines = GetTimelines(pageNumber, TimelineId, SmallDescription, description);
            List<Timeline> listEntities = Timelines.ToList();

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
                ListPanel.Visible = true;
                PagingPanel.Visible = true;
            }


        }
    }
}