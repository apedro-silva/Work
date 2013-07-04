using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class QualificationLevel
    {
        public int QualificationLevelID { get; set; }

        [Required, StringLength(25), Display(Name = "Qualification Level Samll Description")]
        public string SmallDescription { get; set; }

        [Required, StringLength(100), Display(Name = "Qualification Level Description")]
        public string Description { get; set; }
    }

}