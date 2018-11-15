using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace RCSEntities
{
    public class RBACUser
    {
        #region ConnectionString
        static readonly string ConStr = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        readonly SqlConnection connection = new SqlConnection(ConStr);
        #endregion;

        public int User_Id { get; set; }
        public bool IsSysAdmin { get; set; }
        public string Username { get; set; }
        public int RoleName { get; set; }
        private List<UserRole> Roles = new List<UserRole>();

        public RBACUser(string Role)
        {
            this.IsSysAdmin = false;
        }

        public bool HasPermission(string requiredPermission)
        {
            bool bFound = false;
            foreach (UserRole role in this.Roles)
            {
                bFound = (role.Permissions.Where(
                          p => p.PermissionDescription == requiredPermission.ToLower().Trim()).ToList().Count > 0);
                if (bFound)
                    break;
            }
            return bFound;
        }

        public bool HasRole(string role)
        {
            return (Roles.Where(p => p.RoleName.ToLower() == role.ToLower()).ToList().Count > 0);
        }

        public bool HasRoles(string roles)
        {
            bool bFound = false;
            string[] _roles = roles.ToLower().Split(';');
            foreach (UserRole role in this.Roles)
            {
                try
                {
                    bFound = _roles.Contains(role.RoleName.ToLower());
                    if (bFound)
                        return bFound;
                }
                catch (Exception)
                {
                }
            }
            return bFound;
        }

        public bool HasPermission(string requiredPermission, string roleId)
        {
            bool bFound = false;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand com = new SqlCommand("HasPermission", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.Parameters.AddWithValue("@Permission", requiredPermission);
                com.Parameters.AddWithValue("@RoleID", roleId);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    int i = Convert.ToInt32(rdr["count"]);
                    if(i == 1)
                    {
                        bFound = true;
                    }
                }
                return bFound;
            }
        }
    }

    public class UserRole
    {
        public int Role_Id { get; set; }
        public string RoleName { get; set; }
        public List<Permission> Permissions = new List<Permission>();
    }

    public class Permission
    {
        public bool IsActive { get; set; }
        public int ID { get; set; }
        public string PermissionDescription { get; set; }
    }
}
