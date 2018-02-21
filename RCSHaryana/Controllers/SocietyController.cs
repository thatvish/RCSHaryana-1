using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RCSData;
using RCSEntities;
using RCSSerivce;

namespace RCSHaryana.Controllers
{
    public class SocietyController : Controller
    {
        #region ClassObjects
        GetBasicInfo objGBI = new GetBasicInfo();
        SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
        #endregion

        #region GetActionResult
        public ActionResult Application()
        {
            List<SelectListItem> lstGD = new List<SelectListItem>();
            List<SelectListItem> lstGCS = new List<SelectListItem>();
            List<SelectListItem> lstR = new List<SelectListItem>();
            List<SelectListItem> lstO = new List<SelectListItem>();
            List<SelectListItem> lstMCDCM = new List<SelectListItem>();

            lstGD = GetDistrict();
            lstGCS = GetClassOfSociety();
            lstO = GetOccupations();
            lstR = GetRelationship();
            lstMCDCM = GetMemberCommDesignation();

            ViewBag.District = lstGD;
            ViewBag.CommitteMemberDistrict = lstGD;
            ViewBag.ClassOfSociety = lstGCS;
            ViewBag.Occupations = lstO;
            ViewBag.Relationship = lstR;
            ViewBag.MemberCommDesignation = lstMCDCM;

            ViewBag.tabResult = 0;
            Session["SocietyTransID"] = "02192018151828";
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
            bool Status = Verhoeff.validateVerhoeff(input);
            string s = "";
            if (Status == true)
            {
                s = "true";
            }
            return s;
        }
        #endregion

