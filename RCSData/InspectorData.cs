using RCSEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace RCSData
{
    public class InspectorData
    {
        #region ConnectionString
        static readonly string ConStr = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        readonly SqlConnection connection = new SqlConnection(ConStr);
        #endregion;

        public InspectorSocietyStatusModels GetInspectorApplicatonCountDetails(int InspectorCode)
        {
            InspectorSocietyStatusModels objISSM = new InspectorSocietyStatusModels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetInspectorSApplicatonCountDetails]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@InspectorCode", InspectorCode);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objISSM.TotalApprove = Convert.ToInt32(rdr["TotalForwardToARCSOfficer"]);
                    objISSM.TotalPending = Convert.ToInt32(rdr["TotalPending"]);
                    objISSM.Total = objISSM.TotalApprove + objISSM.TotalPending;
                }
                return objISSM;
            }
        }
        public InspectorSocietyStatusModels BackLogGetInspectorApplicatonCountDetails(int InspectorCode)
        {
            InspectorSocietyStatusModels objISSM = new InspectorSocietyStatusModels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[Blog_GetInspectorSApplicatonCountDetails]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@InspectorCode", InspectorCode);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objISSM.TotalApprove = Convert.ToInt32(rdr["TotalForwardToARCSOfficer"]);
                    objISSM.TotalPending = Convert.ToInt32(rdr["TotalPending"]);
                    objISSM.Total = objISSM.TotalApprove + objISSM.TotalPending;
                }
                return objISSM;
            }
        }

        public List<SocietySubmissionFromModels> InspectorDashBoardSocietyList(int OfficerCode)
        {
            List<SocietySubmissionFromModels> lst = new List<SocietySubmissionFromModels>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetSocietyDetailsForARCSOfficer]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@OfficerCode", OfficerCode);
                com.Parameters.AddWithValue("@Role", Convert.ToInt32(HttpContext.Current.Session["RoleId"]));
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new SocietySubmissionFromModels
                    {
                        SocietyName = rdr["SocietyName"].ToString(),
                        ClassSocietyCode = rdr["ClassSocietyCode"].ToString(),
                        SubClassSocietyCode = rdr["SubClassSocietyCode"].ToString(),
                        Address1 = rdr["Address1"].ToString(),
                        Address2 = rdr["Address2"].ToString(),
                        PostOffice = rdr["PostOffice"].ToString(),
                        Pin = rdr["Pin"].ToString(),
                        AreaOfOperation = rdr["AreaOfOperation"].ToString(),
                        Mainobject1 = rdr["Mainobject1"].ToString(),
                        Mainobject2 = rdr["Mainobject2"].ToString(),
                        Mainobject3 = rdr["Mainobject3"].ToString(),
                        Mainobject4 = rdr["Mainobject4"].ToString(),
                        NoOfMembers = Convert.ToInt32(rdr["NoOfMembers"]),
                        OccupationOfMember = Convert.ToInt32(rdr["OccupationOfMember"]),
                        CateOfSociety = rdr["CateOfSociety"].ToString(),
                        DebtsOfMembers = rdr["DebtsOfMembers"].ToString(),
                        AreaMortgaged = rdr["AreaMortgaged"].ToString(),
                        DetailsOfShares = rdr["DetailsOfShares"].ToString(),
                        ValueOfShare = rdr["ValueOfShare"].ToString(),
                        ModeOfPayment = rdr["ModeOfPayment"].ToString(),
                        TypeofSociety = rdr["Type_of_Society"].ToString(),
                    });
                }
                return lst;
            }
        }

        public int ForwardToARCS(ForwardToARCSOfficers objFTAO)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[ForwardToARCSOfficer]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@Remarks", objFTAO.Remarks);
                com.Parameters.AddWithValue("@SocietyTransId", objFTAO.SocietyTransId);
                i = com.ExecuteNonQuery();
            }
            return i;
        }
        public int BackLogForwardToARCS(ForwardToARCSOfficers objFTAO)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[Blog_ForwardToARCSOfficer]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@Remarks", objFTAO.Remarks);
                com.Parameters.AddWithValue("@SocietyTransId", objFTAO.SocietyTransId);
                i = com.ExecuteNonQuery();
            }
            return i;
        }
    }
}
