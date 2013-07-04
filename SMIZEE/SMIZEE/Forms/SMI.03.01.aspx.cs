using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SMIZEE.Models;
using SMIZEE.CodeFirstMembership;

namespace SMIZEE.Forms
{
    public partial class SMI_03_01 : BasePage
    {
        private string currentOption = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options, true, false, false, false, false, false, false);
                SetPageDescription(Resources.MainMenu.IM_03_01);

                currentOption = Options.SelectedValue;

                string pu = Request.QueryString["PU"];
                string fefid = Request.QueryString["FID"];
                string stateid = Request.QueryString["ST"];
                if (fefid != null)
                {
                    CompanyFormID.Value = fefid;
                    currentOption = "UPD";
                    OptionsPanel.Visible = false;
                    SearchPanel.Visible = false;
                    DetailPanel.Visible = true;
                    InputPanel.Visible = true;
                    ConfirmButtonPanel.Visible = true;
                    FormPanel.Visible = true;
                    FormPanel.Enabled = true;

                    BindCountries(ddlCountry);
                    FinancialExportForm entity = CxFinancialExportForm.GetFormById(int.Parse(fefid));
                    FormMessageLiteral.Text = GetPeriodMessage(Resources.Resource.lForm2Message, entity.PeriodNumber, entity.Form.Periodicity.Code, entity.FormDate);

                    if (stateid == "3")
                    {
                        DetailPanel.Enabled = false;
                        InputPanel.Visible = false;
                        ApprovePanel.Visible = true;
                        ConfirmButtonPanel.Visible = false;
                        FormGridView.Columns[0].Visible = false;
                        FormMessageLiteral.Text = "";
                    }


                    ProductionUnitLiteral.Text = pu;
                    FillDateLiteral.Text = DateTime.Now.ToShortDateString();
                    ShowCurrentForm(int.Parse(fefid));
                }
            }
            else
            {
                currentOption = Options.SelectedValue;
                User user = WebSecurity.GetUserInfo(Page.User.Identity.Name);
                int? companyId = user.CompanyID;

                if (companyId != null && companyId!=0)
                {
                    ddlCompany.SelectedValue = companyId.ToString();
                    ddlCompany.Enabled = false;
                }
            }

        }

        protected void OnOptions(object sender, EventArgs e)
        {
            try
            {
                OptionsPanel.Enabled = false;

                if (!currentOption.Equals("CRE"))
                {
                    SearchPanel.Visible = true;
                    SearchPanelButtons.Visible = true;
                }
                else
                {
                    DetailPanel.Visible = true;
                    ConfirmButtonPanel.Visible = true;
                    FormPanel.Visible = true;
                }
                BindCountries(ddlCountry);
                BindCompanies(ddlCompany);
                int companyId = int.Parse(ddlCompany.SelectedValue);
                BindProductionUnits(ddlProductionUnit, companyId);
            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }
        }
        protected void OnQuery(object sender, EventArgs e)
        {
            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                BindEntities(1);

            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }
        protected void OnCancel(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("SMI.03.01.aspx");
            }
            catch (Exception)
            {
            }
        }
        protected void OnAddCountry(object sender, EventArgs e)
        {
            int countryId = int.Parse(ddlCountry.SelectedValue);
            string amountInput = AmountInput.Text;
            try
            {
                CleanMessage(MessagePanel, ErrorPanel);

                FormGridView.SelectedIndex = -1;

                if (ddlCountry.SelectedValue == "0")
                {
                    throw new Exception(Resources.Resource.mMandatoryCountry);
                }
                if (AmountInput.Text.Trim() == "")
                {
                    throw new Exception(Resources.Resource.mMandatoryExportAmount);
                }

                DataTable exportAmounts = GetExportAmounts();

                IsCountryInTable(exportAmounts, countryId);

                decimal amountExport = decimal.Parse(AmountInput.Text.Trim(), new CultureInfo("pt-PT"));

                AddExportAmount(exportAmounts, ddlCountry, amountExport);

                ddlCountry.SelectedIndex = -1;
                AmountInput.Text = "";

                FormGridView.DataSource = exportAmounts;
                FormGridView.DataBind();
                FormGridView.SelectedIndex = -1;
                SearchPanelButtons.Visible = false;
            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }

        private void IsCountryInTable(DataTable exportAmounts, int countryId)
        {
            DataRow[] result = exportAmounts.Select("CountryId=" + countryId);
            if (result.Count() == 1)
                throw new Exception("Pais já seleccionado");

        }

        private DataTable GetExportAmounts()
        {
            DataTable exportAmounts = new DataTable();
            HiddenField countryId = null;
            Label countryDescription = null;
            Label exportAmount = null;

            exportAmounts.Columns.Add(GetDataTableColumn("CountryId", "System.Int32"));
            exportAmounts.Columns.Add(GetDataTableColumn("CountryDescription", "System.String"));
            exportAmounts.Columns.Add(GetDataTableColumn("ExportAmount", "System.Decimal"));

            object[] ea = new object[3];

            foreach (GridViewRow row in FormGridView.Rows)
            {
                countryId = row.FindControl("CountryIdHidden") as HiddenField;
                exportAmount = row.FindControl("ExportAmountLabel") as Label;
                countryDescription = row.FindControl("CountryDescriptionLabel") as Label;

                ea[0] = countryId.Value;
                ea[1] = countryDescription.Text;
                ea[2] = exportAmount.Text;
                exportAmounts.Rows.Add(ea);
            }
            return exportAmounts;
        }

        private void AddExportAmount(DataTable exportAmounts, DropDownList ddlCountry, decimal exportAmount)
        {
            object[] ea = new object[3];

            ea[0] = ddlCountry.SelectedValue;
            ea[1] = ddlCountry.SelectedItem.Text;
            ea[2] = exportAmount;
            exportAmounts.Rows.Add(ea);
        }

        private void RemoveExportAmount()
        {
            CleanMessage(MessagePanel, ErrorPanel);

            if (FormGridView.SelectedIndex < 0)
            {
                ShowInfo(MessagePanel, Resources.Resource.mSelectCountry2Remove);
                return;
            }

            DataTable exportAmounts = GetExportAmounts();
            exportAmounts.Rows[FormGridView.SelectedIndex].Delete();
            FormGridView.DataSource = exportAmounts;
            FormGridView.DataBind();
            FormGridView.SelectedIndex = -1;
        }

        private DataColumn GetDataTableColumn(string columnName, string columnType)
        {
            DataColumn myDataColumn;

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType(columnType);
            myDataColumn.ColumnName = columnName;
            return myDataColumn;
        }

        protected void OnFormGridViewSelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveExportAmount();
        }


        protected void OnGridView1SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow currentRow = GridView1.SelectedRow;
                CleanMessage(MessagePanel, ErrorPanel);
                if (currentRow == null)
                    return;

                DetailPanel.Visible = true;
                DetailPanel.Enabled = true;
                ConfirmButtonPanel.Visible = true;
                BackPanel.Visible = false;
                FormPanel.Visible = true;

                Label productionUnitDescription = currentRow.FindControl("ProductionUnitDescriptionLabel") as Label;
                ProductionUnitLiteral.Text = productionUnitDescription.Text;
                FillDateLiteral.Text = DateTime.Now.ToShortDateString();

                HiddenField periodNumber = currentRow.FindControl("PeriodNumberHidden") as HiddenField;
                HiddenField periodicityCode = currentRow.FindControl("PeriodicityCodeHidden") as HiddenField;
                HiddenField formDateHidden = currentRow.FindControl("FormDateHidden") as HiddenField;
                DateTime formDate = DateTime.Parse(formDateHidden.Value);
                FormMessageLiteral.Text = GetPeriodMessage(Resources.Resource.lForm2Message, int.Parse(periodNumber.Value), periodicityCode.Value, formDate);

                HiddenField financialExportFormId = currentRow.FindControl("FinancialExportFormIdHidden") as HiddenField;
                CompanyFormID.Value = financialExportFormId.Value;

                ShowCurrentForm(int.Parse(financialExportFormId.Value));
            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }

        }
        private void ShowCurrentForm(int formId)
        {
            IQueryable<FinancialExportFormCountry> query = GetExportCountriesForms(formId);
            var result = from f in query
                         select new
                         {
                             CountryId = f.CountryID,
                             CountryDescription = f.Country.Description,
                             ExportAmount = f.ExportAmount
                         };


            FormGridView.DataSource = result.ToList();
            FormGridView.DataBind();

            switch (currentOption)
            {

                case "QRY": ConfirmButtonPanel.Visible = false;
                            DetailPanel.Visible = false;
                            BackPanel.Visible = true;
                            FormGridView.Columns[0].Visible = false;
                            break;
                case "DEL": DetailPanel.Enabled = false;
                    break;
                case "UPD": ListPanel.Visible = false; break;
                case "CRE": break;
            }

        }

        protected void OnPagingSelectedIndexChanged(object sender, EventArgs e)
        {
            // Get page Code
            BindEntities(int.Parse(ddlPaging.SelectedValue));
            PagingPanel.Visible = true;

        }
        protected void OnSubmit(object sender, EventArgs e)
        {
            UpdateEvent(3);
        }

        protected void OnUpdate(object sender, EventArgs e)
        {
            UpdateEvent(2);
        }

        protected void OnApproveForm(object sender, EventArgs e)
        {
            UpdateEvent(4);
        }
        protected void OnRejectForm(object sender, EventArgs e)
        {
            UpdateEvent(5);
        }

        protected void UpdateEvent(int formState)
        {
            string operationResult = "Successo";
            int financialExportFormID = int.Parse(CompanyFormID.Value);

            CleanMessage(MessagePanel, ErrorPanel);
            try
            {
                UpdateRecord(formState, financialExportFormID);
                BackPanel.Visible = true;
                ConfirmButtonPanel.Visible = false;
                DetailPanel.Visible = true;
                DetailPanel.Enabled = false;
                InputPanel.Visible = false;
                FormPanel.Visible = true;
                ApprovePanel.Visible = false;
                if (formState == 2)
                    ShowInfo(MessagePanel, Resources.Resource.mUpdateOK);
                else if (formState == 3)
                    ShowInfo(MessagePanel, Resources.Resource.mSubmitedOK);
                else if (formState == 4)
                    ShowInfo(MessagePanel, Resources.Resource.mApprovedOK);
                else if (formState == 5)
                    ShowInfo(MessagePanel, Resources.Resource.mRejectedOK);
            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
                operationResult = exp.Message;
            }
            finally
            {
                AuditAction.Create(Page.User.Identity.Name, "SMI.03.01", GetOperation("UPD"), operationResult);
            }
        }
        private void UpdateRecord(int formState, int financialExportFormId)
        {
            HiddenField currentCountry = null;
            Label currentAmount = null;
            FinancialExportFormCountry countryExport = null;

            if (FormGridView.Rows.Count == 0)
                throw new Exception(Resources.Resource.mFinancialExportMandatory);

            // remove all country export records associated to the current formId
            CxFinancialExportFormCountry.Delete(financialExportFormId);

            // Query the database for the row to be updated.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.FinancialExportForms where c.FinancialExportFormID == financialExportFormId select c).First();
                if (entity != null)
                {
                    if (formState <= 3)
                    {
                        entity.SubmitDate = DateTime.Now;
                        entity.SubmitUserName = User.Identity.Name;
                    }
                    else
                    {
                        entity.ApprovalUserName = User.Identity.Name;
                        entity.ApprovalDate = DateTime.Now;
                    }
                    entity.StateID = formState;
                    db.SaveChanges();

                    foreach (GridViewRow currentExport in FormGridView.Rows)
                    {
                        currentCountry = currentExport.FindControl("CountryIdHidden") as HiddenField;
                        currentAmount = currentExport.FindControl("ExportAmountLabel") as Label;
                        countryExport = new FinancialExportFormCountry();
                        countryExport.CountryID = int.Parse(currentCountry.Value);
                        countryExport.ExportAmount = GetDecimalValue(currentAmount.Text);
                        countryExport.FinancialExportFormID = entity.FinancialExportFormID;
                        db.FinancialExportFormCountries.Add(countryExport);
                    }
                    db.SaveChanges();
                }

            }
        }

        private void DeleteRecord(int code)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                //db.Database.ExecuteSqlCommand("delete from FinancialExportForms where FinancialExportFormId={0}", code);
                var myItem = (from c in db.FinancialExportForms where c.FinancialExportFormID == code select c).First();
                if (myItem != null)
                {
                    db.FinancialExportForms.Remove(myItem);
                    db.SaveChanges();
                }
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mDeleteOK);

        }

        private void CreateRecord(int formId, int productionUnitId)
        {
            HiddenField currentCountry = null;
            Label currentAmount = null;
            FinancialExportFormCountry countryExport = null;

            if (FormGridView.Rows.Count == 0)
                throw new Exception(Resources.Resource.mFinancialExportMandatory);

            using (var db = new Models.SmizeeContext())
            {
                FinancialExportForm entity = new FinancialExportForm();
                entity.FormID = formId;
                entity.StateID = 1;
                entity.SubmitDate = DateTime.Now;
                //entity.ApprovalDate = DateTime.Parse("01/01/2000 00:00:00");
                entity.SubmitUserName = User.Identity.Name;

                db.FinancialExportForms.Add(entity);
                db.SaveChanges();

                foreach (GridViewRow currentExport in FormGridView.Rows)
                {
                    currentCountry = currentExport.FindControl("CountryIdHidden") as HiddenField;
                    currentAmount = currentExport.FindControl("ExportAmount") as Label;
                    countryExport = new FinancialExportFormCountry();
                    countryExport.CountryID = int.Parse(currentCountry.Value);
                    countryExport.ExportAmount = GetDecimalValue(currentAmount.Text);
                    countryExport.FinancialExportFormID = entity.FinancialExportFormID;
                    db.FinancialExportFormCountries.Add(countryExport);
                }
                db.SaveChanges();
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            FormPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mCreateOK);
        }


        private void BindEntities(int pageNumber)
        {
            int companyId = int.Parse(ddlCompany.SelectedValue);
            int productionUnitId = int.Parse(ddlProductionUnit.SelectedValue);


            IQueryable<FinancialExportForm> forms = GetForms(pageNumber, companyId, productionUnitId);

            var results = from f in forms
                          select new
                          {
                              FinancialExportFormId = f.FinancialExportFormID,
                              FormDescription = f.Form.Description,
                              Company = f.ProductionUnit.Company.CompanyName,
                              ProductionUnitDescription= f.ProductionUnit.Description,
                              PeriodicityDescription = f.Form.Periodicity.Description,
                              PeriodNumber = f.PeriodNumber,
                              PeriodicityCode = f.Form.Periodicity.Code,
                              FormDate = f.FormDate,
                              FormState = f.State.Description,
                          };


            var listEntities = results.ToList();

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
                FormPanel.Visible = false;
                ListPanel.Visible = true;
            }
        }

        public IQueryable<FinancialExportForm> GetForms(int pageNumber, int? companyId, int? productionUnitId)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<FinancialExportForm> query = db.FinancialExportForms;

            query = query.Where(p => (companyId == 0 | (p.ProductionUnit.CompanyID == companyId)) & (productionUnitId == 0 | (p.ProductionUnitID == productionUnitId)))
                .OrderBy(p => p.FormID).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;
        }

        public IQueryable<FinancialExportFormCountry> GetExportCountriesForms(int financialExportFormId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<FinancialExportFormCountry> query = CxFinancialExportFormCountry.GetForms(financialExportFormId);

            return query;
        }

        protected void OnFormGridViewRowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow x = e.Row;

            if (x.RowType != DataControlRowType.DataRow)
                return;

            Label amountLabel = x.FindControl("ExportAmountLabel") as Label;
            string amount = Server.HtmlDecode(amountLabel.Text);
            if (amount.Trim() != "")
                amountLabel.Text = decimal.Parse(amount.Trim()).ToString("###,###.#0");
        }
        protected void OnFormsInQueueRowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow x = e.Row;

            if (x.RowType != DataControlRowType.DataRow)
                return;

            HiddenField periodicityCode = x.FindControl("PeriodicityCodeHidden") as HiddenField;
            HiddenField formDate = x.FindControl("FormDateHidden") as HiddenField;
            HiddenField periodNumber = x.FindControl("PeriodNumberHidden") as HiddenField;
            Label periodicityLabel = x.FindControl("PeriodicityDescriptionLabel") as Label;

            string periodicityText = GetPeriodMessage("{0} {1} {2}", int.Parse(periodNumber.Value), periodicityCode.Value, DateTime.Parse(formDate.Value));
            periodicityLabel.Text = periodicityText;

        }

    }
}