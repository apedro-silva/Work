using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SMIZEE.Models;
using SMIZEE.Classes;

namespace SMIZEE
{
    public class BasePage : Page
    {
        public BasePage()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected override void OnError(EventArgs e)
        {
            base.OnError(e);

            // Get last error from the server.
            Exception exc = Server.GetLastError();

            SMIZEE.Classes.Goodies.LogException(exc, this.Context.Handler.ToString());
        }
        protected override void InitializeCulture()
        {
            string lang = "pt-PT";
            try
            {
                if (Request.Cookies["lang"] != null)
                {
                    lang = Server.HtmlEncode(Request.Cookies["lang"].Value);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                }
                base.InitializeCulture();
            }
            catch (Exception)
            {
            }
        }
        public void SetTransactionOptions(DropDownList options)
        {
            options.Items.Add(new ListItem(Resources.Resource.oQuery, "QRY"));
            options.Items.Add(new ListItem(Resources.Resource.oCreate, "CRE"));
            options.Items.Add(new ListItem(Resources.Resource.oUpdate, "UPD"));
            options.Items.Add(new ListItem(Resources.Resource.oDelete, "DEL"));
        }
        public void SetPageDescription(string pageDescription)
        {
            Session.Remove("PageDescription");
            Session.Add("PageDescription", pageDescription);
        }

        public void SetTransactionOptions(DropDownList options, bool query, bool create, bool update, bool delete, bool run, bool import, bool export)
        {
            if (query)
                options.Items.Add(new ListItem(Resources.Resource.oQuery, "QRY"));
            if (create)
                options.Items.Add(new ListItem(Resources.Resource.oCreate, "CRE"));
            if (update)
                options.Items.Add(new ListItem(Resources.Resource.oUpdate, "UPD"));
            if (delete)
                options.Items.Add(new ListItem(Resources.Resource.oDelete, "DEL"));
            if (run)
                options.Items.Add(new ListItem(Resources.Resource.oExecute, "RUN"));
            if (import)
                options.Items.Add(new ListItem(Resources.Resource.oImport, "IMP"));
            if (export)
                options.Items.Add(new ListItem(Resources.Resource.oExport, "EXP"));
        }
        public void ShowError(Panel errorPanel, string message)
        {
            errorPanel.Visible = true;
            Label errorLabel = errorPanel.FindControl("ErrorMessage") as Label;
            errorLabel.Text = message;

        }
        public void ShowInfo(Panel messagePanel, string message)
        {
            messagePanel.Visible = true;
            Label infoLabel = messagePanel.FindControl("InfoMessage") as Label;
            infoLabel.Text = message;
        }
        public void CleanMessage(Panel infoPanel, Panel errorPanel)
        {
            infoPanel.Visible = false;
            errorPanel.Visible = false;

            Label errorLabel = errorPanel.FindControl("ErrorMessage") as Label;
            Label infoLabel = infoPanel.FindControl("InfoMessage") as Label;

            errorLabel.Text = "";
            infoLabel.Text = "";
        }
        public void SetPages(DropDownList ddlPaging, int pages)
        {
            ddlPaging.Items.Clear();
            for (int count = 1; count <= pages; count++)
                ddlPaging.Items.Add(count.ToString());
        }

