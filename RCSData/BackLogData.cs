using RCSEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace RCSData
{
    public class BackLogData
    {
        #region ConnectionString
        static readonly string ConStr = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        readonly SqlConnection connection = new SqlConnection(ConStr);
        #endregion;

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
        public List<InspectorList> GetInspectorList(int ARCSCode)
        {
            connection.Open();
            List<InspectorList> lstRM = new List<InspectorList>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetInspectorList]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@ARCSCode", ARCSCode);
            var dr = cmd.ExecuteReader();
            var dt = new DataTable();
            if (dr.HasRows)
            {
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        InspectorList objRM = new InspectorList
                        {
                            InspectorCode = Convert.ToInt32(r["InspectorCode"]),
                            InspectorName = r["InspectName"].ToString()
                        };
                        lstRM.Add(objRM);
                    }
                }
            }
            connection.Close();
            return lstRM;
        }       
        public List<GetShareMemberDetail> GetAllMember()
        {
            connection.Open();
            List<GetShareMemberDetail> lstRM = new List<GetShareMemberDetail>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetShareTransferAllMember]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
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
                    byte[] buffer = (byte[])rdr["@imgbyte"];
                    return buffer;
                }
            }
            return fileStream;
        }

        public byte[] GetImageByteForShare(string SocietyTransID, int ShareTransferID)
        {
            byte[] fileStream = new byte[0];
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetImgByteForShare]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                com.Parameters.AddWithValue("@ShareTransferID", ShareTransferID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    byte[] buffer = (byte[])rdr["imgg"];
                    return buffer;
                }

            }
            return fileStream;
        }
        public int BackLogLogInUpdate(BackLogCredential objBck)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[BLog_UpdatePassword]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@password", objBck.NewPassword);
                com.Parameters.AddWithValue("@salt", objBck.Salt);
                com.Parameters.AddWithValue("@LogInId", objBck.LogInId);
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        public DashboardDetail GetDashBoardData(int UserId)
        {
            DashboardDetail objDD = new DashboardDetail();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetDashboardData]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objDD.SocietyName = rdr["SocietyName"].ToString();
                    objDD.RegId = Convert.ToString(rdr["OldRedgNo"]);
                    objDD.DateofRegistration = Convert.ToDateTime(rdr["CreateDate"].ToString());
                    objDD.SocietyTransId = rdr["SocietyTransID"].ToString();
                    objDD.CommunityOfSocietyId = Convert.ToInt32(rdr["CommunityTypeId"]);
                    objDD.KindOfSocietyId = Convert.ToInt32(rdr["KindSocietyTypeId"]);

                }
                return objDD;
            }
        }

        public string GetElectionDate(string SocietyTransId)
        {
            String ElectionDate = "";
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[Blog_GetElectionDate]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransId);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    ElectionDate = rdr["ElectionDate"].ToString();
                }

                return ElectionDate;
            }
        }

        public DashboardDetail GetAuditDetail(string SocietyTransId)
        {
            DashboardDetail objGetAudit = new DashboardDetail();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[Blog_GetAuditDetail]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransId);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objGetAudit.LastDateAudit = rdr["LastDateAudit"].ToString();
                    objGetAudit.LastDateInspection =rdr["LastDateInspection"].ToString();
                    objGetAudit.AmountOfAuditFees = rdr["AmountAuditFees"].ToString();
                    objGetAudit.SocietyTransId = rdr["SocietyTransID"].ToString();
                    objGetAudit.AreaOfOperation = rdr["AreaOfOperation"].ToString();
                    objGetAudit.GeneralBodyMeeting = rdr["GeneralMeetingDate"].ToString();
                }

                return objGetAudit;
            }
        }

        public List<CommunitySociety> GetSubCommunityofSociety()
        {
            List<CommunitySociety> lstSCSM = new List<CommunitySociety>();
            if (connection != null && connection.State == ConnectionState.Closed)
            { 

                connection.Open();
            }
            SqlCommand cmd = new SqlCommand("[dbo].[GetCommunitySociety]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            var dr = cmd.ExecuteReader();
            var dt = new DataTable();
            if (dr.HasRows)
            {
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        CommunitySociety objSCSM = new CommunitySociety
                        {
                            CommunitySocietyId = Convert.ToInt32(r["CommunitySocietyId"].ToString()),
                            CommunitySocietyName = r["CommunitySocietyName"].ToString()
                        };
                        lstSCSM.Add(objSCSM);
                    }
                }
            }

            connection.Close();
            return lstSCSM;
        }
        public List<FormNameList> GetFormNameList()
        {
            List<FormNameList> lstSCSM = new List<FormNameList>();
            if (connection != null && connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand cmd = new SqlCommand("[dbo].[GetFormNameList]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@ARCSCode", Convert.ToInt32(HttpContext.Current.Session["ARCSCode"]));
            var dr = cmd.ExecuteReader();
            var dt = new DataTable();
            if (dr.HasRows)
            {
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        FormNameList objSCSM = new FormNameList
                        {
                            FormId = r["FormName"].ToString(),
                            FormName = r["FormName"].ToString()
                        };
                        lstSCSM.Add(objSCSM);
                    }
                }
            }

            connection.Close();
            return lstSCSM;
        }

        public int SaveAuditDetail(DashboardDetail objAudit)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[Blog_SaveAuditDetail]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@LastDateAudit", objAudit.LastDateAudit);
            cmd.Parameters.AddWithValue("@LastDateInspection", objAudit.LastDateInspection);
            cmd.Parameters.AddWithValue("@AmountAuditFees", objAudit.AmountOfAuditFees);
            cmd.Parameters.AddWithValue("@SocietyTransID", objAudit.SocietyTransId);
            cmd.Parameters.AddWithValue("@AreaOfOperation", objAudit.AreaOfOperation);
            cmd.Parameters.AddWithValue("@GeneralMeetingDate", objAudit.GeneralBodyMeeting);
            cmd.Parameters.AddWithValue("@IPAddress", objAudit.IPAddress);
            cmd.Parameters.AddWithValue("@BrowserName", objAudit.BrowserName);
            cmd.Parameters.AddWithValue("@LoginID", objAudit.Updatedby);
            var details = cmd.ExecuteNonQuery();
            if (details >= 1)
            {
                return 1;
            }
            connection.Close();
            return 0;
        }

        public int SaveDashboardDetail(DashboardDetail objDash)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[Blog_SaveBlogTempDetail]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@SocietyName", objDash.SocietyName);
            cmd.Parameters.AddWithValue("@DateofRegistration", objDash.DateofRegistration);
            cmd.Parameters.AddWithValue("@RegId", objDash.RegId);
            cmd.Parameters.AddWithValue("@CommunitySocietyId", objDash.CommunityOfSocietyId);
            cmd.Parameters.AddWithValue("@KindSocietyId", objDash.KindOfSocietyId);
            cmd.Parameters.AddWithValue("@IPAddress", objDash.IPAddress);
            cmd.Parameters.AddWithValue("@BrowserName", objDash.BrowserName);
            cmd.Parameters.AddWithValue("@LoginID", objDash.Updatedby);
            var details = cmd.ExecuteNonQuery();
            if (details >= 1)
            {
                return 1;
            }
            connection.Close();
            return 0;
        }

        public int SaveKindSociety(string Value, int UserId)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[SaveKindSociety]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Value", Value);
            cmd.Parameters.AddWithValue("@UserId", UserId);
            var details = cmd.ExecuteNonQuery();
            if (details >= 1)
            {
                return 1;
            }
            connection.Close();
            return 0;
        }

        public int SaveElectionDetail(MembershipDetailsModels objElection)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[Blog_SaveElectionDetail]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@SocietyMemberID", objElection.SocietyMemberID);
            cmd.Parameters.AddWithValue("@SocietyTransID", objElection.SocietyTransID);
            cmd.Parameters.AddWithValue("@SocietyMemberName", objElection.SocietyMemberName);
            cmd.Parameters.AddWithValue("@SocietyMemberDesignation", objElection.SocietyMemberDesignation);
            cmd.Parameters.AddWithValue("@RelationshipMemberName", objElection.RelationshipMemberName);
            cmd.Parameters.AddWithValue("@IPAddress", objElection.IPAddress);
            cmd.Parameters.AddWithValue("@BrowserName", objElection.BrowserName);
            cmd.Parameters.AddWithValue("@LoginID", objElection.Updatedby);
            var details = cmd.ExecuteNonQuery();
            if (details >= 1)
            {
                return 1;
            }
            connection.Close();
            return 0;
        }

        public int SaveElectionDate(BacklogElectionDate objelection)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[Blog_SaveElectionDate]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@SocietyTransID", objelection.SocietyTransId);
            cmd.Parameters.AddWithValue("@ElectionDate", objelection.ElectionDate);
            cmd.Parameters.AddWithValue("@IPAddress", objelection.IPAddress);
            cmd.Parameters.AddWithValue("@BrowserName", objelection.BrowserName);
            cmd.Parameters.AddWithValue("@LoginID", objelection.Updatedby);
            var details = cmd.ExecuteNonQuery();
            if (details >= 1)
            {
                return 1;
            }
            connection.Close();
            return 0;
        }

        public int SaveShareTransfer(ShareTransferDetail objMFD)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[SaveShareTransferDetail]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@imgg", objMFD.Imgg);
            cmd.Parameters.AddWithValue("@SocietyTransID", objMFD.SocietyTransID);
            cmd.Parameters.AddWithValue("@ShareTransferIde", objMFD.ShareTransferID);
            cmd.Parameters.AddWithValue("@MemberId", objMFD.MemberId);
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
            cmd.Parameters.AddWithValue("@dob", objMFD.Dob);
            cmd.Parameters.AddWithValue("@FullPath", objMFD.Fullpath);
            cmd.Parameters.AddWithValue("@ShareTransferAppLetterNo", objMFD.ShareTransferAppLetterNo);
            cmd.Parameters.AddWithValue("@ShareTransferApprovalDate", objMFD.ShareTransferApprovalDate);
            cmd.Parameters.AddWithValue("@ExistingMemberName", objMFD.ExistingMemberName);
            cmd.Parameters.AddWithValue("@IPAddress", objMFD.IPAddress);
            cmd.Parameters.AddWithValue("@BrowserName", objMFD.BrowserName);
            cmd.Parameters.AddWithValue("@LoginID", objMFD.Updatedby);
            cmd.Parameters.AddWithValue("@DateofResolution", objMFD.DateofResolution);
            cmd.Parameters.AddWithValue("@FirstShareTrans", objMFD.FirstShareTrans);
            var details = cmd.ExecuteNonQuery();
            connection.Close();
            if (details >= 1)
            {
                return 1;
            }
            return 0;
        }

        public List<ShareTransferDetail> NewSocietyMembersList(string SocietyTransID)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<ShareTransferDetail> lst = new List<ShareTransferDetail>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[NewGetShareTransferMemberDetailsAllList]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new ShareTransferDetail
                    {
                        ShareTransferID = Convert.ToInt32(rdr["ShareTransferID"]),
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
                        EmailId = rdr["EmailId"].ToString(),
                        RelationshipName = rdr["RelationshipName"].ToString(),
                        DistrictName = rdr["DisName"].ToString(),
                        OccupationName = rdr["OccupationVal"].ToString(),
                        Imgg = System.Text.Encoding.Unicode.GetBytes(rdr["imgg"].ToString()),
                        Imgsrc = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(rdr["imgg"].ToString()), 0, System.Text.Encoding.Unicode.GetBytes(rdr["imgg"].ToString()).Length),
                        Fullpath = rdr["FullPath"].ToString(),
                        ShareTransferAppLetterNo = rdr["ShareTransferAppLetterNo"].ToString(),
                        ShareTransferApprovalDate = rdr["ShareTransferApprovalDate"].ToString(),
                        MemberId = Convert.ToInt32(rdr["OldMemberId"].ToString()),
                        OldMemberName = rdr["OldMemberName"].ToString(),
                    });
                }
                return lst;
            }
        }

        public List<ShareTransferDetail> GetbyShareMemberID(int ShareTransferID)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<ShareTransferDetail> lst = new List<ShareTransferDetail>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetShareTransferMemberDetail]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@ShareTransferID", ShareTransferID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new ShareTransferDetail
                    {
                        SocietyTransID = rdr["SocietyTransID"].ToString(),                        
                        MemberName = rdr["MemberName"].ToString(),
                        FatherName = rdr["FatherName"].ToString(),
                        Gender = rdr["Gender"].ToString(),
                        ShareTransferID = Convert.ToInt32(rdr["ShareTransferID"]),
                        Age = Convert.ToInt32(rdr["Age"]),
                        Address1 = rdr["Address1"].ToString(),
                        Address2 = rdr["Address2"].ToString(),
                        PostOffice = rdr["PostOffice"].ToString(),
                        Pin = rdr["Pin"].ToString(),
                        RelationshipCode = Convert.ToInt32(rdr["RelationshipCode"]),
                        DistCode = rdr["DistCode"].ToString(),
                        NoOfShares = Convert.ToInt32(rdr["NoOfShares"]),
                        NomineeName = rdr["NomineeName"].ToString(),
                        NomineeAge = Convert.ToInt32(rdr["NomineeAge"]),
                        OldMemberId = Convert.ToInt32(rdr["OldMemberId"]),
                        Mobile = rdr["Mobile"].ToString(),
                        AadharNo = objGBI.Decrypt(rdr["AadharNo"].ToString(), Convert.ToString(HttpContext.Current.Session["EncrptedDecruptedKey"])),
                        EmailId = rdr["EmailId"].ToString(),
                        RelationshipName = rdr["RelationshipName"].ToString(),
                        DistrictName = rdr["DistName"].ToString(),
                        OccupationVal = rdr["OccupationVal"].ToString(),
                        Imgg = System.Text.Encoding.Unicode.GetBytes(rdr["img"].ToString()),
                        Fullpath = rdr["FullPath"].ToString(),
                        Flfile = rdr["flfile"].ToString(),
                        Dob = rdr["dob"].ToString(),
                        ShareTransferAppLetterNo = rdr["ShareTransferAppLetterNo"].ToString(),
                        ShareTransferApprovalDate = rdr["ShareTransferApprovalDate"].ToString(),
                        DateofResolution = rdr["DateofResolution"].ToString(),
                        FirstShareTrans = rdr["FirstShareTrans"].ToString(),
                        CopyOfResolution = rdr["CopyofResolution"].ToString(),
                    });
                }
                return lst;
            }
        }

        public int DeleteShareTransferMember(int ShareTransferID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[DeleteShareTransferMember]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@ShareTransferID", ShareTransferID);
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        #region UPLOADFORML
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
                SqlCommand cmd = new SqlCommand("[dbo].[SaveContentUploadsContentForBackLog]", connection)
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
        public int CheckFieldStatus(string SocietyTransID)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[CheckFillStatus]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransId", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    i = Convert.ToInt16(rdr["CheckField"]);
                }
                return i;
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
        #endregion


        #region BackLogOfficerHistory
        public List<BackLogHistoryForOfficer> GetHistoryForOfficer(ParamForHistory objPFH)
        {
            List<BackLogHistoryForOfficer> lst = new List<BackLogHistoryForOfficer>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetHistoryForOfficer]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@Formname", objPFH.Formname);
                com.Parameters.AddWithValue("@InspectorCode", objPFH.InspectorCode);
                com.Parameters.AddWithValue("@ARCSCode", objPFH.ARCSCode);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new BackLogHistoryForOfficer
                    {
                        ColumnName = rdr["ColumnName"].ToString(),
                        OldValue = rdr["OldValue"].ToString(),
                        NewVAlue = rdr["NewVAlue"].ToString(),
                        SocietyTransID = rdr["SocietyTransID"].ToString(),
                        ChangeDate = Convert.ToDateTime(rdr["ChangeDate"]),
                        FirstName = rdr["FirstName"].ToString(),
                        SocietyName = rdr["SocietyName"].ToString(),
                        OldRedgNo = rdr["OldRedgNo"].ToString(),
                        Formname = rdr["Formname"].ToString(),
                    });
                }
                return lst;
            }
        }
        public List<BackLogHistoryForOfficer> GetHistoryForOneEntity(string SocietyTransID)
        {
            List<BackLogHistoryForOfficer> lst = new List<BackLogHistoryForOfficer>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetHistoryForOneEntity]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new BackLogHistoryForOfficer
                    {
                        ColumnName = rdr["ColumnName"].ToString(),
                        OldValue = rdr["OldValue"].ToString(),
                        NewVAlue = rdr["NewVAlue"].ToString(),
                        SocietyTransID = rdr["SocietyTransID"].ToString(),
                        ChangeDate = Convert.ToDateTime(rdr["ChangeDate"]),
                        FirstName = rdr["FirstName"].ToString(),
                        SocietyName = rdr["SocietyName"].ToString(),
                        OldRedgNo = rdr["OldRedgNo"].ToString(),
                        Formname = rdr["Formname"].ToString(),
                    });
                }
                return lst;
            }
        }
        public List<BackLogHistoryForOfficer> GetInspectorChange(string SocietyTransID)
        {
            List<BackLogHistoryForOfficer> lst = new List<BackLogHistoryForOfficer>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetHistoryForOneEntityCount]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new BackLogHistoryForOfficer
                    {
                        ColumnName = rdr["ColumnName"].ToString(),
                        OldValue = rdr["OldValue"].ToString(),
                        NewVAlue = rdr["NewVAlue"].ToString(),
                        SocietyTransID = rdr["SocietyTransID"].ToString(),
                        ChangeDate = Convert.ToDateTime(rdr["ChangeDate"]),
                        FirstName = rdr["FirstName"].ToString(),
                        SocietyName = rdr["SocietyName"].ToString(),
                        OldRedgNo = rdr["OldRedgNo"].ToString(),
                        Formname = rdr["Formname"].ToString(),
                    });
                }
                return lst;
            }
        }

        public Boolean GetDeclaration(string SocietyTransID, int UserId)
        {
            int i;
            Boolean ISCHECK = false;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[Blog_GetDeclaration]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                com.Parameters.AddWithValue("@UserId", UserId);
                i = com.ExecuteNonQuery();
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                     ISCHECK = Convert.ToBoolean(rdr["IsCheked"]);
                    

                }
                return ISCHECK;
            }
        }
        #endregion
    }
}
