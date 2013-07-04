using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMIZEE.Models;

namespace SMIZEE.Account
{
    public partial class SMI_02_11 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindArea(null, null, null);
                BindCompany(null, null, null);
                BindSector(null, null, null);
                SetTransactionOptions(Options);
                SetPageDescription(Resources.MainMenu.IM_02_11);
                BigPanel.DefaultButton = "OptionsBtn";
                Options.Focus();
            }

        }


        public IQueryable<ProductionUnit> GetProductionUnits(int pageNumber, int? ProductionUnitId, int? AreaId, int? CompanyId, int? SectorId)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<ProductionUnit> query = db.ProductionUnits;

            query = query.Where(p => (AreaId == 0 | (p.AreaID == AreaId)) & (CompanyId == 0 | (p.CompanyID == CompanyId)) & (SectorId == 0 | (p.SectorID == SectorId)) & (ProductionUnitId == null | (p.ProductionUnitID == ProductionUnitId)))
                .OrderBy(p => p.ProductionUnitID).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;

        }

        public IQueryable<Area> GetAreas( int? AreaId, string SmallDescription, string description)
        {
            //int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<Area> query = db.Areas;

            query = query.Where(p => (SmallDescription == null | (p.SmallDescription == SmallDescription)) & (description == null | (p.Description == description)) & (AreaId == null | (p.AreaID == AreaId)))
                .OrderBy(p => p.AreaID);

            return query;

        }

        public IQueryable<Company> GetCompanies(int? CompanyId, string CompanyName, string description)
        {
            //int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<Company> query = db.Companies;

            query = query.Where(p => (CompanyName == null | (p.CompanyName == CompanyName)) & (description == null | (p.Description == description)) & (CompanyId == null | (p.CompanyID == CompanyId)))
                .OrderBy(p => p.CompanyID);

            return query;

        }

        public IQueryable<Sector> GetSectors(int? SectorId, string SmallDescription, string description)
        {
            //int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<Sector> query = db.Sectors;

            query = query.Where(p => (SmallDescription == null | (p.SmallDescription == SmallDescription)) & (description == null | (p.Description == description)) & (SectorId == null | (p.SectorID == SectorId)))
                .OrderBy(p => p.SectorID);

            return query;

        }

        protected void OnOptions(object sender, EventArgs e)
        {
            OptionsPanel.Enabled = false;
            SearchPanel.Visible = true;
            SearchPanelButtons.Visible = true;
            btnConfirm.CommandName = Options.SelectedValue;
            BigPanel.DefaultButton = "btnSearch";
            ProductionUnitIdSearchInput.Focus();
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
                BigPanel.DefaultButton = "btnConfirm";
                DescriptionInput.Focus();
            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }
        protected void OnCancel(object sender, EventArgs e)
        {
            Response.Redirect("SMI.02.11.aspx");
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

                Label ProductionUnitDescription = currentRow.FindControl("ProductionUnitDescriptionLabel") as Label;
                DescriptionInput.Text = Server.HtmlDecode(ProductionUnitDescription.Text).Trim();


                Label ProductionUnitId = currentRow.FindControl("ProductionUnitIDLabel") as Label;
                IdInput.Text = Server.HtmlDecode(ProductionUnitId.Text).Trim();

                HiddenField AreaId = currentRow.FindControl("AreaIdHidden") as HiddenField;
                CBArea.SelectedValue = AreaId.Value;

                HiddenField CompanyId = currentRow.FindControl("CompanyIdHidden") as HiddenField;
                CBCompany.SelectedValue = CompanyId.Value;

                HiddenField SectorId = currentRow.FindControl("SectorIdHidden") as HiddenField;
                CBSector.SelectedValue = SectorId.Value;

                Label Hectares = currentRow.FindControl("HectaresLabel") as Label;
                HectaresInput.Text = Server.HtmlDecode(Hectares.Text).Trim();

                
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
                BigPanel.DefaultButton = "btnConfirm";
                DescriptionInput.Focus();

            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }
        protected void OnPagingSelectedIndexChanged(object sender, EventArgs e)
        {
            
            // Get page Code
            BindEntities(int.Parse(ddlPaging.SelectedValue));

            PagingPanel.Visible = true;
        }
        protected void OnConfirm(object sender, EventArgs e)
        {
            string operationResult = "Successo";
            string optionSelected = null;

            string description = DescriptionInput.Text;
            int areaId = int.Parse(CBArea.SelectedValue);
            int companyId = int.Parse(CBCompany.SelectedValue);
            int sectorId = int.Parse(CBSector.SelectedValue);
            int? hectares = GetIntValueFromInput(HectaresInput.Text);

            CleanMessage(MessagePanel, ErrorPanel);

            if (DescriptionInput.Text == "")
            {
                ShowError(ErrorPanel, Resources.Resource.mMandatoryDescription);
                return;
            }
            if (areaId == 0)
            {
                ShowError(ErrorPanel, Resources.Resource.mMandatoryArea);
                return;
            }
            if (sectorId == 0)
            {
                ShowError(ErrorPanel, Resources.Resource.mMandatorySector);
                return;
            }
            if (companyId == 0)
            {
                ShowError(ErrorPanel, Resources.Resource.mMandatoryCompany);
                return;
            }


            try
            {
                optionSelected = btnConfirm.CommandName;
                switch (optionSelected)
                {
                    case "CRE": CreateRecord(description, areaId, companyId, sectorId, hectares); 
                                break;
                    case "DEL": DeleteRecord(int.Parse(IdInput.Text)); 
                                break;
                    case "UPD": UpdateRecord(int.Parse(IdInput.Text), description, areaId, companyId, sectorId, hectares); 
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
                AuditAction.Create(Page.User.Identity.Name, "SMI.02.11", GetOperation(optionSelected), operationResult);
            }

        }

        private void UpdateRecord(int code, string description, int AreaId, int CompanyId, int SectorId, int? Hectares)
        {
            CxProductionUnit.UpdateRecord(code, description, AreaId, CompanyId, SectorId, Hectares);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordUpdateOK);
        }

        private void DeleteRecord(int code)
        {
            CxProductionUnit.DeleteRecord(code);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordDeleteOK);

        }

        private void CreateRecord(string description, int AreaId,int CompanyId,int SectorId, int? Hectares)
        {
            CxProductionUnit.CreateRecord(description, AreaId,CompanyId,SectorId, Hectares);

            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mRecordCreateOK);
        }

        private void BindEntities(int pageNumber)
        {
            int AreaId = int.Parse(CBAreaIdSearchInput.SelectedValue);
            int CompanyId = int.Parse(CBCompanyIdSearchInput.SelectedValue);
            int SectorId = int.Parse(CBSectorIdSearchInput.SelectedValue);

            int? ProductionUnitId = null;
            if (ProductionUnitIdSearchInput.Text != "")
                ProductionUnitId = int.Parse(ProductionUnitIdSearchInput.Text);

            IQueryable<ProductionUnit> productionUnits = GetProductionUnits(pageNumber, ProductionUnitId, AreaId, CompanyId, SectorId);

            var results = from p in productionUnits
                          select new
                          {
                              ProductionUnitDescription = p.Description,
                              ProductionUnitId = p.ProductionUnitID,
                              AreaId = p.AreaID,
                              AreaDescription = p.Area.Description,
                              SectorDescription = p.Sector.Description,
                              SectorId = p.SectorID,
                              CompanyDescription = p.Company.CompanyName,
                              CompanyId = p.CompanyID,
                              Hectares = p.Hectares
                          };


            var listEntities = results.ToList();


            GridView1.DataSource = listEntities.Take(10);
            GridView1.DataBind();

            //int pageSize = PageRecordsInput.Text == "" ? GridView1.PageSize : int.Parse(PageRecordsInput.Text);
            int pageSize = GridView1.PageSize;

            if (listEntities.Count> pageSize)
                SetPages(ddlPaging, pageNumber + 1);
            else
                SetPages(ddlPaging, pageNumber);

            ddlPaging.SelectedIndex = pageNumber - 1;

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

                ListPanel.Visible = true;
                DetailPanel.Visible = false;
                BackPanel.Visible = false;
                ConfirmButtonPanel.Visible = false;
            }
        }

        private void BindArea( int? AreaId, string SmallDescription, string description)
        {
            IQueryable<Area> Areas = GetAreas(AreaId, SmallDescription, description);
            List<Area> listEntities = Areas.ToList();

            CBArea.DataSource = listEntities.Take(10);
            CBArea.DataTextField  = "description";
            CBArea.DataValueField  = "AreaId";
            CBArea.DataBind();
            CBArea.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));


            CBAreaIdSearchInput.DataSource = listEntities.Take(10);
            CBAreaIdSearchInput.DataTextField = "description";
            CBAreaIdSearchInput.DataValueField = "AreaId";
            CBAreaIdSearchInput.DataBind();
            CBAreaIdSearchInput.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));

        }
        private void BindCompany(int? CompanyId, string CompanyName, string description)
        {
            IQueryable<Company> Companies = GetCompanies(CompanyId, CompanyName, description);
            List<Company> listEntities = Companies.ToList();

            CBCompany.DataSource = listEntities.Take(10);
            CBCompany.DataTextField = "CompanyName";
            CBCompany.DataValueField = "CompanyId";
            CBCompany.DataBind();
            CBCompany.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));

            CBCompanyIdSearchInput.DataSource = listEntities.Take(10);
            CBCompanyIdSearchInput.DataTextField = "CompanyName";
            CBCompanyIdSearchInput.DataValueField = "CompanyId";
            CBCompanyIdSearchInput.DataBind();
            CBCompanyIdSearchInput.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));

        }
        private void BindSector(int? SectorId, string SmallDescription, string description)
        {
            IQueryable<Sector> Sectors = GetSectors(SectorId, SmallDescription, description);
            List<Sector> listEntities = Sectors.ToList();

            CBSector.DataSource = listEntities.Take(10);
            CBSector.DataTextField = "description";
            CBSector.DataValueField = "SectorId";
            CBSector.DataBind();
            CBSector.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));

            CBSectorIdSearchInput.DataSource = listEntities.Take(10);
            CBSectorIdSearchInput.DataTextField = "description";
            CBSectorIdSearchInput.DataValueField = "SectorId";
            CBSectorIdSearchInput.DataBind();
            CBSectorIdSearchInput.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));
        }

    }
}