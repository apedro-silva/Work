using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Threading;
using System.Web.UI.WebControls;
using System.Web.Security;
using SMIZEE.Models;
using SMIZEE.CodeFirstMembership;

namespace SMIZEE.Management
{
    public partial class SMI_04_01 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.Sleep(300);
            
            if (!IsPostBack)
                SetTransactionOptions(Options, true, true, true, false,false,false,false);
            SetPageDescription(Resources.MainMenu.IM_04_01);
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
                }


                BindCompanies(ddlCompany);
                BindFunctionalArea(ddlFunctionalArea);
                BindRoles();
            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }
        protected void OnCancel(object sender, EventArgs e)
        {
            Response.Redirect("SMI.04.01.aspx");
        }
        protected void OnResetPwd(object sender, EventArgs e)
        {
            MembershipUser user = Membership.GetUser(UserNameInput.Text);
            string newPwd = user.ResetPassword();

        }

        protected void OnGridView1SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Guid userId;
                string optionSelected = btnConfirm.CommandName;
                GridViewRow currentRow = GridView1.SelectedRow;
                CleanMessage(MessagePanel, ErrorPanel);
                if (currentRow == null)
                    return;

                DetailPanel.Visible = true;
                DetailPanel.Enabled = true;
                ConfirmButtonPanel.Visible = true;
                BackPanel.Visible = false;

                Label userName = currentRow.FindControl("UserNameLabel") as Label;
                UserNameInput.Text = Server.HtmlDecode(userName.Text).Trim();

                userId = WebSecurity.GetUserId(userName.Text);

                Label email = currentRow.FindControl("EmailLabel") as Label;
                EmailInput.Text = Server.HtmlDecode(email.Text).Trim();

                Label lastName = currentRow.FindControl("LastNameLabel") as Label;
                LastNameInput.Text = Server.HtmlDecode(lastName.Text).Trim();

                Label firstName = currentRow.FindControl("FirstNameLabel") as Label;
                FirstNameInput.Text = Server.HtmlDecode(firstName.Text).Trim();

                HiddenField companyIdHidden = currentRow.FindControl("CompanyIDHidden") as HiddenField;
                ddlCompany.SelectedValue = companyIdHidden.Value == "" ? "0" : companyIdHidden.Value;

                HiddenField functionalAreaId = currentRow.FindControl("FunctionalAreaIdHidden") as HiddenField;
                ddlFunctionalArea.SelectedValue = functionalAreaId.Value == "" ? "0" : functionalAreaId.Value;

                HiddenField isManager = currentRow.FindControl("IsManagerHidden") as HiddenField;
                cbManager.Checked = isManager.Value=="True"?true:false;

                HiddenField isExecutive = currentRow.FindControl("IsExecutiveHidden") as HiddenField;
                cbExecutive.Checked = isExecutive.Value == "True" ? true : false;

                HiddenField isLockedOut = currentRow.FindControl("IsLockedOutHidden") as HiddenField;
                cbIsLockedOut.Checked = isLockedOut.Value == "True" ? true : false;

                PasswordInput.Text = "";
                ConfirmPasswordInput.Text = "";

                int companyId = -1;
                if (ddlCompany.SelectedValue != "0")
                    companyId = int.Parse(ddlCompany.SelectedValue);
                ProductionUnitGrid.BindList(userId, CxProductionUnit.GetListByCompanyId(companyId));

                Guid? roleId=null;
                string roleName;

                WebSecurity.GetUserRoleName(userName.Text, out roleId, out roleName);
                CurrentRoleNameHidden.Value = roleName;

                if (roleId != null)
                    ddlMembershipRole.SelectedValue = roleId.ToString();
                else
                    ddlMembershipRole.SelectedIndex = 0;

                switch (optionSelected)
                {

                    case "QRY": ConfirmButtonPanel.Visible = false;
                                DetailPanel.Enabled = false;
                                BackPanel.Visible = true;
                                UserNameInput.Enabled = false;
                                break;
                    case "DEL": UserNameInput.Enabled = false; 
                                DetailPanel.Enabled = false;
                                break;
                    case "UPD": UserNameInput.Enabled = false;
                                PasswordInput.Enabled = false;
                                ConfirmPasswordInput.Enabled = false;
                                break;
                    case "CRE": UserNameInput.Enabled = true; 
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
            ddlPaging.Visible = true;
        }
        protected void OnConfirm(object sender, EventArgs e)
        {
            string operationResult = "Successo";
            string optionSelected = null;
            string email = Server.HtmlDecode(EmailInput.Text).Trim();
            string userName = UserNameInput.Text;
            string password = PasswordInput.Text;
            string firstName = FirstNameInput.Text;
            string lastName = LastNameInput.Text;
            int companyID = int.Parse(ddlCompany.SelectedValue);
            int functionalAreaID = int.Parse(ddlFunctionalArea.SelectedValue);
            Boolean isManager = cbManager.Checked;
            Boolean isExecutive = cbExecutive.Checked;
            Boolean? isLockedOut = cbIsLockedOut.Checked;

            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                optionSelected = btnConfirm.CommandName;
                switch (optionSelected)
                {
                    case "CRE": if (!IsValidUser()) return;
                        CreateRecord(userName, password, email, firstName, lastName, companyID, functionalAreaID, isManager, isExecutive, isLockedOut); 
                                break;
                    case "DEL": DeleteRecord(userName); 
                                break;
                    case "UPD": UpdateRecord(userName, password, email, firstName, lastName, companyID, functionalAreaID, isManager, isExecutive, isLockedOut); 
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
                AuditAction.Create(Page.User.Identity.Name, "SMI.04.01", GetOperation(optionSelected), operationResult);
            }

        }

        private bool IsValidUser()
        {
            string email = Server.HtmlDecode(EmailInput.Text).Trim();
            string userName = UserNameInput.Text;
            string password = PasswordInput.Text;
            string firstName = FirstNameInput.Text;
            string lastName = LastNameInput.Text;
            string passwordInput = PasswordInput.Text;
            string confirmPasswordInput = ConfirmPasswordInput.Text;
            if (userName == "")
            {
                ShowError(ErrorPanel, "Introduza um utilizador válido");
                return false;
            }
            if (email == "")
            {
                ShowError(ErrorPanel, "Introduza um email válido");
                return false;
            }
            if (firstName == "")
            {
                ShowError(ErrorPanel, "Introduza um nome válido");
                return false;
            }
            if (lastName == "")
            {
                ShowError(ErrorPanel, "Introduza um apelido válido");
                return false;
            }
            if (passwordInput == "")
            {
                ShowError(ErrorPanel, "Introduza um Código Secreto válido");
                return false;
            }
            if (confirmPasswordInput == "")
            {
                ShowError(ErrorPanel, "Introduza a confirmação de Código Secreto");
                return false;
            }
            if (!confirmPasswordInput.Equals(passwordInput))
            {
                ShowError(ErrorPanel, Resources.Resource.mPasswordsDoNotMatch);
                return false;
            }

            return true;
        }

        private void UpdateRecord(string userName, string password, string email, string firstName, string lastName, int? companyID, int? functionalAreaID, Boolean? isManager, Boolean? isExecutive, Boolean? isLockedOut)
        {

            //Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var myItem = (from c in db.Users where c.Username == userName select c).First();
                if (myItem != null)
                {
                    myItem.FirstName = firstName;
                    myItem.LastName = lastName;
                    myItem.Email = email;
                    myItem.CompanyID = companyID;
                    myItem.FunctionalAreaID = functionalAreaID;
                    myItem.IsManager = isManager;
                    myItem.IsExecutive = isExecutive;
                    db.SaveChanges();
                }

                WebSecurity.LockUser(userName, (bool)isLockedOut);

                string roleName = CurrentRoleNameHidden.Value;
                if (!string.IsNullOrEmpty(roleName))
                    System.Web.Security.Roles.RemoveUserFromRole(userName, roleName);

                if (ddlMembershipRole.SelectedValue.Equals("0"))
                    System.Web.Security.Roles.AddUserToRole(userName, "User");
                else
                    System.Web.Security.Roles.AddUserToRole(userName, ddlMembershipRole.SelectedItem.Text);

                // remove all country export records associated to the current formId
                CxUserProductionUnit.Delete(myItem.UserId);

                GridView gv = ProductionUnitGrid.GetGridView();
                CheckBox selected = null;
                HiddenField pid = null;
                UserProductionUnit upu = null;
                Guid userId = WebSecurity.GetUserId(userName);

                foreach (GridViewRow currentRow in gv.Rows)
                {
                    selected = currentRow.FindControl("SelectRowCheckBox") as CheckBox;
                    if (selected.Checked)
                    {
                        pid = currentRow.FindControl("ProductionUnitIDHidden") as HiddenField;
                        upu = new UserProductionUnit();
                        upu.UserID = userId;
                        upu.ProductionUnitID = int.Parse(pid.Value);
                        db.UserProductionUnits.Add(upu);
                    }
                }
                db.SaveChanges();

            }

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mUserUpdateOK);
        }

        private void DeleteRecord(string userName)
        {
            // do not delete user from database
            // change the status
            //WebSecurity.DeleteUser(userName);
            WebSecurity.LockUser(userName, true);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mUserDeleteOK);

        }

        private void CreateRecord(string userName, string password, string email, string firstName, string lastName, int? companyID, int? functionalAreaID, Boolean? isManager, Boolean? isExecutive, Boolean? isLockedOut)
        {
            MembershipCreateStatus status = WebSecurity.Register(userName, password, email, true, firstName, lastName, companyID == 0 ? null : companyID, functionalAreaID == 0 ? null : functionalAreaID, isManager, isExecutive);

            if (status == MembershipCreateStatus.Success)
            {
                WebSecurity.LockUser(userName, (bool)isLockedOut);

                if (ddlMembershipRole.SelectedValue.Equals("0"))
                    System.Web.Security.Roles.AddUserToRole(userName, "User");
                else
                    System.Web.Security.Roles.AddUserToRole(userName, ddlMembershipRole.SelectedItem.Text);

                GridView gv = ProductionUnitGrid.GetGridView();
                CheckBox selected = null;
                HiddenField pid = null;
                UserProductionUnit upu = null;
                Guid userId = WebSecurity.GetUserId(userName);

                using (var db = new Models.SmizeeContext())
                {
                    foreach (GridViewRow currentRow in gv.Rows)
                    {
                        selected = currentRow.FindControl("SelectRowCheckBox") as CheckBox;
                        if (selected.Checked)
                        {
                            pid = currentRow.FindControl("ProductionUnitIDHidden") as HiddenField;
                            upu = new UserProductionUnit();
                            upu.UserID = userId;
                            upu.ProductionUnitID = int.Parse(pid.Value);
                            db.UserProductionUnits.Add(upu);
                        }
                    }
                    db.SaveChanges();
                }

                BackPanel.Visible = true;
                ConfirmButtonPanel.Visible = false;
                DetailPanel.Visible = false;
                ShowInfo(MessagePanel, Resources.Resource.mUserCreateOK);
            }
            else
                ShowError(ErrorPanel, status.ToString());


        }

        private void BindEntities(int pageNumber)
        {
            string userName = UserNameSearchInput.Text == "" ? null : UserNameSearchInput.Text;
            string firstName = FisrtNameSearchInput.Text == "" ? null : FisrtNameSearchInput.Text;

            IQueryable<User> users = CxUser.GetUsers(pageNumber, userName, firstName, null);
            List<User> listEntities = users.ToList();

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

        private void BindRoles()
        {
            var db = new Models.SmizeeContext();
            IQueryable<Role> query = db.Roles;
            List<Role> listEntities = query.ToList();

            ddlMembershipRole.DataSource = listEntities;
            ddlMembershipRole.DataTextField = "RoleName";
            ddlMembershipRole.DataValueField = "RoleId";

            ddlMembershipRole.DataBind();
            ddlMembershipRole.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));
        }

        protected void OnCompanySelectedIndexChanged(object sender, EventArgs e)
        {
            Guid? userId = null;
            int companyId = -1;

            string optionSelected = btnConfirm.CommandName;
            if (!optionSelected.Equals("CRE"))
                userId = WebSecurity.GetUserId(UserNameInput.Text);

            if (ddlCompany.SelectedValue != "0")
                companyId = int.Parse(ddlCompany.SelectedValue);

            ProductionUnitGrid.BindList(userId, CxProductionUnit.GetListByCompanyId(companyId));
        }

    }
}