        public void BindCountries(DropDownList ddl2Bind)
        {
            ddl2Bind.DataSource = CxCountry.GetCountriesList();
            ddl2Bind.DataTextField = "Description";
            ddl2Bind.DataValueField = "CountryId";

            ddl2Bind.DataBind();
            ddl2Bind.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));
        }

        public void BindCompanies(DropDownList ddl2Bind)
        {
            ddl2Bind.DataSource = CxCompany.GetCompaniesList();
            ddl2Bind.DataTextField = "CompanyName";
            ddl2Bind.DataValueField = "CompanyId";

            ddl2Bind.DataBind();
            ddl2Bind.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));
        }

        public void BindFunctionalArea(DropDownList ddl2Bind)
        {
            ddl2Bind.DataSource = CxFunctionalArea.GetFunctionalAreasList();
            ddl2Bind.DataTextField = "Description";
            ddl2Bind.DataValueField = "FunctionalAreaId";

            ddl2Bind.DataBind();
            ddl2Bind.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));
        }

        public void BindProductionUnits(DropDownList ddl2Bind, int companyId)
        {
            ddl2Bind.DataSource = CxProductionUnit.GetListByCompanyId(companyId);
            ddl2Bind.DataTextField = "Description";
            ddl2Bind.DataValueField = "ProductionUnitId";

            ddl2Bind.DataBind();
            ddl2Bind.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));
        }
        public void BindFormStates(DropDownList ddl2Bind)
        {
            ddl2Bind.DataSource = CxStates.GetList();
            ddl2Bind.DataTextField = "Description";
            ddl2Bind.DataValueField = "StateId";

            ddl2Bind.DataBind();
            ddl2Bind.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));
        }
        public void BindPeriodicity(DropDownList ddl2Bind)
        {
            var db = new Models.SmizeeContext();
            IQueryable<Periodicity> periodicities = db.Periodicities;
            List<Periodicity> listEntities = periodicities.ToList();

            ddl2Bind.DataSource = listEntities;
            ddl2Bind.DataTextField = "Description";
            ddl2Bind.DataValueField = "PeriodicityID";
            ddl2Bind.DataBind();
            ddl2Bind.Items.Insert(0, new ListItem(Resources.Resource.lSelectOne, "0"));
        }

        public void BindYears(DropDownList ddlYear)
        {
            ddlYear.Items.Insert(0, new ListItem("2010", "2010"));
            ddlYear.Items.Insert(0, new ListItem("2011", "2011"));
            ddlYear.Items.Insert(0, new ListItem("2012", "2012"));
            ddlYear.Items.Insert(0, new ListItem("2013", "2013"));
        }


        public string GetPeriodMessage(string baseMessage, int periodNumber, string periodicityCode, DateTime formDate)
        {
            string periodMessage = "";

            switch (periodicityCode)
            {
                case "D": periodMessage = string.Format(baseMessage, periodNumber, "º dia", formDate.Year); break;
                case "W": periodMessage = string.Format(baseMessage, periodNumber, "º semana", formDate.Year); break;
                case "M": periodMessage = string.Format(baseMessage, "", GetMonthDescription(periodNumber), formDate.Year); break;
                case "Q": periodMessage = string.Format(baseMessage, periodNumber, "º trimestre", formDate.Year); break;
                case "S": periodMessage = string.Format(baseMessage, periodNumber, "º semestre", formDate.Year); break;
                case "Y": periodMessage = string.Format(baseMessage, "", " ano ", formDate.Year); break;
            }

            return periodMessage;
        }

        private string GetMonthDescription(int month)
        {
            string monthDescription ="";
            switch (month)
            {
                case 1: monthDescription = Resources.Resource.lJanuary; break;
                case 2: monthDescription = Resources.Resource.lFebruary; break;
                case 3: monthDescription = Resources.Resource.lMarch; break;
                case 4: monthDescription = Resources.Resource.lApril; break;
                case 5: monthDescription = Resources.Resource.lMay; break;
                case 6: monthDescription = Resources.Resource.lJune; break;
                case 7: monthDescription = Resources.Resource.lJuly; break;
                case 8: monthDescription = Resources.Resource.lAugust; break;
                case 9: monthDescription = Resources.Resource.lSeptember; break;
                case 10: monthDescription = Resources.Resource.lOctober; break;
                case 11: monthDescription = Resources.Resource.lNovember; break;
                case 12: monthDescription = Resources.Resource.lDecember; break;

            }
            return monthDescription;
        }

        public decimal? GetDecimalValue(string amount)
        {
            if (amount.Equals(""))
                return null;

            string currencyDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            amount = amount.Replace(",", "").Replace(".", "").Replace(" ", "");
            amount = amount.Insert(amount.Length - 2, currencyDecimalSeparator);
            return decimal.Parse(amount.Trim());
        }
        public decimal? GetDecimalValueFromInput(string amount, bool decimalPlaces)
        {
            if (amount.Equals(""))
                return null;

            string currencyDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            if (amount.LastIndexOf(',') > 0)
            {
                amount = amount.Replace(",", currencyDecimalSeparator);
            }
            else if (decimalPlaces)
            {
                amount = string.Format("{0}{1}00", amount, currencyDecimalSeparator);
            }
            return decimal.Parse(amount);
        }
        public int? GetIntValueFromInput(string amount)
        {
            if (amount.Equals(""))
                return null;
            return int.Parse(amount);
        }

        public string GetStringValue(decimal? amount)
        {
            if (amount==null)
                return null;

            return amount.ToString().Replace(".", ",");

        }
        public string GetDddlSelectedValue(int? inValue)
        {
            if (inValue == null)
                return "0";

            return inValue.ToString();

        }
        public int GetOperation(string operationLabel)
        {
            int operationId = 0;

            switch (operationLabel)
            {
                case "CRE": operationId= (int)OperationEnum.Insert; break;
                case "UPD": operationId = (int)OperationEnum.Update; break;
                case "QRY": operationId = (int)OperationEnum.Query; break;
                case "DEL": operationId = (int)OperationEnum.Delete; break;
                case "RUN": operationId = (int)OperationEnum.Run; break;
                case "EXE": operationId = (int)OperationEnum.Execute; break;
            }
            return operationId;
        }
    }
}