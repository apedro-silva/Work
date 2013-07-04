using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class FinancialForm
    {
        [Key]
        public int FinancialFormID { get; set; }

        public int StateID { get; set; }
        public virtual State State { get; set; }

        public int FormID { get; set; }
        public virtual Form Form { get; set; }

        public int ProductionUnitID { get; set; }
        public virtual ProductionUnit ProductionUnit { get; set; }

        public int PeriodNumber { get; set; }

        [Display(Name = "Sales Net Amount")]
        public decimal? SalesNetAmount { get; set; }

        [Display(Name = "Service Delivery Net Amount")]
        public decimal? ServiceDeliveryNetAmount { get; set; }

        [Display(Name = "Cost Total Amount")]
        public decimal? CostTotalAmount { get; set; }

        [Display(Name = "Operacional Cost Amount")]
        public decimal? OperacionalCostAmount { get; set; }

        [Display(Name = "Product Sales Net Amount Budgeted")]
        public decimal? ProductSalesNetAmountBudgeted { get; set; }

        [Display(Name = "Service Sales Net Amount Budgeted")]
        public decimal? ServiceSalesNetAmountBudgeted { get; set; }

        [Display(Name = "Business Amount")]
        public decimal? BusinessAmount { get; set; }

        [Display(Name = "Training Amount")]
        public decimal? TrainingAmount { get; set; }

        [Display(Name = "Salary Amount")]
        public decimal? SalaryAmount { get; set; }

        [Display(Name = "Social Expenses Amount")]
        public decimal? SocialExpensesAmount { get; set; }

        [Display(Name = "Other Personel Costs Amount")]
        public decimal? OtherPersonelCostsAmount { get; set; }

        [Display(Name = "Direct Labour Costs Amount")]
        public decimal? DirectLabourCostsAmount { get; set; }

        [Display(Name = "Investment Value")]
        public decimal? InvestmentValue { get; set; }

        [Display(Name = "Technology Investment Value")]
        public decimal? TechnologyInvestmentValue { get; set; }

        [Display(Name = "Production Automated control system")]
        public int? ProductionAutomatedControlSystem { get; set; }

        public DateTime FormDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? UserAlertDate { get; set; }
        public DateTime? ManagerAlertDate { get; set; }
        public DateTime? ExecutiveAlertDate { get; set; }

        public string SubmitUserName { get; set; }
        public string ApprovalUserName { get; set; }

    }

    public static class  CxFinancialForm
    {
        public static IQueryable<FinancialForm> GetForms(int pageNumber, int? companyId, int? productionUnitId)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<FinancialForm> query = db.FinancialForms;

            query = query.Where(p => (companyId == 0 | (p.ProductionUnit.CompanyID == companyId)) & (productionUnitId == 0 | (p.ProductionUnitID == productionUnitId)))
                .OrderBy(p => p.FormID).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;
        }
        public static FinancialForm GetFormById(int financialFormId)
        {
            FinancialForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.FinancialForms.FirstOrDefault(f => f.FinancialFormID == financialFormId);
                // force context to get Form
                string code = entity.Form.Periodicity.Code;
                int functionalAreaID = entity.Form.FormType.FunctionalAreaID;
            }
            return entity;
        }

        public static int CreateForm(int formId, int productionUnitId, int periodNumber, DateTime formDate)
        {
            int financialFormId = 0;
            FinancialForm entity = new FinancialForm();
            using (var db = new Models.SmizeeContext())
            {
                entity.FormID = formId;
                entity.StateID = 1;
                entity.ProductionUnitID = productionUnitId;
                entity.PeriodNumber = periodNumber;
                entity.FormDate = formDate;

                db.FinancialForms.Add(entity);
                db.SaveChanges();
                financialFormId = entity.FinancialFormID;
            }
            return financialFormId;
        }
        public static IQueryable<FinancialForm> GetFormsNotApproved(int formId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<FinancialForm> fInQueue = from f in db.FinancialForms
                                                 where f.FormID == formId & f.StateID != 4
                                                 select f;

            return fInQueue;
        }
        public static IQueryable<FinancialForm> GetFormsByProductionUnit(int formId, int productionUnitId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<FinancialForm> fInQueue = from fef in db.FinancialForms
                                                 where (fef.FormID == formId) & (fef.ProductionUnitID == productionUnitId)
                                                 select fef;

            return fInQueue;
        }
        public static bool CheckFormsByFormDate(int formId, int productionUnitId, DateTime formDate)
        {
            bool result = false;

            FinancialForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.FinancialForms.FirstOrDefault(fef => (fef.FormID == formId) & (fef.ProductionUnitID == productionUnitId) & (fef.FormDate == formDate));
                if (entity != null) result = true;
            }
            return result;
        }
        public static IQueryable<FinancialForm> GetUserFormsInQueue(SmizeeContext db, string userName, int? companyId, int? productionUnitId, int? functionalAreaId)
        {
            IQueryable<FinancialForm> fefInQueue = from f in db.FinancialForms
                                                         where (productionUnitId == null | f.ProductionUnitID == productionUnitId)
                                                         & (functionalAreaId == null | f.Form.FormType.FunctionalAreaID == functionalAreaId)
                                                         & (companyId == 0 | f.ProductionUnit.CompanyID == companyId)
                                                         & ((f.SubmitUserName == null & f.StateID == 1)
                                                         | (f.SubmitUserName == userName & (f.StateID == 2 | f.StateID == 5)))
                                                         select f;

            return fefInQueue;
        }
        public static IQueryable<FinancialForm> GetFormsForApproval(SmizeeContext db, string userName, int? companyId, int? productionUnitId, int? functionalAreaId)
        {
            IQueryable<FinancialForm> fefInQueue = from f in db.FinancialForms
                                                   where (productionUnitId == null | f.ProductionUnitID == productionUnitId)
                                                   & (functionalAreaId == null | f.Form.FormType.FunctionalAreaID == functionalAreaId)
                                                   & (companyId == 0 | f.ProductionUnit.CompanyID == companyId)
                                                   & (f.StateID == 3)
                                                   select f;

            return fefInQueue;
        }

        public static void UpdateAlertDate(int financialFormId, bool userAlert, bool managerAlert, bool executiveAlert)
        {
            if (!userAlert && !managerAlert && !executiveAlert)
                return;

            // Query the database for the row to be updated.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.FinancialForms where c.FinancialFormID == financialFormId select c).First();
                if (entity != null)
                {
                    if (userAlert)
                        entity.UserAlertDate = DateTime.Now;
                    if (managerAlert)
                        entity.ManagerAlertDate = DateTime.Now;
                    if (executiveAlert)
                        entity.ExecutiveAlertDate = DateTime.Now;

                    db.SaveChanges();
                }
            }
        }

    }
}