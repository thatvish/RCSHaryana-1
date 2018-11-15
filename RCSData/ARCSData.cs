using Microsoft.ApplicationBlocks.Data;
using RCSEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RCSData
{
    public class ARCSData
    {
        #region ConnectionString
        static readonly string ConStr = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        readonly SqlConnection connection = new SqlConnection(ConStr);
        #endregion;

        public List<RemarkFromAscrOfficer> GetDetailsForRemark(string SocietyTransID)
        {
            List<RemarkFromAscrOfficer> objRmk = new List<RemarkFromAscrOfficer>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetRemarkOfARCSOfficer]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        objRmk.Add(new RemarkFromAscrOfficer
                        {
                            Remarks = rdr["Remarks"].ToString(),
                            Date = Convert.ToDateTime(rdr["Date"]),

                        });
                    }
                    return objRmk;
                }
                return objRmk;
            }
        }

        public List<RemarkFromAscrOfficer> BackLogGetDetailsForRemark(string SocietyTransID)
        {
            List<RemarkFromAscrOfficer> objRmk = new List<RemarkFromAscrOfficer>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[Blog_GetRemarkOfARCSOfficer]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        objRmk.Add(new RemarkFromAscrOfficer
                        {
                            Remarks = rdr["Remarks"].ToString(),
                            Date = Convert.ToDateTime(rdr["Date"]),

                        });
                    }
                    return objRmk;
                }
                return objRmk;
            }
        }
        public List<RemarkFromInspector> GetDetailsOfArcsRemark(string SocietyTransID)
        {
            List<RemarkFromInspector> objRmk = new List<RemarkFromInspector>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetRemarkOfInspector]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        objRmk.Add(new RemarkFromInspector
                        {
                            Remarks = rdr["Remarks"].ToString(),
                            Date = Convert.ToDateTime(rdr["Date"]),
                        });
                    }
                    return objRmk;
                }
                return objRmk;
            }
        }
        public List<RemarkFromInspector> BackLogGetDetailsOfArcsRemark(string SocietyTransID)
        {
            List<RemarkFromInspector> objRmk = new List<RemarkFromInspector>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[Blog_GetRemarkOfInspector]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        objRmk.Add(new RemarkFromInspector
                        {
                            Remarks = rdr["Remarks"].ToString(),
                            Date = Convert.ToDateTime(rdr["Date"]),
                        });
                    }
                    return objRmk;
                }
                return objRmk;
            }
        }
        public List<ARCSFreezeData> GetTotalFreeze(string SocietyTransID)
        {
            List<ARCSFreezeData> objRmk = new List<ARCSFreezeData>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[Blog_GetTotalFreeze]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        objRmk.Add(new ARCSFreezeData
                        {
                            Remark = rdr["Remarks"].ToString(),
                            Date = Convert.ToDateTime(rdr["Date"]),
                        });
                    }
                    return objRmk;
                }
                return objRmk;
            }
        }
        public ARCSSocietyStatusModels GetARCSApplicatonCountDetails(int ARCSCode)
        {
            ARCSSocietyStatusModels objARCSSSM = new ARCSSocietyStatusModels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetARCSApplicatonCountDetails]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@ARCSCode", ARCSCode);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objARCSSSM.TotalApprove = Convert.ToInt32(rdr["TotalApprove"]);
                    objARCSSSM.TotalHold = Convert.ToInt32(rdr["TotalHold"].ToString());
                    objARCSSSM.TotalPending = Convert.ToInt32(rdr["TotalPending"]);
                    objARCSSSM.TotalReject = Convert.ToInt32(rdr["TotalReject"].ToString());
                    objARCSSSM.TotalForwardToInspector = Convert.ToInt32(rdr["TotalForwardToInspector"].ToString());
                    objARCSSSM.TotalApplicationComesFromInspector = Convert.ToInt32(rdr["TotalApplicationComesFromInspector"].ToString());
                    objARCSSSM.Total = objARCSSSM.TotalApplicationComesFromInspector + objARCSSSM.TotalApprove + objARCSSSM.TotalHold + objARCSSSM.TotalPending + objARCSSSM.TotalReject + objARCSSSM.TotalForwardToInspector;
                }
                return objARCSSSM;
            }
        }
        public ARCSSocietyStatusModels BackLogGetARCSApplicatonCountDetails(int ARCSCode)
        {
            ARCSSocietyStatusModels objARCSSSM = new ARCSSocietyStatusModels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[Blog_GetARCSApplicatonCountDetails]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@ARCSCode", ARCSCode);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objARCSSSM.TotalApprove = Convert.ToInt32(rdr["TotalApprove"]);
                    objARCSSSM.TotalHold = Convert.ToInt32(rdr["TotalHold"].ToString());
                    objARCSSSM.TotalPending = Convert.ToInt32(rdr["TotalPending"]);
                    objARCSSSM.TotalReject = Convert.ToInt32(rdr["TotalReject"].ToString());
                    objARCSSSM.TotalForwardToInspector = Convert.ToInt32(rdr["TotalForwardToInspector"].ToString());
                    objARCSSSM.TotalApplicationComesFromInspector = Convert.ToInt32(rdr["TotalApplicationComesFromInspector"].ToString());
                    objARCSSSM.Total = objARCSSSM.TotalApplicationComesFromInspector + objARCSSSM.TotalApprove + objARCSSSM.TotalHold + objARCSSSM.TotalPending + objARCSSSM.TotalReject + objARCSSSM.TotalForwardToInspector;
                }
                return objARCSSSM;
            }
        }

        public List<SocietySubmissionFromModels> ARCSDashBoardSocietyList(int OfficerCode)
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

        public List<SocietySubmissionFromModels> GetSocietyDetails(string SocietyTransID)
        {
            List<SocietySubmissionFromModels> lst = new List<SocietySubmissionFromModels>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetSocietyDetailForARCSOfficerBySocietyTransID]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new SocietySubmissionFromModels
                    {
                        SocietyName = rdr["SocietyName"].ToString(),
                        SocietyClassName = rdr["SocietyClassName"].ToString(),
                        SocietySubClassName = rdr["SocietySubClassName"].ToString(),
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
                        Email1 = ValidateEmail(rdr["Email1"].ToString()),
                        Address3 = rdr["Address3"].ToString(),
                        HouseNoSectorNoRoad1 = rdr["HouseNoSectorNoRoad1"].ToString(),
                        PostOffice1 = rdr["PostOffice1"].ToString(),
                        PostalCode1 = rdr["PostalCode1"].ToString(),
                        DistrictForShowUSer = rdr["DistrictForUser"].ToString(),
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

        public int ForwardToIncepector(ForwardToIncepector objFTI)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[ForwardToInspector]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@OfficerCode", objFTI.OfficerCode);
                com.Parameters.AddWithValue("@Remarks", objFTI.Remarks);
                com.Parameters.AddWithValue("@SocietyTransId", objFTI.SocietyTransId);
                com.Parameters.AddWithValue("@Role", 3);
                i = com.ExecuteNonQuery();
            }
            return i;
        }
        public int BackLogForwardToIncepector(ForwardToIncepector objFTI)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[Blog_ForwardToInspector]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@OfficerCode", objFTI.OfficerCode);
                com.Parameters.AddWithValue("@Remarks", objFTI.Remarks);
                com.Parameters.AddWithValue("@SocietyTransId", objFTI.SocietyTransId);
                com.Parameters.AddWithValue("@Role", 3);
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        public String GetPathofForm(string SocietyTransID)
        {
            string Path = "";
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetPathofForm]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    Path = rdr["Path"].ToString();
                }
                return Path;
            }
        }
        public String GetPathofBankReceipt(string SocietyTransID)
        {
            string Path = "";
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetPathofBankReceipt]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    Path = rdr["Path"].ToString();
                }
                return Path;
            }
        }
        public String GetPathofBackLogFormL(string SocietyTransID)
        {
            string Path = "";
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetPathofBackLogFormL]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    Path = rdr["Path"].ToString();
                }
                return Path;
            }
        }
        public String GetPathofCopyofResolution(string SocietyTransID)
        {
            string Path = "";
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetPathofCOR]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    Path = rdr["Path"].ToString();
                }
                return Path;
            }
        }
        public int OfficerHearingAndApprovalStatus(ForwardToIncepector objFTI)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[OfficerHearingAndApprovalStatus]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@OfficerCode", objFTI.OfficerCode);
                com.Parameters.AddWithValue("@Remarks", objFTI.Remarks);
                com.Parameters.AddWithValue("@SocietyTransId", objFTI.SocietyTransId);
                com.Parameters.AddWithValue("@HearingDate", objFTI.HearingDate);
                com.Parameters.AddWithValue("@Status", objFTI.Status);
                com.Parameters.AddWithValue("@Role", 3);
                i = com.ExecuteNonQuery();
            }
            return i;
        }
        public int BackLogOfficerHearingAndApprovalStatus(ForwardToIncepector objFTI)
        {
            int i;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[Blog_OfficerHearingAndApprovalStatus]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@OfficerCode", objFTI.OfficerCode);
                com.Parameters.AddWithValue("@Remarks", objFTI.Remarks);
                com.Parameters.AddWithValue("@SocietyTransId", objFTI.SocietyTransId);
                com.Parameters.AddWithValue("@Status", objFTI.Status);
                com.Parameters.AddWithValue("@Role", 3);
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        public SocietySubmissionFromModels GetApprovedSocietyDetails(string SocietyTransID)
        {
            SocietySubmissionFromModels objSSFM = new SocietySubmissionFromModels();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetApprovedSocietyList]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objSSFM.SocietyName = rdr["SocietyName"].ToString();
                    objSSFM.SocietyTransID = rdr["SocietyTransID"].ToString();
                    objSSFM.NoOfMembers = Convert.ToInt16(rdr["NoOfMembers"].ToString());
                }
                return objSSFM;
            }
        }

        #region UplaodSection
        public int SaveBackLogDetail(ImportExportModels objIEM)
        {
            var userName = "";
            var password = "";
            var SocietyTransId = DateTime.Now.ToString(("yyyyMMdd"));
            var stringChar = new char[3];
            var stringpswd = new char[7];
            var Characters = new String(objIEM.SocietyName.Where(Char.IsLetter).ToArray());
            if (!string.IsNullOrEmpty(Characters))
            {
                Random rnd = new Random();
                for (int j = 0; j <= 2; j++)
                {
                    int index = rnd.Next(0, Characters.Length);
                    stringChar[j] = Characters[index];

                }

                var finalString = new String(stringChar);
                if (!string.IsNullOrEmpty(objIEM.OldRedgNo))
                {
                    userName = objIEM.OldRedgNo + finalString;
                }
            }
            if (!string.IsNullOrEmpty(userName))
            {
                char[] charArray = userName.ToCharArray();
                Array.Reverse(charArray);
                password = new string(charArray);
                Random rnd = new Random();
                for (int j = 0; j <= 6; j++)
                {
                    int index = rnd.Next(0, password.Length);
                    stringpswd[j] = password[index];

                }
                var pswdstring = new String(stringpswd);
                if (!string.IsNullOrEmpty(objIEM.OldRedgNo))
                {
                    password = objIEM.OldRedgNo + pswdstring;
                }
            }
            else
            {
                userName = "";
                password = "";
            }

            int i;
            connection.Open();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[Blog_SaveBackLogDetail]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OldRegId", objIEM.OldRedgNo);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Kind", objIEM.Kind);
                cmd.Parameters.AddWithValue("@GH_No", objIEM.GH_No);
                cmd.Parameters.AddWithValue("@Sector", objIEM.Sector);
                i = cmd.ExecuteNonQuery();
            }
            connection.Close();
            return i;
        }
        public int BacklogSocietyTranslog(ImportExportModels objIEM, string ARCSCode)
        {
            var SocietyTransId = DateTime.Now.ToString(("yyyyMMdd"));

            int i;
            connection.Open();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[Blog_SaveSocietyTransLog]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@SocietyTransID", Convert.ToString(SocietyTransId));
                cmd.Parameters.AddWithValue("@ARCSCode", ARCSCode);
                cmd.Parameters.AddWithValue("@SocietyName", objIEM.SocietyName);
                cmd.Parameters.AddWithValue("@DateofReg", objIEM.CreateDate);
                cmd.Parameters.AddWithValue("@RegistrationNo", objIEM.OldRedgNo);
                cmd.Parameters.AddWithValue("@FunctionalOrWinding", objIEM.FunctionalorWinding);
                i = cmd.ExecuteNonQuery();
            }
            connection.Close();
            return i;
        }
        public int BackLogPendingRecord(ImportExportModels objIEM)
        {
            int i;
            connection.Open();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[Blog_SavePendingExcelRecord]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@SocietyName", objIEM.SocietyName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DateofReg", objIEM.CreateDate1 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@RegistrationNo", objIEM.OldRedgNo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@FunctionalOrWinding", objIEM.FunctionalorWinding ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Kind", objIEM.Kind ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GH_No", objIEM.GH_No ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Sector", objIEM.Sector ?? (object)DBNull.Value);
                i = cmd.ExecuteNonQuery();
            }
            connection.Close();
            return i;
        }

        public List<ImportExportModels> GetPendingRecord(string currentDate)
        {

            connection.Open();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                List<ImportExportModels> objPendingRec = new List<ImportExportModels>();
                SqlCommand cmd = new SqlCommand("[dbo].[Blog_GetPendingExcelRecord]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@currentDate", currentDate);
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
                            ImportExportModels obRec = new ImportExportModels
                            {
                                SocietyName = r["SocietyName"].ToString(),
                                OldRedgNo = r["OldRedgNo"].ToString(),
                                CreateDate = r["CreateDate"].ToString() == "" ? (DateTime?)null : Convert.ToDateTime(r["CreateDate"].ToString()),
                                FunctionalorWinding = r["Functional_OR_Under_winding"].ToString(),
                                Kind = r["Upload_Kind"].ToString(),
                                GH_No = r["Upload_GH_No"].ToString(),
                                Sector = r["Upload_Sector"].ToString()
                            };
                            objPendingRec.Add(obRec);
                        }
                    }
                }
                connection.Close();
                return objPendingRec;
            }
        }
        #endregion

        public DataSet GetName(string prefix)
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetSuggestion]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@prefix", prefix);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(ds);
                return ds;
            }
        }
        public List<ImportExportModels> GetUploadedData(string ARCSCode)
        {
            ARCSCode = "27";
            GetBasicInfo objGBI = new GetBasicInfo();
            List<ImportExportModels> lst = new List<ImportExportModels>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[UploadExcelList]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@ARCSCode", ARCSCode);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new ImportExportModels
                    {
                        SocietyTransID = rdr["SocietyTransID"].ToString(),
                        OldRedgNo = rdr["OldRedgNo"].ToString(),
                        SocietyName = rdr["SocietyName"].ToString(),
                        UserName = rdr["UserName"].ToString(),
                        Password = rdr["Password"].ToString(),
                        CreateDate1 =rdr["CreateDate"].ToString(),
                    });
                }
                return lst;
            }
        }
        public List<ImportExportModels> GetPrintData(string ARCSCode)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<ImportExportModels> lst = new List<ImportExportModels>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetPrintData]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
               // com.Parameters.AddWithValue("@ARCSCode", ARCSCode);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new ImportExportModels
                    {
                        OldRedgNo = rdr["OldRegId"].ToString(),
                        UserName = rdr["UserName"].ToString(),
                        Password = rdr["Password"].ToString(),  
                        SocietyName=rdr["SocietyName"].ToString(),
                    });
                }
                return lst;
            }
        }
        public int SaveDeclaration(SaveDeclaration _savedeclaration)
        {
            int i;
            connection.Open();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[Blog_SaveDeclaration]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@SocietyTransID", _savedeclaration.SocietyTransID);
                cmd.Parameters.AddWithValue("@UserId", _savedeclaration.UserId);              
                cmd.Parameters.AddWithValue("@Ischeck", _savedeclaration.Ischeck);
                cmd.Parameters.AddWithValue("@Remark", _savedeclaration.Remark);
                cmd.Parameters.AddWithValue("@IPAddress", _savedeclaration.IPAddress);
                cmd.Parameters.AddWithValue("@BrowserName", _savedeclaration.BrowserName);
                i = cmd.ExecuteNonQuery();
            }
            connection.Close();
            return i;
        }
        #region 
        public List<DashboardDetail> GetBackLogMemberDetails(string SocietyTransID)
        {
            List<DashboardDetail> lst = new List<DashboardDetail>();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[Blog_GetBackLogMemberDetail]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new DashboardDetail
                    {
                        SocietyName = rdr["SocietyName"].ToString(),
                        RegId = Convert.ToString(rdr["OldRedgNo"].ToString()),
                        Createdate = rdr["CreateDate"].ToString(),
                        CommunityOfSocietyId = Convert.ToInt32(rdr["CommunityTypeId"].ToString()),
                        KindOfSocietyId = Convert.ToInt32(rdr["KindSocietyTypeId"].ToString()),
                        LastDateAudit = rdr["LastDateAudit"].ToString(),
                        LastDateInspection = rdr["LastDateInspection"].ToString(),
                        GeneralBodyMeeting = rdr["GeneralMeetingDate"].ToString(),
                        AmountOfAuditFees = rdr["AmountAuditFees"].ToString(),
                        AreaOfOperation = rdr["AreaOfOperation"].ToString(),
                        SocietyTransId = rdr["SocietyTransID"].ToString(),
                    });
                }
                return lst;

            }
        }

        #endregion
        #region GenerateUserDetails
        public UserCredential GetGeneratedDetail(SeaechDetail objSearch)
        {
            UserCredential objSSFM = new UserCredential();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[GetBySearchDetailsForARCS]", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@SocietyName", objSearch.SocietyName);
                com.Parameters.AddWithValue("@RegistrationNo", objSearch.RegistrationNo);
                com.Parameters.AddWithValue("@FromDate", objSearch.FromDate);
                com.Parameters.AddWithValue("@Todate", objSearch.Todate);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objSSFM.UserName = rdr["SocietyName"].ToString();
                    objSSFM.Password = rdr["SocietyTransID"].ToString();                    
                }
                return objSSFM;
            }
        }
        public int SaveBckCredential(encryptedDetail objRM)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@encryptUserName", objRM.encryptUserName),
                new SqlParameter("@encryptPwd", objRM.encryptPwd),               
                new SqlParameter("@RegNo", objRM.RegNo),
                new SqlParameter("@Salt", objRM.salt),
                new SqlParameter("@CreatedBy", objRM.createdBy),
            };
            var details = SqlHelper.ExecuteNonQuery(Utility.GetConString(), CommandType.StoredProcedure, "[dbo].[SaveBckCredential]", param);

            if (details >= 1)
            {
                return 1;
            }
            return 0;
        }
        #endregion
        #region Report ARCS
        public DataSet ReportBackLogCredential()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[GetPrintData]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    //cmd.Parameters.AddWithValue("@SocietyTransID", SocietyTransID);

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
