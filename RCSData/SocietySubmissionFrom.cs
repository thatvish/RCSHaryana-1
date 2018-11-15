using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using RCSEntities;

namespace RCSData
{
    public class SocietySubmissionFrom
    {
        #region ConnectionString
        static readonly string ConStr = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        readonly SqlConnection connection = new SqlConnection(ConStr);
        #endregion;

        public bool CheckProposedSoceityName(string SocietyName)
        {
            string status = "";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@SocietyName",SocietyName),
            };
            SqlDataReader rdr = SqlHelper.ExecuteReader(Utility.GetConString(), CommandType.StoredProcedure, "[dbo].[CheckSocietyAvailability]", param);
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    status = rdr["Status"].ToString();
                }
            }
            if (status == "TRUE")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public GetNumberInfo GetTotalMember(int LogInId)
        {
            GetNumberInfo ObjGet = new GetNumberInfo();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetTotalNoOfMembers]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@LogInId", LogInId);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    ObjGet.GetSocietyTransId = rdr["SocietyTransId"].ToString();
                    ObjGet.GetSocietyStatus = Convert.ToInt32(rdr["SocietyStatus1"].ToString());
                    ObjGet.Totalcount = Convert.ToInt32(rdr["Total"].ToString());
                    ObjGet.GetStatusEditable = Convert.ToInt32(rdr["StatusEditable"].ToString());
                    ObjGet.GetFormE = Convert.ToInt32(rdr["FormE"].ToString());
                }
                return ObjGet;
            }
        }

        public FormGmodel GetDetailsForInspectorFormG2(string SocietyTransID)
        {
            FormGmodel objFGM = new FormGmodel();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetInspectorInfoFormG2]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objFGM.AdoptedModalByeLaws = Convert.ToBoolean(rdr["AdoptedModalByeLaws"].ToString());
                    objFGM.AdoptedModalByeLaws_Details = rdr["AdoptedModalByeLaws_Details"].ToString();
                    objFGM.AdverselyEffectOthers = rdr["AdverselyEffectOthers"].ToString();
                    objFGM.AreaPopulation = rdr["AreaPopulation"].ToString();
                    objFGM.ConcludingRemark = rdr["ConcludingRemark"].ToString();
                    objFGM.CorrectnessAllRespects = rdr["CorrectnessAllRespects"].ToString();
                    objFGM.DeviationsModalByeLaws_Details = rdr["DeviationsModalByeLaws_Details"].ToString();
                    objFGM.ExplanationToPromoters = rdr["ExplanationToPromoters"].ToString();
                    objFGM.MainObjectsOfProposedSociety = rdr["MainObjectsOfProposedSociety"].ToString();
                    objFGM.ModeOfApplicationReceived = rdr["ModeOfApplicationReceived"].ToString();
                    objFGM.NameOfSocietyProposed_Suitable = Convert.ToBoolean(rdr["NameOfSocietyProposed_Suitable"].ToString());
                    objFGM.NoOfPromoters = rdr["NoOfPromoters"].ToString();
                    objFGM.NumberAndPaidUpValue = rdr["NumberAndPaidUpValue"].ToString();
                    objFGM.ObjectConsonanceWithCooperativePrinciples = Convert.ToBoolean(rdr["ObjectConsonanceWithCooperativePrinciples"].ToString());
                    objFGM.ObjectConsonanceWithCooperativePrinciples_Details = rdr["ObjectConsonanceWithCooperativePrinciples_Details"].ToString();
                    objFGM.OtherCoopSocietyWithSameObjects = Convert.ToBoolean(rdr["OtherCoopSocietyWithSameObjects"].ToString());
                    objFGM.OtherCoopSocietyWithSameObjects_Details = rdr["OtherCoopSocietyWithSameObjects_Details"].ToString();
                    objFGM.PromoterMembers_CommonInterest = Convert.ToBoolean(rdr["PromoterMembers_CommonInterest"].ToString());
                    objFGM.PromoterMembers_CommonInterest_Details = rdr["PromoterMembers_CommonInterest_Details"].ToString();
                    objFGM.QualificationsMembership = Convert.ToBoolean(rdr["QualificationsMembership"].ToString());
                    objFGM.ReasonForNotJoining = rdr["ReasonForNotJoining"].ToString();
                    objFGM.ShareInKind = rdr["ShareInKind"].ToString();
                    objFGM.SocietyOrganizedByOwnInitiative = rdr["SocietyOrganizedByOwnInitiative"].ToString();
                    objFGM.SocietyOrganizedUnderProjectScheme = Convert.ToBoolean(rdr["SocietyOrganizedUnderProjectScheme"].ToString());
                    objFGM.SocietyOrganizedUnderProjectScheme_Details = rdr["SocietyOrganizedUnderProjectScheme_Details"].ToString();
                    objFGM.VerifiedFormB = rdr["VerifiedFormB"].ToString();
                    objFGM.AreaOfOperation = rdr["AreaOfOperation"].ToString();
                    objFGM.QualificationsMembership_Details = rdr["QualificationsMembership_Details"].ToString();
                    objFGM.Remarks = rdr["Remarks"].ToString();
                    if (rdr.IsDBNull(rdr.GetOrdinal("Inspector_Submission_Date")))
                    {
                        objFGM.DateOfSubmissionFormG = null;
                    }
                    objFGM.DateOfSubmissionFormG = Convert.ToDateTime(rdr["Inspector_Submission_Date"]);

                }
                return objFGM;
            }
        }

        public List<SocietySubmissionFromModels> GetSocietyDetails(int UserID)
        {
            List<SocietySubmissionFromModels> lst = new List<SocietySubmissionFromModels>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetSocietyForUser]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new SocietySubmissionFromModels
                    {
                        DivCode = rdr["DivCode"].ToString(),
                        ARCSCode = rdr["ARCSCode"].ToString(),
                        SocietyName = rdr["SocietyName"].ToString(),
                        SocietyClassName = rdr["ClassSocietyCode"].ToString(),
                        SocietySubClassName = rdr["SubClassSocietyCode"].ToString(),
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
                        CateOfSociety = rdr["CateOfSociety"].ToString(),
                        DebtsOfMembers = rdr["DebtsOfMembers"].ToString(),
                        AreaMortgaged = rdr["AreaMortgaged"].ToString(),
                        DetailsOfShares = rdr["DetailsOfShares"].ToString(),
                        ValueOfShare = rdr["ValueOfShare"].ToString(),
                        ModeOfPayment = rdr["ModeOfPayment"].ToString(),
                        CorrespondenceAddress = rdr["CorrespondenceAddress"].ToString(),
                        Name1 = rdr["Name1"].ToString(),
                        FatherName1 = rdr["FatherName1"].ToString(),
                        Mobile1 = rdr["Mobile1"].ToString(),
                        Email1 = rdr["Email1"].ToString(),
                        Address3 = rdr["Address3"].ToString(),
                        HouseNoSectorNoRoad1 = rdr["HouseNoSectorNoRoad1"].ToString(),
                        PostOffice1 = rdr["PostOffice1"].ToString(),
                        PostalCode1 = rdr["PostalCode1"].ToString(),
                        DistrictForUser1 = rdr["DistrictForUser1"].ToString(),
                        ShareMoney = Convert.ToInt32(rdr["ShareMoney"]),
                        AdmissionFees = Convert.ToInt32(rdr["AdmissionFees"]),
                        Deposits = Convert.ToInt32(rdr["Deposits"]),
                        Total = Convert.ToInt32(rdr["Total"]),
                        MeetingDate = rdr["MeetingDate1"].ToString(),
                        BankName = Convert.ToString(rdr["BankName1"]),
                        MemberSNo = Convert.ToInt32(rdr["MemberSNo"])
                    });
                }                       
                return lst;
                
            }
        }

        public List<SocietyListModel> GetSocietyForUser(int UserId)
        {
            connection.Open();
            List<SocietyListModel> lstSLM = new List<SocietyListModel>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetSocietyForUser]", connection);
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.CommandType = CommandType.StoredProcedure;
            var dr = cmd.ExecuteReader();
            var dt = new DataTable();
            if (dr.HasRows)
            {
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        SocietyListModel objSLM = new SocietyListModel
                        {
                            SocietyName = r["SocietyName"].ToString(),
                            SocietyTransId = r["SocietyTransId"].ToString()
                        };
                        lstSLM.Add(objSLM);
                    }
                }
            }
            connection.Close();
            return lstSLM;
        }

        public FormGmodel GetDetailsForInspectorFormG(string SocietyTransID)
        {
            FormGmodel objFGM = new FormGmodel();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetInspectorInfoFormG]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                if (rdr.HasRows)
                {

                    while (rdr.Read())
                    {
                        objFGM.DateOfSubmittionByInspector = Convert.ToDateTime(rdr["Date"]);
                        objFGM.SocietyName = Convert.ToString(rdr["SOCIETYNAME"]);
                        objFGM.SocietyAddress1 = Convert.ToString(rdr["ADDRESS1"]);
                        objFGM.SocietyAddress2 = Convert.ToString(rdr["ADDRESS2"]);
                        objFGM.SocietyFullAddress = objFGM.SocietyName + " " + objFGM.SocietyAddress1 + " " + objFGM.SocietyAddress2;
                        objFGM.MainObject = Convert.ToString(rdr["MAINOBJECT1"]);
                        objFGM.ArscAddress = Convert.ToString(rdr["ARCSAddress1"]);

                    }
                    return objFGM;
                }
                objFGM.DateOfSubmittionByInspector = null;
                return objFGM;
            }
        }

        public int SaveSocietySubmissionFrom(SocietySubmissionFromModels objSSFM)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[SocietySubmissionFrom]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
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
            cmd.Parameters.AddWithValue("@Name1", objSSFM.Name1);
            cmd.Parameters.AddWithValue("@FatherName1", objSSFM.FatherName1);
            cmd.Parameters.AddWithValue("@Mobile1", objSSFM.Mobile1);
            cmd.Parameters.AddWithValue("@Email1", objSSFM.Email1);
            cmd.Parameters.AddWithValue("@Address3", objSSFM.Address3);
            cmd.Parameters.AddWithValue("@HouseNoSectorNoRoad1", objSSFM.HouseNoSectorNoRoad1);
            cmd.Parameters.AddWithValue("@PostOffice1", objSSFM.PostOffice1);
            cmd.Parameters.AddWithValue("@PostalCode1", objSSFM.PostalCode1);
            cmd.Parameters.AddWithValue("@DistrictForUser1", objSSFM.DistrictForUser1);
            cmd.Parameters.AddWithValue("@IPAddress", objSSFM.IPAddress);
            cmd.Parameters.AddWithValue("@BrowserName", objSSFM.BrowserName);
            cmd.Parameters.AddWithValue("@LoginID", objSSFM.Updatedby);
            var details = cmd.ExecuteNonQuery();
            if (details >= 1)
            {
                return 1;
            }
            connection.Close();
            return 0;
        }

        public int SaveSocietyMemberDetails(MembershipDetailsModels objMDM)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[SaveSocietyMembershipDetails]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@SocietyMemberID", objMDM.SocietyMemberID);
            cmd.Parameters.AddWithValue("@SocietyTransID", objMDM.SocietyTransID);
            cmd.Parameters.AddWithValue("@SocietyMemberName", objMDM.SocietyMemberName);
            cmd.Parameters.AddWithValue("@SocietyMemberDesignation", objMDM.SocietyMemberDesignation);
            cmd.Parameters.AddWithValue("@RelationshipMemberName", objMDM.RelationshipMemberName);
            cmd.Parameters.AddWithValue("@ManagingRelationshipName", objMDM.ManagingRelationshipName);
            cmd.Parameters.AddWithValue("@IPAddress", objMDM.IPAddress);
            cmd.Parameters.AddWithValue("@BrowserName", objMDM.BrowserName);
            cmd.Parameters.AddWithValue("@LoginID", objMDM.Updatedby);
            var details = cmd.ExecuteNonQuery();
            connection.Close();
            if (details >= 1)
            {
                return 1;
            }
            return 0;
        }
        //public byte[] GetPdfByte()
        //{
        //    byte[] fileStream = new byte[0];
        //    using (SqlConnection con = new SqlConnection(ConStr))
        //    {
        //        con.Open();
        //        SqlCommand com = new SqlCommand("[dbo].[GetPdfString]", con)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };
        //        SqlDataReader rdr = com.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            byte[] buffer = (byte[])rdr["ContentUpload_Byte"];
        //            return buffer;
        //        }
        //    }
        //    return fileStream;
        //}

        public string Base64Decode()
        {
            string base64EncodedData = "";
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetPdfString]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    base64EncodedBytes = System.Convert.FromBase64String(rdr["ContentUpload"].ToString());
                    return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                }
            }
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public byte[] GetImageByte(string SocietyTransID, int MemberSno)
        {
            byte[] fileStream = new byte[0];
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetImgByte]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                com.Parameters.AddWithValue("@MemberSno", MemberSno);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    byte[] buffer = (byte[])rdr["imgg"];
                    return buffer;
                }
            }
            return fileStream;
        }
        
        public int SaveMemberFurtherDetails(MemberFurtherDetails objMFD)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[SaveMemberFurtherDetails]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@MemberSNo", objMFD.MemberSNo);
            cmd.Parameters.AddWithValue("@SocietyTransID", objMFD.SocietyTransID);
            cmd.Parameters.AddWithValue("@MemberName", objMFD.MemberName);
            cmd.Parameters.AddWithValue("@FatherName", objMFD.FatherName);
            cmd.Parameters.AddWithValue("@Gender", objMFD.Gender);
            cmd.Parameters.AddWithValue("@Age", objMFD.Age);
            cmd.Parameters.AddWithValue("@Address1", objMFD.Address1);
            cmd.Parameters.AddWithValue("@Address2", objMFD.Address2);
            cmd.Parameters.AddWithValue("@PostOffice", objMFD.PostOffice);
            cmd.Parameters.AddWithValue("@Pin", objMFD.Pin);
            cmd.Parameters.AddWithValue("@DistCode", objMFD.DistCode);
            cmd.Parameters.AddWithValue("@NoOfShares", objMFD.NoOfShares);
            cmd.Parameters.AddWithValue("@NomineeName", objMFD.NomineeName);
            cmd.Parameters.AddWithValue("@OccupationVal", objMFD.OccupationVal);
            cmd.Parameters.AddWithValue("@NomineeAge", objMFD.NomineeAge);
            cmd.Parameters.AddWithValue("@RelationshipCode", objMFD.RelationshipCode);
            cmd.Parameters.AddWithValue("@Mobile", objMFD.Mobile);
            cmd.Parameters.AddWithValue("@AadharNo", objMFD.AadharNo);
            cmd.Parameters.AddWithValue("@EmailId", objMFD.EmailId);
            cmd.Parameters.AddWithValue("@imgg", objMFD.Imgg);
            cmd.Parameters.AddWithValue("@FullPath", objMFD.Fullpath);
            cmd.Parameters.AddWithValue("@dob", objMFD.Dob);
            cmd.Parameters.AddWithValue("@IPAddress", objMFD.IPAddress);
            cmd.Parameters.AddWithValue("@BrowserName", objMFD.BrowserName);
            cmd.Parameters.AddWithValue("@LoginID", objMFD.Updatedby);
            cmd.Parameters.AddWithValue("@ManagingMemberRelationship", objMFD.ManagingMemberRelationship);
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
            GetBasicInfo objGBI = new GetBasicInfo();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetListManagingCommitteeMember]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MembershipDetailsModels
                    {
                        SocietyMemberID = Convert.ToInt32(rdr["SocietyMemberID"]),
                        SocietyMemberName = rdr["SocietyMemberName"].ToString(),
                        MobileNumber = rdr["MobileNumber"].ToString(),
                        Address = rdr["Address"].ToString(),
                        SectorStreet = rdr["SectorStreet"].ToString(),
                        HouseNo = rdr["HouseNo"].ToString(),
                        Email = ValidateEmail(rdr["Email"].ToString()),
                        SocietyMemberDesignationName = rdr["MemberCommDesignationName"].ToString(),
                        RelationshipName = rdr["RelationshipName"].ToString(),
                        DistrictName = rdr["DisName"].ToString(),
                        RelationshipMemberName = rdr["RelationshipMemberName"].ToString(),
                    });
                }
                return lst;
            }
        }

        public List<MembershipDetailsModels> NewManagingCommitteMembersList(string SocietyTransID)
        {
            List<MembershipDetailsModels> lst = new List<MembershipDetailsModels>();
            GetBasicInfo objGBI = new GetBasicInfo();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[NewGetListManagingCommitteeMember]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MembershipDetailsModels
                    {
                        SocietyMemberID = Convert.ToInt32(rdr["SocietyMemberID"]),
                        SocietyMemberName = rdr["SocietyMemberName"].ToString(),
                        SocietyMemberDesignationName = rdr["MemberCommDesignationName"].ToString(),
                        RelationshipMemberName = rdr["RelationshipMemberName"].ToString(),
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
                SqlCommand com = new SqlCommand("[dbo].[GetManagingCommitteeMember]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
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
                        Email = ValidateEmail(rdr["Email"].ToString()),
                        AadharNo = objGBI.Decrypt(rdr["AadharNo"].ToString(), Convert.ToString(HttpContext.Current.Session["EncrptedDecruptedKey"])),
                        IsPresident = Convert.ToBoolean(rdr["IsPresident"]),
                        DistrictName = rdr["DisName"].ToString(),
                        SocietyMemberDesignationName = rdr["MemberCommDesignationName"].ToString(),
                        RelationshipName = rdr["RelationshipName"].ToString(),
                    });
                }
                return lst;
            }
        }

        public List<MembershipDetailsModels> NewGetManagingCommitteeMember(int SocietyMemberID)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<MembershipDetailsModels> lst = new List<MembershipDetailsModels>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[NewGetManagingCommitteeMember]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyMemberID", SocietyMemberID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MembershipDetailsModels
                    {
                        SocietyMemberID = Convert.ToInt32(rdr["SocietyMemberID"]),
                        SocietyMemberName = rdr["SocietyMemberName"].ToString(),
                        SocietyMemberDesignation = rdr["SocietyMemberDesignation"].ToString(),
                        RelationshipMemberName = rdr["RelationshipMemberName"].ToString(),
                        ManagingRelationshipName = Convert.ToInt32(rdr["MNG_Committee_RelationshipName"]),
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
                SqlCommand com = new SqlCommand("[dbo].[DeleteManagingCommitteMember]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyMemberID", SocietyMemberID);
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        public List<MemberFurtherDetails> SocietyMembersList(string SocietyTransID)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<MemberFurtherDetails> lst = new List<MemberFurtherDetails>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetSocietyMemberDetailsAllList]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MemberFurtherDetails
                    {
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
                        AadharNo = objGBI.Decrypt(rdr["AadharNo"].ToString(), Convert.ToString(HttpContext.Current.Session["EncrptedDecruptedKey"])),
                        EmailId = ValidateEmail(rdr["EmailId"].ToString()),
                        RelationshipName = rdr["RelationshipName"].ToString(),
                        DistrictName = rdr["DisName"].ToString(),
                        OccupationName = rdr["OccupationName"].ToString(),
                    });
                }
                return lst;
            }
        }

        public List<LFormForDownload> DownloadLForm(string SocietyTransID)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<LFormForDownload> lst = new List<LFormForDownload>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetSocietyMemberDetailsAllList]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new LFormForDownload
                    {
                        Age = Convert.ToInt32(rdr["Age"]),
                        Img = rdr["img"].ToString(),
                        NameandFatherName = rdr["Name and Father's Name"].ToString(),
                        NameofNomineeAge = rdr["Name of Nominee & Age"].ToString(),
                        Share = rdr["No Of Shares Subs"].ToString(),
                        Occup = rdr["OccupationName"].ToString(),
                        Address = rdr["Place Of Residence"].ToString(),
                        RelationwithNominee = rdr["Relation with Nominee"].ToString(),

                    });
                }
                return lst;
            }
        }

        public List<MemberFurtherDetails> NewSocietyMembersList(string SocietyTransID)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<MemberFurtherDetails> lst = new List<MemberFurtherDetails>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[NewGetSocietyMemberDetailsAllList]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MemberFurtherDetails
                    {
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
                        AadharNo = objGBI.Decrypt(rdr["AadharNo"].ToString(), Convert.ToString(HttpContext.Current.Session["EncrptedDecruptedKey"])),
                        EmailId = rdr["EmailId"].ToString(),
                        RelationshipName = rdr["RelationshipName"].ToString(),
                        DistrictName = rdr["DisName"].ToString(),
                        RelationWithMember = rdr["Relation_With_Member"].ToString(),
                        OccupationName = rdr["OccupationName"].ToString(),
                        Imgg = System.Text.Encoding.Unicode.GetBytes(rdr["imgg"].ToString()),
                        Imgsrc = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(rdr["imgg"].ToString()), 0, System.Text.Encoding.Unicode.GetBytes(rdr["imgg"].ToString()).Length),
                        Fullpath = rdr["FullPath"].ToString(),
                    });
                }
                return lst;
            }
        }
        public List<GetShareMemberDetail> GetMember(string SocietyTransID)
        {
            connection.Open();
            List<GetShareMemberDetail> lstRM = new List<GetShareMemberDetail>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetShareTransferMember]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
            var dr = cmd.ExecuteReader();
            var dt = new DataTable();
            if (dr.HasRows)
            {
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        GetShareMemberDetail objRM = new GetShareMemberDetail
                        {
                            MemberId = Convert.ToInt32(r["MemberSNo"]),
                            MemberName = r["MemberName"].ToString()
                        };
                        lstRM.Add(objRM);
                    }
                }
            }
            connection.Close();
            return lstRM;
        }
        public List<MemberFurtherDetails> ReportNewSocietyMembersList(string SocietyTransID)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<MemberFurtherDetails> lst = new List<MemberFurtherDetails>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[NewGetSocietyMemberDetailsAllList]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MemberFurtherDetails
                    {
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
                        AadharNo = objGBI.Decrypt(rdr["AadharNo"].ToString(), Convert.ToString(HttpContext.Current.Session["EncrptedDecruptedKey"])),
                        EmailId = rdr["EmailId"].ToString(),
                        RelationshipName = rdr["RelationshipName"].ToString(),
                        DistrictName = rdr["DisName"].ToString(),
                        OccupationName = rdr["OccupationName"].ToString(),
                        Imgg = System.Text.Encoding.Unicode.GetBytes(rdr["imgg"].ToString()),
                        Imgsrc = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(rdr["imgg"].ToString()), 0, System.Text.Encoding.Unicode.GetBytes(rdr["imgg"].ToString()).Length),
                        Fullpath = rdr["FullPath"].ToString(),
                    });
                }
                return lst;
            }
        }

        public string ValidateEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                email = email.Replace("@", "[AT]");
                email = email.Replace(".", "[DOT]");
            }
            return email;
        }

        public List<MemberFurtherDetails> GetbySocietyMemberID(int MemberSNo)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<MemberFurtherDetails> lst = new List<MemberFurtherDetails>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetSocietyMemberDetail]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
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
                        AadharNo = objGBI.Decrypt(rdr["AadharNo"].ToString(), Convert.ToString(HttpContext.Current.Session["EncrptedDecruptedKey"])),
                        EmailId = rdr["EmailId"].ToString(),
                        RelationshipName = rdr["Relationship_Nominee_Name"].ToString(),
                        RelationOfMemberName= rdr["Relationship_WithMember_Name"].ToString(),
                        DistrictName = rdr["DistName"].ToString(),
                        OccupationVal = rdr["OccupationVal"].ToString(),
                        Imgg = System.Text.Encoding.Unicode.GetBytes(rdr["imgg"].ToString()),
                        Fullpath = rdr["FullPath"].ToString(),
                        Flfile = rdr["flfile"].ToString(),
                        Dob = rdr["dob"].ToString(),
                        ManagingMemberRelationship = Convert.ToInt32(rdr["ManagingMemberRelationship"]),
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
                SqlCommand com = new SqlCommand("[dbo].[DeleteSocietyMember]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@MemberSNo", SocietyMemberID);
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        public int CheckPresidentValidation(string SocietyTransId, int SocietyMemberDesignation)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[CheckPresidentValidation]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransId", SocietyTransId);
                com.Parameters.AddWithValue("@SocietyMemberDesignation", SocietyMemberDesignation);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    i = Convert.ToInt32(rdr["Count"]);
                    return i;
                }
            }
            return 0;
        }


        public int GetFormE(string SocietyTransId)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetFormE]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransId", SocietyTransId);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    i = Convert.ToInt32(rdr["Count"]);
                    return i;
                }
            }
            return 0;
        }

        public int SaveFormE(FormEModels objFEM)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[SaveFormE]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransId", objFEM.SocietyTransId);
                com.Parameters.AddWithValue("@ProceedingBook", objFEM.ProceedingBook);
                com.Parameters.AddWithValue("@CashBook", objFEM.CashBook);
                com.Parameters.AddWithValue("@LedgerBook", objFEM.LedgerBook);
                com.Parameters.AddWithValue("@MemberRegister", objFEM.MemberRegister);
                com.Parameters.AddWithValue("@ActandRule", objFEM.ActandRule);
                com.Parameters.AddWithValue("@Byelawsofsociety", objFEM.Byelawsofsociety);
                com.Parameters.AddWithValue("@Appsformembership", objFEM.Appsformembership);
                i = com.ExecuteNonQuery();
                return i;
            }
        }

        public string GetSocietyTransIdForSession(int UserID)
        {
            string SocietyTransId;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetSocietyTransIdForSession]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@UserID", UserID);
                SocietyTransId = Convert.ToString(com.ExecuteScalar());
            }
            return SocietyTransId;
        }


        public FormEDModels GetDetailsForFormE(string SocietyTransID)
        {
            FormEDModels objFEM = new FormEDModels();
            //using (SqlConnection con = new SqlConnection(ConStr))
            //{
            //    con.Open();
            //    SqlCommand com = new SqlCommand("[dbo].[GetDetailsForFormE]", con)
            //    {
            //        CommandType = CommandType.StoredProcedure
            //    };
            //    com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
            //    SqlDataReader rdr = com.ExecuteReader();
            //    while (rdr.Read())
            //    {
            //        objFEM.Address = rdr["Address"].ToString();
            //        objFEM.District = rdr["DisName"].ToString();
            //        objFEM.HouseNo = rdr["HouseNo"].ToString();
            //        objFEM.RelationshipName = rdr["RelationshipMemberName"].ToString();
            //        objFEM.SectorStreet = rdr["SectorStreet"].ToString();
            //        objFEM.SocietyMemberName = rdr["SocietyMemberName"].ToString();
            //        objFEM.SocietyName = rdr["SocietyName"].ToString();
            //        objFEM.DivCode = rdr["StateName"].ToString();
            //    }
            //    return objFEM;
            //}
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetDetailsForFormD]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objFEM.Address = rdr["Address1"].ToString();
                    objFEM.District = rdr["DisName"].ToString();
                    objFEM.RelationshipName = rdr["RelationshipName"].ToString();
                    objFEM.RelationshipMemberName = rdr["RelationshipMemberName"].ToString();
                    objFEM.Address2 = rdr["Address2"].ToString();
                    objFEM.SocietyMemberName = rdr["SocietyMemberName"].ToString();
                    objFEM.SocietyName = rdr["SocietyName"].ToString();
                    objFEM.PostOffice = rdr["PostOffice"].ToString();
                }
                return objFEM;
            }
        }
        public FormEDModels GetDetailsForFormD(string SocietyTransID)
        {
            FormEDModels objFEM = new FormEDModels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetDetailsForFormD]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objFEM.Address = rdr["Address1"].ToString();
                    objFEM.District = rdr["DisName"].ToString();
                    objFEM.RelationshipName = rdr["RelationshipName"].ToString();
                    objFEM.RelationshipMemberName = rdr["RelationshipMemberName"].ToString();
                    objFEM.Address2 = rdr["Address2"].ToString();
                    objFEM.SocietyMemberName = rdr["SocietyMemberName"].ToString();
                    objFEM.SocietyName = rdr["SocietyName"].ToString();
                    objFEM.PostOffice = rdr["PostOffice"].ToString();
                }
                return objFEM;
            }
        }
        public FormEModels GetReportE(string SocietyTransID)
        {
            FormEModels objFEM = new FormEModels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetReportE]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objFEM.SocietyTransId = rdr["SocietyTransId"].ToString();
                    objFEM.ProceedingBook = Convert.ToBoolean(rdr["ProceedingBook"].ToString());
                    objFEM.CashBook = Convert.ToBoolean(rdr["CashBook"].ToString());
                    objFEM.LedgerBook = Convert.ToBoolean(rdr["LedgerBook"].ToString());
                    objFEM.MemberRegister = Convert.ToBoolean(rdr["MemberRegister"].ToString());
                    objFEM.ActandRule = Convert.ToBoolean(rdr["ActandRule"].ToString());
                    objFEM.Byelawsofsociety = Convert.ToBoolean(rdr["Byelawsofsociety"].ToString());
                    objFEM.Appsformembership = Convert.ToBoolean(rdr["Appsformembership"].ToString());
                }
                return objFEM;
            }
        }
        public FormEDModels FormEDetails(string SocietyTransID)
        {
            FormEDModels objFEM = new FormEDModels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetDetailsForFormE]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objFEM.Address = rdr["Address1"].ToString();
                    objFEM.District = rdr["DisName"].ToString();
                    objFEM.RelationshipName = rdr["RelationshipName"].ToString();
                    objFEM.RelationshipMemberName = rdr["Custodian_FatherName"].ToString();
                    objFEM.Address2 = rdr["Address2"].ToString();
                    objFEM.SocietyMemberName = rdr["Custodian_SocietyMemberName"].ToString();
                    objFEM.PostOffice = rdr["PostOffice"].ToString();
                    objFEM.CustodianName = rdr["Custodian_SocietyMemberName"].ToString();
                    objFEM.CustodianFatherName = rdr["Custodian_FatherName"].ToString();
                }
                return objFEM;
            }
        }
        public FormAmodels GetDetailsForFormA(string SocietyTransID)
        {
            FormAmodels objFAM = new FormAmodels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[ReportApprovedData]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objFAM.Address1 = rdr["Address1"].ToString();
                    objFAM.Address2 = rdr["Address2"].ToString();
                    objFAM.PostOffice = rdr["PostOffice"].ToString();
                    objFAM.SocietyName = rdr["SocietyName"].ToString();
                    //objFAM.MeetingDate =Convert.ToDateTime(rdr["MeetingDate"].ToString());
                    objFAM.BankName = rdr["BankName"].ToString();
                    objFAM.District = rdr["DisName"].ToString();
                    objFAM.Pin = rdr["Pin"].ToString();
                    objFAM.SocietyMemberName = rdr["SocietyMemberName"].ToString();
                    objFAM.ARCSName = rdr["ARCSName"].ToString();
                    objFAM.ShareMoney = Convert.ToInt64(rdr["ShareMoney"]);
                    objFAM.AdmissionFee = Convert.ToInt64(rdr["AdmissionFees"]);
                    objFAM.APRDistrictName = Convert.ToString(rdr["APRDistrictName"]);
                    objFAM.Total = Convert.ToInt64(rdr["Total"]);
                    objFAM.Deposit = Convert.ToInt64(rdr["Deposit"]);
                    objFAM.DateofApplicationReceived = Convert.ToDateTime(rdr["DateofApplicationReceived"].ToString());
                }
                return objFAM;
            }
        }
        public FormAmodels ReportDetailsForFormA(string SocietyTransID)
        {
            FormAmodels objFAM = new FormAmodels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetDetailsForFormA]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objFAM.Address1 = rdr["Address1"].ToString();
                    objFAM.Address2 = rdr["Address2"].ToString();
                    objFAM.PostOffice = rdr["PostOffice"].ToString();
                    objFAM.SocietyName = rdr["SocietyName"].ToString();
                    objFAM.District = rdr["DisName"].ToString();
                    objFAM.Pin = rdr["Pin"].ToString();
                    objFAM.SocietyMemberName = rdr["SocietyMemberName"].ToString();
                    objFAM.ARCSName = rdr["ARCSName"].ToString();
                    objFAM.ShareMoney = Convert.ToInt64(rdr["ShareMoney"]);
                    objFAM.AdmissionFee = Convert.ToInt64(rdr["AdmissionFees"]);
                    objFAM.Total = Convert.ToInt64(rdr["Total"]);
                    objFAM.DateofApplicationReceived = Convert.ToDateTime(rdr["DateofApplicationReceived"].ToString()); ;
                }
                return objFAM;
            }
        }       
        public FormFmodel GetDetailsForFormF(string SocietyTransID)
        {
            FormFmodel objFFM = new FormFmodel();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetDetailsForFormF]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objFFM.DateOfSubmittionByInspector = Convert.ToDateTime(rdr["DateOfSubmittionByInspector"]);
                    objFFM.InspectName = rdr["InspectName"].ToString();
                }
                return objFFM;
            }
        }

        public int GetUploadStatus(string SocietyTransID)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetUploadStatus]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    i = Convert.ToInt16(rdr["Total"]);
                }
                return i;
            }
        }
        public int GetUploadStatusPrevious(string SocietyTransID)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetUploadStatusPrevious]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    i = Convert.ToInt16(rdr["Total"]);
                }
                return i;
            }
        }

        public List<MembershipDetailsModels> GetMemberDetails(string SocietyTransID)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<MembershipDetailsModels> lst = new List<MembershipDetailsModels>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetMemberDetails]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyMemberID", SocietyTransID);
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
                        IsPresident = Convert.ToBoolean(rdr["IsPresident"]),
                    });
                }
                return lst;
            }
        }      
        public class AttachmentType
        {
            public string MimeType { get; set; }
            public string FriendlyName { get; set; }
            public string Extension { get; set; }
        }

        public int SaveDetailsofCashierReceipt(RCModels objRC)
        {
            int i;
            connection.Open();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[SaveDetailsofCashierReceipt]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ShareMoney", objRC.ShareMoney);
                cmd.Parameters.AddWithValue("@AdmissionFees", objRC.AdmissionFees);
                cmd.Parameters.AddWithValue("@Deposits", objRC.Deposits);
                cmd.Parameters.AddWithValue("@SocietyTransID", objRC.SocietyTransId);
                i = cmd.ExecuteNonQuery();
            }
            connection.Close();
            return i;
        }

        public RCModels GetDetailsForCashierReceipt(string SocietyTransID)
        {
            RCModels objRC = new RCModels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetDetailsofCashierReceipt]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objRC.AdmissionFees = Convert.ToString(rdr["AdmissionFees"]);
                    objRC.Deposits = Convert.ToString(rdr["Deposits"]);
                    objRC.ShareMoney = Convert.ToString(rdr["ShareMoney"]);
                    objRC.Total = Convert.ToString(rdr["Total"]);
                }
                return objRC;
            }
        }

        public int SaveFromCDetails(string MeetingDate, string BankName, string SocietyTransID)
        {
            int i;
            connection.Open();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[SaveFromCDetails]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@MeetingDate", MeetingDate);
                cmd.Parameters.AddWithValue("@BankName", BankName);
                cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                i = cmd.ExecuteNonQuery();
            }
            connection.Close();
            return i;
        }
        public int SaveCustodian(int MemberSNo, string SocietyTransID)
        {
            int i;
            connection.Open();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[SaveCustodian]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@MemberSNo", MemberSNo);
                cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                i = cmd.ExecuteNonQuery();
            }
            connection.Close();
            return i;
        }
        public int SaveContentFileUploadForBank(ContentFileUploadModel objCFU)
        {
            int i;
            connection.Open();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[SaveContentUploadsContentForBank]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };               
                cmd.Parameters.AddWithValue("@ContentUpload", objCFU.ContentUpload);
                cmd.Parameters.AddWithValue("@FormId", objCFU.FormId);
                cmd.Parameters.AddWithValue("@USER_ID", objCFU.USER_ID);
                cmd.Parameters.AddWithValue("@File_Name", objCFU.File_Name);
                cmd.Parameters.AddWithValue("@SocietyTransID", objCFU.SocietyTransID);
                cmd.Parameters.AddWithValue("@Path", objCFU.Path);
                i = cmd.ExecuteNonQuery();
            }
            connection.Close();
            return i;

        }
        public int SaveContentFileUpload(ContentFileUploadModel objCFU)
        {
            int i;
            connection.Open();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[SaveContentUploadsContent]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };             
                cmd.Parameters.AddWithValue("@ContentUpload", objCFU.ContentUpload);
                cmd.Parameters.AddWithValue("@FormId", objCFU.FormId);
                cmd.Parameters.AddWithValue("@USER_ID", objCFU.USER_ID);
                cmd.Parameters.AddWithValue("@File_Name", objCFU.File_Name);
                cmd.Parameters.AddWithValue("@SocietyTransID", objCFU.SocietyTransID);
                i = cmd.ExecuteNonQuery();
            }
            connection.Close();
            return i;

        }

        public int SaveByeLaws(ByeLawsModel objBL)
        {
            int i;
            connection.Open();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[SaveByeLaws]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                cmd.Parameters.AddWithValue("@docs", objBL.Docs);
                cmd.Parameters.AddWithValue("@ByeLawsName", objBL.ByeLawsName);
                cmd.Parameters.AddWithValue("@SocietyTransID", objBL.SocietyTransID);
                cmd.Parameters.AddWithValue("@Path", objBL.Path);
                i = cmd.ExecuteNonQuery();
            }
            connection.Close();
            return i;

        }

        public int SaveDetailsForFormG(FormGmodel objFGM)
        {
            int i = 0;
            connection.Open();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[SaveDetailsForFormG]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@SocietyTransID", objFGM.SocietyTransID);
                cmd.Parameters.AddWithValue("@DateOfSubmittionByInspector", objFGM.DateOfSubmittionByInspector);
                cmd.Parameters.AddWithValue("@ModeOfApplicationReceived", objFGM.ModeOfApplicationReceived);
                cmd.Parameters.AddWithValue("@SocietyName", objFGM.SocietyName);
                cmd.Parameters.AddWithValue("@SocietyOrganizedByOwnInitiative", objFGM.SocietyOrganizedByOwnInitiative);
                cmd.Parameters.AddWithValue("@NameOfSocietyProposed_Suitable", objFGM.NameOfSocietyProposed_Suitable);
                cmd.Parameters.AddWithValue("@SocietyOrganizedUnderProjectScheme", objFGM.SocietyOrganizedUnderProjectScheme);
                cmd.Parameters.AddWithValue("@SocietyOrganizedUnderProjectScheme_Details", objFGM.SocietyOrganizedUnderProjectScheme_Details);
                cmd.Parameters.AddWithValue("@MainObjectsOfProposedSociety", objFGM.MainObjectsOfProposedSociety);
                cmd.Parameters.AddWithValue("@ObjectConsonanceWithCooperativePrinciple", objFGM.ObjectConsonanceWithCooperativePrinciples);
                cmd.Parameters.AddWithValue("@ObjectConsonanceWithCooperativePrinciples_Details", objFGM.ObjectConsonanceWithCooperativePrinciples_Details);
                cmd.Parameters.AddWithValue("@AdoptedModalByeLaws", objFGM.AdoptedModalByeLaws);
                cmd.Parameters.AddWithValue("@AdoptedModalByeLaws_Details", objFGM.AdoptedModalByeLaws_Details);
                cmd.Parameters.AddWithValue("@DeviationsModalByeLaws_Details", objFGM.DeviationsModalByeLaws_Details);
                cmd.Parameters.AddWithValue("@QualificationsMembership", objFGM.QualificationsMembership);
                cmd.Parameters.AddWithValue("@QualificationsMembership_Details", objFGM.QualificationsMembership_Details);
                cmd.Parameters.AddWithValue("@PromoterMembers_CommonInterest", objFGM.PromoterMembers_CommonInterest);
                cmd.Parameters.AddWithValue("@PromoterMembers_CommonInterest_Details", objFGM.PromoterMembers_CommonInterest_Details);
                cmd.Parameters.AddWithValue("@VerifiedFormB", objFGM.VerifiedFormB);
                cmd.Parameters.AddWithValue("@CorrectnessAllRespects", objFGM.CorrectnessAllRespects);
                cmd.Parameters.AddWithValue("@NoOfPromoters", objFGM.NoOfPromoters);
                cmd.Parameters.AddWithValue("@AreaOfOperation", objFGM.AreaOfOperation);
                cmd.Parameters.AddWithValue("@AreaPopulation", objFGM.AreaPopulation);
                cmd.Parameters.AddWithValue("@NumberAndPaidUpValue", objFGM.NumberAndPaidUpValue);
                cmd.Parameters.AddWithValue("@ShareInKind", objFGM.ShareInKind);
                cmd.Parameters.AddWithValue("@ExplanationToPromoters", objFGM.ExplanationToPromoters);
                cmd.Parameters.AddWithValue("@OtherCoopSocietyWithSameObjects", objFGM.OtherCoopSocietyWithSameObjects);
                cmd.Parameters.AddWithValue("@OtherCoopSocietyWithSameObjects_Details", objFGM.OtherCoopSocietyWithSameObjects_Details);
                cmd.Parameters.AddWithValue("@ReasonForNotJoining", objFGM.ReasonForNotJoining);
                cmd.Parameters.AddWithValue("@AdverselyEffectOthers", objFGM.AdverselyEffectOthers);
                cmd.Parameters.AddWithValue("@ConcludingRemark", objFGM.ConcludingRemark);
                cmd.Parameters.AddWithValue("@Remarks", objFGM.Remarks);
                i = cmd.ExecuteNonQuery();
            }
            connection.Close();
            return i;
        }

        public List<SocietyStatusModels> GetSoceityStatus(string SocietyTransID)
        {
            List<SocietyStatusModels> lst = new List<SocietyStatusModels>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetsocietyStatus]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new SocietyStatusModels
                    {
                        Remarks = rdr["Remarks"].ToString(),
                        Status = rdr["Description"].ToString(),
                        SubmittedDate = DateTime.ParseExact(rdr["Date"].ToString(), "dd/MM/yyyy", null),
                    });
                }
                return lst;
            }
        }
