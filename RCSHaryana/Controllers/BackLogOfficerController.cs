using RCSData;
using RCSEntities;
using RCSSerivce;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RCSHaryana.Controllers
{
    public class BackLogOfficerController : Controller
    {
        #region BackLog Section 
        ARCSData objARCSD = new ARCSData();
        CS4HJ obj = new CS4HJ();
        InspectorData objID = new InspectorData();
        BackLogData objBdata = new BackLogData();
        GetBasicInfo objGBI = new GetBasicInfo();
        List<SelectListItem> lstGD = new List<SelectListItem>();
        List<SelectListItem> lstR = new List<SelectListItem>();
        List<SelectListItem> lstMember = new List<SelectListItem>();
        List<SelectListItem> lstMCDCM = new List<SelectListItem>();
        List<SelectListItem> lstARCSCode = new List<SelectListItem>();
        List<SelectListItem> InsPklLst = new List<SelectListItem>();
        List<FormNameList> FormNamelst = new List<FormNameList>();
        List<SelectListItem> CommunityofSociety = new List<SelectListItem>();
        List<SelectListItem> lstSL = new List<SelectListItem>();
        List<SelectListItem> Inspkl = new List<SelectListItem>();
        List<SelectListItem> lstI = new List<SelectListItem>();
        List<SelectListItem> lstSL1 = new List<SelectListItem>();

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
        public List<SelectListItem> GetCommunityofSociety()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objBdata.GetSubCommunityofSociety())
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.CommunitySocietyName),
                    Value = Convert.ToString(item.CommunitySocietyId)
                });
            }
            return items;
        }
        #region Upload Section
        [HttpGet]
        public ActionResult UploadExcel()
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
            ARCSData objARCSD = new ARCSData();
            //var store = ARCSData.GetPassword();
            return View();
        }
        #region backlogDashBarod
        public ActionResult BackLogDashboard()
        {
            ARCSSocietyStatusModels objARCSSM = new ARCSSocietyStatusModels();
            objARCSSM = objARCSD.BackLogGetARCSApplicatonCountDetails(Convert.ToInt32(Session["ARCSCode"]));
            ViewBag.Total = objARCSSM.Total;
            ViewBag.TotalApprove = objARCSSM.TotalApprove;
            ViewBag.TotalHold = objARCSSM.TotalHold;
            ViewBag.TotalPending = objARCSSM.TotalPending;
            ViewBag.TotalReject = objARCSSM.TotalReject;
            ViewBag.TotalForwardedToInspector = objARCSSM.TotalForwardToInspector;
            ViewBag.TotalApplicationComesFromInspector = objARCSSM.TotalApplicationComesFromInspector;
            return View();
        }
        #endregion
        #endregion
        public ActionResult BackLogApplicationDetails()
        {
            return View();
        }
        public ActionResult BackLogToInspector()
        {
            return View();
        }
        public List<SelectListItem> GetAllShareTransferMember()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objBdata.GetAllMember())
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.MemberName),
                    Value = Convert.ToString(item.MemberId)
                });
            }
            return items;
        }
        public List<SelectListItem> GetsubClassSocieties()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetSubClassOfSociety(3))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SocietySubClassName),
                    Value = Convert.ToString(item.SocietySubClassCode)
                });
            }
            return items;
        }
        public List<FormNameList> GetFormNameList()
        {
            List<FormNameList> items = new List<FormNameList>();
            foreach (var item in objBdata.GetFormNameList())
            {
                items.Add(new FormNameList
                {
                    FormName = Convert.ToString(item.FormName),
                    FormId = Convert.ToString(item.FormName)
                });
            }
            return items;
        }
        public List<SelectListItem> GetShareTransferMember()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string SocietyTransId = Convert.ToString(Session["SocietyTransID"]);
            foreach (var item in objBdata.GetMember(SocietyTransId))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.MemberName),
                    Value = Convert.ToString(item.MemberId)
                });
            }
            return items;
        }
        public List<SelectListItem> GetRelationship()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetRelationship())
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.RelationshipName),
                    Value = Convert.ToString(item.RelationshipCode)
                });
            }
            return items;
        }
        public List<SelectListItem> GetDistrict()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetDistrict())
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.DistrictName),
                    Value = Convert.ToString(item.DistrictCode)
                });
            }
            return items;
        }
        public List<SelectListItem> GetSocietyList()
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetSocietyListBackLog(Convert.ToInt32(Session["ARCSCode"])))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SocietyName),
                    Value = Convert.ToString(item.SocietyTransId)
                });
            }
            return items;
        }
        public List<SelectListItem> GetTotalFreezeSocietyList()
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetTotalFreezeSocietyList(Convert.ToInt32(Session["ARCSCode"])))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SocietyName),
                    Value = Convert.ToString(item.SocietyTransId)
                });
            }
            return items;
        }
        public List<SelectListItem> GetInspectorSocietyList()
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetSocietyListBackLog(Convert.ToInt32(Session["InsceptorCode"])))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SocietyName),
                    Value = Convert.ToString(item.SocietyTransId)
                });
            }
            return items;
        }
        public List<SelectListItem> GetSocietyListForApproval()
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetBackLogSocietyListForApproval(Convert.ToInt32(Session["ARCSCode"])))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SocietyName),
                    Value = Convert.ToString(item.SocietyTransId)
                });
            }
            return items;
        }
        public List<SelectListItem> GetInspectorList()
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetInspectorList(Convert.ToInt32(Session["ARCSCode"])))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.InspectorName),
                    Value = Convert.ToString(item.InspectorId)
                });
            }
            return items;
        }
        public ActionResult BackLogMemberDetail()
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
            lstGD = GetDistrict();
            ViewBag.District = lstGD;
            lstR = GetRelationship();
            ViewBag.Relationship = lstR;
            lstMember = GetAllShareTransferMember();
            ViewBag.MemberDetail = lstMember;
            if (!string.IsNullOrEmpty(Convert.ToString(Session["SocietyTransID"])))
            {
                lstMember = GetShareTransferMember();
                ViewBag.MemberDetail = lstMember;
            }
            lstMCDCM = GetMemberCommDesignation();
            ViewBag.MemberCommDesignation = lstMCDCM;
            lstARCSCode = GetsubClassSocieties();
            ViewBag.lstARCSCode = lstARCSCode;
            CommunityofSociety = GetCommunityofSociety();
            ViewBag.CommunityofSociety = CommunityofSociety;
            lstSL = GetSocietyList();
            ViewBag.lstSL = lstSL;
            Inspkl = GetInspectorSocietyList();
            ViewBag.Inspkl = Inspkl;
            lstI = GetInspectorList();
            ViewBag.lstI = lstI;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BackLogMemberDetail(FormCollection fc)
        {
            try
            {
                int result = 0;
                List<SelectListItem> lstSL = new List<SelectListItem>();
                List<SelectListItem> lstI = new List<SelectListItem>();
                ARCSData objARCSD = new ARCSData();
                InspectorData objID = new InspectorData();
                ForwardToIncepector objFTI = new ForwardToIncepector();

                if (Convert.ToInt32(Session["RoleId"]) == 3)
                {
                    string IPAddress = GetIPAddress();
                    string Browser = GetWebBrowserName();
                    ForwardToARCSOfficers objFTAO = new ForwardToARCSOfficers
                    {
                        IncepectorCode = Convert.ToInt32(Session["InsceptorCode"]),
                        SocietyTransId = Convert.ToString(Session["SocietyTransID"]),
                        Remarks = fc.Get("ARCSRemarks").ToString()
                    };
                    if (Convert.ToInt16(Session["InspectorCount"]) >= 1)
                    {
                        Boolean IsCheck = Convert.ToBoolean(fc.Get("Insprctorchk1"));
                        SaveDeclaration savedeclaration = new SaveDeclaration
                        {
                            UserId = Convert.ToInt32(Session["UserId"]),
                            IPAddress = IPAddress,
                            BrowserName = Browser,
                            Ischeck = IsCheck,
                            Remark = objFTAO.Remarks,
                            SocietyTransID = objFTAO.SocietyTransId,
                        };
                        objARCSD.SaveDeclaration(savedeclaration);
                    }
                    result = objID.BackLogForwardToARCS(objFTAO);
                    if (result >= 1)
                    {
                        if (Convert.ToInt32(Session["InspectorCount"]) > 1)
                        {
                            ViewBag.InspectorChange = "Yes";
                        }
                        ViewBag.show = "BackLog Application successfully forwarded to ARCS Officer";
                        ViewBag.result = "1";
                        ViewBag.tabResult = "1";
                    }
                    else
                    {
                        ViewBag.show = "BackLog Application not successfully forwarded to ARCS Officer";
                        ViewBag.result = "0";
                        ViewBag.tabResult = "0";
                    }
                    lstSL = GetSocietyList();
                    ViewBag.lstSL = lstSL;
                }
                if (Convert.ToInt32(Session["RoleId"]) == 2)
                {
                    if (string.IsNullOrEmpty(fc.Get("IncepectorList")))
                    {
                        ViewBag.show = "Kindly Select Inspector";
                        ViewBag.result = "0";
                        ViewBag.tabResult = "5";
                        return View();
                    }
                    if (string.IsNullOrEmpty(fc.Get("ARCSRemarks")))
                    {
                        ViewBag.show = "fill your remarks";
                        ViewBag.result = "0";
                        ViewBag.tabResult = "5";
                        return View();
                    }
                    objFTI.OfficerCode = Convert.ToInt32(fc.Get("IncepectorList"));
                    var SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
                    Session["SocietyTransID"] = SocietyTransID;
                    objFTI.SocietyTransId = Convert.ToString(Session["SocietyTransID"]);
                    objFTI.Remarks = fc.Get("ARCSRemarks").ToString();
                    result = objARCSD.BackLogForwardToIncepector(objFTI);
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
                    lstI = GetInspectorSocietyList();
                    ViewBag.BcklstI = lstI;
                }
                lstGD = GetDistrict();
                ViewBag.District = lstGD;

                lstR = GetRelationship();
                ViewBag.Relationship = lstR;
                lstMember = GetAllShareTransferMember();
                ViewBag.MemberDetail = lstMember;
                lstMCDCM = GetMemberCommDesignation();
                ViewBag.MemberCommDesignation = lstMCDCM;
                lstARCSCode = GetsubClassSocieties();
                ViewBag.lstARCSCode = lstARCSCode;
                CommunityofSociety = GetCommunityofSociety();
                ViewBag.CommunityofSociety = CommunityofSociety;
                lstSL = GetSocietyList();
                ViewBag.lstSL = lstSL;
                Inspkl = GetInspectorSocietyList();
                ViewBag.Inspkl = Inspkl;
                lstI = GetInspectorList();
                ViewBag.lstI = lstI;
            }

            catch (Exception ex)
            {
                return RedirectToAction("Error", "Unauthorised");
                throw ex;
            }
            return View();
        }

        public JsonResult GetBackLogDetails(string SocietyTransID)
        {
            SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
            Session["SocietyTransID"] = SocietyTransID;
            return Json(objARCSD.GetBackLogMemberDetails(SocietyTransID), JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> GetMemberCommDesignation()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetMemberCommDesignation())
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.MemberCommDesignationName),
                    Value = Convert.ToString(item.MemberCommDesignationCode)
                });
            }
            return items;
        }
        public JsonResult ElectionDetails(string SocietyTransID)
        {
            List<SelectListItem> lstMCDCM = new List<SelectListItem>();
            lstMCDCM = GetMemberCommDesignation();
            var GetElectionDate = objBdata.GetElectionDate(SocietyTransID);
            ViewBag.GetElectionDate = GetElectionDate;
            ViewBag.MemberCommDesignation = lstMCDCM;
            return Json(GetElectionDate, JsonRequestBehavior.AllowGet);

        }
        public string GetIPAddress()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            return ipAddress;
        }
        private string GetWebBrowserName()
        {
            StringBuilder myStr = new StringBuilder();
            myStr.Append(Request.Browser.Browser);
            myStr.Append("-");
            myStr.Append(Request.Browser.Platform);
            myStr.Append("-");
            myStr.Append(Request.Browser.MajorVersion);
            myStr.Append("-");
            myStr.Append(Request.Browser.MinorVersion);
            //myStr.Append(Request.LogonUserIdentity.User.Value);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashdata = sha.ComputeHash(Encoding.UTF8.GetBytes(myStr.ToString()));
            return myStr.ToString();
        }
        public JsonResult SaveElectionDate(BacklogElectionDate objdata)
        {
            BacklogElectionDate objele = new BacklogElectionDate
            {
                SocietyTransId = Convert.ToString(Session["SocietyTransID"]),
                IPAddress = GetIPAddress(),
                BrowserName = GetWebBrowserName(),
                Updatedby = Convert.ToInt32(Session["UserId"]),
                ElectionDate = objdata.ElectionDate
            };
            return Json(objBdata.SaveElectionDate(objele), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShareMembersList()
        {
            var SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            if (!string.IsNullOrEmpty(SocietyTransID))
            {
                var getRecord = objBdata.NewSocietyMembersList(SocietyTransID);
                Session["AddedMember"] = getRecord.Count;
                var TotalCount = Convert.ToInt16(Session["NoOfMembers"]);
                var result = new { getRecord, TotalCount };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            Session["AddedMember"] = 0;
            return Json(JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyShareTransferMemberID(int ShareTransferID)
        {
            var SocietyMemberDetail = objBdata.GetbyShareMemberID(ShareTransferID);
            return Json(SocietyMemberDetail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BackLogPendingAction()
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
            List<SelectListItem> lstI = new List<SelectListItem>();
            obj.CreatSession();
            lstSL1 = GetSocietyListForApproval();
            ViewBag.lstSL1 = lstSL1;
            lstGD = GetDistrict();
            ViewBag.District = lstGD;
            lstR = GetRelationship();
            ViewBag.Relationship = lstR;
            lstMember = GetAllShareTransferMember();
            ViewBag.MemberDetail = lstMember;
            lstMCDCM = GetMemberCommDesignation();
            ViewBag.MemberCommDesignation = lstMCDCM;
            lstARCSCode = GetsubClassSocieties();
            FormNamelst = GetFormNameList();
            ViewBag.lstARCSCode = lstARCSCode;
            ViewBag.FormNamelst = FormNamelst;
            InsPklLst = GetInspectorList();
            ViewBag.InsPklLst = InsPklLst;
            CommunityofSociety = GetCommunityofSociety();
            ViewBag.CommunityofSociety = CommunityofSociety;
            lstSL = GetSocietyList();
            ViewBag.lstSL = lstSL;
            Inspkl = GetInspectorSocietyList();
            ViewBag.Inspkl = Inspkl;
            lstI = GetInspectorList();
            ViewBag.lstI = lstI;
            return View();
        }
        [HttpPost]
        public ActionResult BackLogPendingAction(FormCollection fc)
        {
            List<SelectListItem> lstSL = new List<SelectListItem>();
            List<SelectListItem> lstI = new List<SelectListItem>();
            List<SelectListItem> ttlfreeze = new List<SelectListItem>();
            ForwardToIncepector objFTI = new ForwardToIncepector();
            if (string.IsNullOrEmpty(fc.Get("ARCSRemarks")))
            {
                ViewBag.show = "fill your remarks";
                ViewBag.result = "0";
                ViewBag.tabResult = "5";
                return View();
            }
            objFTI.Status = 12;
            objFTI.OfficerCode = Convert.ToInt32(Session["ARCSCode"]);
            objFTI.SocietyTransId = Convert.ToString(Session["SocietyTransID"]);
            objFTI.Remarks = fc.Get("ARCSRemarks").ToString();
            int result = objARCSD.BackLogOfficerHearingAndApprovalStatus(objFTI);
            if (result >= 1)
            {
                ViewBag.show = "Application has been Freezed";
                ViewBag.result = "1";
                ViewBag.tabResult = "1";
            }
            else
            {
                ViewBag.show = "Sorry Application isn't Freezed yet.";
                ViewBag.result = "0";
                ViewBag.tabResult = "0";
            }
            lstSL1 = GetSocietyListForApproval();
            ViewBag.lstSL1 = lstSL1;
            lstGD = GetDistrict();
            ViewBag.District = lstGD;
            lstR = GetRelationship();
            ViewBag.Relationship = lstR;
            lstMember = GetAllShareTransferMember();
            ViewBag.MemberDetail = lstMember;
            lstMCDCM = GetMemberCommDesignation();
            ViewBag.MemberCommDesignation = lstMCDCM;
            lstARCSCode = GetsubClassSocieties();
            ViewBag.lstARCSCode = lstARCSCode;
            CommunityofSociety = GetCommunityofSociety();
            ViewBag.CommunityofSociety = CommunityofSociety;
            lstSL = GetSocietyList();
            ViewBag.lstSL = lstSL;
            Inspkl = GetInspectorSocietyList();
            ViewBag.Inspkl = Inspkl;
            lstI = GetInspectorList();
            ViewBag.lstI = lstI;
            return View();
        }
        public ActionResult BackLogDashboardInspector()
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
            InspectorSocietyStatusModels objISSM = new InspectorSocietyStatusModels();
            objISSM = objID.BackLogGetInspectorApplicatonCountDetails(Convert.ToInt32(Session["InsceptorCode"]));
            ViewBag.Total = objISSM.Total;
            ViewBag.TotalApprove = objISSM.TotalApprove;
            ViewBag.TotalPending = objISSM.TotalPending;
            return View();
        }
        public JsonResult BackLogMemberRemarkInspector(string SocietyTransID)
        {
            ARCSData objARCD = new ARCSData();
            Session["SocietyTransID"] = SocietyTransID;
            if (!string.IsNullOrEmpty(SocietyTransID))
            {
                return Json(objARCD.BackLogGetDetailsOfArcsRemark(SocietyTransID), JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        public JsonResult BackLogMemberRemarkARCS(string SocietyTransID)
        {
            ARCSData objARCSD = new ARCSData();
            Session["SocietyTransID"] = SocietyTransID;
            if (!string.IsNullOrEmpty(SocietyTransID))
            {
                return Json(objARCSD.BackLogGetDetailsForRemark(SocietyTransID), JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult TotalFreeze()
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
            lstGD = GetDistrict();
            ViewBag.District = lstGD;
            lstR = GetRelationship();
            ViewBag.Relationship = lstR;
            lstMember = GetAllShareTransferMember();
            ViewBag.MemberDetail = lstMember;
            lstMCDCM = GetMemberCommDesignation();
            ViewBag.MemberCommDesignation = lstMCDCM;
            lstARCSCode = GetsubClassSocieties();
            ViewBag.lstARCSCode = lstARCSCode;
            CommunityofSociety = GetCommunityofSociety();
            ViewBag.CommunityofSociety = CommunityofSociety;
            lstSL = GetTotalFreezeSocietyList();
            ViewBag.lstSL = lstSL;
            Inspkl = GetInspectorSocietyList();
            ViewBag.Inspkl = Inspkl;
            lstI = GetInspectorList();
            ViewBag.lstI = lstI;
            Session["SocietyStatus"] = 12;
            return View();
        }
        #endregion

        #region History
        public JsonResult GetInspectorChange(string SocietyTransID)
        {
            var GetRecord = objBdata.GetInspectorChange(SocietyTransID);
            var GetChecked = false;
            if (Convert.ToInt32(Session["RoleId"]) == 2)
            {
                GetChecked = objBdata.GetDeclaration(SocietyTransID, Convert.ToInt32(Session["UserId"]));
            }
            var GetCount = GetRecord.Count;
            Session["InspectorCount"] = GetCount;

            int RoleId = Convert.ToInt32(Session["RoleId"]);
            var result = new { RoleId, GetCount, GetChecked };
            if (Convert.ToInt32(Session["RoleId"]) == 3)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
                //return Json(result,JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindHistoryView(string SocietyTransID)
        {
            return Json(objBdata.GetHistoryForOneEntity(SocietyTransID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDeclaration(int IsCheck, string Remark, string SocietyTransID)
        {
            int i = 0;
            bool IsChecked = false;
            string IPAddress = GetIPAddress();
            string BrowserName = GetWebBrowserName();
            if (IsCheck == 1)
            {
                IsChecked = true;
            }
            if (IsCheck == 0)
            {
                IsChecked = false;
            }
            SaveDeclaration savedeclaration = new SaveDeclaration
            {
                UserId = Convert.ToInt32(Session["UserId"]),
                IPAddress = IPAddress,
                BrowserName = BrowserName,
                Ischeck = IsChecked,
                Remark = Remark,
                SocietyTransID = SocietyTransID,
            };
            i = objARCSD.SaveDeclaration(savedeclaration);
            if (i >= 1)
            {
                return Json(i.ToString(), JsonRequestBehavior.AllowGet);
            }
            return Json("Something went wrong", JsonRequestBehavior.AllowGet);
        }
        public ActionResult BackLogHistory()
        {
            FormNamelst = GetFormNameList();
            ViewBag.lstARCSCode = lstARCSCode;
            ViewBag.FormNamelst = FormNamelst;
            InsPklLst = GetInspectorList();
            ViewBag.InsPklLst = InsPklLst;
            return View();
        }

        [HttpPost]
        public ActionResult BackLogHistory(FormCollection fc)
        {
            FormNamelst = GetFormNameList();
            ViewBag.lstARCSCode = lstARCSCode;
            ViewBag.FormNamelst = FormNamelst;
            InsPklLst = GetInspectorList();
            ViewBag.InsPklLst = InsPklLst;

            ParamForHistory objPFH = new ParamForHistory
            {
                ARCSCode = Convert.ToInt32(Session["ARCSCode"]),
                InspectorCode = Convert.ToInt16(fc.Get("InspectorCode")),
                Formname = fc.Get("FormNameId")
            };

            ViewBag.BackLogHistory = objBdata.GetHistoryForOfficer(objPFH);
            return View();
        }

        #endregion

        //private byte[] StreamPDFToByte()
        //{
        //    string filename = "C:\\sample.pdf";
        //    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

        //    // Create a byte array of file stream length
        //    byte[] byteData = new byte[fs.Length];

        //    //Read block of bytes from stream into the byte array
        //    fs.Read(byteData, 0, System.Convert.ToInt32(fs.Length));

        //    //Close the File Stream
        //    fs.Close();
        //    return byteData; //return the byte data
        //}

        //public string GetPdf()
        //{
        //    SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
        //    byte[] fileStream = new byte[0];
        //    fileStream = objSSF.GetPdfByte();
        //    StreamByteToPDF(fileStream);
        //    return "hello";
        //}


        // private FileStreamResult StreamByteToPDF(byte[] byteData)
        //    {
        //    string filename = "D:\\Newsample.pdf";
        //    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

        //    //Read block of bytes from stream into the byte array
        //    fs.Read(byteData, 0, byteData.Length);
        //    var fsResult = new FileStreamResult(fs, "application/pdf");
        //    //PdfWriter.GetInstance(myDocument, new FileStream("mydocument.pdf", FileMode.Create));
        //    //myDocument.Open();
        //    //myDocument.Add(new Paragraph(Encoding.UTF8.GetString(bytes)));
        //    //myDocument.Close()
        //    //Close the File Stream
        //    fs.Close();
        //    return fsResult;
        //}
        //public ActionResult GetPdf(string fileName)
        //{
        //    SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
        //    byte[] fileStream = new byte[0];

        //    string filename = "D:\\Newsample.pdf";
        //    //fileStream = objSSF.GetPdfByte();
        //    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
        //    //byte[] data = Encoding.UTF8.GetBytes(objSSF.Base64Decode());           
        //    //byte[] bytes = Convert.FromBase64String(b64);
        //    //fs.Read(bytes, 0, bytes.Length);
        //    var fsResult = new FileStreamResult(fs, "application/pdf");
        //    //System.IO.File.WriteAllBytes("D:\\bbb.pdf", bytes);            
        //    System.IO.File.ReadAllBytes("D:\\bbb.pdf");
        //    fs.Read(fileStream, 0, fileStream.Length);                
        //    return fsResult;
        //}

    }
}