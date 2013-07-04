using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMIZEE.DAL
{
    public static class CurrentArea
    {
        public static void AddNewArea(string name)
        {
            using (var db = new AreaContext())
            {
                var area = new Area { Name = name };
                db.Areas.Add(area);
                db.SaveChanges();
            }
        }
    }

    public class Area
    {
        public int AreaId { get; set; }
        public string Name { get; set; }
        public virtual List<Area> Areas { get; set; }

    }

    public class AreaContext : DbContext
    {
        public AreaContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<Area> Areas { get; set; }
    }
}
