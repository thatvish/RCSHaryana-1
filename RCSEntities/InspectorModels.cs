using System;

namespace RCSEntities
{
    public class InspectorSocietyStatusModels
    {
        public int TotalPending { get; set; }
        public int TotalApprove { get; set; }
        public int TotalHold { get; set; }
        public int TotalReject { get; set; }
        public int Total { get; set; }
    }

    public class ForwardToARCSOfficers
    {
        public string SocietyTransId { get; set; }
        public int IncepectorCode { get; set; }
        public string Remarks { get; set; }
    }

    public class FormFmodel
    {
        public DateTime DateOfSubmittionByInspector { get; set; }
        public string InspectName { get; set; }
    }

    public class FormGmodel
    {
        public string SocietyTransID { get; set; }
        public string ARCSCode { get; set; }
        public string NameOfSocietyProposed_Suitable_Detail { get; set; }
        public DateTime? DateOfSubmittionByInspector { get; set; }
        public DateTime? DateOfSubmissionFormG { get; set; }
        public string ModeOfApplicationReceived { get; set; }
        public string SocietyOrganizedByOwnInitiative { get; set; }
        public bool NameOfSocietyProposed_Suitable { get; set; }
        public bool SocietyOrganizedUnderProjectScheme { get; set; }
        public string SocietyOrganizedUnderProjectScheme_Details { get; set; }
        public string MainObjectsOfProposedSociety { get; set; }
        public bool ObjectConsonanceWithCooperativePrinciples { get; set; }
        public string ObjectConsonanceWithCooperativePrinciples_Details { get; set; }
        public bool AdoptedModalByeLaws { get; set; }
        public string AdoptedModalByeLaws_Details { get; set; }
        public string DeviationsModalByeLaws { get; set; }
        public string DeviationsModalByeLaws_Details { get; set; }
        public bool QualificationsMembership { get; set; }
        public string QualificationsMembership_Details { get; set; }
        public bool PromoterMembers_CommonInterest { get; set; }
        public string PromoterMembers_CommonInterest_Details { get; set; }
        public string VerifiedFormB { get; set; }
        public string CorrectnessAllRespects { get; set; }
        public string NoOfPromoters { get; set; }
        public string AreaPopulation { get; set; }
        public string NumberAndPaidUpValue { get; set; }
        public string ShareInKind { get; set; }
        public string ExplanationToPromoters { get; set; }
        public bool OtherCoopSocietyWithSameObjects { get; set; }
        public string OtherCoopSocietyWithSameObjects_Details { get; set; }
        public string ReasonForNotJoining { get; set; }
        public string AdverselyEffectOthers { get; set; }
        public string ConcludingRemark { get; set; }
        public string AreaOfOperation { get; set; }
        public string ArscAddress { get; set; }
        public string SocietyName { get; set; }
        public string MainObject { get; set; }
        public string SocietyAddress1 { get; set; }
        public string SocietyAddress2 { get; set; }
        public string SocietyFullAddress { get; set; }
        public string Remarks { get; set; }
    }

    public class RemarkFromInspector
    {
        public string SocietyTransId { get; set; }
        public string Remarks { get; set; }
        public DateTime? Date { get; set; }
        public int Status { get; set; }
    }
}
