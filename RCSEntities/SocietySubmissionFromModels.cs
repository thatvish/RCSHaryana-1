using System;
using System.ComponentModel.DataAnnotations;

namespace RCSEntities
{
    public class SocietySubmissionFromModels
    {
        public int SocietyId { get; set; }
        [Required(ErrorMessage = "District is required")]
        public string DivCode { get; set; }
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
    }

    public class MembershipDetailsModels
    {
        public int Id { get; set; }
        public int SocietyId { get; set; }
        public string ARCSCode { get; set; }
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
    }

    public class MemberFurtherDetails
    {
        public int ID { get; set; }
        public int SocietyId { get; set; }
        public string ARCSCode { get; set; }
        public string SocietyTransID { get; set; }
        public int MemberSNo { get; set; }
        public string MemberName { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int OccupationCode { get; set; }
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
    }

    public class FormEModels
    {
        public string SocietyMemberName { get; set; }
        public string RelationshipName { get; set; }
        public string Address { get; set; }
        public string HouseNo { get; set; }
        public string SectorStreet { get; set; }
        public string District { get; set; }
        public string SocietyName { get; set; }
    }

    public class FormAmodels
    {
        public string District { get; set; }
        public string SocietyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostOffice { get; set; }
        public DateTime DateofApplicationReceived { get; set; }
    }
}
