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
    public partial class SMI_02_05 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options);
                SetPageDescription(Resources.MainMenu.IM_02_05);
                BigPanel.DefaultButton = "OptionsBtn";
                Options.Focus();
            }
        }


        public IQueryable<FunctionalArea> GetFunctionalAreas(int pageNumber, int? FunctionalAreaId, string SmallDescription, string description)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<FunctionalArea> query = db.FunctionalAreas;

            query = query.Where(p => (SmallDescription == null | (p.SmallDescription.StartsWith(SmallDescription))) & (description == null | (p.Description == description)) & (FunctionalAreaId == null | (p.FunctionalAreaID == FunctionalAreaId)))
                .OrderBy(p => p.FunctionalAreaID ).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;

        }
        protected void OnOptions(object sender, EventArgs e)
        {
            OptionsPanel.Enabled = false;
            SearchPanel.Visible = true;
            SearchPanelButtons.Visible = true;
            btnConfirm.CommandName = Options.SelectedValue;
            BigPanel.DefaultButton = "btnSearch";
            DescriptionInput.Focus();
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
            Response.Redirect("SMI.02.05.aspx");
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

                Label FunctionalAreaId = currentRow.FindControl("FunctionalAreaIdLabel") as Label;
                IdInput.Text = Server.HtmlDecode(FunctionalAreaId.Text).Trim();

                Label FunctionalAreaDescription = currentRow.FindControl("FunctionalAreaDescriptionLabel") as Label;
                DescriptionInput.Text = Server.HtmlDecode(FunctionalAreaDescription.Text).Trim();
                                                                              
                Label FunctionalAreaSmallDescription = currentRow.FindControl("FunctionalAreaSmallDescriptionLabel") as Label;
                SmallDescriptionInput.Text = Server.HtmlDecode(FunctionalAreaSmallDescription.Text).Trim();

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
                AuditAction.Create(Page.User.Identity.Name, "SMI.02.05", GetOperation(optionSelected), operationResult);
            }

        }

        private void UpdateRecord(int code,string SmallDescription, string description)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var myItem = (from c in db.FunctionalAreas where c.FunctionalAreaID == code select c).First();
                if (myItem != null)
                {
                    myItem.SmallDescription = SmallDescription;
                    myItem.Description = description;
                    db.SaveChanges();
                }
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordUpdateOK);
        }

        private void DeleteRecord(int code)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                db.Database.ExecuteSqlCommand("delete from FunctionalAreas where FunctionalAreaId={0}", code);
                //var myItem = (from c in db.Companies where c.CompanyID == code select c).First();
                //if (myItem != null)
                //{
                //    db.Companies.Remove(myItem);
                //    db.SaveChanges();
                //}
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordDeleteOK);

        }

        private void CreateRecord(string SmallDescription,string description)
        {
            using (var db = new Models.SmizeeContext())
            {
                FunctionalArea entity = new FunctionalArea();
                entity.SmallDescription = SmallDescription;
                entity.Description = description;

                db.FunctionalAreas.Add(entity);
                db.SaveChanges();
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordCreateOK);
        }

        private void BindEntities(int pageNumber)
        {
            string description = FunctionalAreaDescriptionSearchInput.Text == "" ? null : FunctionalAreaDescriptionSearchInput.Text;
            string SmallDescription = FunctionalAreaSmallDescriptionSearchInput.Text == "" ? null : FunctionalAreaSmallDescriptionSearchInput.Text;

            int? FunctionalAreaId = null;
            if (FunctionalAreaIdSearchInput.Text != "")
                FunctionalAreaId = int.Parse(FunctionalAreaIdSearchInput.Text);


            IQueryable<FunctionalArea> FunctionalAreas = GetFunctionalAreas(pageNumber, FunctionalAreaId, SmallDescription, description);
            List<FunctionalArea> listEntities = FunctionalAreas.ToList();

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