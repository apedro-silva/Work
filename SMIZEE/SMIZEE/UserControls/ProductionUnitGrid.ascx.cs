using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMIZEE.Models;

namespace SMIZEE.UserControls
{
    public partial class ProductionUnitGrid : System.Web.UI.UserControl
    {
        private Guid? _UserId;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindList(Guid? userId, List<ProductionUnit> productionUnitDS)
        {
            _UserId = userId;

            var results = from f in productionUnitDS
                          select new
                          {
                              ProductionUnitID = f.ProductionUnitID,
                              Description = f.Description,
                          };


            var listEntities = results.ToList();

            GridView1.DataSource = listEntities;
            GridView1.DataBind();

        }

        public GridView GetGridView()
        {
            return GridView1;
        }

        protected void OnGridView1RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (row.RowType != DataControlRowType.DataRow)
                return;

            CheckBox selectRowCheckBox = row.FindControl("SelectRowCheckBox") as CheckBox;

            HiddenField productionUnitIDHidden = row.FindControl("ProductionUnitIDHidden") as HiddenField;
            int puid = int.Parse(productionUnitIDHidden.Value);

            if (_UserId!=null && CxUserProductionUnit.IsUserProductionUnit(_UserId, puid))
                selectRowCheckBox.Checked = true;
        }
    }
}
