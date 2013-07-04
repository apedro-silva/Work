using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SMIZEE.Models;
using SMIZEE.CodeFirstMembership;

namespace SMIZEE.Forms
{
    public partial class SMI_03_02 : BasePage
    {
        private string currentOption = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTransactionOptions(Options, true, false, false, false, false, false, false);
                SetPageDescription(Resources.MainMenu.IM_03_02);
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
                    ConfirmButtonPanel.Visible = true;

                    FinancialForm entity = CxFinancialForm.GetFormById(int.Parse(fefid));
                    FormMessageLiteral.Text = GetPeriodMessage(Resources.Resource.lForm2Message, entity.PeriodNumber, entity.Form.Periodicity.Code, entity.FormDate);

                    if (stateid == "3")
                    {
                        DetailPanel.Enabled = false;
                        ApprovePanel.Visible = true;
                        ConfirmButtonPanel.Visible = false;
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

                if (companyId != null && companyId != 0)
                {
                    ddlCompany.SelectedValue = companyId.ToString();
                    ddlCompany.Enabled = false;
                }
            }

        }

        protected void OnOptions(object sender, EventArgs e)
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
            }
            BindCompanies(ddlCompany);
            int companyId = int.Parse(ddlCompany.SelectedValue);
            BindProductionUnits(ddlProductionUnit, companyId);


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
            Response.Redirect("SMI.03.02.aspx");
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

                Label productionUnitDescription = currentRow.FindControl("ProductionUnitDescriptionLabel") as Label;
                ProductionUnitLiteral.Text = productionUnitDescription.Text;
                FillDateLiteral.Text = DateTime.Now.ToShortDateString();

                HiddenField periodNumber = currentRow.FindControl("PeriodNumberHidden") as HiddenField;
                HiddenField periodicityCode = currentRow.FindControl("PeriodicityCodeHidden") as HiddenField;

                HiddenField financialFormIdHidden = currentRow.FindControl("FinancialFormIdHidden") as HiddenField;
                int financialFormId = int.Parse(financialFormIdHidden.Value);
                CompanyFormID.Value = financialFormIdHidden.Value;

                HiddenField formDateHidden = currentRow.FindControl("FormDateHidden") as HiddenField;
                DateTime formDate = DateTime.Parse(formDateHidden.Value);

                FormMessageLiteral.Text = GetPeriodMessage(Resources.Resource.lForm2Message, int.Parse(periodNumber.Value), periodicityCode.Value, formDate);
                ShowCurrentForm(financialFormId);

            }
            catch (Exception exp)
            {
                ShowError(ErrorPanel, exp.Message);
            }
        }
        private void ShowCurrentForm(int formId)
        {
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.FinancialForms where c.FinancialFormID == formId select c).First();
                if (entity != null)
                {
                    ProductSalesNetAmountBudgetedInput.Text = GetStringValue(entity.ProductSalesNetAmountBudgeted);
                    ServiceSalesNetAmountBudgetedInput.Text = GetStringValue(entity.ServiceSalesNetAmountBudgeted);
                    CostTotalAmountInput.Text = GetStringValue(entity.CostTotalAmount);
                    DirectLabourCostsAmountInput.Text = GetStringValue(entity.DirectLabourCostsAmount);
                    BusinessAmountInput.Text = GetStringValue(entity.BusinessAmount);
                    OperacionalCostAmountInput.Text = GetStringValue(entity.OperacionalCostAmount);
                    OtherPersonelCostsAmountInput.Text = GetStringValue(entity.OtherPersonelCostsAmount);
                    SalaryAmountInput.Text = GetStringValue(entity.SalaryAmount);
                    SalesNetAmountInput.Text = GetStringValue(entity.SalesNetAmount);
                    ServiceDeliveryNetAmountInput.Text = GetStringValue(entity.ServiceDeliveryNetAmount);
                    SocialExpensesAmountInput.Text = GetStringValue(entity.SocialExpensesAmount);
                    TrainingAmountInput.Text = GetStringValue(entity.TrainingAmount);

                    ProductSalesNetAmountBudgetedInput.Text = GetStringValue(entity.ProductSalesNetAmountBudgeted);
                    ServiceSalesNetAmountBudgetedInput.Text = GetStringValue(entity.ServiceSalesNetAmountBudgeted);

                    InvestmentValueInput.Text = GetStringValue(entity.InvestmentValue);
                    TechnologyInvestmentValueInput.Text = GetStringValue(entity.TechnologyInvestmentValue);

                    int? x = entity.ProductionAutomatedControlSystem;
                    if (x==null)
                        ddlProductionAutomatedControlSystem.SelectedIndex = 0;
                    else
                        ddlProductionAutomatedControlSystem.SelectedIndex = (int)x;
                }
            }

            switch (currentOption)
            {

                case "QRY": ConfirmButtonPanel.Visible = false;
                            DetailPanel.Enabled = false;
                            BackPanel.Visible = true;
                            break;
                case "DEL": DetailPanel.Enabled = false;
                    break;
                case "UPD": break;
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
                AuditAction.Create(Page.User.Identity.Name, "SMI.03.02", GetOperation("UPD"), operationResult);
            }

        }

        private void UpdateRecord(int formState, int financialFormId)
        {
            decimal? salesNetAmount = GetDecimalValueFromInput(SalesNetAmountInput.Text, true);
            decimal? serviceDeliveryNetAmount = GetDecimalValueFromInput(ServiceDeliveryNetAmountInput.Text, true);
            decimal? costTotalAmount = GetDecimalValueFromInput(CostTotalAmountInput.Text, true);
            decimal? operacionalCostAmount = GetDecimalValueFromInput(OperacionalCostAmountInput.Text, true);
            decimal? productSalesNetAmountBudgeted = GetDecimalValueFromInput(ProductSalesNetAmountBudgetedInput.Text, true);
            decimal? serviceSalesNetAmountBudgeted = GetDecimalValueFromInput(ServiceSalesNetAmountBudgetedInput.Text, true);
            decimal? businessAmount = GetDecimalValueFromInput(BusinessAmountInput.Text, true);
            decimal? trainingAmount = GetDecimalValueFromInput(TrainingAmountInput.Text, true);
            decimal? salaryAmount = GetDecimalValueFromInput(SalaryAmountInput.Text, true);
            decimal? socialExpensesAmount = GetDecimalValueFromInput(SocialExpensesAmountInput.Text, true);
            decimal? otherPersonelCostsAmount = GetDecimalValueFromInput(OtherPersonelCostsAmountInput.Text, true);
            decimal? directLabourCostsAmount = GetDecimalValueFromInput(DirectLabourCostsAmountInput.Text, true);
            decimal? investmentValue = GetDecimalValueFromInput(InvestmentValueInput.Text, true);
            decimal? technologyInvestmentValue = GetDecimalValueFromInput(TechnologyInvestmentValueInput.Text, true);
            int productionAutomatedControlSystem = ddlProductionAutomatedControlSystem.SelectedValue=="1"?1:0;

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.FinancialForms where c.FinancialFormID == financialFormId select c).First();
                if (entity != null)
                {
                    entity.ProductSalesNetAmountBudgeted = productSalesNetAmountBudgeted;
                    entity.ServiceSalesNetAmountBudgeted = serviceSalesNetAmountBudgeted;
                    entity.CostTotalAmount = costTotalAmount;
                    entity.DirectLabourCostsAmount = directLabourCostsAmount;
                    entity.BusinessAmount = businessAmount;
                    entity.OperacionalCostAmount = operacionalCostAmount;
                    entity.OtherPersonelCostsAmount = otherPersonelCostsAmount;
                    entity.SalaryAmount = salaryAmount;
                    entity.SalesNetAmount = salesNetAmount;
                    entity.ServiceDeliveryNetAmount = serviceDeliveryNetAmount;
                    entity.SocialExpensesAmount = socialExpensesAmount;
                    entity.TrainingAmount = trainingAmount;
                    entity.InvestmentValue = investmentValue;
                    entity.TechnologyInvestmentValue = technologyInvestmentValue;

                    entity.ProductionAutomatedControlSystem = productionAutomatedControlSystem;
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
                }
            }
        }

        private void DeleteRecord(int code)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                //db.Database.ExecuteSqlCommand("delete from FinancialExportForms where FinancialExportFormId={0}", code);
                var entity = (from c in db.FinancialExportForms where c.FinancialExportFormID == code select c).First();
                if (entity != null)
                {
                    db.FinancialExportForms.Remove(entity);
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
            decimal? salesNetAmount = GetDecimalValueFromInput(SalesNetAmountInput.Text, true);
            decimal? serviceDeliveryNetAmount = GetDecimalValueFromInput(ServiceDeliveryNetAmountInput.Text, true);
            decimal? costTotalAmount = GetDecimalValueFromInput(CostTotalAmountInput.Text, true);
            decimal? operacionalCostAmount = GetDecimalValueFromInput(OperacionalCostAmountInput.Text, true);
            decimal? productSalesNetAmountBudgeted = GetDecimalValueFromInput(ProductSalesNetAmountBudgetedInput.Text, true);
            decimal? serviceSalesNetAmountBudgeted = GetDecimalValueFromInput(ServiceSalesNetAmountBudgetedInput.Text, true);
            decimal? businessAmount = GetDecimalValueFromInput(BusinessAmountInput.Text, true);
            decimal? trainingAmount = GetDecimalValueFromInput(TrainingAmountInput.Text, true);
            decimal? salaryAmount = GetDecimalValueFromInput(SalaryAmountInput.Text, true);
            decimal? socialExpensesAmount = GetDecimalValueFromInput(SocialExpensesAmountInput.Text, true);
            decimal? otherPersonelCostsAmount = GetDecimalValueFromInput(OtherPersonelCostsAmountInput.Text, true);
            decimal? directLabourCostsAmount = GetDecimalValueFromInput(DirectLabourCostsAmountInput.Text, true);
            decimal? investmentValue = GetDecimalValueFromInput(InvestmentValueInput.Text, true);
            decimal? technologyInvestmentValue = GetDecimalValueFromInput(TechnologyInvestmentValueInput.Text, true);
            int productionAutomatedControlSystem = ddlProductionAutomatedControlSystem.SelectedValue == "1" ? 1 : 0;

            using (var db = new Models.SmizeeContext())
            {
                FinancialForm entity = new FinancialForm();
                entity.StateID = 1;
                entity.FormID = formId;
                entity.SalesNetAmount = salesNetAmount;
                entity.ServiceDeliveryNetAmount = serviceDeliveryNetAmount;
                entity.CostTotalAmount = costTotalAmount;
                entity.OperacionalCostAmount = operacionalCostAmount;
                entity.ProductSalesNetAmountBudgeted = productSalesNetAmountBudgeted;
                entity.ServiceSalesNetAmountBudgeted = serviceSalesNetAmountBudgeted;

                entity.BusinessAmount = businessAmount;
                entity.TrainingAmount = trainingAmount;
                entity.SalaryAmount = salaryAmount;
                entity.SocialExpensesAmount = socialExpensesAmount;
                entity.OtherPersonelCostsAmount = otherPersonelCostsAmount;
                entity.DirectLabourCostsAmount = directLabourCostsAmount;

                entity.InvestmentValue = investmentValue;
                entity.TechnologyInvestmentValue = technologyInvestmentValue;
                entity.ProductionAutomatedControlSystem = productionAutomatedControlSystem;


                entity.SubmitDate = DateTime.Now;
                //entity.ApprovalDate = DateTime.Parse("01/01/2000 00:00:00");
                entity.SubmitUserName = User.Identity.Name;
                db.FinancialForms.Add(entity);
                db.SaveChanges();
            }
            BackPanel.Visible = true;
            ConfirmButtonPanel.Visible = false;
            DetailPanel.Visible = false;
            ShowInfo(MessagePanel, Resources.Resource.mCreateOK);
        }


        private void BindEntities(int pageNumber)
        {
            int companyId = int.Parse(ddlCompany.SelectedValue);
            int productionUnitId = int.Parse(ddlProductionUnit.SelectedValue);

            IQueryable<FinancialForm> forms = GetForms(pageNumber, companyId, productionUnitId);

            var results = from f in forms
                          select new
                          {
                              FinancialFormId = f.FinancialFormID,
                              FormDescription = f.Form.Description,
                              Company = f.ProductionUnit.Company.CompanyName,
                              ProductionUnitDescription = f.ProductionUnit.Description,
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
                ListPanel.Visible = true;
            }
        }

        public IQueryable<FinancialForm> GetForms(int pageNumber, int? companyId, int? productionUnitId)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<FinancialForm> query = db.FinancialForms;

            query = query.Where(p => (companyId == 0 | (p.ProductionUnit.CompanyID == companyId)) & (productionUnitId == 0 | (p.ProductionUnitID == productionUnitId)))
                .OrderBy(p => p.FormID).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;
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