using RCSEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RCSData
{
    public class GetBasicInfo
    {
        #region ConnectionString
        static readonly string ConStr = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConStr);
        #endregion;

        public List<GetACRS> ARCSOfficerById(int ARCSCode)
        {
            connection.Open();
            List<GetACRS> lstACRS = new List<GetACRS>();
            SqlCommand cmd = new SqlCommand("[dbo].[BindARCSOfficer]", connection)
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
                        GetACRS obj = new GetACRS
                        {
                            ACRSCode = Convert.ToInt32(r["ARCSCode"]),
                            ACRSName = r["ARCSName"].ToString()
                        };
                        lstACRS.Add(obj);
                    }
                }
            }
            connection.Close();
            return lstACRS;
        }

        public List<GetDistrict> GetDistrict()
        {
            connection.Open();
            List<GetDistrict> lstGD = new List<GetDistrict>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetDistrict]", connection)
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
                        GetDistrict obj = new GetDistrict
                        {
                            DistrictCode = Convert.ToInt32(r["DisCode"]),
                            DistrictName = r["DisName"].ToString()
                        };
                        lstGD.Add(obj);
                    }
                }
            }
            connection.Close();
            return lstGD;
        }

        public List<MemberCommDesignationCodeModel> GetMemberCommDesignation()
        {
            connection.Open();
            List<MemberCommDesignationCodeModel> lstMCDCM = new List<MemberCommDesignationCodeModel>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetMemberCommDesignation]", connection)
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
                        MemberCommDesignationCodeModel obj = new MemberCommDesignationCodeModel
                        {
                            MemberCommDesignationCode = Convert.ToInt32(r["MemberCommDesignationCode"]),
                            MemberCommDesignationName = r["MemberCommDesignationName"].ToString()
                        };
                        lstMCDCM.Add(obj);
                    }
                }
            }
            connection.Close();
            return lstMCDCM;
        }

        public List<GetACRS> GetACRS(int DistrictCode)
        {
            connection.Open();
            List<GetACRS> lstACRS = new List<GetACRS>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetACRSOffice]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@DistrictCode", DistrictCode);
            var dr = cmd.ExecuteReader();
            var dt = new DataTable();
            if (dr.HasRows)
            {
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        GetACRS obj = new GetACRS
                        {
                            ACRSCode = Convert.ToInt32(r["ARCSCode"]),
                            ACRSName = r["ARCSName"].ToString()
                        };
                        lstACRS.Add(obj);
                    }
                }
            }
            connection.Close();
            return lstACRS;
        }

        public List<ClassOfSocietyModels> GetClassOfSociety()
        {
            connection.Open();
            List<ClassOfSocietyModels> lstCSM = new List<ClassOfSocietyModels>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetClassOfSociety]", connection)
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
                        ClassOfSocietyModels objCSM = new ClassOfSocietyModels
                        {
                            SocietyClassCode = Convert.ToInt32(r["SocietyClassCode"]),
                            SocietyClassName = r["SocietyClassName"].ToString()
                        };
                        lstCSM.Add(objCSM);
                    }
                }
            }
            connection.Close();
            return lstCSM;
        }

        public List<SubClassOfSocietyModels> GetSubClassOfSociety(int SocietySubClassCode, int userid = 0)
        {
            connection.Open();
            List<SubClassOfSocietyModels> lstSCSM = new List<SubClassOfSocietyModels>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetSubClassOfSociety]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@SocietyClassCode", SocietySubClassCode);
            cmd.Parameters.AddWithValue("@userid", userid);
            var dr = cmd.ExecuteReader();
            var dt = new DataTable();
            if (dr.HasRows)
            {
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        SubClassOfSocietyModels objSCSM = new SubClassOfSocietyModels
                        {
                            SocietySubClassCode = Convert.ToInt32(r["SocietyClassCode"]),
                            SocietySubClassName = r["SocietySubClassName"].ToString()
                        };
                        lstSCSM.Add(objSCSM);
                    }
                }
            }
            connection.Close();
            return lstSCSM;
        }

        public List<RelationshipModels> GetRelationship()
        {
            connection.Open();
            List<RelationshipModels> lstRM = new List<RelationshipModels>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetRelationship]", connection)
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
                        RelationshipModels objRM = new RelationshipModels
                        {
                            RelationshipCode = Convert.ToInt32(r["RelationshipCode"]),
                            RelationshipName = r["RelationshipName"].ToString()
                        };
                        lstRM.Add(objRM);
                    }
                }
            }
            connection.Close();
            return lstRM;
        }

        public List<OccupationModels> GetOccupations()
        {
            connection.Open();
            List<OccupationModels> lstOM = new List<OccupationModels>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetOccupations]", connection)
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
                        OccupationModels objOM = new OccupationModels
                        {
                            OccupationCode = Convert.ToInt32(r["OccupationCode"]),
                            OccupationName = r["OccupationName"].ToString()
                        };
                        lstOM.Add(objOM);
                    }
                }
            }
            connection.Close();
            return lstOM;
        }

        public string Md5(string value)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(value);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }

        public string Encrypt(string input, string key)
        {
            if (!string.IsNullOrEmpty(input))
            {
                byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider
                {
                    Key = UTF8Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            return "";

        }
        public string Decrypt(string input, string key)
        {
            if(!string.IsNullOrEmpty(input))
            {
                byte[] inputArray = Convert.FromBase64String(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider
                {
                    Key = UTF8Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            return "";
        }

        public List<SocietyListModel> GetSocietyList(int OfficerCode)
        {
            connection.Open();
            List<SocietyListModel> lstSLM = new List<SocietyListModel>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetSocietyDetailsForARCSOfficer]", connection);
            cmd.Parameters.AddWithValue("@OfficerCode", OfficerCode);
            cmd.Parameters.AddWithValue("@Role", Convert.ToInt32(HttpContext.Current.Session["RoleId"]));
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
        public List<SocietyListModel> GetSocietyListBackLog(int OfficerCode)
        {
            connection.Open();
            List<SocietyListModel> lstSLM = new List<SocietyListModel>();
            SqlCommand cmd = new SqlCommand("[dbo].[Blog_GetSocietyDetailsForARCSOfficer]", connection);
            cmd.Parameters.AddWithValue("@OfficerCode", OfficerCode);
            cmd.Parameters.AddWithValue("@Role", Convert.ToInt32(HttpContext.Current.Session["RoleId"]));
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
        public List<SocietyListModel> GetTotalFreezeSocietyList(int OfficerCode)
        {
            connection.Open();
            List<SocietyListModel> lstSLM = new List<SocietyListModel>();
            SqlCommand cmd = new SqlCommand("[dbo].[Blog_GetTotalFreezeForARCSOfficer]", connection);
            cmd.Parameters.AddWithValue("@OfficerCode", OfficerCode);
            cmd.Parameters.AddWithValue("@Role", Convert.ToInt32(HttpContext.Current.Session["RoleId"]));
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


        public List<SocietyListModel> GetSocietyListForApproval(int OfficerCode)
        {
            connection.Open();
            List<SocietyListModel> lstSLM = new List<SocietyListModel>();
            SqlCommand cmd = new SqlCommand("[dbo].[ARCSShowListForApprovalSociety]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@OfficerCode", OfficerCode);
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

        public List<SocietyListModel> GetBackLogSocietyListForApproval(int OfficerCode)
        {
            connection.Open();
            List<SocietyListModel> lstSLM = new List<SocietyListModel>();
            SqlCommand cmd = new SqlCommand("[dbo].[Blog_ARCSShowListForApprovalSociety]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@OfficerCode", OfficerCode);
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
        public List<SocietyListModel> GetApprovedSocieties(int OfficerCode)
        {
            connection.Open();
            List<SocietyListModel> lstSLM = new List<SocietyListModel>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetApprovedSocieties]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@OfficerCode", OfficerCode);
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

        public string Key()
        {
            string keypath = File.ReadAllText(@"c:\file.txt", Encoding.UTF8);
            return keypath;
        }
     
    }
}
