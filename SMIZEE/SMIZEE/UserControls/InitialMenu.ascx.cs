using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SMIZEE.UserControls
{
    public partial class InitialMenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           NameValueCollection reqParams = Request.Params;

           string pathInfo = reqParams["PATH_INFO"];

        }
    }
}