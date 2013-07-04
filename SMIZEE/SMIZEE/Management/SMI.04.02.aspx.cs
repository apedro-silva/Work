using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMIZEE.Models;

namespace SMIZEE.Management
{
    public partial class SMI_04_02 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options,true, false, false, false, false, false, false);
                SetPageDescription(Resources.MainMenu.IM_04_02);
            }
        }

        public IQueryable<Role> GetRoles(int pageNumber, string roleName)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<Role> query = db.Roles;

            query = query.Where(p => (roleName == null | (p.RoleName == roleName)))
                .OrderBy(p => p.RoleId).Skip(numberOfObjectsPerPage * (pageNumber-1)).Take(numberOfObjectsPerPage+1);

            return query;

        }
        protected void OnOptions(object sender, EventArgs e)
        {
            OptionsPanel.Enabled = false;
            SearchPanel.Visible = true;
            SearchPanelButtons.Visible = true;
            btnConfirm.CommandName = Options.SelectedValue;
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
            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }
        protected void OnCancel(object sender, EventArgs e)
        {
            Response.Redirect("SMI.04.02.aspx");
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

                Label roleId = currentRow.FindControl("RoleIdLabel") as Label;
                IdInput.Text = Server.HtmlDecode(roleId.Text).Trim();

                Label roleName = currentRow.FindControl("RoleNameLabel") as Label;
                DescriptionInput.Text = Server.HtmlDecode(roleName.Text).Trim();

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
            ddlPaging.Visible = true;
        }
        protected void OnConfirm(object sender, EventArgs e)
        {
            string operationResult = "Successo";
            string optionSelected = null;
            string description = Server.HtmlDecode(DescriptionInput.Text).Trim();
            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                optionSelected = btnConfirm.CommandName;
                switch (optionSelected)
                {
                    case "CRE": CreateRecord(description); 
                                break;
                    case "DEL": DeleteRecord(int.Parse(IdInput.Text)); 
                                break;
                    case "UPD": UpdateRecord(int.Parse(IdInput.Text), description); 
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
                AuditAction.Create(Page.User.Identity.Name, "SMI.04.02", GetOperation(optionSelected), operationResult);
            }

        }

        private void UpdateRecord(int code, string description)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var myItem = (from c in db.Companies where c.CompanyID == code select c).First();
                if (myItem != null)
                {
                    myItem.CompanyName = description;
                    db.SaveChanges();
                }
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mUpdateOK);
        }

        private void DeleteRecord(int code)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                db.Database.ExecuteSqlCommand("delete from Companies where CompanyId={0}", code);
                //var myItem = (from c in db.Company where c.CompanyID == code select c).First();
                //if (myItem != null)
                //{
                //    db.Company.Remove(myItem);
                //    db.SaveChanges();
                //}
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mDeleteOK);

        }

        private void CreateRecord(string description)
        {
            using (var db = new Models.SmizeeContext())
            {
                Company entity = new Company();
                entity.CompanyName = description;
                entity.Description = description;

                db.Companies.Add(entity);
                db.SaveChanges();
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mCreateOK);
        }

        private void BindEntities(int pageNumber)
        {
            string roleName = RoleNameSearchInput.Text == "" ? null : RoleNameSearchInput.Text;

            IQueryable<Role> roles = GetRoles(pageNumber, roleName);
            List<Role> listEntities = roles.ToList();

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