using GraduApp.DataAccess.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.DataAccess.GraduDBOperations
{
    public class GraduDBOperationsFactory
    {
        public IGraduDBOperations Create(GraduDBType dbType, IConfiguration configuration, bool useEntityFramework, bool enableLogging = false, bool disableChangeTracking = false)
        {
            if (useEntityFramework)
            {
                return new EFGraduDBOperations(dbType, enableLogging, configuration, !disableChangeTracking);
            }
            else
            { 
                if(dbType == GraduDBType.SQLServer)
                {
                    return new MSSQLAdoNetDBOperations(configuration.GetConnectionString("SqlServerConnection"));
                }
                else if (dbType == GraduDBType.MYSQL)
                {
                    return new MySQLAdoNetDBOperations(configuration.GetConnectionString("MySQLConnection"));
                }
                else if (dbType == GraduDBType.PostgreSQL)
                {
                    return new PostgreSQLAdoNetOperations(configuration.GetConnectionString("PostgreSQLrConnection"));
                }
                else if (dbType == GraduDBType.Oracle)
                {
                    return new OracleAdoNetDBOperations(configuration.GetConnectionString("OracleConnection"));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
