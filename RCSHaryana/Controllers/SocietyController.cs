using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using RCSData;
using RCSEntities;
using RCSSerivce;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Reporting.WinForms;
using System.Data;

namespace RCSHaryana.Controllers
{
    public class SocietyController : Controller
    {
        #region ClassObjects
        GetBasicInfo objGBI = new GetBasicInfo();
        SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
        CS4HJ obj = new CS4HJ();
        List<SelectListItem> lstMember = new List<SelectListItem>();

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

        public class AttachmentType
        {
            public string MimeType { get; set; }
            public string FriendlyName { get; set; }
            public string Extension { get; set; }
        }
        #endregion

        #region GetActionResult
        public JsonResult GetSocietyDetails(string SocietyTransID)
        {
            SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
            Session["SocietyTransID"] = SocietyTransID;

            return Json(objSSF.GetSocietyDetails(Convert.ToInt32(Session["UserId"])), JsonRequestBehavior.AllowGet);
        }
        public List<SelectListItem> GetShareTransferMember()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string SocietyTransId = Convert.ToString(Session["SocietyTransID"]);
            foreach (var item in objSSF.GetMember(SocietyTransId))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.MemberName),
                    Value = Convert.ToString(item.MemberId)
                });
            }
            return items;
        }
        [RBAC]
        public ActionResult Application()
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
           if (TempData["FormC"] != null)
            {
                ViewBag.tabResult = 4;
                ViewBag.result = "1";
                ViewBag.show = "Meeting details save successfully";
            }
            if (TempData["procedurefee"] != null)
            {
                ViewBag.tabResult = 5;
                ViewBag.result = "1";
                ViewBag.show = "Receipt of Cashier save successully";
            }
            if (TempData["FormE"] != null)
            {
                Session["FormE"] = 1;
                ViewBag.tabResult = 6;
                ViewBag.FormEValue = 1;
                ViewBag.result = "1";
                ViewBag.show = "Receipt of Custodian of Books save successully";
            }
            if (Convert.ToString(TempData["FormC"]) == "2")
            {
                ViewBag.tabResult = 0;
                ViewBag.result = "0";              
                ViewBag.show = "Please Fill Society Details First";
            }
            if (Convert.ToInt16(Session["FormE"]) == 1)
            {
                ViewBag.FormEValue = 1;
            }

            var result1 = objSSF.GetTotalMember(Convert.ToInt32(Session["UserId"]));
            Session["NoOfMembers"] = result1.Totalcount;
            var getRecord = objSSF.NewSocietyMembersList(Convert.ToString(Session["SocietyTransID"]));
            Session["AddedMember"] = getRecord.Count;

            List<SelectListItem> lstGD = new List<SelectListItem>();
            List<SelectListItem> lstGCS = new List<SelectListItem>();
            List<SelectListItem> lstR = new List<SelectListItem>();
            List<SelectListItem> lstO = new List<SelectListItem>();
            List<SelectListItem> lstSL = new List<SelectListItem>();
            List<SelectListItem> lstMember = new List<SelectListItem>();
            lstSL = GetSociety();
            ViewBag.lstSL = lstSL;
            List<SelectListItem> lstMCDCM = new List<SelectListItem>();
            List<SelectListItem> lstARCSCode = new List<SelectListItem>();
            lstARCSCode = GetsubClassSocieties();
            ViewBag.lstARCSCode = lstARCSCode;
            obj.CreatSession();
            lstMember = GetShareTransferMember();
            lstGD = GetDistrict();
            lstGCS = GetClassOfSociety();
            lstO = GetOccupations();
            lstR = GetRelationship();
            ViewBag.MemberDetail = lstMember;
            lstMCDCM = GetMemberCommDesignation();
            ViewBag.District = lstGD;
            ViewBag.DistrictForUser = lstGD;
            ViewBag.CommitteMemberDistrict = lstGD;
            ViewBag.ClassOfSociety = lstGCS;
            ViewBag.Occupations = lstO;
            ViewBag.Relationship = lstR;
            ViewBag.MemberCommDesignation = lstMCDCM;
            if (Convert.ToInt32(Session["SocietyStatus"]) != 0)
            {
                ViewBag.SocietyStatus = Convert.ToInt32(Session["SocietyStatus"]);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Session["SocietyTransID"])) && Convert.ToString(Session["SocietyTransID"]) != "0")
            {
                ViewBag.SocietyTransID = 1;
            }
            return View();
        }
        #endregion

        #region BindDropdownInList
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

        public List<SelectListItem> GetClassOfSociety()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetClassOfSociety())
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SocietyClassName),
                    Value = Convert.ToString(item.SocietyClassCode)
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

        public List<SelectListItem> GetOccupations()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetOccupations())
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.OccupationName),
                    Value = Convert.ToString(item.OccupationCode)
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

        #region BindDropdonwListByAjaxDropdownSelection
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult BindARCSOffice(string DistrictCode)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetACRS(Convert.ToInt32(DistrictCode)))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.ACRSName),
                    Value = Convert.ToString(item.ACRSCode)
                });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult BindSubClassOfSociety(string SocietySubClassCode)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetSubClassOfSociety(Convert.ToInt32(SocietySubClassCode)))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SocietySubClassName),
                    Value = Convert.ToString(item.SocietySubClassCode)
                });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetSocietyMemberOccupation()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetOccupations())
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.OccupationName),
                    Value = Convert.ToString(item.OccupationCode)
                });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult BindSocietyMemberRelationshipWithNominee()
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
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region ValidateAadharCard
        public string ValidateAadharCard(string input)
        {
            bool Status = Verhoeff.ValidateVerhoeff(input);
            string s = "";
            if (Status == true)
            {
                s = "true";
            }
            return s;
        }
        #endregion

        public List<SelectListItem> GetSociety()
        {
            SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objSSF.GetSocietyForUser(Convert.ToInt32(Session["UserId"])))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SocietyName),
                    Value = Convert.ToString(item.SocietyTransId)
                });
            }
            return items;
        }

        #region PostActionResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Application(FormCollection collection)
        {
            int result = 0;
            List<SelectListItem> lstGD = new List<SelectListItem>();
            List<SelectListItem> lstGCS = new List<SelectListItem>();
            List<SelectListItem> lstMCDCM = new List<SelectListItem>();
            List<SelectListItem> lstARCSCode = new List<SelectListItem>();
            lstARCSCode = GetsubClassSocieties();

            ViewBag.lstARCSCode = lstARCSCode;
            lstGD = GetDistrict();
            lstGCS = GetClassOfSociety();
            lstMCDCM = GetMemberCommDesignation();

            ViewBag.District = lstGD;
            ViewBag.ClassOfSociety = lstGCS;
            List<SelectListItem> lstR = new List<SelectListItem>();
            List<SelectListItem> lstO = new List<SelectListItem>();
            lstR = GetRelationship();
            lstO = GetOccupations();
            lstMember = GetShareTransferMember();
            ViewBag.MemberDetail = lstMember;
            ViewBag.Relationship = lstR;
            ViewBag.Occupations = lstO;
            ViewBag.MemberCommDesignation = lstMCDCM;
            ViewBag.CommitteMemberDistrict = lstGD;
            ViewBag.DistrictForUser = lstGD;
            SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
            SocietySubmissionFromModels objSSFM = new SocietySubmissionFromModels();

            if (Convert.ToInt32(Session["SocietyStatus"]) != 0)
            {
                ViewBag.SocietyStatus = Convert.ToInt32(Session["SocietyStatus"]);
            }

            objSSFM.DivCode = collection.Get("District");
            objSSFM.ARCSCode = collection.Get("ARCSOffice");

            objSSFM.SocietyName = collection.Get("NameofProposedSociety");

            objSSFM.ClassSocietyCode = collection.Get("ClassofSocietyandLiability");
            objSSFM.SubClassSocietyCode = collection.Get("SubClassOfSociety");
            objSSFM.Address1 = collection.Get("RegisteredAddress");
            objSSFM.Address2 = collection.Get("HouseNoSectorNoRoad");
            objSSFM.PostOffice = collection.Get("PostOffice");
            objSSFM.Pin = collection.Get("PostalCode");
            objSSFM.AreaOfOperation = collection.Get("AreaofOperation");
            objSSFM.Mainobject1 = collection.Get("Mainobject1");
            objSSFM.Mainobject2 = collection.Get("Mainobject2");
            objSSFM.Mainobject3 = collection.Get("Mainobject3");
            objSSFM.Mainobject4 = collection.Get("Mainobject4");
            objSSFM.NoOfMembers = Convert.ToInt32(collection.Get("Noofmemberspresent"));
            //UserInfo.CitizenInfo.NoOfMembers = objSSFM.NoOfMembers;
            objSSFM.CateOfSociety = collection.Get("Categoryofsociety");
            objSSFM.DebtsOfMembers = collection.Get("Estimatedunsecureddebtsofmembers");
            objSSFM.AreaMortgaged = collection.Get("AreaMortgagedbymembers");
            objSSFM.DetailsOfShares = collection.Get("detailsofshares");
            objSSFM.ValueOfShare = collection.Get("Valueofashare");
            objSSFM.ModeOfPayment = collection.Get("ModeofPayment");
            objSSFM.CorrespondenceAddress = collection.Get("ModeofPayment");
            objSSFM.UserId = Convert.ToInt32(Session["UserId"]);
            if (Convert.ToString(Session["SocietyTransID"]) == "0")
            {
                objSSFM.SocietyTransID = DateTime.Now.ToString(("yyyyMMdd"));
            }
            else
            {
                objSSFM.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            }
            objSSFM.Name1 = collection.Get("Name1");
            objSSFM.FatherName1 = collection.Get("FatherName1");
            objSSFM.Mobile1 = collection.Get("Mobile1");
            objSSFM.Email1 = collection.Get("Email1");
            objSSFM.Address3 = collection.Get("Address3");
            objSSFM.HouseNoSectorNoRoad1 = collection.Get("HouseNoSectorNoRoad1");
            objSSFM.PostOffice1 = collection.Get("PostOffice1");
            objSSFM.PostalCode1 = collection.Get("PostalCode1");
            objSSFM.DistrictForUser1 = collection.Get("DistrictForUser1");
            objSSFM.Mainobject1 = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objSSFM.Mainobject1);
            objSSFM.Mainobject2 = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objSSFM.Mainobject2);
            objSSFM.Mainobject3 = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objSSFM.Mainobject3);
            objSSFM.Mainobject4 = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objSSFM.Mainobject4);
            objSSFM.SocietyName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objSSFM.SocietyName);
            objSSFM.CateOfSociety = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objSSFM.CateOfSociety);
            objSSFM.DebtsOfMembers = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objSSFM.DebtsOfMembers);
            objSSFM.AreaMortgaged = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objSSFM.AreaMortgaged);
            objSSFM.Address1 = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objSSFM.Address1);
            objSSFM.Address2 = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objSSFM.Address2);
            objSSFM.PostOffice = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objSSFM.PostOffice);
            objSSFM.FatherName1 = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objSSFM.FatherName1);
            objSSFM.Mainobject1 = XCCPrevent.FilterBadchars1(objSSFM.Mainobject1);
            objSSFM.Mainobject2 = XCCPrevent.FilterBadchars1(objSSFM.Mainobject2);
            objSSFM.Mainobject3 = XCCPrevent.FilterBadchars1(objSSFM.Mainobject3);
            objSSFM.Mainobject4 = XCCPrevent.FilterBadchars1(objSSFM.Mainobject4);
            objSSFM.SocietyName = XCCPrevent.FilterBadchars1(objSSFM.SocietyName);
            objSSFM.CateOfSociety = XCCPrevent.FilterBadchars1(objSSFM.CateOfSociety);
            objSSFM.DebtsOfMembers = XCCPrevent.FilterBadchars1(objSSFM.DebtsOfMembers);
            objSSFM.AreaMortgaged = XCCPrevent.FilterBadchars1(objSSFM.AreaMortgaged);
            objSSFM.Address1 = XCCPrevent.FilterBadchars1(objSSFM.Address1);
            objSSFM.Address2 = XCCPrevent.FilterBadchars1(objSSFM.Address2);
            objSSFM.PostOffice = XCCPrevent.FilterBadchars1(objSSFM.PostOffice);
            objSSFM.FatherName1 = XCCPrevent.FilterBadchars1(objSSFM.FatherName1);
            objSSFM.BrowserName = GetWebBrowserName();
            objSSFM.Updatedby = Convert.ToInt32(Session["UserId"]);
            objSSFM.IPAddress = GetIPAddress();
        
            var getRecord = objSSF.NewSocietyMembersList(Convert.ToString(Session["SocietyTransID"]));
            Session["AddedMember"] = getRecord.Count;
            if (Convert.ToInt32(Session["AddedMember"]) > objSSFM.NoOfMembers)
            {
                List<SelectListItem> lstSL = new List<SelectListItem>();
                lstSL = GetSociety();
                ViewBag.lstSL = lstSL;
                ViewBag.show = "No of members can't be changed.";
                ViewBag.result = "0";
                ViewBag.SocietyStatus = 0;
                ViewBag.SocietyTransID = 1;
                return View();
            }

            result = objSSF.SaveSocietySubmissionFrom(objSSFM);
            if (result == 1)
            {
                if (Convert.ToString(Session["SocietyTransID"]) == "0")
                {
                    Session["SocietyTransID"] = objSSF.GetSocietyTransIdForSession(Convert.ToInt32(Session["UserId"]));
                }

                var result1 = objSSF.GetTotalMember(Convert.ToInt32(Session["UserId"]));
                Session["NoOfMembers"] = result1.Totalcount;

                List<SelectListItem> lstSL = new List<SelectListItem>();
                lstSL = GetSociety();
                ViewBag.lstSL = lstSL;
                ViewBag.show = "Draft saved successfully.";
                ViewBag.result = "1";
                ViewBag.tabResult = "1";
                ViewBag.SocietyStatus = 0;
                ViewBag.SocietyTransID = 1;
            }
            else
            {
                List<SelectListItem> lstSL = new List<SelectListItem>();
                lstSL = GetSociety();
                ViewBag.lstSL = lstSL;
                ViewBag.show = "Draft not save successfully.";
                ViewBag.result = "0";
                ViewBag.tabResult = "0";
                ViewBag.SocietyStatus = 0;
            }
            ViewBag.NoOfMembers = Convert.ToInt32(Session["NoOfMembers"]);
            return View();
        }
        #endregion

        #region JsonCall
        #region ManagingCommitteMembersMethods
        public JsonResult ManagingCommitteMembersList()
        {
            var SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            if (!string.IsNullOrEmpty(SocietyTransID))
            {
                return Json(objSSF.NewManagingCommitteMembersList(SocietyTransID), JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMember()
        {
            var LogInId = Convert.ToInt32(Session["UserId"]);
            return Json(objSSF.GetTotalMember(LogInId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteManagingCommitteMember(int SocietyMemberID)
        {
            int i = objSSF.DeleteManagingCommitteMember(SocietyMemberID);
            if (i >= 1)
            {
                var result1 = objSSF.GetTotalMember(Convert.ToInt32(Session["UserId"]));
                Session["NoOfMembers"] = result1.Totalcount;
                var getRecord = objSSF.NewSocietyMembersList(Convert.ToString(Session["SocietyTransID"]));
                Session["AddedMember"] = getRecord.Count;
            }
            return Json(i.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbyManagingCommitteeMemberID(int SocietyMemberID)
        {
            var ManagingCommitteeMember = objSSF.NewGetManagingCommitteeMember(SocietyMemberID);
            return Json(ManagingCommitteeMember, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EncryptObject(MembershipDetailsModels objMDM)
        {
            objMDM.SocietyMemberName = objGBI.Encrypt(objMDM.SocietyMemberName, "sblw-3hn8-sqoy19");
            objMDM.RelationshipMemberName = objGBI.Encrypt(objMDM.RelationshipMemberName, "sblw-3hn8-sqoy19");

            if (!string.IsNullOrEmpty(objMDM.AadharNo))
            {
                objMDM.AadharNo = objGBI.Encrypt(objMDM.AadharNo, Convert.ToString(Session["EncrptedDecruptedKey"]));
            }
            else
            {
                objMDM.AadharNo = "";
            }
            return Json(objMDM, JsonRequestBehavior.AllowGet);
        }

        #region
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
            myStr.Append(Request.LogonUserIdentity.User.Value);            
            return myStr.ToString();
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

        #endregion

        public JsonResult AddManagingCommitteMember(MembershipDetailsModels objMDM)
        {
            string IPAddress = GetIPAddress();
            string BrowserName = GetWebBrowserName();
            objMDM.SocietyMemberName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMDM.SocietyMemberName);
            objMDM.RelationshipMemberName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMDM.RelationshipMemberName);
            objMDM.ManagingRelationshipName = objMDM.ManagingRelationshipName;
            objMDM.RelationshipMemberName = XCCPrevent.FilterBadchars(objMDM.RelationshipMemberName);
            objMDM.SocietyMemberName = XCCPrevent.FilterBadchars(objMDM.SocietyMemberName);
            objMDM.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            objMDM.IPAddress = IPAddress;
            objMDM.BrowserName = BrowserName;
            objMDM.Updatedby = Convert.ToInt32(Session["UserId"]);
            if (!string.IsNullOrEmpty(objMDM.SocietyTransID))
            {
                if (!string.IsNullOrEmpty(objMDM.AadharNo))
                {
                    objMDM.AadharNo = objGBI.Encrypt(objMDM.AadharNo, Convert.ToString(Session["EncrptedDecruptedKey"]));
                }
                else
                {
                    objMDM.AadharNo = "";
                }
                objMDM.SocietyMemberID = 0;

                int i = objSSF.SaveSocietyMemberDetails(objMDM);
                if (i >=1 )
                {
                    var result1 = objSSF.GetTotalMember(Convert.ToInt32(Session["UserId"]));
                    Session["NoOfMembers"] = result1.Totalcount;
                    var getRecord = objSSF.NewSocietyMembersList(Convert.ToString(Session["SocietyTransID"]));
                    Session["AddedMember"] = getRecord.Count;
                }
                return Json(i.ToString(), JsonRequestBehavior.AllowGet);
            }
            return Json("Kindly fill the first form then you can add managing committe members", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateManagingCommitteMember(MembershipDetailsModels objMDM)
        {
            string IPAddress = GetIPAddress();
            string BrowserName = GetWebBrowserName();
            objMDM.SocietyMemberName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMDM.SocietyMemberName);
            objMDM.RelationshipMemberName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMDM.RelationshipMemberName);
            objMDM.RelationshipMemberName = RCSSerivce.XCCPrevent.FilterBadchars1(objMDM.RelationshipMemberName);
            objMDM.ManagingRelationshipName = objMDM.ManagingRelationshipName;
            objMDM.SocietyMemberName = RCSSerivce.XCCPrevent.FilterBadchars1(objMDM.SocietyMemberName);
            objMDM.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            objMDM.IPAddress = IPAddress;
            objMDM.BrowserName = BrowserName;
            objMDM.Updatedby = Convert.ToInt32(Session["UserId"]);
            if (!string.IsNullOrEmpty(objMDM.AadharNo))
            {
                objMDM.AadharNo = objGBI.Encrypt(objMDM.AadharNo, Convert.ToString(Session["EncrptedDecruptedKey"]));
            }
            else
            {
                objMDM.AadharNo = "";
            }
            return Json(objSSF.SaveSocietyMemberDetails(objMDM), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SocietyMemberDetailMethods
        public JsonResult AddSocietyMember(MemberFurtherDetails objMFD)
        {
            objMFD.BrowserName = GetWebBrowserName();
            objMFD.Updatedby = Convert.ToInt32(Session["UserId"]);
            objMFD.IPAddress = GetIPAddress();
            objMFD.MemberName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.MemberName);
            objMFD.FatherName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.FatherName);
            objMFD.Address1 = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.Address1);
            objMFD.Address2 = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.Address2);
            objMFD.PostOffice = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.PostOffice);
            objMFD.NomineeName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.NomineeName);
            objMFD.EmailId = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.EmailId);
            objMFD.OccupationVal = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.OccupationVal);
            objMFD.MemberName = XCCPrevent.FilterBadchars1(objMFD.MemberName);
            objMFD.FatherName = XCCPrevent.FilterBadchars1(objMFD.FatherName);
            objMFD.Address1 = XCCPrevent.FilterBadchars1(objMFD.Address1);
            objMFD.Address2 = XCCPrevent.FilterBadchars1(objMFD.Address2);
            objMFD.PostOffice = XCCPrevent.FilterBadchars1(objMFD.PostOffice);
            objMFD.NomineeName = XCCPrevent.FilterBadchars1(objMFD.NomineeName);
            objMFD.EmailId = XCCPrevent.FilterBadchars1(objMFD.EmailId);
            objMFD.Dob = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.Dob);

            objMFD.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            if (!string.IsNullOrEmpty(objMFD.SocietyTransID))
            {
                if (string.IsNullOrEmpty(objMFD.EmailId))
                {
                    objMFD.EmailId = "";
                }
                if (!string.IsNullOrEmpty(objMFD.AadharNo))
                {
                    objMFD.AadharNo = objGBI.Encrypt(objMFD.AadharNo, Convert.ToString(Session["EncrptedDecruptedKey"]));
                }
                else
                {
                    objMFD.AadharNo = "";
                }
                if (Session["MemberPhoto"] != null)
                {
                    objMFD.Imgg = (Byte[])Session["MemberPhoto"];
                    //objMFD.Extension = Convert.ToString(Session["FileExtension"]);
                    objMFD.Fullpath = Convert.ToString(Session["FilePath"]);
                    Session["MemberPhoto"] = (byte[])null;
                    Session["FilePath"] = "";
                }               
                else
                {                    
                    objMFD.Imgg = objSSF.GetImageByte(objMFD.SocietyTransID, objMFD.MemberSNo);                 
                }

                objMFD.MemberSNo = 0;

                int i = objSSF.SaveMemberFurtherDetails(objMFD);
                if (i >= 1)
                {
                    var result1 = objSSF.GetTotalMember(Convert.ToInt32(Session["UserId"]));
                    Session["NoOfMembers"] = result1.Totalcount;
                    var getRecord = objSSF.NewSocietyMembersList(Convert.ToString(Session["SocietyTransID"]));
                    Session["AddedMember"] = getRecord.Count;
                }
                return Json(i.ToString(), JsonRequestBehavior.AllowGet);
            }
            return Json("Kindly fill the first form then you can add committe members", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SocietyMembersList()
        {
            var SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            if (!string.IsNullOrEmpty(SocietyTransID))
            {
                var getRecord = objSSF.NewSocietyMembersList(SocietyTransID);
                Session["AddedMember"] = getRecord.Count;
                var TotalCount = Convert.ToInt32(Session["NoOfMembers"]);
                var result = new { getRecord, TotalCount };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            Session["AddedMember"] = 0;
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindARCSOfficeById(string ARCSCode)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.ARCSOfficerById(Convert.ToInt32(ARCSCode)))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.ACRSName),
                    Value = Convert.ToString(item.ACRSCode)
                });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbySocietyMemberID(int MemberSNo)
        {
            var SocietyMemberDetail = objSSF.GetbySocietyMemberID(MemberSNo);
            return Json(SocietyMemberDetail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateSocietyMemberDetail(MemberFurtherDetails objMFD)
        {
            objMFD.BrowserName = GetWebBrowserName();
            objMFD.Updatedby = Convert.ToInt32(Session["UserId"]);
            objMFD.IPAddress = GetIPAddress();
            if (string.IsNullOrEmpty(objMFD.EmailId))
            {
                objMFD.EmailId = "";
            }
            else
            {
                objMFD.EmailId = XCCPrevent.FilterBadchars(objMFD.EmailId);
            }
            if (!string.IsNullOrEmpty(objMFD.AadharNo))
            {
                objMFD.AadharNo = objGBI.Encrypt(objMFD.AadharNo, Convert.ToString(Session["EncrptedDecruptedKey"]));
            }
            else
            {
                objMFD.AadharNo = "";
            }
            objMFD.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            if (Session["MemberPhoto"] != null)
            {
                objMFD.Imgg = (Byte[])Session["MemberPhoto"];
                //objMFD.Extension = Convert.ToString(Session["FileExtension"]);
                objMFD.Fullpath = Convert.ToString(Session["FilePath"]);
                Session["MemberPhoto"] = (byte[])null;
                Session["FilePath"] = "";
            }
            else
            {
                objMFD.Imgg = objSSF.GetImageByte(objMFD.SocietyTransID, objMFD.MemberSNo);
            }
            objMFD.MemberName = XCCPrevent.FilterBadchars(objMFD.MemberName);
            objMFD.FatherName = XCCPrevent.FilterBadchars(objMFD.FatherName);
            objMFD.Address1 = XCCPrevent.FilterBadchars(objMFD.Address1);
            objMFD.Address2 = XCCPrevent.FilterBadchars(objMFD.Address2);
            objMFD.PostOffice = XCCPrevent.FilterBadchars(objMFD.PostOffice);
            objMFD.NomineeName = XCCPrevent.FilterBadchars(objMFD.NomineeName);
            objMFD.OccupationVal = XCCPrevent.FilterBadchars(objMFD.OccupationVal);
            objMFD.Flfile = XCCPrevent.FilterBadchars(objMFD.Flfile);
            if (string.IsNullOrEmpty(objMFD.Dob))
            {
                objMFD.Dob = "";
            }
            else
            {
                objMFD.Dob = XCCPrevent.FilterBadchars(objMFD.Dob);
            }
            return Json(objSSF.SaveMemberFurtherDetails(objMFD), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSocietyMember(MemberFurtherDetails objMFD)
        {
            int i = objSSF.DeleteSocietyMember(objMFD.MemberSNo);
            if (i >= 1)
            {
                var result1 = objSSF.GetTotalMember(Convert.ToInt32(Session["UserId"]));
                Session["NoOfMembers"] = result1.Totalcount;
                var getRecord = objSSF.NewSocietyMembersList(Convert.ToString(Session["SocietyTransID"]));
                Session["AddedMember"] = getRecord.Count;
            }
            return Json(i.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckPresidentValidation(int SocietyMemberDesignation)
        {
            return Json(objSSF.CheckPresidentValidation(Convert.ToString(Session["SocietyTransID"]), SocietyMemberDesignation), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region FormEDetails
        [RBAC]
        public ActionResult FormE()
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
            FormEDModels objFEM = new FormEDModels();
            objFEM = objSSF.GetDetailsForFormE(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.Address = objFEM.Address;
            ViewBag.District = objFEM.District;
            ViewBag.HouseNo = objFEM.HouseNo;
            ViewBag.RelationshipName = objFEM.RelationshipName;
            ViewBag.SectorStreet = objFEM.SectorStreet;
            ViewBag.SocietyMemberName = objFEM.SocietyMemberName;
            ViewBag.SocietyName = objFEM.SocietyName;           
            ViewBag.FormEValue = Convert.ToInt16(Session["FormE"]);
            return View();
        }

        [HttpPost]
        public ActionResult FormE(FormCollection fc)
        {
            if (Convert.ToString(Session["SocietyTransID"]) == "0" || string.IsNullOrEmpty(Convert.ToString(Session["SocietyTransID"])))
            {
                TempData["FormC"] = 2;
                return RedirectToAction("Application", "Society");
            }
            FormEModels objFEM = new FormEModels
            {
                SocietyTransId = Convert.ToString(Session["SocietyTransID"]),
                ProceedingBook = Convert.ToBoolean(fc.Get("chk1")),
                CashBook = Convert.ToBoolean(fc.Get("chk2")),
                LedgerBook = Convert.ToBoolean(fc.Get("chk3")),
                MemberRegister = Convert.ToBoolean(fc.Get("chk4")),
                ActandRule = Convert.ToBoolean(fc.Get("chk5")),
                Byelawsofsociety = Convert.ToBoolean(fc.Get("chk6")),
                Appsformembership = Convert.ToBoolean(fc.Get("chk7"))
            };
            int i = objSSF.SaveFormE(objFEM);
            if (i > 0)
            {
                TempData["FormE"] = 5;
            }
            return RedirectToAction("Application", "Society");
        }

        public ActionResult Form()
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
            return RedirectToAction("FormE", "Society");
        }
        #endregion

        #region FormAdetails
        [RBAC]
        public ActionResult FormA()
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
            FormAmodels objFAM = new FormAmodels();
            objFAM = objSSF.GetDetailsForFormA(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.Address1 = objFAM.Address1;
            ViewBag.APRDistrictName = objFAM.APRDistrictName;
            ViewBag.Address2 = objFAM.Address2;
            ViewBag.District = objFAM.District;
            ViewBag.PostOffice = objFAM.PostOffice;
            if (objFAM.DateofApplicationReceived.ToShortDateString() != "01-01-1900")
            {
            ViewBag.DateofApplicationReceived = objFAM.DateofApplicationReceived.ToShortDateString();
            }
            ViewBag.SocietyName = objFAM.SocietyName;
            RCModels objRC = new RCModels();
            objRC = objSSF.GetDetailsForCashierReceipt(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.Total = objRC.Total;
            ViewBag.SocietyStatus = Convert.ToInt32(Session["SocietyStatus"]);
            return View();
        }
        #endregion

        #region FormDdetails
        [RBAC]
        public ActionResult FormD()
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
            FormEDModels objFEM = new FormEDModels();
            objFEM = objSSF.GetDetailsForFormE(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.Address = objFEM.Address;
            ViewBag.District = objFEM.District;
            ViewBag.HouseNo = objFEM.HouseNo;
            ViewBag.RelationshipName = objFEM.RelationshipName;
            ViewBag.SectorStreet = objFEM.SectorStreet;
            ViewBag.SocietyMemberName = objFEM.SocietyMemberName;
            ViewBag.SocietyName = objFEM.SocietyName;
            ViewBag.DivCode = objFEM.DivCode;
            RCModels objRC = new RCModels();
            objRC = objSSF.GetDetailsForCashierReceipt(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.AdmissionFees = objRC.AdmissionFees;
            ViewBag.ShareMoney = objRC.ShareMoney;
            ViewBag.Deposits = objRC.Deposits;
            ViewBag.Total = objRC.Total;
            ViewBag.SocietyStatus = Convert.ToInt32(Session["SocietyStatus"]);
            return View();
        }
        #endregion

        #region FormCDetails
        [RBAC]
        [HttpGet]
        public ActionResult FormC()
        {
            ViewBag.lstFormc = objSSF.ManagingCommitteMembersList(Convert.ToString(Session["SocietyTransID"]));
            FormAmodels objFAM = new FormAmodels();
            objFAM = objSSF.GetDetailsForFormA(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.Address1 = objFAM.Address1;
            ViewBag.Address2 = objFAM.Address2;
            ViewBag.District = objFAM.District;
            ViewBag.PostOffice = objFAM.PostOffice;
            ViewBag.DateofApplicationReceived = objFAM.DateofApplicationReceived;
            ViewBag.Pin = objFAM.Pin;
            ViewBag.SocietyName = objFAM.SocietyName;
            FormEDModels objFEM = new FormEDModels();
            objFEM = objSSF.GetDetailsForFormE(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.SocietyMemberName = objFEM.SocietyMemberName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormC(FormCollection fc)
        {
            if (Convert.ToString(Session["SocietyTransID"]) == "0" || string.IsNullOrEmpty(Convert.ToString(Session["SocietyTransID"])))
            {
                TempData["FormC"] = 2;
                return RedirectToAction("Application", "Society");
            }
            string MeetingDate = fc.Get("datepicker1").ToString();
            //DateTime MeetingDate = Convert.ToDateTime(fc.Get("datepicker1"));
            string BankName = Convert.ToString(fc.Get("Bank"));
            int a = objSSF.SaveFromCDetails(MeetingDate, BankName, Convert.ToString(Session["SocietyTransID"]));
            if (a >= 1)
            {
                TempData["FormC"] = 1;
            }
            else
            {
                TempData["FormC"] = 0;
            }
            return RedirectToAction("Application", "Society");
        }
        #endregion
        [ValidateAntiForgeryToken]
        public ActionResult CustodianSave(FormCollection fc)
        {
            if (Convert.ToString(Session["SocietyTransID"]) == "0" || string.IsNullOrEmpty(Convert.ToString(Session["SocietyTransID"])))
            {
                TempData["FormC"] = 2;
                return RedirectToAction("Application", "Society");
            }
            int CustodianMemberId = Convert.ToInt32(fc.Get("CustodianMemberId").ToString());
            //DateTime MeetingDate = Convert.ToDateTime(fc.Get("datepicker1"));   
            int a = objSSF.SaveCustodian(CustodianMemberId, Convert.ToString(Session["SocietyTransID"]));
            //if (a >= 1)
            //{
            //    TempData["FormC"] = 1;
            //}
            //else
            //{
            //    TempData["FormC"] = 0;
            //}
            return RedirectToAction("Application", "Society");
        }
        
        private string GetRandomText()
        {
            StringBuilder randomText = new StringBuilder();
            string alphabets = "012345679ACEFGHKLMNPRSWXZabcdefghijkhlmnopqrstuvwxyz";
            Random r = new Random();
            for (int j = 0; j <= 5; j++)
            {
                randomText.Append(alphabets[r.Next(alphabets.Length)]);
            }
            return randomText.ToString();
        }
        #region uploaddocuments
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files, FormCollection collections)
        {
            try
            {
                ByeLawsModel objBL = new ByeLawsModel();
                ContentFileUploadModel objCFU = new ContentFileUploadModel();
                int i = 1;
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
                            decimal size = Math.Round(((decimal)file.ContentLength / (decimal)1024), 2);
                            if (size < 1000)
                            {
                                if (objCFU.FormId == 5)
                                {
                                    //Code added on 02 August 
                                    var filename = Path.GetFileNameWithoutExtension(file.FileName);
                                    string extensionName = Path.GetExtension(file.FileName);
                                    var fullFile = filename + GetRandomText() + extensionName;
                                    var dbsavePath = "/pdf/" + file.FileName;
                                    fullFile = Path.Combine(Server.MapPath("~/pdf/"), fullFile);
                                    file.SaveAs(fullFile);
                                    objBL.Path = fullFile;
                                    int a = objSSF.SaveByeLaws(objBL);
                                }
                                else if (objCFU.FormId == 6)
                                {
                                    //Code added on 02 August 
                                    var filename = Path.GetFileNameWithoutExtension(file.FileName);
                                    string extensionName = Path.GetExtension(file.FileName);
                                    var fullFile = filename + GetRandomText() + extensionName;
                                    var dbsavePath = "/pdf/" + file.FileName;
                                    fullFile = Path.Combine(Server.MapPath("~/pdf/"), fullFile);
                                    file.SaveAs(fullFile);
                                    objCFU.Path = fullFile;
                                    int a = objSSF.SaveContentFileUploadForBank(objCFU);
                                }
                                else
                                {
                                    int a = objSSF.SaveContentFileUpload(objCFU);
                                }
                            }
                        }
                    }
                    i = i + 1;
                }
                int j = objSSF.GetUploadStatusPrevious(objBL.SocietyTransID);
                if (j == 6)
                {
                    Session["SocietyStatus"] = 2;
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Unauthorised");
                throw ex;
            }
            return RedirectToAction("FormA");
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

        #region feesubition
        [RBAC]
        public ActionResult Procedurefee()
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
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Procedurefee(FormCollection fc)
        {
            try
            {
                if (Convert.ToString(Session["SocietyTransID"]) == "0" || string.IsNullOrEmpty(Convert.ToString(Session["SocietyTransID"])))
                {
                    TempData["FormC"] = 2;
                    return RedirectToAction("Application", "Society");
                }
                RCModels objrc = new RCModels
                {
                    ShareMoney = Convert.ToString(fc.Get("ShareMoney")),
                    AdmissionFees = Convert.ToString(fc.Get("AdmissionFee")),
                    Deposits = Convert.ToString(fc.Get("Deposits")),
                    SocietyTransId = Convert.ToString(Session["SocietyTransID"])
                };
                int i = objSSF.SaveDetailsofCashierReceipt(objrc);
                if (i >= 1)
                {
                    Session["TabResult"] = 4;
                }
                else
                {
                    Session["TabResult"] = 3;
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Unauthorised");
                throw ex;
            }
            TempData["procedurefee"] = 4;
            return RedirectToAction("Application", "Society");
        }
        #endregion

        public ActionResult Status()
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
            ViewBag.SocietyStatus1 = objSSF.GetSoceityStatus(Convert.ToString(Session["SocietyTransID"]));
            return View();
        }

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
            return View();
        }

        [AllowAnonymous]
        public string CheckProposedSoceityName(string input)
        {
            input = input.TrimEnd();
            input = input.TrimStart();
            input = input.Trim();
            bool ifuser = objSSF.CheckProposedSoceityName(input);
            if (ifuser == false)
            {
                return input;
            }
            if (ifuser == true)
            {
                return "Not Available";
            }
            return "";
        }

        public JsonResult ImageSave()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    #pragma warning disable
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        var filename = Path.GetFileNameWithoutExtension(file.FileName);
                        string extensionName = Path.GetExtension(fname);
                        var fullFile = filename + extensionName;
                        var dbsavePath = "/UploadedImages/" + fname;
                        var imagepath = "~/UploadedImages/" + fname;
                        fullFile = Path.Combine(Server.MapPath("~/UploadedImages/"), fullFile);
                        file.SaveAs(fullFile);
                        int filesize = file.ContentLength;
                        if (extensionName.ToLower() == ".jpg" || extensionName.ToLower() == ".png" || extensionName.ToLower() == ".jpeg")
                        {
                            Stream stream = file.InputStream;
                            BinaryReader binaryreader = new BinaryReader(stream);
                            byte[] bytes = binaryreader.ReadBytes((int)stream.Length);
                            Session["MemberPhoto"] = bytes;
                            //Session["FileExtension"] = extensionName;
                            Session["FilePath"] = dbsavePath;
                            return Json(dbsavePath, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("File type is not supported.");
                        }
                    }
                    #pragma warning restore
                    return Json(new EmptyResult(), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        public void ImageSave2()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        var filename = Path.GetFileNameWithoutExtension(file.FileName);
                        string extensionName = Path.GetExtension(fname);
                        var fullFile = filename + extensionName;
                        var dbsavePath = "/UploadedImages/" + fname;
                        var imagepath = "~/UploadedImages/" + fname;
                        fullFile = Path.Combine(Server.MapPath("~/UploadedImages/"), fullFile);
                        file.SaveAs(fullFile);
                        int filesize = file.ContentLength;
                        if (extensionName.ToLower() == ".jpg" || extensionName.ToLower() == ".png" || extensionName.ToLower() == ".jpeg")
                        {
                            Stream stream = file.InputStream;
                            BinaryReader binaryreader = new BinaryReader(stream);
                            byte[] bytes = binaryreader.ReadBytes((int)stream.Length);
                            Session["MemberPhoto"] = bytes;
                            //Session["FileExtension"] = extensionName;
                            Session["FilePath"] = dbsavePath;
                        }
                    }
                    // Returns message that successfully uploaded  
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }   
        
        public ActionResult Report(string id, string value)
        {
            string path = "";
            value = value.Replace("  ", String.Empty);
            LocalReport lr = new LocalReport();
            FormEModels GetFormE = new FormEModels();
            GetFormE = objSSF.GetReportE(Convert.ToString(Session["SocietyTransID"]));
            if (GetFormE.ActandRule == false)
            {
                if (value == "FormE")
                {
                    path = Path.Combine(Server.MapPath("~/Reports/FormE.rdlc"));
                }
            }
            else if(GetFormE.ActandRule==true)
            {
                if (value == "FormE")
                {
                    path = Path.Combine(Server.MapPath("~/Reports/FormECheckbox.rdlc"));
                }
                
            }
            if (value == "FormL")
            {
            path = Path.Combine(Server.MapPath("~/Reports/FormL.rdlc"));
            }
            if (value == "FormC")
            {
            path = Path.Combine(Server.MapPath("~/Reports/FormC.rdlc"));
            }
            if (value == "FormD")
            {
            path = Path.Combine(Server.MapPath("~/Reports/FormD.rdlc"));
            }
            if (value == "UploadFormL")
            {
            path = Path.Combine(Server.MapPath("~/Reports/FormL.rdlc"));
            }
            if (value == "Certificate")
            {
                path = Path.Combine(Server.MapPath("~/Reports/Certificate.rdlc"));
            }
            if (value == "ApprovedReport")
            {
                path = Path.Combine(Server.MapPath("~/Reports/ApprovedReport.rdlc"));
            }         
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return RedirectToAction("Error", "Unauthorised");
            }
            if (value == "ApprovedReport")
            {
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                ds1 = objSSF.ReportFormA(Convert.ToString(Session["SocietyTransID"]));
                ds2 = objSSF.ReportFormA2(Convert.ToString(Session["SocietyTransID"]));
                DataTable ApprovedReport = ds1.Tables[0];
                DataTable ApprovedReport2 = ds2.Tables[0];
                ReportDataSource rd = new ReportDataSource("FormA", ApprovedReport);
                ReportDataSource rd2 = new ReportDataSource("DataSet1", ApprovedReport2);
                lr.DataSources.Add(rd);
                lr.DataSources.Add(rd2);
            }
            if (value == "FormL")
            {
                DataSet ds1 = new DataSet();
                ds1 = objSSF.ReportFormL(Convert.ToString(Session["SocietyTransID"]));
                DataTable FormL = ds1.Tables[0];
                ReportDataSource rd = new ReportDataSource("DataSet1", FormL);
                lr.DataSources.Add(rd);
            }
            if (value == "FormC")
            {
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();
                DataSet ds4 = new DataSet();
                DataSet ds5 = new DataSet();
                DataSet ds6 = new DataSet();
                DataSet ds7 = new DataSet();
                DataSet ds8 = new DataSet();
                DataSet ds9 = new DataSet();
                DataSet ds10 = new DataSet();
                ds1 = objSSF.ReportFormC(Convert.ToString(Session["SocietyTransID"]));
                ds2 = objSSF.ReportFormC2(Convert.ToString(Session["SocietyTransID"]));
                ds3 = objSSF.ReportFormC3(Convert.ToString(Session["SocietyTransID"]));
                ds4 = objSSF.ReportFormCPresident(Convert.ToString(Session["SocietyTransID"]));
                ds5 = objSSF.ReportFormCVicePresident(Convert.ToString(Session["SocietyTransID"]));
                ds6 = objSSF.ReportFormCashier(Convert.ToString(Session["SocietyTransID"]));
                ds7 = objSSF.ReportFormMember(Convert.ToString(Session["SocietyTransID"]));
                ds8 = objSSF.ReportFormC1(Convert.ToString(Session["SocietyTransID"]));
                ds9 = objSSF.ReportFormC5(Convert.ToString(Session["SocietyTransID"]));
                ds10 = objSSF.ReportFormC6(Convert.ToString(Session["SocietyTransID"]));
                DataTable FormC = ds1.Tables[0];
                DataTable FormC2 = ds2.Tables[0];
                DataTable FormC3 = ds3.Tables[0];
                DataTable FormC4 = ds4.Tables[0];
                DataTable FormC5 = ds5.Tables[0];
                DataTable FormC6 = ds6.Tables[0];
                DataTable FormC7 = ds7.Tables[0];
                DataTable FormC8 = ds8.Tables[0];
                DataTable FormC9 = ds9.Tables[0];
                DataTable FormC10 = ds10.Tables[0];
                ReportDataSource rd = new ReportDataSource("DataSet1", FormC);
                ReportDataSource rd2 = new ReportDataSource("DataSet2", FormC2);
                ReportDataSource rd3 = new ReportDataSource("DataSet3", FormC3);
                ReportDataSource rd4 = new ReportDataSource("DataSetPresident", FormC4);
                ReportDataSource rd5 = new ReportDataSource("DataSet4ViceP", FormC5);
                ReportDataSource rd6 = new ReportDataSource("DataSet4Cashier", FormC6);
                ReportDataSource rd7 = new ReportDataSource("DataSet4Member", FormC7);
                ReportDataSource rd8 = new ReportDataSource("DataSet4", FormC8);
                ReportDataSource rd9 = new ReportDataSource("DataSet5", FormC9);
                ReportDataSource rd10 = new ReportDataSource("DataSet6", FormC10);
                lr.DataSources.Add(rd);
                lr.DataSources.Add(rd2);
                lr.DataSources.Add(rd3);
                lr.DataSources.Add(rd4);
                lr.DataSources.Add(rd5);
                lr.DataSources.Add(rd6);
                lr.DataSources.Add(rd7);
                lr.DataSources.Add(rd8);
                lr.DataSources.Add(rd9);
                lr.DataSources.Add(rd10);
            }
            if (value == "FormD")
            {
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();
                DataSet ds4 = new DataSet();
                ds1 = objSSF.ReportFormD(Convert.ToString(Session["SocietyTransID"]));
                ds2 = objSSF.ReportFormD2(Convert.ToString(Session["SocietyTransID"]));
                ds3 = objSSF.ReportFormD3(Convert.ToString(Session["SocietyTransID"]));
                ds4 = objSSF.ReportFormD4(Convert.ToString(Session["SocietyTransID"]));
                DataTable FormD = ds1.Tables[0];
                DataTable FormD2 = ds2.Tables[0];
                DataTable FormD3 = ds3.Tables[0];
                DataTable FormD4 = ds4.Tables[0];
                ReportDataSource rd = new ReportDataSource("FormDDataSet", FormD);
                ReportDataSource rd2 = new ReportDataSource("FormDDataSet2", FormD2);
                ReportDataSource rd3 = new ReportDataSource("DataSetCashier", FormD3);
                ReportDataSource rd4 = new ReportDataSource("FromDDataSet3", FormD4);
                lr.DataSources.Add(rd);
                lr.DataSources.Add(rd2);
                lr.DataSources.Add(rd3);
                lr.DataSources.Add(rd4);
            }
            if (value == "FormE")
            {
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();
                ds1 = objSSF.ReportFormE(Convert.ToString(Session["SocietyTransID"]));
                ds2 = objSSF.ReportFormE2(Convert.ToString(Session["SocietyTransID"]));
                ds3 = objSSF.ReportFormE3(Convert.ToString(Session["SocietyTransID"]));
                DataTable FormE = ds1.Tables[0];
                DataTable FormE2 = ds2.Tables[0];
                DataTable FormE3 = ds3.Tables[0];
                ReportDataSource rd = new ReportDataSource("DataSet1", FormE);
                ReportDataSource rd2 = new ReportDataSource("DataSet2", FormE2);
                ReportDataSource rd3 = new ReportDataSource("DataSet3", FormE3);
                lr.DataSources.Add(rd);
                lr.DataSources.Add(rd2);
                lr.DataSources.Add(rd3);
            }
            if (value == "Certificate")
            {
                DataSet ds1 = new DataSet();
                ds1 = objSSF.ReportFormF2(Convert.ToString(Session["SocietyTransID"]));
                DataTable Certificate = ds1.Tables[0];
                ReportDataSource rd = new ReportDataSource("GetDetailsForFormC", Certificate);
                lr.DataSources.Add(rd);
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