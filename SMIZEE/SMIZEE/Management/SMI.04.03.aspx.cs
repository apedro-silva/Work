using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMIZEE.Models;
using SMIZEE.Classes;

namespace SMIZEE.Management
{
    public partial class SMI_04_03 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options,true,false,false,false,false,false,false);
                SetPageDescription(Resources.MainMenu.IM_04_03);
                BindServcies();
            }
        }

        protected void OnOptions(object sender, EventArgs e)
        {
            OptionsPanel.Enabled = false;
            SearchPanel.Visible = true;
            SearchPanelButtons.Visible = true;
        }
        protected void OnQuery(object sender, EventArgs e)
        {
            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                string optionSelected = btnConfirm.CommandName;

                BindEntities(1);

            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }
        protected void OnCancel(object sender, EventArgs e)
        {
            Response.Redirect("SMI.04.03.aspx");
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

                ConfirmButtonPanel.Visible = false;
                DetailPanel.Enabled = false;
                BackPanel.Visible = true;

                Label auditId = currentRow.FindControl("AuditIdLabel") as Label;
                AuditIdInput.Text = Server.HtmlDecode(auditId.Text).Trim();

                Label userName = currentRow.FindControl("UserNameLabel") as Label;
                UserNameInput.Text = Server.HtmlDecode(userName.Text).Trim();

                Label applicationPage = currentRow.FindControl("ApplicationPageLabel") as Label;
                ApplicationPageInput.Text = Server.HtmlDecode(applicationPage.Text).Trim();

                Label serviceDescription = currentRow.FindControl("ServiceDescriptionLabel") as Label;
                ServiceDescriptionInput.Text = Server.HtmlDecode(serviceDescription.Text).Trim();

                Label operation = currentRow.FindControl("OperationLabel") as Label;
                OperationInput.Text = Server.HtmlDecode(operation.Text).Trim();

                Label result = currentRow.FindControl("ResultLabel") as Label;
                ResultInput.Text = Server.HtmlDecode(result.Text).Trim();

                Label timestamp = currentRow.FindControl("TimestampLabel") as Label;
                TimestampInput.Text = Server.HtmlDecode(timestamp.Text).Trim();

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
            ddlPaging.Visible = true;
        }
        protected void OnConfirm(object sender, EventArgs e)
        {
            string operationResult = "Successo";
            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                BindEntities(1);
            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
                operationResult = exp.Message;
            }
            finally
            {
                AuditAction.Create(Page.User.Identity.Name, "SMI.04.03", GetOperation("QRY"), operationResult);
            }

        }

        private void BindEntities(int pageNumber)
        {
            CultureInfo culture = new CultureInfo("pt-PT");
            string applicationPage = ApplicationPageSearchInput.SelectedValue == "0" ? null : ApplicationPageSearchInput.SelectedValue;
            string userName = UserNameSearchInput.Text == "" ? null : UserNameSearchInput.Text;
            DateTime? startTimestamp = null;
            DateTime? endTimestamp =  null;

            if (StartDateInputControl.Text != "")
                startTimestamp = DateTime.Parse(StartDateInputControl.Text + " 00:00");

            if (EndDateInputControl.Text != "")
                endTimestamp = DateTime.Parse(EndDateInputControl.Text + " 23:59");

            SmizeeContext db = new Models.SmizeeContext();
            IQueryable<Audit> audits = CxAudits.GetList(db, pageNumber, applicationPage, userName, startTimestamp, endTimestamp);

            var srv = from f in CxAudits.GetServices(db) select f;

            var listEntities = from f in audits
                               join y1 in srv on f.ApplicationPage equals y1.ApplicationPage
                               select new
                                {
                                    AuditId = f.AuditID,
                                    UserName = f.UserName,
                                    ApplicationPage = f.ApplicationPage,
                                    Description=f.Operation.Description,
                                    Result = f.Result,
                                    Timestamp=f.Timestamp,
                                    ServiceDescription = y1.Description
                                    
                                };

            GridView1.DataSource = listEntities.ToList().Take(10);
            GridView1.DataBind();

            int pageSize = GridView1.PageSize;

            if (listEntities.Count() == 0)
            {
                ShowInfo(MessagePanel, Resources.Resource.mNoRecordsFound);
                ListPanel.Visible = false;
                PagingPanel.Visible = false;
            }
            else
            {
                if (listEntities.Count() > pageSize)
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
        public void BindServcies()
        {
            var db = new Models.SmizeeContext();
            IQueryable<Service> services= db.Services;
            List<Service> listEntities = services.ToList();

            ApplicationPageSearchInput.DataSource = listEntities;
            ApplicationPageSearchInput.DataTextField = "Description";
            ApplicationPageSearchInput.DataValueField = "ApplicationPage";
            ApplicationPageSearchInput.DataBind();
            ApplicationPageSearchInput.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));
        }

    }
}