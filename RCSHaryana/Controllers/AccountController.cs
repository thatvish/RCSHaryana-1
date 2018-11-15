using RCSData;
using RCSEntities;
using RCSSerivce;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RCSHaryana.Controllers
{
    public class AccountController : Controller
    {
        #region Capctha
        /// <summary>
        /// get random string
        /// </summary>
        /// <returns></returns>
        private string GetRandomText()
        {
            StringBuilder randomText = new StringBuilder();
            string alphabets = "012345679ACEFGHKLMNPRSWXZabcdefghijkhlmnopqrstuvwxyz";
            Random r = new Random();
            for (int j = 0; j <= 3; j++)
            {
                randomText.Append(alphabets[r.Next(alphabets.Length)]);
            }
            return randomText.ToString();
        }

        public FileResult GetCaptchaImage()
        {
            string text = Convert.ToString(Session["Captcha"]);// UserInfo.CitizenInfo.Captcha;

            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            Font font = new Font("Arial", 15);
            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width + 40, (int)textSize.Height + 20);
            drawing = Graphics.FromImage(img);

            Color backColor = Color.SeaShell;
            Color textColor = Color.Red;
            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 20, 10);

            drawing.Save();

            font.Dispose();
            textBrush.Dispose();
            drawing.Dispose();

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            img.Dispose();

            return File(ms.ToArray(), "image/png");
        }
        #endregion

        #region Login
        private void GenerateHashKeyForStore()
        {
            StringBuilder myStr = new StringBuilder();
            myStr.Append(Request.Browser.Browser);
            myStr.Append(Request.Browser.Platform);
            myStr.Append(Request.Browser.MajorVersion);
            myStr.Append(Request.Browser.MinorVersion);
            myStr.Append(Request.LogonUserIdentity.User.Value);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashdata = sha.ComputeHash(Encoding.UTF8.GetBytes(myStr.ToString()));
            Session["BrowserId"] = Convert.ToBase64String(hashdata);
        }

        private void GenerateHashKeyForCheckEveryTime()
        {
            StringBuilder myStr = new StringBuilder();
            myStr.Append(Request.Browser.Browser);
            myStr.Append(Request.Browser.Platform);
            myStr.Append(Request.Browser.MajorVersion);
            myStr.Append(Request.Browser.MinorVersion);
            myStr.Append(Request.LogonUserIdentity.User.Value);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashdata = sha.ComputeHash(Encoding.UTF8.GetBytes(myStr.ToString()));
            //UserInfo.CitizenInfo.BrowserId = Convert.ToBase64String(hashdata);
        }

        private void ClearSessionAndCookies()
        {
            GenerateHashKeyForStore();
            Response.Cookies["yoyo"].Value = "";
            Request.Cookies["yoyo"].Secure = true;
            Session.Remove("yoyo");
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Clear();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        [AllowAnonymous]
        [HttpGet]
        [HandleErrorAttribute]
        public ActionResult Login()
        {
            //RCSEntities.UserInfo.CitizenInfo.ResetValue();
            //ClearSessionAndCookies();
            if (TempData["message"] != null)
            {
                ViewBag.SuccessMessage = "Registered";
            }
            Session["Captcha"] = GetRandomText();
            GetCaptchaImage();
            return View();
        }

        [HttpPost]
        [HandleErrorAttribute]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection collection, string pwd)
        {
            try
            {
                Account objA = new Account();
                LoginAttemptsModels objLAM = new LoginAttemptsModels();

                string clientCaptcha = collection["clientCaptcha"];
                string serverCaptcha = Convert.ToString(Session["Captcha"]);

                if (!clientCaptcha.Equals(serverCaptcha))
                {
                    ViewBag.ShowCAPTCHA = serverCaptcha;
                    ViewBag.CaptchaError = "Sorry, please write exact text as written above.";
                    Session["Captcha"] = GetRandomText();
                    GetCaptchaImage();
                    return View();
                }
                Session["Captcha"] = "";
                CS4HJ obj = new CS4HJ();
                obj.CreatSession();
                if (ModelState.IsValid)
                {
                    EncryptionService objES = new EncryptionService();
                    GetBasicInfo objGBI = new GetBasicInfo();
                    Login objL = new Login
                    {
                        UserName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(collection.Get("username"))
                    };
                    objL.UserName = XCCPrevent.FilterBadchars1(objL.UserName);
                    objL.Salt = objA.GetEncrptedSalt(objL.UserName);
                    if (!string.IsNullOrEmpty(objL.Salt))
                    {
                        GenerateHashKeyForStore();
                        objL.Password = collection.Get("Password");
                        objL.Password = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objL.Password);
                        objL.Password = XCCPrevent.FilterBadchars1(objL.Password);
                        objL.Password = objES.EncryptPassword(objL.Password, objL.Salt);
                        Int64 result = objA.ValidateUser(objL);
                        if (result >= 1)
                        {
                            LoginUserDetails objLUD = new LoginUserDetails();
                            LoginUserDetails objLUDCheckRole = new LoginUserDetails();
                            objLUDCheckRole = objA.GetRoleId(result);
                            if (objLUDCheckRole.Role == 5)
                            {
                                objLUD.Role = 5;
                                objLUD.SocietyTransId = objLUDCheckRole.SocietyTransId;
                                objLUD.BackLogResetStatus = objLUDCheckRole.BackLogResetStatus;
                                objLUD.UserId = objLUDCheckRole.UserId;
                                objLUD.SocietyStatus = objLUDCheckRole.SocietyStatus;
                            }
                            else
                            {
                                objLUD = objA.GetLoginUserDetails(result);
                            }
                            objLAM = objA.GetLoginAttempts(objL.UserName);
                            if (objLAM.IntervalPending <= 5 && objLAM.LoginAttempts >= 2)
                            {
                                int a = 5 - (objLAM.IntervalPending);
                                if (a == 0)
                                {
                                    ViewBag.Err = "you can try login after " + 1 + " mintues";
                                }
                                else
                                {
                                    ViewBag.Err = "you can try login after " + Convert.ToString(5 - (objLAM.IntervalPending)) + " mintues";
                                }
                                Session["Captcha"] = GetRandomText();
                                GetCaptchaImage();
                                return View();
                            }
                            if (objLUD.Role == 1)
                            {
                                //UserInfo.CitizenInfo.Name = objLUD.Name.ToString();
                                //UserInfo.CitizenInfo.RoleId = objLUD.Role;
                                //UserInfo.CitizenInfo.UserId = objLUD.UserId;
                                //UserInfo.CitizenInfo.EncrptedDecruptedKey = "sblw-3hn8-sqoy19";
                                //= Convert.ToInt32(Session["SocietyStatus"]) = objLUD.SocietyStatus;
                                //UserInfo.CitizenInfo.StatusEditable = objLUD.StatusEditable;
                                //UserInfo.CitizenInfo.UserType = objLUD.UserType;
                                //UserInfo.CitizenInfo.FormE = objLUD.FormE;
                                Session["Name"] = objLUD.Name.ToString();
                                Session["RoleId"] = objLUD.Role;
                                Session["UserId"] = objLUD.UserId;
                                Session["EncrptedDecruptedKey"] = "sblw-3hn8-sqoy19";
                                Session["SocietyStatus"] = objLUD.SocietyStatus;
                                Session["StatusEditable"] = objLUD.StatusEditable;
                                Session["UserType"] = objLUD.UserType;
                                if (!string.IsNullOrEmpty(objLUD.SocietyTransId))
                                {
                                    //UserInfo.CitizenInfo.SocietyTransID = objLUD.SocietyTransId;
                                    Session["SocietyTransID"] = objLUD.SocietyTransId;
                                }
                                else
                                {
                                    Session["SocietyTransID"] = "0";
                                }
                                if (objLUD.Total > 0)
                                {
                                    //UserInfo.CitizenInfo.NoOfMembers = objLUD.Total;
                                    Session["NoOfMembers"] = objLUD.Total;
                                }
                                else
                                {
                                    Session["NoOfMembers"] = 0;
                                }
                                Session["FormE"] = objLUD.FormE;
                                return RedirectToAction("Application", "Society");
                            }
                            else if (objLUD.Role == 2)
                            {
                                //UserInfo.CitizenInfo.Name = objLUD.Name.ToString();
                                Session["Name"] = objLUD.Name.ToString();
                                //UserInfo.CitizenInfo.ARCSCode = objLUD.ARCSCode;
                                Session["ARCSCode"] = objLUD.ARCSCode;
                                //UserInfo.CitizenInfo.RoleId = objLUD.Role;
                                Session["RoleId"] = objLUD.Role;
                                //UserInfo.CitizenInfo.UserId = objLUD.UserId;
                                Session["UserId"] = objLUD.UserId;
                                //UserInfo.CitizenInfo.EncrptedDecruptedKey = "sblw-3hn8-sqoy19";
                                Session["EncrptedDecruptedKey"] = "sblw-3hn8-sqoy19";
                                //UserInfo.CitizenInfo.UserType = objLUD.UserType;
                                Session["UserType"] = objLUD.UserType;
                                return RedirectToAction("Dashboard", "ARCS");
                            }
                            else if (objLUD.Role == 3)
                            {
                                //UserInfo.CitizenInfo.Name = objLUD.Name.ToString();
                                Session["Name"] = objLUD.Name.ToString();
                                //Convert.ToInt32(Session["InsceptorCode"]) = objLUD.ARCSCode;
                                Session["InsceptorCode"] = objLUD.ARCSCode;
                                //UserInfo.CitizenInfo.RoleId = objLUD.Role;
                                Session["RoleId"] = objLUD.Role;
                                //UserInfo.CitizenInfo.UserId = objLUD.UserId;
                                Session["UserId"] = objLUD.UserId;
                                //UserInfo.CitizenInfo.UserType = objLUD.UserType;
                                Session["UserType"] = objLUD.UserType;
                                //UserInfo.CitizenInfo.EncrptedDecruptedKey = "sblw-3hn8-sqoy19";
                                Session["EncrptedDecruptedKey"] = "sblw-3hn8-sqoy19";
                                return RedirectToAction("Dashboard", "Inspector");
                            }
                            else if (objLUD.Role == 4)
                            {
                                //UserInfo.CitizenInfo.Name = objLUD.Name.ToString();
                                Session["Name"] = objLUD.Name.ToString();
                                //UserInfo.CitizenInfo.RoleId = objLUD.Role;
                                Session["RoleId"] = objLUD.Role;
                                //UserInfo.CitizenInfo.UserId = objLUD.UserId;
                                Session["UserId"] = objLUD.UserId;
                                //UserInfo.CitizenInfo.EncrptedDecruptedKey = "sblw-3hn8-sqoy19";
                                Session["EncrptedDecruptedKey"] = "sblw-3hn8-sqoy19";
                                //Convert.ToInt32(Session["SocietyStatus"]) = objLUD.SocietyStatus;
                                Session["SocietyStatus"]  = objLUD.SocietyStatus;
                                //UserInfo.CitizenInfo.StatusEditable = objLUD.StatusEditable;
                                Session["StatusEditable"] = objLUD.StatusEditable;
                                //UserInfo.CitizenInfo.UserType = objLUD.UserType;
                                Session["UserType"] = objLUD.UserType;
                            }
                            else if (objLUD.Role == 5)
                            {
                                if (!string.IsNullOrEmpty(objLUD.SocietyTransId))
                                {
                                    //UserInfo.CitizenInfo.SocietyTransID = objLUD.SocietyTransId;
                                    Session["SocietyTransID"] = objLUD.SocietyTransId;
                                }
                                //UserInfo.CitizenInfo.RoleId = objLUD.Role;
                                Session["RoleId"] = objLUD.Role;
                                //UserInfo.CitizenInfo.Name= objLUD.Name.ToString();
                                Session["Name"] = objLUD.Name;
                                //UserInfo.CitizenInfo.UserId= objLUD.UserId;                              
                                Session["UserId"] = objLUD.UserId;
                                //UserInfo.CitizenInfo.UserType= objLUD.UserType;
                                //Convert.ToInt32(Session["SocietyStatus"]) = objLUD.SocietyStatus;
                                //UserInfo.CitizenInfo.BackLogResetStatus = objLUD.BackLogResetStatus;
                                Session["BackLogResetStatus"] = objLUD.BackLogResetStatus;
                                Session["SocietyStatus"] = objLUD.SocietyStatus;
                                //UserInfo.CitizenInfo.EncrptedDecruptedKey = "sblw-3hn8-sqoy19";
                                Session["EncrptedDecruptedKey"] = "sblw-3hn8-sqoy19";
                                if (objLUD.BackLogResetStatus == 0)
                                {
                                    return RedirectToAction("ResetPassword", "BackLog");
                                }
                                else
                                {
                                    return RedirectToAction("Dashboard", "BackLog");
                                }
                            }
                        }
                        else if (result == 2)
                        {
                            Session["Captcha"] = GetRandomText();
                            GetCaptchaImage();
                            ViewBag.Err = "User doesn't exists!";
                            return View();
                        }
                        else
                        {
                            objLAM = objA.GetLoginAttempts(objL.UserName);
                            objLAM.UserId = objL.UserName;
                            if (objLAM.LoginAttempts >= 2)
                            {
                                Session["Captcha"] = GetRandomText();
                                GetCaptchaImage();
                                objLAM.LoginAttempts = objLAM.LoginAttempts + 1;
                                objA.UpdateLoginAttempts(objLAM);
                                ViewBag.Err = "Account has been locked, try after five mintues";
                                return View();
                            }

                            if (objLAM.LoginAttempts >= 1)
                            {
                                objLAM.LoginAttempts = objLAM.LoginAttempts + 1;
                                objA.UpdateLoginAttempts(objLAM);
                            }
                            else
                            {
                                objLAM.LoginAttempts = 1;
                                objA.UpdateLoginAttempts(objLAM);
                            }
                            Session["Captcha"] = GetRandomText();
                            GetCaptchaImage();
                            ViewBag.Err = "invalid credentials!";
                            return View();
                        }
                    }
                    else
                    {
                        Session["Captcha"] = GetRandomText();
                        GetCaptchaImage();
                        ViewBag.Err = "User doesn't exists!";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                Session["Captcha"] = GetRandomText();
                GetCaptchaImage();
                return View();
                throw ex;
            }
        }
        #endregion

        #region LogOut
        //[HttpPost]
        //[HandleErrorAttribute]
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            //RCSEntities.UserInfo.CitizenInfo.ResetValue();
            TempData.Clear();
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Clear();
            ClearSessionAndCookies();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOut(FormCollection fc)
        {
            //RCSEntities.UserInfo.CitizenInfo.ResetValue();
            TempData.Clear();
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Clear();
            ClearSessionAndCookies();
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region UserRegistration
        public ActionResult Registration()
        {
            Session.RemoveAll();
            Session.Clear();
            try
            {
                List<SelectListItem> lstSQM = new List<SelectListItem>();
                List<SelectListItem> lstGD = new List<SelectListItem>();
                lstSQM = GetSecurityQuestions();
                lstGD = GetDistrict();
                ViewBag.SecurityQuestions = lstSQM;
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
            GetBasicInfo objGBI = new GetBasicInfo();
            Account objAcc = new Account();
            //string UserName = objGBI.Encrypt(input, "sblw-3hn8-sqoy19");
            //bool ifuser = objAcc.ValidateUser(input);
            //if (ifuser == false)
            //{
            //    return input;
            //}
            //if (ifuser == true)
            //{
            //    return "Not Available";
            //}
            return "";
        }

        [AllowAnonymous]
        public string GetUserName(string input)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            string UserName = objGBI.Decrypt(input, "sblw-3hn8-sqoy19");
            return UserName;
        }

        [AllowAnonymous]
        public string GetUserNameEncrpted(string input)
        {
            return "sblw-3hn8-sqoy19";
        }

        [AllowAnonymous]
        public string EncrptPWD(string input)
        {
            return "sblw-3hn8-sqoy19";
        }

        [AllowAnonymous]
        public string EncrptPWD1(string input)
        {
            return "sblw-3hn8-sqoy19";
        }
        #endregion

        #region captcha
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomCaptcha(FormCollection form)
        {
            string clientCaptcha = form["clientCaptcha"];
            string serverCaptcha = Session["CAPTCHA"].ToString();
            if (!clientCaptcha.Equals(serverCaptcha))
            {
                ViewBag.ShowCAPTCHA = serverCaptcha;
                ViewBag.CaptchaError = "Sorry, please write exact text as written above.";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Account objA = new Account();
                    Login objL = new Login
                    {
                        UserName = form.Get("username"),
                        Password = form.Get("password")
                    };
                    Int64 result = objA.ValidateUser(objL);
                    if (result == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                    }
                }
            }
            // go ahead and validate username and password
            // string userName = form["username"];
            //string password = form["password"];
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public void CreateSession()
        {
            CS4HJ obj = new CS4HJ();
            obj.CreatSession();
        }
        #endregion

        #region BindDropDowns
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

        #region HTTPPOST
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Registration(FormCollection collection)
        {
            try
            {
                ResgirationModels objRM = new ResgirationModels();
                EncryptionService objES = new EncryptionService();
                Account objAcc = new Account();
                var salt = objES.CreateSalt();
                objRM.Username = collection.Get("UserName");
                string pwd = collection.Get("Password");
                objRM.Password = objES.EncryptPassword(pwd, salt);
                objRM.SecurityQuestionCode = collection.Get("SecurityQuestions");
                objRM.SecurityAnswer = collection.Get("Anwser");
                objRM.Salt = salt;

                objRM.FirstName = collection.Get("Name");
                if (string.IsNullOrEmpty(objRM.FirstName))
                {
                    ModelState.AddModelError("Name", "Please Enter the Name");
                }
                objRM.Gender = collection.Get("Gender");
                objRM.Age = Convert.ToInt32(collection.Get("Age"));
                objRM.Mobile = collection.Get("Mobile");
                objRM.EmailID = collection.Get("Email");
                objRM.Address1 = collection.Get("Address");
                objRM.Address2 = collection.Get("HouseNoSectorNoRoad");
                objRM.PostOffice = collection.Get("PostOffice");
                objRM.PostalCode = collection.Get("PostalCode");
                objRM.DisCode = collection.Get("District");

                objRM.UserTypeCode = 3;
                objRM.Role = 1;
                objRM.CreatedBy = "self";

                objRM.FirstName = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objRM.FirstName);
                objRM.EmailID = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objRM.EmailID);
                objRM.SecurityAnswer = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objRM.SecurityAnswer);
                objRM.Address1 = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objRM.Address1);
                objRM.Address2 = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objRM.Address2);
                objRM.PostOffice = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(objRM.PostOffice);

                objRM.FirstName = XCCPrevent.FilterBadchars1(objRM.FirstName);
                objRM.EmailID = XCCPrevent.FilterBadchars1(objRM.EmailID);
                objRM.SecurityAnswer = XCCPrevent.FilterBadchars1(objRM.SecurityAnswer);
                objRM.Address1 = XCCPrevent.FilterBadchars1(objRM.Address1);
                objRM.Address2 = XCCPrevent.FilterBadchars1(objRM.Address2);
                objRM.PostOffice = XCCPrevent.FilterBadchars1(objRM.PostOffice);
                if (ModelState.IsValid)
                {
                    int result = objAcc.SaveResgiratedUser(objRM);
                    if (result == 1)
                    {                        
                        TempData["message"] = "Registered";
                        return RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Unauthorised");
                throw ex;
            }
            return View();
        }
        #endregion
    }
}