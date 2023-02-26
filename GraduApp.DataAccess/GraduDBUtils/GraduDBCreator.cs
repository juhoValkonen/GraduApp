
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.DataAccess.GraduDBUtils
{
    public class GraduDBCreator
    {
        GraduDBContext ctx;

        public GraduDBCreator(GraduDBContext ctx)
        {
            this.ctx = ctx;
        }

        public void CreateDatabase()
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
        }
    }
}
