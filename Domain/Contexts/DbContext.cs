using Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contexts
{
    public class DbContext : DapperContext
    {
        public DbContext()
        {
        }

        protected override IDbConnection CreateConnection()
        {
            var config = ConfigurationManager.ConnectionStrings["Default"];
            var factory = DbProviderFactories.GetFactory(config.ProviderName);

            var conn = factory.CreateConnection();
            conn.ConnectionString = config.ConnectionString;

            return conn;
        }
    }
}