        #region PostActionResult
        [HttpPost]
        public ActionResult Application(FormCollection collection)
        {
            int result = 0;
            List<SelectListItem> lstGD = new List<SelectListItem>();
            List<SelectListItem> lstGCS = new List<SelectListItem>();
            List<SelectListItem> lstMCDCM = new List<SelectListItem>();

            lstGD = GetDistrict();
            lstGCS = GetClassOfSociety();
            lstMCDCM = GetMemberCommDesignation();

            ViewBag.District = lstGD;
            ViewBag.ClassOfSociety = lstGCS;
            List<SelectListItem> lstR = new List<SelectListItem>();
            List<SelectListItem> lstO = new List<SelectListItem>();
            lstR = GetRelationship();
            lstO = GetOccupations();
            ViewBag.Relationship = lstR;
            ViewBag.Occupations = lstO;
            ViewBag.MemberCommDesignation = lstMCDCM;
            ViewBag.CommitteMemberDistrict = lstGD;

            SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
            SocietySubmissionFromModels objSSFM = new SocietySubmissionFromModels();
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
            objSSFM.OccupationOfMember = Convert.ToInt32(collection.Get("Occupationofmembers"));
            objSSFM.CateOfSociety = collection.Get("Categoryofsociety");
            objSSFM.DebtsOfMembers = collection.Get("Estimatedunsecureddebtsofmembers");
            objSSFM.AreaMortgaged = collection.Get("AreaMortgagedbymembers");
            objSSFM.DetailsOfShares = collection.Get("detailsofshares");
            objSSFM.ValueOfShare = collection.Get("Valueofashare");
            objSSFM.ModeOfPayment = collection.Get("ModeofPayment");
            objSSFM.UserId = Convert.ToInt32(collection.Get("UserId"));
            objSSFM.SocietyTransID = DateTime.Now.ToString(("MMddyyyyHHmmss"));
            Session["SocietyTransID"] = objSSFM.SocietyTransID;
            result = objSSF.SaveSocietySubmissionFrom(objSSFM);
            if (result == 1)
            {
                ViewBag.show = "Draft saved successfully.";
                ViewBag.result = "1";
                ViewBag.tabResult = "1";
            }
            else
            {
                ViewBag.show = "Draft not save successfully.";
                ViewBag.result = "0";
                ViewBag.tabResult = "0";
            }
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
                return Json(objSSF.ManagingCommitteMembersList(SocietyTransID), JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteManagingCommitteMember(int SocietyMemberID)
        {
            return Json(objSSF.DeleteManagingCommitteMember(SocietyMemberID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getbyManagingCommitteeMemberID(int SocietyMemberID)
        {
            var ManagingCommitteeMember = objSSF.GetManagingCommitteeMember(SocietyMemberID);
            return Json(ManagingCommitteeMember, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddManagingCommitteMember(MembershipDetailsModels objMDM)
        {
            objMDM.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            if (!string.IsNullOrEmpty(objMDM.SocietyTransID))
            {
                objMDM.AadharNo = objGBI.Encrypt(objMDM.AadharNo, "sblw-3hn8-sqoy19");
                objMDM.SocietyMemberID = 0;
                return Json(objSSF.SaveSocietyMemberDetails(objMDM), JsonRequestBehavior.AllowGet);
            }
            return Json("Kindly fill the first form then you can add managing committe members", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateManagingCommitteMember(MembershipDetailsModels objMDM)
        {
            objMDM.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            objMDM.AadharNo = objGBI.Encrypt(objMDM.AadharNo, "sblw-3hn8-sqoy19");
            return Json(objSSF.SaveSocietyMemberDetails(objMDM), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SocietyMemberDetailMethods
        public JsonResult AddSocietyMember(MemberFurtherDetails objMFD)
        {
            objMFD.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            if (!string.IsNullOrEmpty(objMFD.SocietyTransID))
            {
                objMFD.AadharNo = objGBI.Encrypt(objMFD.AadharNo, "sblw-3hn8-sqoy19");
                objMFD.MemberSNo = 0;
                return Json(objSSF.SaveMemberFurtherDetails(objMFD), JsonRequestBehavior.AllowGet);
            }
            return Json("Kindly fill the first form then you can add committe members", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SocietyMembersList()
        {
            var SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            if (!string.IsNullOrEmpty(SocietyTransID))
            {
                return Json(objSSF.SocietyMembersList(SocietyTransID), JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult getbySocietyMemberID(int MemberSNo)
        {
            var SocietyMemberDetail = objSSF.GetbySocietyMemberID(MemberSNo);
            return Json(SocietyMemberDetail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateSocietyMemberDetail(MemberFurtherDetails objMFD)
        {
            objMFD.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            objMFD.AadharNo = objGBI.Encrypt(objMFD.AadharNo, "sblw-3hn8-sqoy19");
            return Json(objSSF.SaveMemberFurtherDetails(objMFD), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSocietyMember(MemberFurtherDetails objMFD)
        {
            return Json(objSSF.DeleteSocietyMember(objMFD.MemberSNo), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region FormEDetails
        public ActionResult FormE()
        {
            FormEModels objFEM = new FormEModels();
            objFEM = objSSF.GetDetailsForFormE(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.Address = objFEM.Address;
            ViewBag.District = objFEM.District;
            ViewBag.HouseNo = objFEM.HouseNo;
            ViewBag.RelationshipName = objFEM.RelationshipName;
            ViewBag.SectorStreet = objFEM.SectorStreet;
            ViewBag.SocietyMemberName = objFEM.SocietyMemberName;
            ViewBag.SocietyName = objFEM.SocietyName;
            return View();
        }
        #endregion
        #region FormAdetails
        public ActionResult FormA()
        {
            FormAmodels objFAM = new FormAmodels();
            objFAM = objSSF.GetDetailsForFormA(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.Address1 = objFAM.Address1;
            ViewBag.Address2 = objFAM.Address2;
            ViewBag.District = objFAM.District;
            ViewBag.DateofApplicationReceived = objFAM.DateofApplicationReceived;
            ViewBag.SocietyName = objFAM.SocietyName;
        return View();
        }



        #endregion

        #region downloadformE

        #endregion

    }
}