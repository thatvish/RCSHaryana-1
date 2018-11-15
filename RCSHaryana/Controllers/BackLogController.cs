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
    public class BackLogController : Controller
    {
        BackLogData objBdata = new BackLogData();
        GetBasicInfo objGBI = new GetBasicInfo();
        DashboardDetail objDD = new DashboardDetail();
        DashboardDetail GetAuditDD = new DashboardDetail();
        BackLogCredential BackLogCrd = new BackLogCredential();
        List<SelectListItem> lstR = new List<SelectListItem>();       
        List<SelectListItem> lstGD = new List<SelectListItem>();
        List<SelectListItem> lstMember = new List<SelectListItem>();
        CS4HJ obj = new CS4HJ();
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
        #region DropdownMethod
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


        #endregion

        public List<SelectListItem> GetsubClassSocieties()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetSubClassOfSociety(3, Convert.ToInt32(Session["UserId"])))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SocietySubClassName),
                    Value = Convert.ToString(item.SocietySubClassCode)
                });
            }
            return items;
        }

        #region Dashboard
        public ActionResult Dashboard()
        {
            if (Convert.ToInt32(Session["BackLogResetStatus"]) == 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["SocietyTransID"])))
                {
                    TempData["Result"] = 1;
                    ViewBag.Err = "First you set your new password, then you will be go further.";
                    return RedirectToAction("ResetPassword", "BackLog");
                }
                else
                {
                    return RedirectToAction("LogIn", "Account");
                }

            }
            if (TempData["ResetMassageToDashboard"] != null && Convert.ToInt16(TempData["ResetMassageToDashboard"]) == 1)
            {
                ViewBag.DashboardMessage = "your security setting has been updated, next time you will login with updated credential.";
            }
            try
            {
                List<SelectListItem> lstARCSCode = new List<SelectListItem>();
                lstARCSCode = GetsubClassSocieties();
                ViewBag.lstARCSCode = lstARCSCode;

                var UserId = Convert.ToInt32(Session["UserId"]);
                List<SelectListItem> CommunityofSociety = new List<SelectListItem>();
                CommunityofSociety = GetCommunityofSociety();
                ViewBag.CommunityofSociety = CommunityofSociety;
                objDD = objBdata.GetDashBoardData(UserId);
                GetAuditDD = objBdata.GetAuditDetail(objDD.SocietyTransId);
                string LastDateAudit = GetAuditDD.LastDateAudit;
                string LastDateInspection = GetAuditDD.LastDateInspection;
                string GeneralBodyMeeting = GetAuditDD.GeneralBodyMeeting;
                ViewBag.SocietyName = objDD.SocietyName;
                ViewBag.RegId = objDD.RegId;
                ViewBag.DateofRegistration = objDD.DateofRegistration.ToShortDateString();
                ViewBag.SocietyTransId = objDD.SocietyTransId;
                ViewBag.BackLogAreaOfOperation = GetAuditDD.AreaOfOperation;
                ViewBag.LastDateAudit = LastDateAudit;
                ViewBag.LastDateInspection = LastDateInspection;
                ViewBag.GeneralBodyMeeting = GeneralBodyMeeting;
                ViewBag.AmountOfAuditFees = GetAuditDD.AmountOfAuditFees;
                ViewBag.CommunityTypeId = objDD.CommunityOfSocietyId;
                ViewBag.KindSocietyTypeId = objDD.KindOfSocietyId;
                Session["SocietyTransID"] = objDD.SocietyTransId;
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "Account");
                throw ex;
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddKindSociety(FormCollection fc)
        {
            var value = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(fc.Get("Value"));
            int i = objBdata.SaveKindSociety(value, Convert.ToInt32(Session["UserId"]));
            if (i >= 1)
            {
                if (Convert.ToInt32(Session["RoleId"]) == 2)
                {
                    return RedirectToAction("BackLogMemberDetail", "BackLogOfficer");
                }
                if (Convert.ToInt32(Session["RoleId"]) == 3)
                {
                    return RedirectToAction("BackLogMemberDetail", "BackLogOfficer");
                }
                return RedirectToAction("Dashboard", "BackLog");
            }
            ViewBag.Error = "Something went wrong, try again.";
            return View();
        }

        [HttpPost]
        public ActionResult Dashboard(FormCollection fc)
        {
            string IPAddress = GetIPAddress();
            string BrowserName = GetWebBrowserName();
            try
            {
                objDD.SocietyTransId = Convert.ToString(Session["SocietyTransID"]);
                if (!string.IsNullOrEmpty(objDD.SocietyTransId))
                {
                    objDD.AreaOfOperation = fc.Get("backlogAreaopt").ToString();
                    objDD.AmountOfAuditFees = fc.Get("backlogAmountFee").ToString();
                    objDD.GeneralBodyMeeting = fc.Get("backlogBodyMeeting").ToString();
                    objDD.LastDateAudit = fc.Get("lastauditdatetime").ToString();
                    objDD.LastDateInspection = fc.Get("datetimeInspection").ToString();
                    objDD.IPAddress = IPAddress;
                    objDD.BrowserName = BrowserName;
                    objDD.Updatedby = Convert.ToInt32(Session["UserId"]);
                    List<SelectListItem> CommunityofSociety = new List<SelectListItem>();
                    CommunityofSociety = GetCommunityofSociety();
                    ViewBag.CommunityofSociety = CommunityofSociety;
                    int i = objBdata.SaveAuditDetail(objDD);
                    if (i == 1)
                    {
                        TempData["AuditSuccess"] = "Success";
                    }
                    if (Convert.ToInt32(Session["RoleId"]) == 2)
                    {
                        return RedirectToAction("BackLogMemberDetail", "BackLogOfficer");
                    }
                    if (Convert.ToInt32(Session["RoleId"]) == 3)
                    {
                        return RedirectToAction("BackLogMemberDetail", "BackLogOfficer");
                    }
                    return RedirectToAction("Dashboard", "BackLog");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "Account");
                throw ex;
            }
            return View(objDD);
        }

        public ActionResult ViewBackLogFormDocs()
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
                return PartialView("~/Views/Shared/_ViewDocsForBackLog.cshtml");
            }
            else
            {
                return View();
            }
        }
        public JsonResult AddBackLogData(DashboardDetail objMFD)
        {
            objMFD.SocietyName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.SocietyName);
            objMFD.RegId = Convert.ToString(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(Convert.ToString(objMFD.RegId)));
            objMFD.DateofRegistration = Convert.ToDateTime(Convert.ToString(objMFD.DateofRegistration.ToShortDateString()));
            objMFD.CommunityOfSocietyId = Convert.ToInt32(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(Convert.ToString(objMFD.CommunityOfSocietyId)));
            objMFD.KindOfSocietyId = Convert.ToInt32(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(Convert.ToString(objMFD.KindOfSocietyId)));
            objMFD.IPAddress = GetIPAddress();
            objMFD.BrowserName = GetWebBrowserName();
            objMFD.Updatedby = Convert.ToInt32(Session["UserId"]);
            return Json(objBdata.SaveDashboardDetail(objMFD), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ResetPassword
        public ActionResult ResetPassword()
        {
            try
            {
                if (Convert.ToInt32(Session["UserId"]) == 0 || Convert.ToString(Session["UserId"]) == null || Convert.ToString(Session["UserId"]) == "")
                {
                    return RedirectToAction("LogIn", "Account");
                }
                if (TempData["Result"] != null && Convert.ToInt16(TempData["Result"]) == 1)
                {
                    ViewBag.Result = 1;
                    ViewBag.Err = "First you set your new password, then you will be go further.";
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "Account");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(FormCollection fc)
        {
            try
            {
                BackLogCrd.LogInId = Convert.ToInt32(Session["UserId"]);
                BackLogCrd.NewPassword = fc["ConfirmPassword"];
                EncryptionService objES = new EncryptionService();
                BackLogCrd.Salt = objES.CreateSalt();
                BackLogCrd.NewPassword = objES.EncryptPassword(BackLogCrd.NewPassword, BackLogCrd.Salt);
                int j = objBdata.BackLogLogInUpdate(BackLogCrd);
                if (j >= 1)
                {
                    Session["BackLogResetStatus"] = 1;
                    TempData["ResetMassageToDashboard"] = 1;
                    TempData["Success"] = "Success";
                    return RedirectToAction("Dashboard", "BackLog");
                }
                if (Convert.ToInt32(Session["BackLogResetStatus"]) == 0)
                {
                    ViewBag.result = 1;
                    ViewBag.Err = "First you set your new password, then you will be go further.";
                    return RedirectToAction("ResetPassword", "BackLog");
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "Account");
                throw ex;
            }
        }
        #endregion

        #region ElectionDetails
        public ActionResult ElectionDetails()
        {
            try
            {
                List<SelectListItem> lstMCDCM = new List<SelectListItem>();
                lstMCDCM = GetMemberCommDesignation();
                if (Convert.ToInt32(Session["BackLogResetStatus"]) == 0)
                {
                    TempData["Result"] = 1;
                    return RedirectToAction("ResetPassword", "BackLog");
                }
                var GetElectionDate = objBdata.GetElectionDate(Convert.ToString(Session["SocietyTransID"]));
                ViewBag.GetElectionDate = GetElectionDate;
                ViewBag.MemberCommDesignation = lstMCDCM;
                lstR = GetRelationship();
                ViewBag.Relationship = lstR;
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "Account");
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ElectionDetails(MembershipDetailsModels objMD)
        {
            try
            {
                List<SelectListItem> lstMCDCM = new List<SelectListItem>();
                lstMCDCM = GetMemberCommDesignation();
                if (Convert.ToInt32(Session["BackLogResetStatus"]) == 0)
                {
                    TempData["Result"] = 1;
                    return RedirectToAction("ResetPassword", "BackLog");
                }
                string IPAddress = GetIPAddress();
                string BrowserName = GetWebBrowserName();
                ViewBag.MemberCommDesignation = lstMCDCM;
                objMD.IPAddress = IPAddress;
                objMD.BrowserName = BrowserName;
                objMD.Updatedby = Convert.ToInt32(Session["UserId"]);
                var result = objBdata.SaveElectionDetail(objMD);
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "Account");
                throw ex;
            }
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
        #endregion

        #region FormL
        public ActionResult FormL()
        {
            try
            {
                if (Convert.ToInt32(Session["BackLogResetStatus"]) == 0)
                {
                    TempData["Result"] = 1;
                    return RedirectToAction("ResetPassword", "BackLog");
                }
                List<SelectListItem> lstR = new List<SelectListItem>();
                List<SelectListItem> lstGD = new List<SelectListItem>();
                lstR = GetRelationship();
                lstGD = GetDistrict();
                ViewBag.Relationship = lstR;
                ViewBag.District = lstGD;
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "Account");
                throw ex;
            }
        }
        #endregion

        #region ShareTransfer 
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

        public ActionResult ShareTransfer()
        {
            try
            {
                if (Convert.ToInt32(Session["BackLogResetStatus"]) == 0)
                {
                    TempData["Result"] = 1;
                    return RedirectToAction("ResetPassword", "BackLog");
                }
               
                lstR = GetRelationship();
                lstMember = GetShareTransferMember();
                lstGD = GetDistrict();
                ViewBag.Relationship = lstR;
                ViewBag.District = lstGD;
                ViewBag.MemberDetail = lstMember;
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "Account");
                throw ex;
            }
        }

        public JsonResult SaveShareTransfer(ShareTransferDetail objMFD)
        {
            objMFD.FirstShareTrans = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.FirstShareTrans);
            objMFD.DateofResolution = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.DateofResolution);
            objMFD.MemberName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.MemberName);
            objMFD.MemberId = Convert.ToInt32(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(Convert.ToString(objMFD.MemberId)));
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
            objMFD.ShareTransferAppLetterNo = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.ShareTransferAppLetterNo);
            objMFD.ShareTransferApprovalDate = Convert.ToString(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(Convert.ToString(objMFD.ShareTransferApprovalDate)));
            objMFD.ExistingMemberName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objMFD.ExistingMemberName);
            objMFD.BrowserName = GetWebBrowserName();
            objMFD.Updatedby = Convert.ToInt32(Session["UserId"]);
            objMFD.IPAddress = GetIPAddress();
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
                    objMFD.Imgg = objBdata.GetImageByteForShare(objMFD.SocietyTransID, objMFD.ShareTransferID);
                }
                objMFD.MemberSNo = 0;
                return Json(objBdata.SaveShareTransfer(objMFD), JsonRequestBehavior.AllowGet);
            }
            return Json("Kindly fill the first form then you can add committe members", JsonRequestBehavior.AllowGet);
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
        public JsonResult DeleteShareTransferMember(ShareTransferDetail objMFD)
        {
            var SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            var getAllRecord = objBdata.DeleteShareTransferMember(objMFD.ShareTransferID);
            var getRecord = objBdata.NewSocietyMembersList(SocietyTransID);
            Session["AddedMember"] = getRecord.Count;
            return Json(getAllRecord, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region
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

        #region uploaddocuments
        public ActionResult BackLogFormL()
        {
            try
            {
                if (Convert.ToInt32(Session["BackLogResetStatus"]) == 0)
                {
                    TempData["Result"] = 1;
                    return RedirectToAction("ResetPassword", "BackLog");
                }
                if (Convert.ToString(Session["BrowserId"]) != GenerateHashKeyForCheckBroswerEveryCall())
                {
                    return RedirectToAction("Login", "Account");
                }
                //int i = obj.CheckSessionEveryCall();
                //if (i != 0)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                ViewBag.SocietyStatus = Convert.ToInt32(Session["SocietyStatus"]);
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "Account");
                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFormL(IEnumerable<HttpPostedFileBase> files, FormCollection collections)
        {
            try
            {
                int check = objBdata.CheckFieldStatus(Convert.ToString(Session["SocietyTransID"]));
                if (check == 0)
                {
                    ByeLawsModel objBL = new ByeLawsModel();
                    ContentFileUploadModel objCFU = new ContentFileUploadModel();
                    int i = 8;
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
                            byte[] data = Encoding.UTF8.GetBytes(base64ImageRepresentation);
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
                                        int a = objBdata.SaveByeLaws(objBL);
                                    }
                                    else
                                    {
                                        objCFU.FormId = 10;
                                        var filename = Path.GetFileNameWithoutExtension(file.FileName);
                                        string extensionName = Path.GetExtension(file.FileName);
                                        var fullFile = filename + GetRandomText() + extensionName;
                                        var dbsavePath = "/pdf/" + file.FileName;
                                        fullFile = Path.Combine(Server.MapPath("~/pdf/"), fullFile);
                                        file.SaveAs(fullFile);
                                        objCFU.Path = fullFile;
                                        int a = objBdata.SaveContentFileUpload(objCFU);
                                    }
                                }
                            }
                        }
                        i = i + 1;
                    }
                    int j = objBdata.GetUploadStatus(objBL.SocietyTransID);
                    if (j == 1)
                    {
                        Session["SocietyStatus"] = 11;
                    }
                }
                else
                {
                    TempData["CheckField"] = "Incomplete";
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Unauthorised");
                throw ex;
            }
            return RedirectToAction("BackLogFormL");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CopyofResolution(IEnumerable<HttpPostedFileBase> files, FormCollection collections)
        {
            try
            {
                ByeLawsModel objBL = new ByeLawsModel();
                ContentFileUploadModel objCFU = new ContentFileUploadModel();
                int i = 11;
                lstR = GetRelationship();
                lstMember = GetShareTransferMember();
                lstGD = GetDistrict();
                ViewBag.Relationship = lstR;
                ViewBag.District = lstGD;
                ViewBag.MemberDetail = lstMember;
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
                        byte[] data = Encoding.UTF8.GetBytes(base64ImageRepresentation);
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
                                 objCFU.FormId = 11;
                                    var filename = Path.GetFileNameWithoutExtension(file.FileName);
                                    string extensionName = Path.GetExtension(file.FileName);
                                    var fullFile = filename + GetRandomText() + extensionName;
                                    var dbsavePath = "/pdf/" + file.FileName;
                                    fullFile = Path.Combine(Server.MapPath("~/pdf/"), fullFile);
                                    file.SaveAs(fullFile);
                                    objCFU.Path = fullFile;
                                    int a = objBdata.SaveContentFileUpload(objCFU);                               
                            }
                        }
                    }
                    
                }             
            }           
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Unauthorised");
                throw ex;
            }
            return View("~/Views/BackLog/ShareTransfer.cshtml");
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
            ViewBag.SocietyStatus1 = objBdata.GetSoceityStatus(Convert.ToString(Session["SocietyTransID"]));
            return View();
        }

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
        #endregion
    }
}