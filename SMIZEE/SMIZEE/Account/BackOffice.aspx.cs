using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SMIZEE.Account
{
    public partial class BackOffice : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageDescription(Resources.Resource.lBackOffice);

        }

    }
}