using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCSEntities;

namespace RCSData
{
    public class SocietySubmissionFrom
    {
        #region ConnectionString
        static string ConStr = "Data Source=localhost;Initial Catalog=NewRcsHry;Integrated Security=True";
        //string cs = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConStr);
        #endregion;

        public int SaveSocietySubmissionFrom(SocietySubmissionFromModels objSSFM)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[SocietySubmissionFrom]", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SocietyTransID", objSSFM.SocietyTransID);
            cmd.Parameters.AddWithValue("@DivCode", objSSFM.DivCode);
            cmd.Parameters.AddWithValue("@ARCSCode", objSSFM.ARCSCode);
            cmd.Parameters.AddWithValue("@SocietyName", objSSFM.SocietyName);
            cmd.Parameters.AddWithValue("@ClassSocietyCode", objSSFM.ClassSocietyCode);
            cmd.Parameters.AddWithValue("@SubClassSocietyCode", objSSFM.SubClassSocietyCode);
            cmd.Parameters.AddWithValue("@Address1", objSSFM.Address1);
            cmd.Parameters.AddWithValue("@Address2", objSSFM.Address2);
            cmd.Parameters.AddWithValue("@PostOffice", objSSFM.PostOffice);
            cmd.Parameters.AddWithValue("@Pin", objSSFM.Pin);
            cmd.Parameters.AddWithValue("@AreaOfOperation", objSSFM.AreaOfOperation);
            cmd.Parameters.AddWithValue("@Mainobject1", objSSFM.Mainobject1);
            cmd.Parameters.AddWithValue("@Mainobject2", objSSFM.Mainobject2);
            cmd.Parameters.AddWithValue("@Mainobject3", objSSFM.Mainobject3);
            cmd.Parameters.AddWithValue("@Mainobject4", objSSFM.Mainobject4);
            cmd.Parameters.AddWithValue("@NoOfMembers", objSSFM.NoOfMembers);
            cmd.Parameters.AddWithValue("@OccupationOfMember", objSSFM.OccupationOfMember);
            cmd.Parameters.AddWithValue("@CateOfSociety", objSSFM.CateOfSociety);
            cmd.Parameters.AddWithValue("@DebtsOfMembers", objSSFM.DebtsOfMembers);
            cmd.Parameters.AddWithValue("@AreaMortgaged", objSSFM.AreaMortgaged);
            cmd.Parameters.AddWithValue("@DetailsOfShares", objSSFM.DetailsOfShares);
            cmd.Parameters.AddWithValue("@ValueOfShare", objSSFM.ValueOfShare);
            cmd.Parameters.AddWithValue("@ModeOfPayment", objSSFM.ModeOfPayment);
            cmd.Parameters.AddWithValue("@UserId", objSSFM.UserId);
            var details = cmd.ExecuteNonQuery();
            if (details >=1 )
            {
                return 1;
            }
            connection.Close();
            return 0;
        }

        public int SaveSocietyMemberDetails(MembershipDetailsModels objMDM)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[SaveSocietyMembershipDetails]", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SocietyMemberID", objMDM.SocietyMemberID);
            cmd.Parameters.AddWithValue("@SocietyTransID", objMDM.SocietyTransID);
            cmd.Parameters.AddWithValue("@SocietyMemberName", objMDM.SocietyMemberName);
            cmd.Parameters.AddWithValue("@SocietyMemberDesignation", objMDM.SocietyMemberDesignation);
            cmd.Parameters.AddWithValue("@RelationshipCode", objMDM.RelationshipCode);
            cmd.Parameters.AddWithValue("@RelationshipMemberName", objMDM.RelationshipMemberName);
            cmd.Parameters.AddWithValue("@Address", objMDM.Address);
            cmd.Parameters.AddWithValue("@HouseNo", objMDM.HouseNo);
            cmd.Parameters.AddWithValue("@SectorStreet", objMDM.SectorStreet);
            cmd.Parameters.AddWithValue("@District", objMDM.District);
            cmd.Parameters.AddWithValue("@MobileNumber", objMDM.MobileNumber);
            cmd.Parameters.AddWithValue("@Email", objMDM.Email);
            cmd.Parameters.AddWithValue("@AadharNo", objMDM.AadharNo);
            cmd.Parameters.AddWithValue("@IsPresident", objMDM.IsPresident);
            var details = cmd.ExecuteNonQuery();
            connection.Close();
            if (details >= 1)
            {
                return 1;
            }
            return 0;
        }

        public int SaveMemberFurtherDetails(MemberFurtherDetails objMFD)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[SaveMemberFurtherDetails]", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MemberSNo", objMFD.MemberSNo);
            cmd.Parameters.AddWithValue("@SocietyTransID", objMFD.SocietyTransID);
            cmd.Parameters.AddWithValue("@MemberName", objMFD.MemberName);
            cmd.Parameters.AddWithValue("@FatherName", objMFD.FatherName);
            cmd.Parameters.AddWithValue("@Gender", objMFD.Gender);
            cmd.Parameters.AddWithValue("@Age", objMFD.Age);
            cmd.Parameters.AddWithValue("@OccupationCode", objMFD.OccupationCode);
            cmd.Parameters.AddWithValue("@Address1", objMFD.Address1);
            cmd.Parameters.AddWithValue("@Address2", objMFD.Address2);
            cmd.Parameters.AddWithValue("@PostOffice", objMFD.PostOffice);
            cmd.Parameters.AddWithValue("@Pin", objMFD.Pin);
            cmd.Parameters.AddWithValue("@DistCode", objMFD.DistCode);
            cmd.Parameters.AddWithValue("@NoOfShares", objMFD.NoOfShares);
            cmd.Parameters.AddWithValue("@NomineeName", objMFD.NomineeName);
            cmd.Parameters.AddWithValue("@NomineeAge", objMFD.NomineeAge);
            cmd.Parameters.AddWithValue("@RelationshipCode", objMFD.RelationshipCode);
            cmd.Parameters.AddWithValue("@Mobile", objMFD.Mobile);
            cmd.Parameters.AddWithValue("@AadharNo", objMFD.AadharNo);
            cmd.Parameters.AddWithValue("@EmailId", objMFD.EmailId);
            var details = cmd.ExecuteNonQuery();
            connection.Close();
            if (details >= 1)
            {
                return 1;
            }
            return 0;
        }

        public List<MembershipDetailsModels> ManagingCommitteMembersList(string SocietyTransID)
        {
            List<MembershipDetailsModels> lst = new List<MembershipDetailsModels>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetListManagingCommitteeMember]", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MembershipDetailsModels
                    {
                        SocietyMemberID = Convert.ToInt32(rdr["SocietyMemberID"]),
                        SocietyMemberName = rdr["SocietyMemberName"].ToString(),
                        MobileNumber = rdr["MobileNumber"].ToString(),
                    });
                }
                return lst;
            }
        }

        public List<MembershipDetailsModels> GetManagingCommitteeMember(int SocietyMemberID)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<MembershipDetailsModels> lst = new List<MembershipDetailsModels>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetManagingCommitteeMember]", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SocietyMemberID", SocietyMemberID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MembershipDetailsModels
                    {
                        SocietyMemberID = Convert.ToInt32(rdr["SocietyMemberID"]),
                        SocietyMemberName = rdr["SocietyMemberName"].ToString(),
                        SocietyMemberDesignation = rdr["SocietyMemberDesignation"].ToString(),
                        RelationshipCode = Convert.ToInt32(rdr["RelationshipCode"]),
                        RelationshipMemberName = rdr["RelationshipMemberName"].ToString(),
                        Address = rdr["Address"].ToString(),
                        HouseNo = rdr["HouseNo"].ToString(),
                        SectorStreet = rdr["SectorStreet"].ToString(),
                        District = Convert.ToInt32(rdr["District"]),
                        MobileNumber = rdr["MobileNumber"].ToString(),
                        Email = rdr["Email"].ToString(),
                        AadharNo = objGBI.Decrypt(rdr["AadharNo"].ToString(), "sblw-3hn8-sqoy19"),
                        IsPresident = Convert.ToBoolean( rdr["IsPresident"]),
                    });
                }
                return lst;
            }
        }

        public int DeleteManagingCommitteMember(int SocietyMemberID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[DeleteManagingCommitteMember]", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SocietyMemberID", SocietyMemberID);
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        public List<MemberFurtherDetails> SocietyMembersList(string SocietyTransID)
        {
            List<MemberFurtherDetails> lst = new List<MemberFurtherDetails>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetSocietyMemberDetailsAllList]", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MemberFurtherDetails
                    {
                        MemberSNo = Convert.ToInt32(rdr["MemberSNo"]),
                        MemberName = rdr["MemberName"].ToString(),
                        FatherName = rdr["FatherName"].ToString(),
                        Mobile = rdr["Mobile"].ToString(),
                        NomineeName = rdr["NomineeName"].ToString(),
                    });
                }
                return lst;
            }
        }

        public List<MemberFurtherDetails> GetbySocietyMemberID(int MemberSNo)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<MemberFurtherDetails> lst = new List<MemberFurtherDetails>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetSocietyMemberDetail]", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MemberSNo", MemberSNo);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MemberFurtherDetails
                    {
                        SocietyTransID = rdr["SocietyTransID"].ToString(),
                        MemberSNo = Convert.ToInt32(rdr["MemberSNo"]),
                        MemberName = rdr["MemberName"].ToString(),
                        FatherName = rdr["FatherName"].ToString(),
                        Gender = rdr["Gender"].ToString(),
                        Age = Convert.ToInt32(rdr["Age"]),
                        OccupationCode = Convert.ToInt32(rdr["OccupationCode"]),
                        Address1 = rdr["Address1"].ToString(),
                        Address2 = rdr["Address2"].ToString(),
                        PostOffice = rdr["PostOffice"].ToString(),
                        Pin = rdr["Pin"].ToString(),
                        DistCode = rdr["DistCode"].ToString(),
                        NoOfShares = Convert.ToInt32(rdr["NoOfShares"]),
                        NomineeName = rdr["NomineeName"].ToString(),
                        NomineeAge = Convert.ToInt32(rdr["NomineeAge"]),
                        RelationshipCode = Convert.ToInt32(rdr["RelationshipCode"]),
                        Mobile = rdr["Mobile"].ToString(),
                        AadharNo = objGBI.Decrypt(rdr["AadharNo"].ToString(), "sblw-3hn8-sqoy19"),
                        EmailId = rdr["EmailId"].ToString(),
                    });
                }
                return lst;
            }
        }

        public int DeleteSocietyMember(int SocietyMemberID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[DeleteSocietyMember]", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MemberSNo", SocietyMemberID);
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        public FormEModels GetDetailsForFormE(string SocietyTransID)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            FormEModels objFEM = new FormEModels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetDetailsForFormE]", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objFEM.Address = rdr["Address"].ToString();
                    objFEM.District = rdr["DisName"].ToString();
                    objFEM.HouseNo = rdr["HouseNo"].ToString();
                    objFEM.RelationshipName = rdr["RelationshipMemberName"].ToString();
                    objFEM.SectorStreet = rdr["SectorStreet"].ToString();
                    objFEM.SocietyMemberName = rdr["SocietyMemberName"].ToString();
                    objFEM.SocietyName = rdr["SocietyName"].ToString();
                }
                return objFEM;
            }
        }
        
        public FormAmodels GetDetailsForFormA(string SocietyTransID)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            FormAmodels objFAM = new FormAmodels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetDetailsForFormA]", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objFAM.Address1 = rdr["Address1"].ToString();
                    objFAM.Address2 = rdr["Address2"].ToString();
                    objFAM.PostOffice = rdr["PostOffice"].ToString();
                    objFAM.SocietyName = rdr["SocietyName"].ToString();
                    objFAM.District = rdr["DisName"].ToString();
                    objFAM.DateofApplicationReceived = Convert.ToDateTime(rdr["DateofApplicationReceived"]);
                 }
                return objFAM;

            }
        }
    }
}
