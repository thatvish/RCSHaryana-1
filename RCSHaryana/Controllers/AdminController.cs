using RCSData;
using RCSEntities;
using RCSSerivce;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace RCSHaryana.Controllers
{
    public class AdminController : Controller
    {
        #region ClassObjects
        GetBasicInfo objGBI = new GetBasicInfo();
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


        public class AttachmentType
        {
            public string MimeType { get; set; }
            public string FriendlyName { get; set; }
            public string Extension { get; set; }
        }
        #endregion

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
            AdminData objAD = new AdminData();
            Accountstatusmodel objASM = new Accountstatusmodel();
            objASM = objAD.GetAccountsDetails(Convert.ToInt32(Session["UserType"]));
            ViewBag.Totalaccounts = objASM.Totalaccount;
            ViewBag.ARCSaccount = objASM.ARCSaccount;
            ViewBag.inspectoraccount = objASM.Inspectoraccount;
            return View();
        }

        #region ARCSaccountCreate
        public ActionResult CreateARCSAccount()
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
            try
            {
                List<SelectListItem> lstSQM = new List<SelectListItem>();
                List<SelectListItem> lstGDN = new List<SelectListItem>();
                List<SelectListItem> lstGD = new List<SelectListItem>();
                List<SelectListItem> lstarcs = new List<SelectListItem>();
                lstSQM = GetSecurityQuestions();
                lstGDN = GetDRCSName();
                ViewBag.SecurityQuestions = lstSQM;
                ViewBag.DRCSName = lstGDN;
                lstGD = GetDistrict();
                ViewBag.District = lstGD;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Unauthorised");
                throw ex;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateARCSAccount(FormCollection collection)
        {
            ResgirationModels objRM = new ResgirationModels();
            EncryptionService objES = new EncryptionService();
            GetDRCSName objGDN = new GetDRCSName();
            Account objAcc = new Account();
            var salt = objES.CreateSalt();
            objRM.Username = collection.Get("UserName");
            objRM.Password = objES.EncryptPassword(collection.Get("Password"), salt);
            objRM.SecurityQuestionCode = collection.Get("SecurityQuestions");
            objRM.SecurityAnswer = collection.Get("Anwser");
            objRM.Salt = salt;
            objGDN.DRCSName = collection.Get("DRCSName");
            objRM.FirstName = collection.Get("Name");
            if (string.IsNullOrEmpty(objRM.FirstName))
            {
                ModelState.AddModelError("Name", "Please Enter the Name");
            }
            objRM.Mobile = collection.Get("Mobile");
            objRM.EmailID = collection.Get("Email");
            objRM.Gender = collection.Get("Gender");
            objRM.DisCode = collection.Get("District");
            objRM.ARCSCode = collection.Get("ARCSOffice");
            objRM.UserTypeCode = 2;
            objRM.Role = 2;
            objRM.CreatedBy = "Admin";
            if (ModelState.IsValid)
            {
                int result = objAcc.SaveResgiratedUser(objRM);
                if (result == 1)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            else
            {
                return View();
            }
            return View();
        }
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

        #region GetInspectorList
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult BindInspector(string DistrictCode)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetInspectorList(Convert.ToInt32(DistrictCode)))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.InspectorName),
                    Value = Convert.ToString(item.InspectorId)
                });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region username
        [HttpPost]
        public JsonResult DoesUserNameExist(string UserName)
        {
            var user = Membership.GetUser(UserName);
            return Json(user == null);
        }

        public JsonResult Check(FormCollection form)
        {
            System.Threading.Thread.Sleep(3000);
            string name = form["username"];
            if (name.Equals("mark"))
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }

        [AllowAnonymous]
        public string CheckUserName(string input)
        {
            Account objAcc = new Account();
            bool ifuser = objAcc.ValidateUser(input);
            if (ifuser == false)
            {
                return "Available";
            }
            if (ifuser == true)
            {
                return "Not Available";
            }
            return "";
        }
        #endregion

        #region GetSecurityQuestions
        public List<SelectListItem> GetSecurityQuestions()
        {
            Account objA = new Account();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objA.GetSecurityQuestions())
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SecurityQuestion),
                    Value = Convert.ToString(item.QuestionId)
                });
            }
            return items;
        }
        #endregion

        #region BindDropdonwListByAjaxDropdownSelection
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult BindARCSOffice(string DistrictCode)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
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
        #endregion

        #region getdistrict
        public List<SelectListItem> GetDistrict()
        {
            GetBasicInfo objGBI = new GetBasicInfo();
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
        #endregion

        #region getDRCS
        public List<SelectListItem> GetDRCSName()
        {
            AdminData objAD = new AdminData();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objAD.GetDRCSName())
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.DRCSName),
                    Value = Convert.ToString(item.DRCSCode)
                });
            }
            return items;
        }
        #endregion

        #region Aadhar
        public JsonResult AddManagingCommitteMember(MembershipDetailsModels objMDM)
        {
            objMDM.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
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
                return Json(objSSF.SaveSocietyMemberDetails(objMDM), JsonRequestBehavior.AllowGet);
            }
            return Json("Kindly fill the first form then you can add managing committe members", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Inspectoraccountcreate
        public ActionResult CreateInspectorAccount()
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
            try
            {
                List<SelectListItem> lstSQM = new List<SelectListItem>();
                List<SelectListItem> lstGDN = new List<SelectListItem>();
                List<SelectListItem> lstGD = new List<SelectListItem>();
                List<SelectListItem> lstarcs = new List<SelectListItem>();
                List<SelectListItem> lstGIL = new List<SelectListItem>();
                lstSQM = GetSecurityQuestions();
                lstGDN = GetDRCSName();
                ViewBag.SecurityQuestions = lstSQM;
                ViewBag.DRCSName = lstGDN;
                lstGD = GetDistrict();
                ViewBag.District = lstGD;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Unauthorised");
                throw ex;
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateInspectorAccount(FormCollection collection)
        {
            InspectorListModel objILM = new InspectorListModel();
            ResgirationModels objRM = new ResgirationModels();
            EncryptionService objES = new EncryptionService();
            GetDRCSName objGDN = new GetDRCSName();
            Account objAcc = new Account();
            var salt = objES.CreateSalt();
            objRM.Username = collection.Get("UserName");
            objRM.Password = objES.EncryptPassword(collection.Get("Password"), salt);
            objRM.SecurityQuestionCode = collection.Get("SecurityQuestions");
            objRM.SecurityAnswer = collection.Get("Anwser");
            objRM.Salt = salt;
            objGDN.DRCSName = collection.Get("DRCSName");
            objILM.InspectorName = collection.Get("InspectorOffice");
            objRM.FirstName = collection.Get("Name");
            if (string.IsNullOrEmpty(objRM.FirstName))
            {
                ModelState.AddModelError("Name", "Please Enter the Name");
            }
            objRM.Mobile = collection.Get("Mobile");
            objRM.EmailID = collection.Get("Email");
            objRM.Gender = collection.Get("Gender");
            objRM.DisCode = collection.Get("District");
            objRM.ARCSCode = collection.Get("ARCSOffice");
            objRM.UserTypeCode = 4;
            objRM.Role = 3;
            objRM.CreatedBy = "Admin";
            if (ModelState.IsValid)
            {
                int result = objAcc.SaveResgiratedUser(objRM);
                if (result == 1)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            else
            {
                return View();
            }
            return View();
        }
        #endregion
    }
}