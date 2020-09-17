using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GST_Designs.DBAccess
{
    public class DatabaseConnection
    {
        public static SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["GST_DesignsConnection"].ConnectionString);
            con.Open();
            return con;
        }
    }
}