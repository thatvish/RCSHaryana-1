namespace RCSEntities
{
    public class Accountstatusmodel
    {
        public int Totalaccount { get; set; }
        public int ARCSaccount { get; set; }
        public int Inspectoraccount { get; set; }
    }

    public class GetDRCSName
    {
        public int DRCSCode { get; set; }
        public string DRCSName { get; set; }
    }
}
