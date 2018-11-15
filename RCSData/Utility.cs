using System;
using System.Configuration;

namespace RCSData
{
    public class Utility
    {
        public static String GetConString()
        {
            return ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        }
    }
}
