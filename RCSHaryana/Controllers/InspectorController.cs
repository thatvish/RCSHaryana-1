using Microsoft.Reporting.WebForms;
using RCSData;
using RCSEntities;
using RCSSerivce;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RCSHaryana.Controllers
{
    public class InspectorController : Controller
    {
        // GET: Inspector
        InspectorData objID = new InspectorData();
        SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
        CS4HJ obj = new CS4HJ();

        private string GenerateHashKeyForCheckBroswerEveryCall()
        {
            StringBuilder myStr = new StringBuilder();
            myStr.Append(Request.Browser.Browser);
            myStr.Append(Request.Browser.Platform);
            myStr.Append(Request.Browser.MajorVersion);
            myStr.Append(Request.Browser.MinorVersion);
            myStr.Append(Request.LogonUserIdentity.User.Value);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashdata = sha.ComputeHash(Encoding.UTF8.GetBytes(myStr.ToString()));
            return Convert.ToBase64String(hashdata);
        }

        [RBAC]
        public ActionResult Dashboard()
        {
            if (Convert.ToString(Session["BrowserId"]) != GenerateHashKeyForCheckBroswerEveryCall())
            {
                return RedirectToAction("Login", "Account");
            }
            //int i = obj.CheckSessionEveryCall();
            //if (i != 0)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            InspectorSocietyStatusModels objISSM1 = new InspectorSocietyStatusModels();
            objISSM1 = objID.BackLogGetInspectorApplicatonCountDetails(Convert.ToInt32(Session["InsceptorCode"]));
            ViewBag.TotalBackLog = objISSM1.Total;
            InspectorSocietyStatusModels objISSM = new InspectorSocietyStatusModels();
            objISSM = objID.GetInspectorApplicatonCountDetails(Convert.ToInt32(Session["InsceptorCode"]));
            ViewBag.Total = objISSM.Total;
            ViewBag.TotalApprove = objISSM.TotalApprove;
            ViewBag.TotalPending = objISSM.TotalPending;
            return View();
        }

        public JsonResult InspectorDashBoardSocietyList()
        {
            return Json(objID.InspectorDashBoardSocietyList(Convert.ToInt32(Session["InsceptorCode"])), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SocietyMemberRemarkInspector(string SocietyTransID)
        {
            ARCSData objARCD = new ARCSData();
            Session["SocietyTransID"] = SocietyTransID;
            if (!string.IsNullOrEmpty(SocietyTransID))
            {
                return Json(objARCD.GetDetailsOfArcsRemark(SocietyTransID), JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> GetSocietyList()
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetSocietyList(Convert.ToInt32(Session["InsceptorCode"])))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SocietyName),
                    Value = Convert.ToString(item.SocietyTransId)
                });
            }
            return items;
        }

        public ActionResult SocietyMemberDetails()
        {
            if (Convert.ToString(Session["BrowserId"]) != GenerateHashKeyForCheckBroswerEveryCall())
            {
                return RedirectToAction("Login", "Account");
            }
            //int i = obj.CheckSessionEveryCall();
            //if (i != 0)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            List<SelectListItem> lstSL = new List<SelectListItem>();
            List<SelectListItem> lstI = new List<SelectListItem>();
            GetBasicInfo objGBI = new GetBasicInfo();
            lstSL = GetSocietyList();
            ViewBag.lstSL = lstSL;
            ViewBag.lstI = lstI;
            return View();
        }

        public class AttachmentType
        {
            public string MimeType { get; set; }
            public string FriendlyName { get; set; }
            public string Extension { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RBAC]
        public ActionResult SocietyMemberDetails(FormCollection fc, IEnumerable<HttpPostedFileBase> files)
        {
            ByeLawsModel objBL = new ByeLawsModel();
            ContentFileUploadModel objCFU = new ContentFileUploadModel();
            try
            {
                int i = 4;
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        AttachmentType aa = new AttachmentType();
                        Stream str = file.InputStream;
                        objCFU.File_Name = file.FileName;
                        objBL.ByeLawsName = file.FileName;
                        string extenstion = "pdf";
                        BinaryReader Br = new BinaryReader(str);
                        byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                        string base64ImageRepresentation = Convert.ToBase64String(FileDet);
                        objBL.Docs = base64ImageRepresentation;
                        objCFU.ContentUpload = base64ImageRepresentation;
                        objBL.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
                        objCFU.USER_ID = Convert.ToInt32(Session["UserId"]);
                        objCFU.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
                        aa = GetMimeType(base64ImageRepresentation);
                        objCFU.FormId = i;
                        if (extenstion.IndexOf(aa.Extension.Replace(".", "")) < 0 || aa.Extension == "")
                        {
                        }
                        else
                        {
                            Int64 filesizeallowedfor_FileUpload = 2048 * 5;
                            if (file.ContentLength < filesizeallowedfor_FileUpload)
                            {
                                int a = objSSF.SaveContentFileUpload(objCFU);
                            }
                        }
                    }
                    i = i + 1;
                }
                List<SelectListItem> lstSL = new List<SelectListItem>();
                List<SelectListItem> lstI = new List<SelectListItem>();
                InspectorData objID = new InspectorData();
                ForwardToARCSOfficers objFTAO = new ForwardToARCSOfficers
                {
                    IncepectorCode = Convert.ToInt32(Session["InsceptorCode"]),
                    SocietyTransId = Convert.ToString(Session["SocietyTransID"]),
                    Remarks = fc.Get("ARCSRemarks").ToString()
                };
                int result = objID.ForwardToARCS(objFTAO);
                if (result >= 1)
                {
                    ViewBag.show = "Application successfully forwarded to incepector";
                    ViewBag.result = "1";
                    ViewBag.tabResult = "1";
                }
                else
                {
                    ViewBag.show = "Application not successfully forwarded to incepector";
                    ViewBag.result = "0";
                    ViewBag.tabResult = "0";
                }
                lstSL = GetSocietyList();
                ViewBag.lstSL = lstSL;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Unauthorised");
                throw ex;
            }
            return View();
        }

        #region FormF
        public ActionResult FormF()
        {
            if (Convert.ToString(Session["BrowserId"]) != GenerateHashKeyForCheckBroswerEveryCall())
            {
                return RedirectToAction("Login", "Account");
            }
            //int i = obj.CheckSessionEveryCall();
            //if (i != 0)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            FormFmodel objFFM = new FormFmodel();
            FormAmodels objFAM = new FormAmodels();
            FormGmodel objFGM = new FormGmodel();
            FormGmodel objFF2 = new FormGmodel();
            objFF2 = objSSF.GetDetailsForInspectorFormG(Convert.ToString(Session["SocietyTransID"]));
            objFGM = objSSF.GetDetailsForInspectorFormG2(Convert.ToString(Session["SocietyTransID"]));
            objFFM = objSSF.GetDetailsForFormF(Convert.ToString(Session["SocietyTransID"]));
            objFAM = objSSF.GetDetailsForFormA(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.ARCSName = objFAM.ARCSName;
            ViewBag.Address1 = objFAM.Address1;
            ViewBag.Address2 = objFAM.Address2;
            ViewBag.District = objFAM.District;
            ViewBag.DateOfSubmittionByInspector = Convert.ToDateTime(objFF2.DateOfSubmittionByInspector).ToShortDateString();
            ViewBag.SocietyName = objFAM.SocietyName;
            ViewBag.Pin = objFAM.Pin;
            ViewBag.PostOffice = objFAM.PostOffice;
            ViewBag.InspectName = objFFM.InspectName;
            ViewBag.Remarks = objFGM.Remarks;
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Inspector/FormF.cshtml");
            }
            return View();
        }
        #endregion

        #region FormG
        public List<SelectListItem> GetSocietyListnew()
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetSocietyList(Convert.ToInt32(Session["ARCSCode"])))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SocietyName),
                    Value = Convert.ToString(item.SocietyTransId)
                });
            }
            return items;
        }
        public ActionResult ViewFormDocs()
        {
            if (Convert.ToString(Session["BrowserId"]) != GenerateHashKeyForCheckBroswerEveryCall())
            {
                return RedirectToAction("Login", "Account");
            }
            //int i = obj.CheckSessionEveryCall();
            //if (i != 0)
            //{
            //    return RedirectToAction("Login", "Account");
            //}           
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Shared/_ViewDocsForOfficers.cshtml");
            }
            else
            {
                return View();
            }
        }

        public ActionResult FormG()
        {
            if (Convert.ToString(Session["BrowserId"]) != GenerateHashKeyForCheckBroswerEveryCall())
            {
                return RedirectToAction("Login", "Account");
            }
            //int i = obj.CheckSessionEveryCall();
            //if (i != 0)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            FormGmodel objFGM = new FormGmodel();
            FormGmodel objFGM2 = new FormGmodel();
            objFGM = objSSF.GetDetailsForInspectorFormG2(Convert.ToString(Session["SocietyTransID"]));
            objFGM2 = objSSF.GetDetailsForInspectorFormG(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.DateOfSubmittionByInspector = objFGM2.DateOfSubmittionByInspector;
            ViewBag.SocietyFullAddress = objFGM2.SocietyFullAddress;
            ViewBag.MainObjectsOfProposedSociety = objFGM2.MainObjectsOfProposedSociety;
            ViewBag.ArscAddress = objFGM2.ArscAddress;
            ViewBag.AdoptedModalByeLaws = objFGM.AdoptedModalByeLaws;
            ViewBag.AdoptedModalByeLaws_Details = objFGM.AdoptedModalByeLaws_Details;
            ViewBag.AdverselyEffectOthers = objFGM.AdverselyEffectOthers;
            ViewBag.AreaPopulation = objFGM.AreaPopulation;
            ViewBag.ConcludingRemark = objFGM.ConcludingRemark;
            ViewBag.CorrectnessAllRespects = objFGM.CorrectnessAllRespects;
            ViewBag.DeviationsModalByeLaws = objFGM.DeviationsModalByeLaws;
            ViewBag.DeviationsModalByeLaws_Details = objFGM.DeviationsModalByeLaws_Details;
            ViewBag.ExplanationToPromoters = objFGM.ExplanationToPromoters;
            ViewBag.MainObjectsOfProposedSociety = objFGM.MainObjectsOfProposedSociety;
            ViewBag.ModeOfApplicationReceived = objFGM.ModeOfApplicationReceived;
            ViewBag.NameOfSocietyProposed_Suitable = objFGM.NameOfSocietyProposed_Suitable;
            ViewBag.NoOfPromoters = objFGM.NoOfPromoters;
            ViewBag.NumberAndPaidUpValue = objFGM.NumberAndPaidUpValue;
            ViewBag.ObjectConsonanceWithCooperativePrinciples = objFGM.ObjectConsonanceWithCooperativePrinciples;
            ViewBag.ObjectConsonanceWithCooperativePrinciples_Details = objFGM.ObjectConsonanceWithCooperativePrinciples_Details;
            ViewBag.OtherCoopSocietyWithSameObjects = objFGM.OtherCoopSocietyWithSameObjects;
            ViewBag.OtherCoopSocietyWithSameObjects_Details = objFGM.OtherCoopSocietyWithSameObjects_Details;
            ViewBag.PromoterMembers_CommonInterest = objFGM.PromoterMembers_CommonInterest;
            ViewBag.PromoterMembers_CommonInterest_Details = objFGM.PromoterMembers_CommonInterest_Details;
            ViewBag.QualificationsMembership = objFGM.QualificationsMembership;
            ViewBag.QualificationsMembership_Details = objFGM.QualificationsMembership_Details;
            ViewBag.ReasonForNotJoining = objFGM.ReasonForNotJoining;
            ViewBag.ShareInKind = objFGM.ShareInKind;
            ViewBag.SocietyOrganizedByOwnInitiative = objFGM.SocietyOrganizedByOwnInitiative;
            ViewBag.SocietyOrganizedUnderProjectScheme = objFGM.SocietyOrganizedUnderProjectScheme;
            ViewBag.SocietyOrganizedUnderProjectScheme_Details = objFGM.SocietyOrganizedUnderProjectScheme_Details;
            ViewBag.VerifiedFormB = objFGM.VerifiedFormB;
            ViewBag.AreaOfOperation = objFGM.AreaOfOperation;
            ViewBag.DateOfSubmittionByInspector = objFGM2.DateOfSubmittionByInspector;
            ViewBag.Remarks = objFGM.Remarks;
            List<SelectListItem> lstSL = new List<SelectListItem>();
            lstSL = GetSocietyList();
            ViewBag.lstSL = lstSL;
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Inspector/FormG.cshtml");
            }
            else
            {
                return View();
            }
        }

        public ActionResult DownloadFormG()
        {
            if (Convert.ToString(Session["BrowserId"]) != GenerateHashKeyForCheckBroswerEveryCall())
            {
                return RedirectToAction("Login", "Account");
            }
            //int i = obj.CheckSessionEveryCall();
            //if (i != 0)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            FormGmodel objFGM = new FormGmodel();
            FormGmodel objFGM2 = new FormGmodel();
            objFGM = objSSF.GetDetailsForInspectorFormG2(Convert.ToString(Session["SocietyTransID"]));
            objFGM2 = objSSF.GetDetailsForInspectorFormG(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.DateOfSubmittionByInspector = objFGM2.DateOfSubmittionByInspector;
            ViewBag.SocietyFullAddress = objFGM2.SocietyFullAddress;
            ViewBag.MainObjectsOfProposedSociety = objFGM2.MainObjectsOfProposedSociety;
            ViewBag.ArscAddress = objFGM2.ArscAddress;
            ViewBag.AdoptedModalByeLaws = (Convert.ToBoolean(objFGM.AdoptedModalByeLaws) == true) ? "Yes" : "No";
            ViewBag.AdoptedModalByeLaws_Details = objFGM.AdoptedModalByeLaws_Details;
            ViewBag.AdverselyEffectOthers = objFGM.AdverselyEffectOthers;
            ViewBag.AreaPopulation = objFGM.AreaPopulation;
            ViewBag.ConcludingRemark = objFGM.ConcludingRemark;
            ViewBag.CorrectnessAllRespects = objFGM.CorrectnessAllRespects;
            ViewBag.DeviationsModalByeLaws = objFGM.DeviationsModalByeLaws;
            ViewBag.DeviationsModalByeLaws_Details = objFGM.DeviationsModalByeLaws_Details;
            ViewBag.ExplanationToPromoters = objFGM.ExplanationToPromoters;
            ViewBag.MainObjectsOfProposedSociety = objFGM.MainObjectsOfProposedSociety;
            ViewBag.ModeOfApplicationReceived = objFGM.ModeOfApplicationReceived;
            ViewBag.NameOfSocietyProposed_Suitable = (Convert.ToBoolean(objFGM.NameOfSocietyProposed_Suitable) == true) ? "Yes" : "No";
            ViewBag.NoOfPromoters = objFGM.NoOfPromoters;
            ViewBag.NumberAndPaidUpValue = objFGM.NumberAndPaidUpValue;
            ViewBag.ObjectConsonanceWithCooperativePrinciples = (Convert.ToBoolean(objFGM.ObjectConsonanceWithCooperativePrinciples) == true) ? "Yes" : "No";
            ViewBag.ObjectConsonanceWithCooperativePrinciples_Details = objFGM.ObjectConsonanceWithCooperativePrinciples_Details;
            ViewBag.OtherCoopSocietyWithSameObjects = (Convert.ToBoolean(objFGM.OtherCoopSocietyWithSameObjects) == true) ? "Yes" : "No";
            ViewBag.OtherCoopSocietyWithSameObjects_Details = objFGM.OtherCoopSocietyWithSameObjects_Details;
            ViewBag.PromoterMembers_CommonInterest = (Convert.ToBoolean(objFGM.PromoterMembers_CommonInterest) == true) ? "Yes" : "No";
            ViewBag.PromoterMembers_CommonInterest_Details = objFGM.PromoterMembers_CommonInterest_Details;
            ViewBag.QualificationsMembership = (Convert.ToBoolean(objFGM.QualificationsMembership) == true) ? "Yes" : "No";
            ViewBag.QualificationsMembership_Details = objFGM.QualificationsMembership_Details;
            ViewBag.ReasonForNotJoining = objFGM.ReasonForNotJoining;
            ViewBag.ShareInKind = objFGM.ShareInKind;
            ViewBag.SocietyOrganizedByOwnInitiative = objFGM.SocietyOrganizedByOwnInitiative;
            ViewBag.SocietyOrganizedUnderProjectScheme = (Convert.ToBoolean(objFGM.SocietyOrganizedUnderProjectScheme) == true) ? "Yes" : "No";
            ViewBag.SocietyOrganizedUnderProjectScheme_Details = objFGM.SocietyOrganizedUnderProjectScheme_Details;
            ViewBag.VerifiedFormB = objFGM.VerifiedFormB;
            ViewBag.AreaOfOperation = objFGM.AreaOfOperation;
            ViewBag.DateOfSubmittionByInspector = objFGM2.DateOfSubmittionByInspector;
            ViewBag.Remarks = objFGM.Remarks;
            ViewBag.DateOfSubmissionFormG = (Convert.ToBoolean(objFGM.DateOfSubmissionFormG == null) ? null : Convert.ToDateTime(objFGM.DateOfSubmissionFormG).ToShortDateString());
            List<SelectListItem> lstSL = new List<SelectListItem>();
            lstSL = GetSocietyList();
            ViewBag.lstSL = lstSL;
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Inspector/DownloadFormG.cshtml");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult FormG(FormGmodel objFGM)
        {
            try
            {
                int i = 0;
                objFGM.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
                if (objFGM.DateOfSubmittionByInspector == null)
                {
                    objFGM.DateOfSubmittionByInspector = DateTime.Today;

                }
                if (!string.IsNullOrEmpty(objFGM.SocietyTransID))
                {
                    objFGM.DateOfSubmittionByInspector = Convert.ToDateTime(objFGM.DateOfSubmittionByInspector);
                    objFGM.ModeOfApplicationReceived = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.ModeOfApplicationReceived);
                    objFGM.SocietyName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.SocietyName);
                    objFGM.SocietyOrganizedByOwnInitiative = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.SocietyOrganizedByOwnInitiative);
                    objFGM.NameOfSocietyProposed_Suitable = Convert.ToBoolean(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(Convert.ToString(objFGM.NameOfSocietyProposed_Suitable)));
                    objFGM.NameOfSocietyProposed_Suitable_Detail = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.NameOfSocietyProposed_Suitable_Detail);
                    objFGM.SocietyOrganizedUnderProjectScheme = Convert.ToBoolean(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(Convert.ToString(objFGM.SocietyOrganizedUnderProjectScheme)));
                    objFGM.SocietyOrganizedUnderProjectScheme_Details = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.SocietyOrganizedUnderProjectScheme_Details);
                    objFGM.MainObjectsOfProposedSociety = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.MainObjectsOfProposedSociety);
                    objFGM.ObjectConsonanceWithCooperativePrinciples = Convert.ToBoolean(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(Convert.ToString(objFGM.ObjectConsonanceWithCooperativePrinciples)));
                    objFGM.ObjectConsonanceWithCooperativePrinciples_Details = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.ObjectConsonanceWithCooperativePrinciples_Details);
                    objFGM.AdoptedModalByeLaws = Convert.ToBoolean(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(Convert.ToString(objFGM.AdoptedModalByeLaws)));
                    objFGM.AdoptedModalByeLaws_Details = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.AdoptedModalByeLaws_Details);
                    objFGM.DeviationsModalByeLaws_Details = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.DeviationsModalByeLaws_Details);
                    objFGM.QualificationsMembership = Convert.ToBoolean(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(Convert.ToString(objFGM.QualificationsMembership)));
                    objFGM.QualificationsMembership_Details = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.QualificationsMembership_Details);
                    objFGM.PromoterMembers_CommonInterest = Convert.ToBoolean(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(Convert.ToString(objFGM.PromoterMembers_CommonInterest)));
                    objFGM.PromoterMembers_CommonInterest_Details = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.PromoterMembers_CommonInterest_Details);
                    objFGM.VerifiedFormB = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.VerifiedFormB);
                    objFGM.CorrectnessAllRespects = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.CorrectnessAllRespects);
                    objFGM.NoOfPromoters = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.NoOfPromoters);
                    objFGM.AreaOfOperation = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.AreaOfOperation);
                    objFGM.AreaPopulation = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.AreaPopulation);
                    objFGM.NumberAndPaidUpValue = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.NumberAndPaidUpValue);
                    objFGM.ShareInKind = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.ShareInKind);
                    objFGM.ExplanationToPromoters = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.ExplanationToPromoters);
                    objFGM.OtherCoopSocietyWithSameObjects = Convert.ToBoolean(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(Convert.ToString(objFGM.OtherCoopSocietyWithSameObjects)));
                    objFGM.OtherCoopSocietyWithSameObjects_Details = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.OtherCoopSocietyWithSameObjects_Details);
                    objFGM.ReasonForNotJoining = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.ReasonForNotJoining);
                    objFGM.AdverselyEffectOthers = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.AdverselyEffectOthers);
                    objFGM.ConcludingRemark = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.ConcludingRemark);
                    objFGM.Remarks = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objFGM.Remarks);
                    i = objSSF.SaveDetailsForFormG(objFGM);
                    return Json(i, JsonRequestBehavior.AllowGet);
                }
                i = 0;
                return Json(i, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Unauthorised");
                throw ex;
            }
        }

        protected AttachmentType GetMimeType(string value)
        {
            if (String.IsNullOrEmpty(value))
                return new AttachmentType
                {
                    FriendlyName = "Unknown",
                    MimeType = "application/octet-stream",
                    Extension = ""
                };

            var data = value.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return new AttachmentType
                    {
                        FriendlyName = "Photo",
                        MimeType = "image/png",
                        Extension = ".png"
                    };
                case "/9J/4":
                    return new AttachmentType
                    {
                        FriendlyName = "Photo",
                        MimeType = "image/jpeg",
                        Extension = ".jpeg"
                    };

                case "AAAAF":
                    return new AttachmentType
                    {
                        FriendlyName = "Video",
                        MimeType = "video/mp4",
                        Extension = ".mp4"
                    };
                case "JVBER":
                    return new AttachmentType
                    {
                        FriendlyName = "Document",
                        MimeType = "application/pdf",
                        Extension = ".pdf"
                    };
                case "UESDB":
                    return new AttachmentType
                    {
                        FriendlyName = "kmz",
                        MimeType = "application/vnd.google-earth.kmz",
                        Extension = ".kmz"
                    };
                case "PD94B":
                    return new AttachmentType
                    {
                        FriendlyName = "Kml",
                        MimeType = "application/vnd.google-earth.kml+xml",
                        Extension = ".kml"
                    };
                case "QUMXM":
                    return new AttachmentType
                    {
                        FriendlyName = "dwg",
                        MimeType = "application/acad, application/x-acad, application/autocad_dwg, image/x-dwg, application/dwg, application/x-dwg, application/x-autocad, image/vnd.dwg, drawing/dwg",
                        Extension = ".dwg"
                    };
                default:
                    return new AttachmentType
                    {
                        FriendlyName = "Unknown",
                        MimeType = string.Empty,
                        Extension = ""
                    };
            }
        }
        #endregion

        #region BackLog

        #endregion
        public ActionResult Report(string id, string value)
        {
            string path = "";
            LocalReport lr = new LocalReport();
            if (value == "FormF")
            {
                path = Path.Combine(Server.MapPath("~/Reports/FormF.rdlc"));
            }
            if (value == "FormFNegative")
            {
                path = Path.Combine(Server.MapPath("~/Reports/FormFNegative.rdlc"));
            }
            if (value == "FormG")
            {
                path = Path.Combine(Server.MapPath("~/Reports/FormG.rdlc"));
            }
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return RedirectToAction("LogIn", "Account");
            }
            if (value == "FormF" || value == "FormFNegative")
            {
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();
                ds1 = objSSF.ReportFormF(Convert.ToString(Session["SocietyTransID"]));
                ds2 = objSSF.ReportFormF2(Convert.ToString(Session["SocietyTransID"]));
                ds3 = objSSF.ReportFormF3(Convert.ToString(Session["SocietyTransID"]));
                DataTable FormF = ds1.Tables[0];
                DataTable FormF2 = ds2.Tables[0];
                DataTable FormF3 = ds3.Tables[0];
                ReportDataSource rd = new ReportDataSource("GetDetailsForFormF", FormF);
                ReportDataSource rd2 = new ReportDataSource("GetDetailsForFormC", FormF2);
                ReportDataSource rd3 = new ReportDataSource("GetInspectorInfoFormG", FormF3);
                lr.DataSources.Add(rd);
                lr.DataSources.Add(rd2);
                lr.DataSources.Add(rd3);
            }
            if (value == "FormG")
            {
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();
                ds1 = objSSF.ReportFormG(Convert.ToString(Session["SocietyTransID"]));
                ds2 = objSSF.ReportFormG2(Convert.ToString(Session["SocietyTransID"]));
                ds3 = objSSF.ReportFormG3(Convert.ToString(Session["SocietyTransID"]));
                DataTable FormG = ds1.Tables[0];                            
                DataTable FormG2 = ds2.Tables[0];
                DataTable FormG3 = ds3.Tables[0];
                ReportDataSource rd = new ReportDataSource("GetInspectorInfoFormG", FormG);
                ReportDataSource rd2 = new ReportDataSource("GetInspectorInfoFormG2", FormG2);
                ReportDataSource rd3 = new ReportDataSource("GetDetailsForFormF", FormG3);
                lr.DataSources.Add(rd);
                lr.DataSources.Add(rd2);
                lr.DataSources.Add(rd3);
            }         
            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }
    }
}