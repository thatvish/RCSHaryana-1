using System;

namespace RCSEntities
{
    public class GetShareMemberDetail
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
    }
    public class AttachmentType
    {
        public string MimeType { get; set; }
        public string FriendlyName { get; set; }
        public string Extension { get; set; }
    }
    public class InspectorList
    {
        public string InspectorName { get; set; }
        public int InspectorCode { get; set; }
    }
    public class SaveDeclaration
    {
        public string SocietyTransID { get; set; }       
        public int UserId { get; set; }       
        public Boolean Ischeck { get; set; }
        public string Remark { get; set; }
        public string IPAddress { get; set; }
        public string BrowserName { get; set; }
    }
    public class ShareTransferDetail
    {
        public string NewMemberName { get; set; }
        public string FirstShareTrans { get; set; }
        public string DateofResolution { get; set; }
        public string CopyOfResolution { get; set; }
        public string OldMemberName { get; set; }
        public string ExistingMemberName { get; set; }
        public string ShareTransferApprovalDate { get; set; }
        public string ShareTransferAppLetterNo { get; set; }
        public string Imgsrc { get; set; }
        public string Fullpath { get; set; }
        public string Extension { get; set; }
        public string Dob { get; set; }
        public int ShareTransferID { get; set; }
        public int MemberId { get; set; }
        public int SocietyId { get; set; }
        public string ARCSCode { get; set; }
        public string SocietyTransID { get; set; }
        public int MemberSNo { get; set; }
        public string MemberName { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int OldMemberId { get; set; }
        public int OccupationCode { get; set; }
        public string OccupationVal { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostOffice { get; set; }
        public string Pin { get; set; }
        public string DistCode { get; set; }
        public int NoOfShares { get; set; }
        public string NomineeName { get; set; }
        public int NomineeAge { get; set; }
        public int RelationshipCode { get; set; }
        public string Mobile { get; set; }
        public string AadharNo { get; set; }
        public string EmailId { get; set; }
        public Byte[] Imgg { get; set; }
        public string Flfile { get; set; }
        public string SocietyMemberDesignationName { get; set; }
        public string RelationshipName { get; set; }
        public string DistrictName { get; set; }
        public string OccupationName { get; set; }
        public int Length { get; set; }
        public string BrowserName { get; set; }
        public string IPAddress { get; set; }
        public int Updatedby { get; set; }
    }

    public class BackLogCredential
    {
        public string NewPassword { get; set; }
        public string Salt { get; set; }
        public int LogInId { get; set; }
    }

    public class BacklogElectionDate
    {
        public string ElectionDate { get; set; }
        public int Updatedby { get; set; }
        public string BrowserName { get; set; }
        public string IPAddress { get; set; }
        public string SocietyTransId { get; set; }
    }

    public class CommunitySociety
    {
        public int CommunitySocietyId { get; set; }
        public string CommunitySocietyName { get; set; }
    }

    public class FormNameList
    {
        public string FormId { get; set; }
        public string FormName { get; set; }
    }

    public class DashboardDetail
    {
        public string SocietyName { get; set; }
        public string SocietyTransId { get; set; }
        public DateTime DateofRegistration { get; set; }
        public string Createdate { get; set; }
        public string AmountOfAuditFees { get; set; }
        public string LastDateInspection { get; set; }
        public string LastDateAudit { get; set; }
        public string GeneralBodyMeeting { get; set; }
        public string AreaOfOperation { get; set; }
        public string BrowserName { get; set; }
        public string IPAddress { get; set; }
        public int Updatedby { get; set; }
        public string CommunityOfSociety { get; set; }
        public int CommunityOfSocietyId { get; set; }
        public int KindOfSocietyId { get; set; }
        public string RegId { get; set; }
    }

    #region BackLogOfficerHistory
    public class BackLogHistoryForOfficer
    {
        public string ColumnName { get; set; }
        public string OldValue { get; set; }
        public string NewVAlue { get; set; }
        public string SocietyTransID { get; set; }
        public DateTime ChangeDate { get; set; }
        public string FirstName { get; set; }
        public string SocietyName { get; set; }
        public string OldRedgNo { get; set; }
        public string Formname { get; set; }
    }

    public class ParamForHistory
    {
        public string Formname { get; set; }
        public int ARCSCode { get; set; }
        public int InspectorCode { get; set; }
    }
    #endregion
}
