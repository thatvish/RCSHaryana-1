using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Collections.Generic;


namespace RCSEntities
{
    public class ImportExport
    {
        [Required(ErrorMessage = "Please select file")]
        [FileExt(Allow = ".xls,.xlsx", ErrorMessage = "Only excel file")]
        public HttpPostedFileBase File { get; set; }
    }

    public class ImportExportModels
    {
        public string SocietyName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string OldRedgNo { get; set; }
        public string CreateDate1 { get; set; }
        public DateTime? CreateDate { get; set; }
        public string SocietyTransID { get; set; }
        public string FunctionalorWinding { get; set; }
        public string Kind { get; set; }
        public string Sector { get; set; }
        public string GH_No { get; set; }
        public List<ImportExportModels> Objimport  { get; set; }
}

    public class FileExt : ValidationAttribute
    {
        public string Allow;
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string extension = ((System.Web.HttpPostedFileBase)value).FileName.Split('.')[1];
                if (Allow.Contains(extension))
                    return ValidationResult.Success;
                else
                    return new ValidationResult(ErrorMessage);
            }
            else
                return ValidationResult.Success;
        }
    }
}
