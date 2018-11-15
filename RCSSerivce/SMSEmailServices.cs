using System;
using System.IO;
using System.Net;

namespace RCSSerivce
{
    public class SMSEmailServices
    {
        public void Sendmsg(string smsno, string smsmsg)
        {
            StreamReader reader = null;
            HttpWebResponse response = null;
            try
            {
                if (smsno.Trim().Length >= 10 && smsno.Trim().Length <= 12)
                {
                    string msg = smsmsg;
                    string req = "http://smsgw.sms.gov.in/failsafe/HttpLink?username=ifmshry.sms&pin=Py%2346sfd5d&message=" + msg + "&mnumber=" + Convert.ToInt64(smsno) + "&signature=nicsms";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(req);
                    response = (HttpWebResponse)request.GetResponse();
                    //Assign the response version to the String variable.
                    String ver = response.ProtocolVersion.ToString();
                    /* Create and assign the response stream to the StreamReader
                    variable.*/
                    reader = new StreamReader(response.GetResponseStream());
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }
        }
    }
}
