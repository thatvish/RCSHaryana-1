using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCSEntities;
using System.Configuration;


namespace RCSData
{
    public class Account
    {
        #region ConnectionString
        //SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);
        static string ConStr = "Data Source=localhost;Initial Catalog=NewRcsHry;Integrated Security=True";
        SqlConnection connection = new SqlConnection(ConStr);
        #endregion;

        #region ValidateUser
        public int ValidateUser(Login objLogin)
        {
            int IsUserValid = IsValidOrNot(objLogin);
            return IsUserValid;
        }

        public string GetEncrptedSalt(string UserName)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[GetUserEncrptedKey]", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", UserName);
            SqlDataReader rdr = cmd.ExecuteReader();
            string EncryptedSalt = "";
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    EncryptedSalt = rdr["Salt"].ToString();
                }
            }
            connection.Close();
            return EncryptedSalt;
        }

        private int IsValidOrNot(Login objL)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[ValidateUser]", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", objL.UserName);
            cmd.Parameters.AddWithValue("@Password", objL.Password);
            var details = cmd.ExecuteReader();
            var statusMaster = new DataTable();
            if (details.HasRows)
            {
                statusMaster.Load(details);
                return Convert.ToInt16(statusMaster.Rows[0]["LoginId"]);
            }
            connection.Close();
            return 0;
        }

        public LoginUserDetails GetLoginUserDetails(int UserId)
        {
            LoginUserDetails objLUD = new LoginUserDetails();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].[LoginUserDetails]", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@LoginId", UserId);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    objLUD.UserId = Convert.ToInt32(rdr["LoginId"]);
                    objLUD.Role = rdr["Role"].ToString();
                    objLUD.Name = rdr["FirstName"].ToString();
                    objLUD.UserType = Convert.ToInt32(rdr["UserTypeCode"]);
                }
                return objLUD;
            }
        }
        #endregion

        #region UserRegistration
        public List<SecurityQuestionsModels> GetSecurityQuestions()
        {
            connection.Open();
            List<SecurityQuestionsModels> lstSQM = new List<SecurityQuestionsModels>();
            SqlCommand cmd = new SqlCommand("[dbo].[GetSecurityQuestions]", connection);
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
                        SecurityQuestionsModels objSQM = new SecurityQuestionsModels();
                        objSQM.QuestionId = Convert.ToInt32(r["QuestionId"]);
                        objSQM.SecurityQuestion = r["Question"].ToString();
                        lstSQM.Add(objSQM);
                    }
                }
            }
            connection.Close();
            return lstSQM;
        }

        public int SaveResgiratedUser(ResgirationModels objRM)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[SaveResgiratedUser]", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Password", objRM.Password);
            cmd.Parameters.AddWithValue("@DeptID", objRM.DeptID);
            cmd.Parameters.AddWithValue("@DisCode", objRM.DisCode);
            cmd.Parameters.AddWithValue("@ARCSCode", objRM.ARCSCode);
            cmd.Parameters.AddWithValue("@Username", objRM.Username);
            cmd.Parameters.AddWithValue("@UserTypeCode", objRM.UserTypeCode);
            cmd.Parameters.AddWithValue("@DeptDesignationCode", objRM.DeptDesignationCode);
            cmd.Parameters.AddWithValue("@FirstName", objRM.FirstName);
            cmd.Parameters.AddWithValue("@EmailID", objRM.EmailID);
            cmd.Parameters.AddWithValue("@IsChanged", objRM.IsChanged);
            cmd.Parameters.AddWithValue("@CreatedBy", objRM.CreatedBy);
            cmd.Parameters.AddWithValue("@SecurityQuestionCode", objRM.SecurityQuestionCode);
            cmd.Parameters.AddWithValue("@SecurityAnswer", objRM.SecurityAnswer);
            cmd.Parameters.AddWithValue("@Role", objRM.Role);
            cmd.Parameters.AddWithValue("@Address1", objRM.Address1);
            cmd.Parameters.AddWithValue("@Address2", objRM.Address2);
            cmd.Parameters.AddWithValue("@PostalCode", objRM.PostalCode);
            cmd.Parameters.AddWithValue("@Salt", objRM.Salt);
            cmd.Parameters.AddWithValue("@DistrictOfOperation", objRM.DistrictOfOperation);
            cmd.Parameters.AddWithValue("@PostOffice", objRM.PostOffice);
            cmd.Parameters.AddWithValue("@Age", objRM.Age);
            cmd.Parameters.AddWithValue("@Gender", objRM.Gender);
            cmd.Parameters.AddWithValue("@Mobile", objRM.Mobile);
            cmd.Parameters.AddWithValue("@SocietyStatus", objRM.SocietyStatus);
            cmd.Parameters.AddWithValue("@SocietyRegistrationNo", objRM.SocietyRegistrationNo);
            var details = cmd.ExecuteNonQuery();
            if (details >= 1)
            {
                return 1;
            }
            connection.Close();
            return 0;
        }

        public bool ValidateUser(string UserName)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("[dbo].[CheckUserAvailability]", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", UserName);
            SqlDataReader rdr = cmd.ExecuteReader();
            string status = "";
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    status = rdr["Status"].ToString();
                }
            }
            connection.Close();
            if (status == "TRUE")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
