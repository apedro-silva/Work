using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class FunctionalArea
    {
        public int FunctionalAreaID { get; set; }

        [Required, StringLength(25), Display(Name = "Functional Area Small Description")]
        public string SmallDescription { get; set; }

        [Required, StringLength(100), Display(Name = "Functional Area Description")]
        public string Description { get; set; }
    }

    public static class FunctionalAreaStart
    {
        public static List<FunctionalArea> GetFunctionalAreas()
        {
            var forms = new List<FunctionalArea> {
                new FunctionalArea
                {
                    FunctionalAreaID = 1,
                    Description = "Financeiro",
                    SmallDescription = "FIN"
                },
                new FunctionalArea
                {
                    FunctionalAreaID = 2,
                    Description = "Operacional",
                    SmallDescription = "OPE"
                },
                new FunctionalArea
                {
                    FunctionalAreaID = 3,
                    Description = "Recursos Humanos",
                    SmallDescription = "RH"
                },
            };

            return forms;
        }
    }

    public static class CxFunctionalArea
    {
        public static FunctionalArea GetFunctionalArea(int? FunctionalAreaId)
        {
            FunctionalArea FunctionalArea = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                FunctionalArea = Context.FunctionalAreas.FirstOrDefault(cp => cp.FunctionalAreaID == FunctionalAreaId);
            }
            return FunctionalArea;

        }

        public static List<FunctionalArea> GetFunctionalAreasList()
        {
            var db = new Models.SmizeeContext();
            IQueryable<FunctionalArea> query = db.FunctionalAreas;
            return query.ToList();
        }

    }

}