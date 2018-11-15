using RCSData;
using RCSEntities;
using RCSSerivce;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace RCSHaryana.Controllers
{
    public class DownloadsController : Controller
    {
        #region ClassObjects
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

        // GET: Downloads
        public ActionResult Index()
        {
            return View();
        }

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
            FormEDModels objFEM1 = new FormEDModels();
            objFEM = objSSF.GetDetailsForFormD(Convert.ToString(Session["SocietyTransID"]));
            objFEM1 = objSSF.FormEDetails(Convert.ToString(Session["SocietyTransID"]));
            
            ViewBag.Address = objFEM1.Address;
            ViewBag.District = objFEM1.District;
            ViewBag.HouseNo = objFEM1.Address2;
            ViewBag.RelationshipName = objFEM1.RelationshipName;
            ViewBag.RelationshipMemberName = objFEM1.RelationshipMemberName;
            ViewBag.PostOffice = objFEM1.PostOffice;
            ViewBag.SocietyMemberName = objFEM1.SocietyMemberName;
            ViewBag.SocietyName = objFEM.SocietyName;
            ViewBag.FormEValue = Convert.ToInt16(Session["FormE"]);
            return View();
        }

        [HttpPost]
        public ActionResult FormE(FormCollection fc)
        {
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
                int j = objSSF.GetFormE(Convert.ToString(Session["SocietyTransID"]));
                if (j == 1)
                {
                    Session["FormE"] = 1;
                    return RedirectToAction("Form", "Society");
                }
                else
                {
                    Session["FormE"] = 0;
                }
            }
            return View();
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
            ViewBag.Address2 = objFAM.Address2;
            ViewBag.District = objFAM.District;
            ViewBag.DateofApplicationReceived = objFAM.DateofApplicationReceived.ToShortDateString();
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
            objFEM = objSSF.GetDetailsForFormD(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.Address = objFEM.Address;
            ViewBag.District = objFEM.District;
            ViewBag.HouseNo = objFEM.Address2;
            ViewBag.RelationshipName = objFEM.RelationshipName;
            ViewBag.RelationshipMemberName = objFEM.RelationshipMemberName;
            ViewBag.PostOffice = objFEM.PostOffice;
            ViewBag.SocietyMemberName = objFEM.SocietyMemberName;
            ViewBag.SocietyName = objFEM.SocietyName;
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
        public ActionResult FormC()
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
            ViewBag.lstFormc = objSSF.ManagingCommitteMembersList(Convert.ToString(Session["SocietyTransID"]));
            FormAmodels objFAM = new FormAmodels();
            FormEDModels objFEM1 = new FormEDModels();
            objFAM = objSSF.GetDetailsForFormA(Convert.ToString(Session["SocietyTransID"]));
            objFEM1 = objSSF.FormEDetails(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.MeetingDate = objFAM.MeetingDate.ToShortDateString();
            ViewBag.ARCSName = objFAM.ARCSName;
            ViewBag.CustodianFatherName = objFEM1.CustodianFatherName;
            ViewBag.CustodianName = objFEM1.CustodianName;
            ViewBag.CustodianRelationName = objFEM1.RelationshipName;
            ViewBag.Address1 = objFAM.Address1;
            ViewBag.Address2 = objFAM.Address2;
            ViewBag.District = objFAM.District;
            ViewBag.BankName = objFAM.BankName;
            ViewBag.PostOffice = objFAM.PostOffice;
            ViewBag.DateofApplicationReceived = objFAM.DateofApplicationReceived;
            ViewBag.ShareMoney = objFAM.ShareMoney;
            ViewBag.Deposit = objFAM.Deposit;
            ViewBag.AdminssionFee = objFAM.AdmissionFee;
            ViewBag.Pin = objFAM.Pin;
            ViewBag.SocietyName = objFAM.SocietyName;
            ViewBag.Total = objFAM.Total;
            FormEDModels objFEM = new FormEDModels();
            objFEM = objSSF.GetDetailsForFormE(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.SocietyMemberName = objFEM.SocietyMemberName;
            return View();
        }
        #endregion

        public ActionResult FormL()
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
            ViewBag.FormL = objSSF.NewSocietyMembersList(Convert.ToString(Session["SocietyTransID"]));
            return View();
        }
    }
}