using RCSEntities;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RCSData
{
    public class ImportExportData
    {
        #region ConnectionString
        static readonly string ConStr = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        readonly SqlConnection connection = new SqlConnection(ConStr);
        #endregion;

        public int SaveImportedExcel(ImportExportModels objIEM)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[SaveImportedExcel]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@CreateDate", objIEM.CreateDate);
                com.Parameters.AddWithValue("@OldRedgNo", objIEM.OldRedgNo);
                com.Parameters.AddWithValue("@SocietyName", objIEM.SocietyName);
                com.Parameters.AddWithValue("@SocietyTransID", DateTime.Now.ToString(("yyyyMMdd")));
                i = com.ExecuteNonQuery();
            }
            return i;
        }
    }
}
