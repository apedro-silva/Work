using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMIZEE.Models;
using SMIZEE.CodeFirstMembership;

namespace SMIZEE
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Lang.SelectedValue = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                LanguageLiteral.Text = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            }
            if (Request.IsAuthenticated)
            {
                MenuContent.Controls.Add(LoadControl("UserControls/AplMenu.ascx"));
            }
            else
                MenuContent.Controls.Add(LoadControl("UserControls/InitialMenu.ascx"));

            if (WebSecurity.IsAuthenticated)
            {
                FeaturedContent.Controls.Add(LoadControl("UserControls/AplContext.ascx"));
                ShowUserContext();
            }
        }
        protected void OnLangSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies["lang"].Value = Lang.SelectedItem.Value;
                Response.Cookies["lang"].Expires = DateTime.Now.AddYears(10);
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            catch (Exception exp)
            {
                string x = exp.Message;
            }
        }

        private void ShowUserContext()
        {
            User user = WebSecurity.GetUserInfo(Page.User.Identity.Name);

            Company company = CxCompany.GetCompany(user.CompanyID);

            if (company != null)
            {
                LoginName loginName = AplLoginView.FindControl("LoginName1") as LoginName;
                loginName.FormatString = "{0} | " + company.Description;

                Session.Add("CompanyID", user.CompanyID);
            }
        }
    }
}