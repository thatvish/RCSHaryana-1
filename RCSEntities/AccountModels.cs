using System;
using System.ComponentModel.DataAnnotations;

namespace RCSEntities
{
    public class Login
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }
        public string Salt { get; set; }
    }

    public class LoginUserDetails
    {
        public string Salt { get; set; }
        public int Role { get; set; }
        public int UserType { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public int ARCSCode { get; set; }
        public string SocietyTransId { get; set; }
        public int SocietyStatus { get; set; }
        public int StatusEditable { get; set; }
        public int FormE { get; set; }
        public int Total { get; set; }
        public int BackLogResetStatus { get; set; }
    }

    public class SecurityQuestionsModels
    {
        public int QuestionId { get; set; }
        public string SecurityQuestion { get; set; }
    }

    public class LoginAttemptsModels
    {
        public int LoginId { get; set; }
        public int LoginAttempts { get; set; }
        public int IntervalPending { get; set; }
        public string UserId { get; set; }
    }

    public class ResgirationModels
    {
        public int LoginID { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public int DeptID { get; set; }
        public string DisCode { get; set; }
        public string ARCSCode { get; set; }
        [Required]
        [Display(Name = "UserName")]
        public string Username { get; set; }
        public int UserTypeCode { get; set; }
        public int DeptDesignationCode { get; set; }
        public string FirstName { get; set; }
        public string EmailID { get; set; }
        public bool IsActive { get; set; }
        public bool IsChanged { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string TransferRecordModifiedBy { get; set; }
        public string SecurityQuestionCode { get; set; }
        public string SecurityAnswer { get; set; }
        public int Role { get; set; }
        public string Hash { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string Salt { get; set; }

        public int DistrictOfOperation { get; set; }
        public string PostOffice { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }

        public string SocietyStatus { get; set; }
        public string SocietyRegistrationNo { get; set; }
    }
}
