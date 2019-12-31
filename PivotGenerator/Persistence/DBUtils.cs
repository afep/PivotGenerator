using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
namespace PivotGenerator
{
    public class DBUtils
    {
        public static OracleConnection GetDBConnection()
        {
            string host = "localhost";
            int port = 1521;
            string sid = "XE";
            string user = "system";
            string password = "password";

            return Connection.GetDBConnection(host, port, sid, user, password);
        }
    }

}
