using Microsoft.ApplicationBlocks.Data;
using RCSEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RCSData
{
    public class AdminData
    {
        #region ConnectionString
        static readonly string ConStr = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        readonly SqlConnection connection = new SqlConnection(ConStr);
        #endregion;

        public List<GetDRCSName> GetDRCSName()
        {
            List<GetDRCSName> lstGDN = new List<GetDRCSName>();
            SqlDataReader rdr = SqlHelper.ExecuteReader(Utility.GetConString(), CommandType.StoredProcedure, "[dbo].[GetDRCSName]");
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    GetDRCSName objGDN = new GetDRCSName
                    {
                        DRCSCode = Convert.ToInt32(rdr["DRCSCode"]),
                        DRCSName = rdr["DRCSName"].ToString()
                    };
                    lstGDN.Add(objGDN);
                }
            }
            return lstGDN;
        }

        public Accountstatusmodel GetAccountsDetails(int UserTypeCode)
        {
            Accountstatusmodel objASM = new Accountstatusmodel();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetNumberOfAccount]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@UserTypeCode", UserTypeCode);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objASM.Totalaccount = Convert.ToInt32(rdr["Totalaccount"]);
                    objASM.ARCSaccount = Convert.ToInt32(rdr["ARCSaccount"]);
                    objASM.Inspectoraccount = Convert.ToInt32(rdr["inspectoraccount"]);
                }
                return objASM;
            }
        }

        public List<InspectorListModel> GetInspectorList(int ARCSOfficerCode)
        {
            connection.Open();
            List<InspectorListModel> lstILM = new List<InspectorListModel>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetInsepectorList]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@ARCSCode", ARCSOfficerCode);
          
            var dr = cmd.ExecuteReader();
            var dt = new DataTable();
            if (dr.HasRows)
            {
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        InspectorListModel objILM = new InspectorListModel
                        {
                            InspectorId = r["InspectorCode"].ToString(),
                            InspectorName = r["InspectName"].ToString()
                        };
                        lstILM.Add(objILM);
                    }
                }
            }
            connection.Close();
            return lstILM;
        }
    }
}
