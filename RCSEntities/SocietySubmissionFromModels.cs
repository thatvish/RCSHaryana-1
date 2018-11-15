using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace RCSEntities
{
    public class GetNumberInfo
    {
        public string GetSocietyTransId { get; set; }
        public int GetSocietyStatus { get; set; }
        public int GetStatusEditable { get; set; }
        public int GetFormE { get; set; }
        public int Totalcount { get; set; }
        public int NoofMember { get; set; }
    }

    public class SocietySubmissionFromModels
    {
        public int SocietyId { get; set; }
        [Required(ErrorMessage = "District is required")]
        public string DivCode { get; set; }
        public string TypeofSociety { get; set; }
        public string ARCSCode { get; set; }
        public string SocietyTransID { get; set; }
        public string SocietyCodeManual { get; set; }
        public string SocietyName { get; set; }
        public string ClassSocietyCode { get; set; }
        public string SubClassSocietyCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostOffice { get; set; }
        public string Pin { get; set; }
        public string AreaOfOperation { get; set; }
        public string Mainobject1 { get; set; }
        public string Mainobject2 { get; set; }
        public string Mainobject3 { get; set; }
        public string Mainobject4 { get; set; }
        public int NoOfMembers { get; set; }
        public int OccupationOfMember { get; set; }
        public string CateOfSociety { get; set; }
        public string DebtsOfMembers { get; set; }
        public string AreaMortgaged { get; set; }
        public string DetailsOfShares { get; set; }
        public string ValueOfShare { get; set; }
        public string ModeOfPayment { get; set; }
        public string CorrespondenceAddress { get; set; }
        public string NameAndAddressPromoters { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateofApplicationReceived { get; set; }
        public DateTime DateOfApproval { get; set; }
        public string ResolutionFilePath { get; set; }
        public string FormDFilePath { get; set; }
        public bool FormB { get; set; }
        public bool FormC { get; set; }
        public bool FormD { get; set; }
        public bool FormE { get; set; }
        public bool ByLawsDocs { get; set; }
        public bool Challan { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime FinalSubmitDate { get; set; }
        public bool IsActive { get; set; }

        public string SocietyClassName { get; set; }
        public string SocietySubClassName { get; set; }
        public string OccupationName { get; set; }

        //Corrpondence Address Models

        public string Name1 { get; set; }
        public string FatherName1 { get; set; }
        public string Mobile1 { get; set; }
        public string Email1 { get; set; }
        public string Address3 { get; set; }
        public string HouseNoSectorNoRoad1 { get; set; }
        public string PostOffice1 { get; set; }
        public string PostalCode1 { get; set; }
        public string DistrictForUser1 { get; set; }

        public string DistrictForShowUSer { get; set; }

        //For Deposit Fee
        public int ShareMoney { get; set; }
        public int AdmissionFees { get; set; }
        public int Deposits { get; set; }
        public int Total { get; set; }
        public string MeetingDate { get; set; }
        public string BankName { get; set; }
        public int MemberSNo { get; set; }       
        public string BrowserName { get; set; }
        public string IPAddress { get; set; }
        public int Updatedby { get; set; }
    }

    public class SocietyStatusModels
    {
        public string Status { get; set; }
        public DateTime? HearingDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string Remarks { get; set; }
    }

    public class MembershipDetailsModels
    {
        public int Id { get; set; }
        public int SocietyId { get; set; }
        public string ARCSCode { get; set; }
        public int ManagingRelationshipName { get; set; }       
        public string SocietyTransID { get; set; }
        public int SocietyMemberID { get; set; }
        [Required]
        public string SocietyMemberName { get; set; }
        public string SocietyMemberDesignation { get; set; }
        public int RelationshipCode { get; set; }
        public string RelationshipMemberName { get; set; }
        public string Address { get; set; }
        public string HouseNo { get; set; }
        public string SectorStreet { get; set; }
        public int District { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string AadharNo { get; set; }
        public bool IsPresident { get; set; }
        public int Updatedby { get; set; }
        public string BrowserName { get; set; }
        public string IPAddress { get; set; }
        public string SocietyMemberDesignationName { get; set; }
        public string RelationshipName { get; set; }
        public string DistrictName { get; set; }
    }

    public class MemberFurtherDetails
    {
        public string Imgsrc { get; set; }
        public string Fullpath { get; set; }
        public string Extension { get; set; }
        public string RelationWithMember { get; set; }
        public string RelationOfMemberName { get; set; }
        public string Dob { get; set; }
        public int ID { get; set; }
        public int SocietyId { get; set; }
        public string ARCSCode { get; set; }
        public string SocietyTransID { get; set; }
        public int MemberSNo { get; set; }
        public int ManagingMemberRelationship { get; set; }
        public string MemberName { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
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

    public class LFormForDownload
    {
        public string NameandFatherName { get; set; }
        public int Age { get; set; }
        public string Occup { get; set; }
        public string Address { get; set; }
        public string Share { get; set; }
        public string NameofNomineeAge { get; set; }
        public string RelationwithNominee { get; set; }
        public string Img { get; set; }
    }

    public class FormEDModels
    {
        public string SocietyMemberName { get; set; }
        public string CustodianFatherName { get; set; }
        public string CustodianName { get; set; }
        public string CustodianRelationName { get; set; }
        public string RelationshipName { get; set; }
        public string RelationshipMemberName { get; set; }
        public string PostOffice { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string HouseNo { get; set; }
        public string SectorStreet { get; set; }
        public string District { get; set; }
        public string SocietyName { get; set; }
        public string DivCode { get; set; }
    }

    public class FormEModels
    {
        public int Id { get; set; }
        public string SocietyTransId { get; set; }
        public bool ProceedingBook { get; set; }
        public bool CashBook { get; set; }
        public bool LedgerBook { get; set; }
        public bool MemberRegister { get; set; }
        public bool ActandRule { get; set; }
        public bool Byelawsofsociety { get; set; }
        public bool Appsformembership { get; set; }
        public bool Other { get; set; }
    }

    public class FormAmodels
    {
        public string District { get; set; }
        public string BankName { get; set; }
        public string SocietyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostOffice { get; set; }
        public DateTime MeetingDate { get; set; }
        public DateTime DateofApplicationReceived { get; set; }
        public string Pin { get; set; }
        public string SocietyMemberName { get; set; }
        public string ARCSName { get; set; }
        public string ARCSDesignation { get; set; }
        public string APRDistrictName { get; set; }
        public Int64 ShareMoney { get; set; }
        public Int64 AdmissionFee { get; set; }
        public Int64 Total { get; set; }
        public Int64 Deposit { get; set; }

    }

    public class RCModels
    {
        public string ShareMoney { get; set; }
        public string AdmissionFees { get; set; }
        public string Deposits { get; set; }
        public string Total { get; set; }
        public string SocietyTransId { get; set; }
    }

    public class ContentFileUploadModel
    {
        public string SocietyTransID { get; set; }
        public string ContentUpload { get; set; }
        public Byte[] ContentUpload_Byte { get; set; }
        public int FormId { get; set; }
        public int USER_ID { get; set; }
        public string File_Name { get; set; }
        public string Path { get; set; }
    }

    public class ByeLawsModel
    {
        public string Docs { get; set; }
        public string ByeLawsName { get; set; }
        public string SocietyTransID { get; set; }
        public string Path { get; set; }
    }

    public class SocetyListModel
    {
        public string SocietyName { get; set; }
        public string SocietyTransId { get; set; }
    }

    public class FileUploadModel
    {
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}
