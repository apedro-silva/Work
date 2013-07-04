using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SMIZEE.Management
{
    public partial class Management : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageDescription(Resources.Resource.lManagement);

        }

    }
}