#region  DataSet For Reports
        public DataSet ReportFormA(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetDetailsForFormA]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormA2(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[ReportApprovedData]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormL(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[NewGetSocietyMemberDetailsAllList]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormD(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetDetailsForFormD]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormD2(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetDetailsofCashierReceipt]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormD3(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetMemberCommitteCashier]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormD4(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetDetailsForFormD]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormE(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetDetailsForFormD]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormE2(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetReportE]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormE3(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetDetailsForFormE]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormC(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetDetailsForFormA]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormC2(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetDetailsForFormD]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormC3(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetListManagingCommitteeMember]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormCPresident(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetMemberCommittePresidet]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormCVicePresident(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetMemberCommitteViceP]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormCashier(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetMemberCommitteCashier]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormMember(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetMemberCommitte]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormC1(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[ReportFormC]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormC5(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[ReportDataFormC]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormC6(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetDetailsForFormE]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormF(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetDetailsForFormF]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormF2(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetDetailsForFormC]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormF3(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetInspectorInfoFormG]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormG(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetInspectorInfoFormG]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormG2(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetInspectorInfoFormG2]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        public DataSet ReportFormG3(string SocietyTransID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetDetailsForFormF]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                //Obravnava napak

            }
            return null;
        }
        
        #endregion

    }
}
