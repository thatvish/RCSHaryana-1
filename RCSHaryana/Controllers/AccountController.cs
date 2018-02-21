using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RCSEntities;
using RCSData;
using CaptchaMvc.HtmlHelpers;
using System.Web.Security;
using RCSSerivce;

namespace RCSHaryana.Controllers
{
    public class AccountController : Controller
    {
        #region Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                if (ModelState.IsValid)
                {
                    EncryptionService objES = new EncryptionService();
                    Account objA = new Account();
                    Login objL = new Login();
                    objL.UserName = collection.Get("username");
                    objL.Salt = objA.GetEncrptedSalt(objL.UserName);
                    if (!string.IsNullOrEmpty(objL.Salt))
                    {
                        objL.Password = objES.EncryptPassword(collection.Get("Password"), objL.Salt);
                        int result = objA.ValidateUser(objL);
                        if (result >= 1)
                        {
                            LoginUserDetails objLUD = new LoginUserDetails();
                            objLUD = objA.GetLoginUserDetails(result);
                            if (objLUD.Role == "Citizen")
                            {
                                Session["Name"] = objLUD.Name.ToString();
                                Session["Role"] = objLUD.Role;
                                Session["Salt"] = objLUD.Salt;
                                Session["UserType"] = objLUD.UserType;
                                Session["UserId"] = objLUD.UserId;
                                return RedirectToAction("Application", "Society");
                            }
                        }
                        else if (result == 2)
                        {
                            ViewBag.Err = "User doesn't exists!";
                            return View();
                        }
                        else
                        {
                            ViewBag.Err = "invalid credentials!";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Err = "User doesn't exists!";
                        return View();
                    }
                }
            }
            ViewBag.ErrMessage = "Error: captcha is not valid.";
            return View();
        }
        #endregion

        #region LogOut
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region UserRegistration
        public ActionResult Registration()
        {
            List<SelectListItem> lstSQM = new List<SelectListItem>();
            List<SelectListItem> lstGD = new List<SelectListItem>();
            lstSQM = GetSecurityQuestions();
            lstGD = GetDistrict();
            ViewBag.SecurityQuestions = lstSQM;
            ViewBag.District = lstGD;
            return View();
        }

        [HttpPost]
        public JsonResult doesUserNameExist(string UserName)
        {
            var user = Membership.GetUser(UserName);
            return Json(user == null);
        }

        public JsonResult check(FormCollection form)
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
                    Login objL = new Login();
                    objL.UserName = form.Get("username");
                    objL.Password = form.Get("password");
                    int result = objA.ValidateUser(objL);
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
        public ActionResult Registration(FormCollection collection)
        {
            ResgirationModels objRM = new ResgirationModels();
            EncryptionService objES = new EncryptionService();
            Account objAcc = new Account();
            var salt = objES.CreateSalt();
            objRM.Username = collection.Get("UserName");
            objRM.Password = objES.EncryptPassword(collection.Get("Password"), salt); 
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

            objRM.DistrictOfOperation = Convert.ToInt32(collection.Get("DistrictOfOperation"));
            objRM.ARCSCode = collection.Get("ARCSOffice");

            objRM.SocietyStatus = collection.Get("SocietyStatus");
            objRM.SocietyRegistrationNo = collection.Get("SocietyRegisteredNo");
            if (string.IsNullOrEmpty(objRM.SocietyRegistrationNo))
            {
                objRM.SocietyRegistrationNo = "";
            }

            objRM.UserTypeCode = 3;
            objRM.Role = "Citizen";
            objRM.CreatedBy = "self";

            if (ModelState.IsValid)
            {
                int result = objAcc.SaveResgiratedUser(objRM);
                if (result == 1)
                {
                    return RedirectToAction("Login", "Account");
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