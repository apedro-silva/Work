using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMIZEE.Models;
using SMIZEE.CodeFirstMembership;

namespace SMIZEE.UserControls
{
    public partial class AplContext : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (WebSecurity.IsAuthenticated && !IsPostBack)
                {
                    //User user = WebSecurity.GetUserInfo(Page.User.Identity.Name);
                    //UserNameLiteral.Text = string.Format("{0} {1}", user.FirstName, user.LastName);

                    //Session.Add("CompanyID", user.CompanyID);

                    //Company company = CxCompany.GetCompany(user.CompanyID);

                    //if (company != null)
                    //    CompanyLiteral.Text = company.Description;
                    //else
                    //    CompanyLiteral.Text = "";

                    string returnUrl = HttpUtility.UrlEncode(Request.QueryString["PU"]);

                    string pageDescription = (string)(Session["PageDescription"]);
                    PageLiteral.Text = pageDescription;
                }

            }
            catch (Exception)
            {
            }

        }
    }
}
