using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RCSEntities;

namespace RCSData
{
    public class GetBasicInfo
    {
        #region ConnectionString
        static string ConStr = "Data Source=localhost;Initial Catalog=NewRcsHry;Integrated Security=True";
        SqlConnection connection = new SqlConnection(ConStr);
        #endregion;

        public List<GetDistrict> GetDistrict()
        {
            connection.Open();
            List<GetDistrict> lstGD = new List<GetDistrict>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetDistrict]", connection);
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
                        GetDistrict obj = new GetDistrict();
                        obj.DistrictCode = Convert.ToInt32(r["DisCode"]);
                        obj.DistrictName = r["DisName"].ToString();
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
            SqlCommand cmd = new SqlCommand("[dbo].[GetMemberCommDesignation]", connection);
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
                        MemberCommDesignationCodeModel obj = new MemberCommDesignationCodeModel();
                        obj.MemberCommDesignationCode = Convert.ToInt32(r["MemberCommDesignationCode"]);
                        obj.MemberCommDesignationName = r["MemberCommDesignationName"].ToString();
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
            SqlCommand cmd = new SqlCommand("[dbo].[GetACRSOffice]", connection);
            cmd.Parameters.AddWithValue("@DistrictCode", DistrictCode);
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
                        GetACRS obj = new GetACRS();
                        obj.ACRSCode = Convert.ToInt32(r["ARCSCode"]);
                        obj.ACRSName = r["ARCSName"].ToString();
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
            SqlCommand cmd = new SqlCommand("[dbo].[GetClassOfSociety]", connection);
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
                        ClassOfSocietyModels objCSM = new ClassOfSocietyModels();
                        objCSM.SocietyClassCode = Convert.ToInt32(r["SocietyClassCode"]);
                        objCSM.SocietyClassName = r["SocietyClassName"].ToString();
                        lstCSM.Add(objCSM);
                    }
                }
            }
            connection.Close();
            return lstCSM;
        }

        public List<SubClassOfSocietyModels> GetSubClassOfSociety(int SocietySubClassCode)
        {
            connection.Open();
            List<SubClassOfSocietyModels> lstSCSM = new List<SubClassOfSocietyModels>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetSubClassOfSociety]", connection);
            cmd.Parameters.AddWithValue("@SocietyClassCode", SocietySubClassCode);
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
                        SubClassOfSocietyModels objSCSM = new SubClassOfSocietyModels();
                        objSCSM.SocietySubClassCode = Convert.ToInt32(r["SocietySubClassCode"]);
                        objSCSM.SocietySubClassName = r["SocietySubClassName"].ToString();
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
            SqlCommand cmd = new SqlCommand("[dbo].[GetRelationship]", connection);
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
                        RelationshipModels objRM = new RelationshipModels();
                        objRM.RelationshipCode = Convert.ToInt32(r["RelationshipCode"]);
                        objRM.RelationshipName = r["RelationshipName"].ToString();
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
            SqlCommand cmd = new SqlCommand("[dbo].[GetOccupations]", connection);
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
                        OccupationModels objOM = new OccupationModels();
                        objOM.OccupationCode = Convert.ToInt32(r["OccupationCode"]);
                        objOM.OccupationName = r["OccupationName"].ToString();
                        lstOM.Add(objOM);
                    }
                }
            }
            connection.Close();
            return lstOM;
        }

        public string md5(string value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
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
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public string Decrypt(string input, string key)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
