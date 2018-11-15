using Microsoft.ApplicationBlocks.Data;
using RCSEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RCSData
{
    public class Account
    {
        #region ValidateUser
        public int ValidateUser(Login objLogin)
        {
            int IsUserValid = IsValidOrNot(objLogin);
            return IsUserValid;
        }

        public string GetEncrptedSalt(string UserName)
        {
            string EncryptedSalt = "";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@UserName",UserName),
            };
            SqlDataReader rdr = SqlHelper.ExecuteReader(Utility.GetConString(), CommandType.StoredProcedure, "[dbo].[GetUserEncrptedKey]", param);
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    EncryptedSalt = rdr["Salt"].ToString();
                }
            }
            return EncryptedSalt;
        }

        private int IsValidOrNot(Login objL)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@UserName",objL.UserName),
                new SqlParameter("@Password",objL.Password),
            };
            SqlDataReader rdr = SqlHelper.ExecuteReader(Utility.GetConString(), CommandType.StoredProcedure, "[dbo].[ValidateUser]", param);
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    return Convert.ToInt16(rdr["LoginId"]);
                }
            }
            return 0;
        }

        public LoginUserDetails GetLoginUserDetails(Int64 UserId)
        {
            LoginUserDetails objLUD = new LoginUserDetails();
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@LoginId", UserId),
            };
            SqlDataReader rdr = SqlHelper.ExecuteReader(Utility.GetConString(), CommandType.StoredProcedure, "[dbo].[LoginUserDetails]", param);
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    objLUD.UserId = Convert.ToInt32(rdr["LoginId"]);
                    objLUD.Role = Convert.ToInt32(rdr["Role"].ToString());
                    objLUD.Name = rdr["FirstName"].ToString();
                    objLUD.UserType = Convert.ToInt32(rdr["UserTypeCode"]);
                    objLUD.UserName = rdr["UserName"].ToString();
                    objLUD.Salt = rdr["Salt"].ToString();
                    objLUD.ARCSCode = Convert.ToInt32(rdr["ARCSCode"].ToString());
                    objLUD.SocietyTransId = rdr["SocietyTransId"].ToString();
                    objLUD.SocietyStatus = Convert.ToInt32(rdr["SocietyStatus1"]);
                    objLUD.StatusEditable = Convert.ToInt32(rdr["StatusEditable"]);
                    objLUD.FormE = Convert.ToInt32(rdr["FormE"]);
                    objLUD.Total = Convert.ToInt32(rdr["Total"]);
                    objLUD.BackLogResetStatus = Convert.ToInt32(rdr["BackLogResetStatus"]);
                }
            }
            return objLUD;
        }
        public LoginUserDetails GetRoleId(Int64 UserId)
        {
            LoginUserDetails objLUD1 = new LoginUserDetails();
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@LoginId", UserId),
            };
            SqlDataReader rdr = SqlHelper.ExecuteReader(Utility.GetConString(), CommandType.StoredProcedure, "[dbo].[GetRoleId]", param);
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    
                    objLUD1.Role = Convert.ToInt32(rdr["Role"].ToString());
                    objLUD1.SocietyTransId = rdr["SocietyTransID"].ToString();
                    objLUD1.UserId = Convert.ToInt32(rdr["LoginID"].ToString());
                    objLUD1.BackLogResetStatus = Convert.ToInt32(rdr["BackLogResetStatus"].ToString());
                    objLUD1.SocietyStatus = Convert.ToInt32(rdr["SocietyStatus"].ToString());
                }
            }
            return objLUD1;
        }
        public int UpdateLoginAttempts(LoginAttemptsModels objLAM)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@LoginAttempts", objLAM.LoginAttempts),
                new SqlParameter("@UserId", objLAM.UserId),
            };
            var details = SqlHelper.ExecuteNonQuery(Utility.GetConString(), CommandType.StoredProcedure, "[dbo].[UpdateLoginAttempts]", param);
            if (details >= 1)
            {
                return 1;
            }
            return 0;
        }
        #endregion

        public LoginAttemptsModels GetLoginAttempts(string UserId)
        {
            LoginAttemptsModels objLAM = new LoginAttemptsModels();
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@UserId", UserId),
            };
            SqlDataReader rdr = SqlHelper.ExecuteReader(Utility.GetConString(), CommandType.StoredProcedure, "[dbo].[GetLoginAttempts]", param);
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    objLAM.IntervalPending = Convert.ToInt32(rdr["IntervalPending"]);
                    objLAM.LoginAttempts = Convert.ToInt32(rdr["LoginAttempts"].ToString());
                }
            }
            return objLAM;
        }

        #region UserRegistration
        public List<SecurityQuestionsModels> GetSecurityQuestions()
        {
            List<SecurityQuestionsModels> lstSQM = new List<SecurityQuestionsModels>();
            SqlDataReader rdr = SqlHelper.ExecuteReader(Utility.GetConString(), CommandType.StoredProcedure, "[dbo].[GetSecurityQuestions]");
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    SecurityQuestionsModels objSQM = new SecurityQuestionsModels
                    {
                        QuestionId = Convert.ToInt32(rdr["QuestionId"]),
                        SecurityQuestion = rdr["Question"].ToString()
                    };
                    lstSQM.Add(objSQM);
                }
            }
            return lstSQM;
        }

        public int SaveResgiratedUser(ResgirationModels objRM)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Password", objRM.Password),
                new SqlParameter("@DeptID", objRM.DeptID),
                new SqlParameter("@DisCode", objRM.DisCode),
                new SqlParameter("@Username", objRM.Username),
                new SqlParameter("@UserTypeCode", objRM.UserTypeCode),
                new SqlParameter("@DeptDesignationCode", objRM.DeptDesignationCode),
                new SqlParameter("@FirstName", objRM.FirstName),
                new SqlParameter("@EmailID", objRM.EmailID),
                new SqlParameter("@IsChanged", objRM.IsChanged),
                new SqlParameter("@CreatedBy", objRM.CreatedBy),
                new SqlParameter("@SecurityQuestionCode", objRM.SecurityQuestionCode),
                new SqlParameter("@SecurityAnswer", objRM.SecurityAnswer),
                new SqlParameter("@Role", objRM.Role),
                new SqlParameter("@Address1", objRM.Address1),
                new SqlParameter("@Address2", objRM.Address2),
                new SqlParameter("@PostalCode", objRM.PostalCode),
                new SqlParameter("@Salt", objRM.Salt),
                new SqlParameter("@PostOffice", objRM.PostOffice),
                new SqlParameter("@Age", objRM.Age),
                new SqlParameter("@Gender", objRM.Gender),
                new SqlParameter("@Mobile", objRM.Mobile),
            };
            var details = SqlHelper.ExecuteNonQuery(Utility.GetConString(), CommandType.StoredProcedure, "[dbo].[SaveResgiratedUser]", param);

            if (details >= 1)
            {
                return 1;
            }
            return 0;
        }

        public bool ValidateUser(string UserName)
        {
            string status = "";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@UserName",UserName),
            };
            SqlDataReader rdr = SqlHelper.ExecuteReader(Utility.GetConString(), CommandType.StoredProcedure, "[dbo].[CheckUserAvailability]", param);
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
        #endregion
    }
}
