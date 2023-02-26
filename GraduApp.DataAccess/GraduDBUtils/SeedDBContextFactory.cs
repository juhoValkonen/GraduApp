using GraduApp.DataAccess.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.DataAccess.GraduDBUtils
{
    public class SeedDBContextFactory
    {
        public GraduDBContext MakeSeedDBContext(IConfiguration configuration)
        {
            return new GraduDBContext(GraduDBType.SQLServer, configuration, true);
        }
    }
}
