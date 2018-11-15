using System;

namespace RCSEntities
{
    public class ARCSSocietyStatusModels
    {
        public int TotalPending { get; set; }
        public int TotalApprove { get; set; }
        public int TotalHold { get; set; }
        public int TotalReject { get; set; }
        public int Total { get; set; }
        public int TotalForwardToInspector { get; set; }
        public int TotalApplicationComesFromInspector { get; set; }
    }
    [Serializable]
    public class encryptedDetail
    {
        public string encryptUserName { get; set; }
        public string RegNo { get; set; }
        public string encryptPwd { get; set; }
        public string salt { get; set; }
        public string createdBy { get; set; }
    }
    public class RemarkFromAscrOfficer
    {
        public string SocietyTransId { get; set; }
        public string Remarks { get; set; }
        public DateTime? Date { get; set; }
        public int Status { get; set; }
    }
    public class ARCSFreezeData
    {
        public string Remark { get; set; }      
        public DateTime? Date { get; set; }       
    }

    public class UserCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class SeaechDetail
    {
        public string SocietyName { get; set; }
        public string RegistrationNo { get; set; }    
        public DateTime? FromDate { get; set; }
        public DateTime? Todate { get; set; }
    }

    public class ForwardToIncepector
    {
        public string SocietyTransId { get; set; }
        public string Remarks { get; set; }
        public int Role { get; set; }
        public int OfficerCode { get; set; }
        public DateTime? HearingDate { get; set;}
        public int Status { get; set; }
    }
}
