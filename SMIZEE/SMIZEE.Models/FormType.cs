using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class FormType
    {
        [Key]
        public int FormTypeID { get; set; }

        [Required, StringLength(100), Display(Name = "Description")]
        public string Description { get; set; }

        [Required, StringLength(100), Display(Name = "URL")]
        public string FormPage { get; set; }

        public int FunctionalAreaID { get; set; }
        public virtual FunctionalArea FunctionalArea { get; set; }

    }

    public static class FormTypeStart
    {
        public static List<FormType> GetFormTypes()
        {
            var forms = new List<FormType> {
                new FormType
                {
                    FormTypeID = 1,
                    Description = "FORM 3 -SDZEE - Financeiro Exportações",
                    FunctionalAreaID = 1,
                    FormPage = "~/Forms/SMI.03.01.aspx"
                },
                new FormType
                {
                    FormTypeID = 2,
                    Description = "FORM 3 -SDZEE - Financeiro",
                    FunctionalAreaID = 1,
                    FormPage = "~/Forms/SMI.03.02.aspx"
                },
                new FormType
                {
                    FormTypeID = 3,
                    Description = "FORM 1 -SDZEE - Operacional Licenças",
                    FunctionalAreaID = 2,
                    FormPage = "~/Forms/SMI.03.03.aspx"
                },
                new FormType
                {
                    FormTypeID = 4,
                    Description = "FORM 2 -SDZEE - Operacional",
                    FunctionalAreaID = 2,
                    FormPage = "~/Forms/SMI.03.04.aspx",
                },
                new FormType
                {
                    FormTypeID = 5,
                    Description = "FORM 3 -SDZEE - Recursos Humanos",
                    FunctionalAreaID = 3,
                    FormPage = "~/Forms/SMI.03.05.aspx"
                },
                new FormType
                {
                    FormTypeID = 6,
                    Description = "FORM 3 -SDZEE - Recursos Humanos Qualificação",
                    FunctionalAreaID = 3,
                    FormPage = "~/Forms/SMI.03.06.aspx"
                },
                new FormType
                {
                    FormTypeID = 7,
                    Description = "FORM 5 -SDZEE - Financeiro SDZEE",
                    FunctionalAreaID = 1,
                    FormPage = "~/Forms/SMI.03.07.aspx"
                },
                new FormType
                {
                    FormTypeID = 8,
                    Description = "FORM 5 -SDZEE - Recursos Humanos SDZEE",
                    FunctionalAreaID = 3,
                    FormPage = "~/Forms/SMI.03.08.aspx"
                }
            };

            return forms;
        }
    }

    public static class CxFormType
    {
        //public static IQueryable<Form> GetFormsByProductionUnit(int productionUnitId)
        //{
        //    var db = new Models.SmizeeContext();
        //    IQueryable<Form> query = db.Forms;

        //    query = query.Where(p => ((p.ProductionUnitID == productionUnitId)));

        //    return query;

        //}

    }

}