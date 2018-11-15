namespace RCSSerivce
{
    public class XCCPrevent
    {
        public static string FilterBadchars(string str)
        {
            string newchars = "";
            int i;
            string[] badchars = new string[14] { "</script>", "<script>", "select", ";", "--", "insert", "delete", "sp_", "xp_", " and ", " or ", "union", "drop", ";" };
            int n = badchars.Length;
            if (!string.IsNullOrEmpty(str))
            {
                newchars = str;
                for (i = 0; i < n; i++)
                {
                    newchars = newchars.Replace(badchars[i], "");
                }
            }
            return newchars;
        }

        public static string FilterBadcharsEmail(string str)
        {
            string newchars;
            int i;
            string[] badchars = new string[14] { "</script>", "<script>", "select", ";", "--", "insert", "delete", "sp_", "xp_", " and ", " or ", "union", "drop", ";" };
            int n = badchars.Length;
            newchars = str;
            for (i = 0; i < n; i++)
            {
                newchars = newchars.Replace(badchars[i], "");
            }
            return newchars;
        }

        public static string FilterBadchars1(string str)
        {
            string newchars = "";
            int i;
            string[] badchars = new string[] { "</script>", "<script>", "select", ";", "--", "insert", "delete", "sp_", "xp_", " and ", " or ", "union", "drop", ";", "<", ">", "(", ")", "'", ":", ";", "|", "\\", "//", ",", "~", "`", "+", "-", "?", "^", "&", "where", "=", "Truncate", "*", "[", "]" };
            int n = badchars.Length;
            if (!string.IsNullOrEmpty(str))
            {
                newchars = str;
                for (i = 0; i < n; i++)
                {
                    newchars = newchars.Replace(badchars[i], "");
                }
            }
            return newchars;
        }
    }
}
