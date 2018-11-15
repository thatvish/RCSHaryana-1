namespace RCSEntities
{
    public class GetBasicInfoModels
    {
        public string Keypath { get; set; }
    }

    public class GetDistrict
    {
        public string DistrictName { get; set; }
        public int DistrictCode { get; set; }
    }

    public class GetACRS
    {
        public string ACRSName { get; set; }
        public int ACRSCode { get; set; }
    }

    public class ClassOfSocietyModels
    {
        public int SocietyClassCode { get; set; }
        public string SocietyClassName { get; set; }
    }

    public class SubClassOfSocietyModels
    {
        public int SocietySubClassCode { get; set; }
        public string SocietySubClassName { get; set; }
    }

    public class RelationshipModels
    {
        public int RelationshipCode { get; set; }
        public string RelationshipName { get; set; }
    }

    public class OccupationModels
    {
        public int OccupationCode { get; set; }
        public string OccupationName { get; set; }
    }

    public class MemberCommDesignationCodeModel
    {
        public int MemberCommDesignationCode { get; set; }
        public string MemberCommDesignationName { get; set; }
    }

    public class SocietyListModel
    {
        public string SocietyTransId { get; set; }
        public string SocietyName { get; set; }
    }

    public class InspectorListModel
    {
        public string InspectorId  { get; set; }
        public string InspectorName { get; set; }
    }
}
