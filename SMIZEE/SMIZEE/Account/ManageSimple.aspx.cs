using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SMIZEE.Account
{
    public partial class ManageSimple : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected bool CanRemoveExternalLogins
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/ManageSimple.aspx");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "Your password has been changed."
                        : message == "SetPwdSuccess" ? "Your password has been set."
                        : message == "RemoveLoginSuccess" ? "The external login was removed."
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }
            }

        }

        protected void OnPasswordChange(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Response.Redirect("~/Account/ManageSimple.aspx?m=SetPwdSuccess");
            }
        }
        

        protected void setPassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Response.Redirect("~/Account/ManageSimple.aspx?m=SetPwdSuccess");
            }
        }

    }
}