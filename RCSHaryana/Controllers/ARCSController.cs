using Microsoft.Reporting.WebForms;
using OfficeOpenXml;
using RCSData;
using RCSEntities;
using RCSSerivce;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using PagedList.Mvc;
using PagedList;


namespace RCSHaryana.Controllers
{
    public class ARCSController : Controller
    {
        // GET: ARCS
        ARCSData objARCSD = new ARCSData();
        CS4HJ obj = new CS4HJ();
        BackLogData objBdata = new BackLogData();
        GetBasicInfo objGBI = new GetBasicInfo();

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

        [RBAC]
        public ActionResult Dashboard()
        {
            ARCSSocietyStatusModels objbacklog = new ARCSSocietyStatusModels();
            objbacklog = objARCSD.BackLogGetARCSApplicatonCountDetails(Convert.ToInt32(Session["ARCSCode"]));
            ViewBag.Totalbacklog = objbacklog.Total;
            ARCSSocietyStatusModels objARCSSM = new ARCSSocietyStatusModels();
            objARCSSM = objARCSD.GetARCSApplicatonCountDetails(Convert.ToInt32(Session["ARCSCode"]));
            ViewBag.Total = objARCSSM.Total;
            ViewBag.TotalApprove = objARCSSM.TotalApprove;
            ViewBag.TotalHold = objARCSSM.TotalHold;
            ViewBag.TotalPending = objARCSSM.TotalPending;
            ViewBag.TotalReject = objARCSSM.TotalReject;
            ViewBag.TotalForwardedToInspector = objARCSSM.TotalForwardToInspector;
            ViewBag.TotalApplicationComesFromInspector = objARCSSM.TotalApplicationComesFromInspector;
            return View();
        }

        public JsonResult ARCSDashBoardSocietyList()
        {
            return Json(objARCSD.ARCSDashBoardSocietyList(Convert.ToInt32(Session["ARCSCode"])), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSocietyDetails(string SocietyTransID)
        {
            Session["SocietyTransID"] = SocietyTransID;
            return Json(objARCSD.GetSocietyDetails(SocietyTransID), JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> GetSocietyList()
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

        public List<SelectListItem> GetSocietyListForApproval()
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetSocietyListForApproval(Convert.ToInt32(Session["ARCSCode"])))
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.SocietyName),
                    Value = Convert.ToString(item.SocietyTransId)
                });
            }
            return items;
        }

        public List<SelectListItem> GetApprovedSocieties()
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in objGBI.GetApprovedSocieties(Convert.ToInt32(Session["ARCSCode"])))
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
        public ActionResult GetPdf(string fileName,string Value)
        {
            string path = "";

            if (Value == "ByeLaws")
            {
                path = objARCSD.GetPathofForm(Convert.ToString(Session["SocietyTransID"]));
            }
            if (Value =="BankReceipt")
            {
                path = objARCSD.GetPathofBankReceipt(Convert.ToString(Session["SocietyTransID"]));
            }
            if (Value == "BackLogFormL")
            {
                path = objARCSD.GetPathofBackLogFormL(Convert.ToString(Session["SocietyTransID"]));
            }
            if (Value == "CopyOfResolution")
            {
                path = objARCSD.GetPathofCopyofResolution(Convert.ToString(Session["SocietyTransID"]));
            }
            if (string.IsNullOrEmpty(path))
            {
                ViewBag.BlankSoceity = "Yes";
                //return RedirectToAction("BackLogMemberDetail", "BackLogOfficer");
                return View("~/Views/Shared/_ViewDocsForBackLog.cshtml");
            }
            //var fullFile = Path.Combine(Server.MapPath("~/pdf/"), fileName +".pdf");
            var fileStream = new FileStream(path,
                                             FileMode.Open,
                                             FileAccess.Read
                                           );
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            return fsResult;
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
        public ActionResult CopyofResolution(IEnumerable<HttpPostedFileBase> files, FormCollection collections)
        {
            try
            {
                List<SelectListItem> lstR = new List<SelectListItem>();
                List<SelectListItem> lstGD = new List<SelectListItem>();
                List<SelectListItem> lstMember = new List<SelectListItem>();
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
            return RedirectToAction("BackLogMemberDetail","BackLogOfficer");
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
        [RBAC]
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
            lstSL = GetSocietyList();
            ViewBag.lstSL = lstSL;
            lstI = GetInspectorList();
            ViewBag.lstI = lstI;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SocietyMemberDetails(FormCollection fc)
        {
            try
            {
                List<SelectListItem> lstSL = new List<SelectListItem>();
                List<SelectListItem> lstI = new List<SelectListItem>();
                ARCSData objARCSD = new ARCSData();
                ForwardToIncepector objFTI = new ForwardToIncepector();
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
                int result = objARCSD.ForwardToIncepector(objFTI);
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

        public JsonResult ManagingCommitteMembersList(string SocietyTransID)
        {
            SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
            Session["SocietyTransID"] = SocietyTransID;
            if (!string.IsNullOrEmpty(SocietyTransID))
            {
                return Json(objSSF.ManagingCommitteMembersList(SocietyTransID), JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult SocietyMemberRemark(string SocietyTransID)
        {
            ARCSData objARCSD = new ARCSData();
            Session["SocietyTransID"] = SocietyTransID;
            if (!string.IsNullOrEmpty(SocietyTransID))
            {
                return Json(objARCSD.GetDetailsForRemark(SocietyTransID), JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult SocietyMembersList(string SocietyTransID)
        {
            SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
            if (!string.IsNullOrEmpty(SocietyTransID))
            {
                return Json(objSSF.SocietyMembersList(SocietyTransID), JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        [RBAC]
        public ActionResult PendingAction()
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
            obj.CreatSession();
            List<SelectListItem> lstSL = new List<SelectListItem>();
            List<SelectListItem> lstI = new List<SelectListItem>();
            lstSL = GetSocietyListForApproval();
            ViewBag.lstSL = lstSL;
            return View();
        }

        [HttpPost]
        public ActionResult PendingAction(FormCollection fc)
        {
            List<SelectListItem> lstSL = new List<SelectListItem>();
            List<SelectListItem> lstI = new List<SelectListItem>();

            ForwardToIncepector objFTI = new ForwardToIncepector();
            if (string.IsNullOrEmpty(fc.Get("Action")))
            {
                ViewBag.show = "Kindly Select Status";
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
            objFTI.Status = Convert.ToInt32(fc.Get("Action"));
            objFTI.OfficerCode = Convert.ToInt32(Session["ARCSCode"]);
            objFTI.SocietyTransId = Convert.ToString(Session["SocietyTransID"]);
            objFTI.Remarks = fc.Get("ARCSRemarks").ToString();
            if (!string.IsNullOrEmpty(fc.Get("HearingDate").ToString()))
            {
                objFTI.HearingDate = Convert.ToDateTime(fc.Get("HearingDate").ToString());
            }
            else
            {
                objFTI.HearingDate = null;
            }

            int result = objARCSD.OfficerHearingAndApprovalStatus(objFTI);
            if (result >= 1)
            {
                ViewBag.show = "Society Status Has Been Changed";
                ViewBag.result = "1";
                ViewBag.tabResult = "1";
            }
            else
            {
                ViewBag.show = "Society Status Has Been not Changed";
                ViewBag.result = "0";
                ViewBag.tabResult = "0";
            }

            lstSL = GetSocietyListForApproval();
            ViewBag.lstSL = lstSL;

            return View();
        }

        public JsonResult GetbyManagingCommitteeMemberID(int SocietyMemberID)
        {
            SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
            var ManagingCommitteeMember = objSSF.GetManagingCommitteeMember(SocietyMemberID);
            return Json(ManagingCommitteeMember, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbySocietyMemberID(int MemberSNo)
        {
            SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
            var SocietyMemberDetail = objSSF.GetbySocietyMemberID(MemberSNo);
            return Json(SocietyMemberDetail, JsonRequestBehavior.AllowGet);
        }

        private DataTable ConvertListToDataTable(List<string[]> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }

            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }

            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }

            return table;
        }

        protected void GridView2_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox check = (CheckBox)e.Row.FindControl("CheckBox1");
                CheckBox check2 = (CheckBox)e.Row.FindControl("CheckBox2");
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                int accesType = row.Field<int>("AccessType");
                check.Checked = accesType == 1;
                check2.Checked = accesType == 2;
            }
        }


        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public ActionResult ApprovedSociety()
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

            lstSL = GetApprovedSocieties();
            ViewBag.lstSL = lstSL;
            return View();
        }

        [HttpPost]
        public ActionResult ApprovedSociety(FormCollection fc)
        {
            List<SelectListItem> lstSL = new List<SelectListItem>();

            ARCSData objARCSD = new ARCSData();
            SocietySubmissionFromModels objSSFM = new SocietySubmissionFromModels();
            var SocietyTransID = fc.Get("SocietyList").ToString();
            Session["SocietyTransID"] = SocietyTransID;
            objSSFM = objARCSD.GetApprovedSocietyDetails(SocietyTransID);
            ViewBag.SocietyTransID = objSSFM.SocietyTransID;
            ViewBag.SocietyName = objSSFM.SocietyName;
            lstSL = GetApprovedSocieties();
            ViewBag.lstSL = lstSL;
            return View();
        }

        public static DataTable ConvertListToDataTable1(List<string[]> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }

            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }

            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }

            return table;
        }

        public ActionResult Registrationcertificate()
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
            SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
            objFAM = objSSF.GetDetailsForFormA(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);
            ViewBag.Address1 = objFAM.Address1;
            ViewBag.Address2 = objFAM.Address2;
            ViewBag.District = objFAM.District;
            ViewBag.SocietyName = objFAM.SocietyName;
            ViewBag.ARCSName = objFAM.ARCSName;
            ViewBag.ARCSDesignation = objFAM.ARCSDesignation;
            return View();
        }
        [HttpGet]
        public ActionResult GeneratePassword()
        {
            string ARCSCode = Convert.ToString(Session["ARCSCode"]);
            ViewBag.ShowGrid = objARCSD.GetUploadedData(ARCSCode);
            var count = ViewBag.ShowGrid.Count;
            ViewBag.TotalCount = count;
            return View();
        }
        public ActionResult Report(string id, string value)
        {
            string path = "";
            LocalReport lr = new LocalReport();
            if (value == "BackLogCredential")
            {
                path = Path.Combine(Server.MapPath("~/Reports/BackLogCredential.rdlc"));
            }            
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return RedirectToAction("Error", "Unauthorised");
            }           
            if (value == "BackLogCredential")
            {
                DataSet ds1 = new DataSet();               
                ds1 = objARCSD.ReportBackLogCredential();               
                DataTable FormG = ds1.Tables[0];
                ReportDataSource rd = new ReportDataSource("GetPrintData", FormG);
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
        public ActionResult PrintPassword()
        {                    
            string ARCSCode = Convert.ToString(Session["ARCSCode"]);
            ViewBag.ShowGrid = objARCSD.GetPrintData(ARCSCode);
            var count = ViewBag.ShowGrid.Count;
            ViewBag.TotalCount = count;
            return View();
        }
        public ActionResult ListPassword(int? page)
        {

            int pagesize = 10;
            int pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            string ARCSCode = Convert.ToString(Session["ARCSCode"]);
            ViewBag.ShowGrid = objARCSD.GetPrintData(ARCSCode);
            var count = ViewBag.ShowGrid.Count;
            IPagedList<ImportExportModels> objlist = null;
            ImportExportModels objexp = new ImportExportModels();
            List<ImportExportModels> objlisting = new List<ImportExportModels>();
            objlisting = objARCSD.GetPrintData(ARCSCode);
            objexp.Objimport = objlisting;
            objlist = objlisting.ToPagedList(pageindex, pagesize);           
            ViewBag.ShowGrid = objlist;
            ViewBag.TotalCount = count;
            return View(objlist);
        }
        public JsonResult SaveGenerateDetail(encryptedDetail _encryptedDetail)
        {
            int result = 0 ;
            EncryptionService objES = new EncryptionService();
            var salt = objES.CreateSalt();
            _encryptedDetail.salt = salt;
            _encryptedDetail.createdBy = Convert.ToString(Convert.ToInt32(Session["ARCSCode"]));
            _encryptedDetail.encryptPwd = objES.EncryptPassword(_encryptedDetail.encryptPwd, salt);
            if (ModelState.IsValid)
            {
                if (_encryptedDetail.RegNo != null && _encryptedDetail.encryptPwd != null && _encryptedDetail.encryptUserName != null)
                {
                   result = objARCSD.SaveBckCredential(_encryptedDetail);
                }
            }        
             return Json(result, JsonRequestBehavior.AllowGet);                    
        }
        public JsonResult GetPasswordList()
        {
            string ARCSCode = Convert.ToString(Session["ARCSCode"]);
            if (!string.IsNullOrEmpty(ARCSCode))
            {
                var getRecord = objARCSD.GetUploadedData(ARCSCode);
                var counter = getRecord.Count;              
                var result = new { getRecord, counter };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            Session["AddedMember"] = 0;
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public string CheckUserName(string input)
        {
            GetBasicInfo objGBI = new GetBasicInfo();
            Account objAcc = new Account();
            //string UserName = objGBI.Encrypt(input, "sblw-3hn8-sqoy19");
            bool ifuser = objAcc.ValidateUser(input);
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
        [HttpPost]
        public ActionResult GeneratePasswordBackLog(int i)
        {
            EncryptionService objES = new EncryptionService();
            List<ImportExportModels> lst = new List<ImportExportModels>();
            string ARCSCode = Convert.ToString(Session["ARCSCode"]);
            lst = objARCSD.GetUploadedData(ARCSCode);
            foreach (var item in lst)
            {
                string OldRedgNo = item.OldRedgNo;
                var salt = objES.CreateSalt();
                string pwd = GetRandomText();
                string encryptPwd = objES.EncryptPassword(pwd, salt);               
                string userName = GetRandomText();
                string dbUserName = CheckUserName(userName);
                userName = GetRandomText();
                if (dbUserName != "Not Available")
                {

                }
                else
                {
                  userName = GetRandomText();
                }
            }
            return View();
        }
        public ActionResult ApprovedSocieties()
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
            SocietySubmissionFrom objSSF = new SocietySubmissionFrom();
            objFAM = objSSF.GetDetailsForFormA(Convert.ToString(Session["SocietyTransID"]));
            ViewBag.SocietyTransID = Convert.ToString(Session["SocietyTransID"]);   
            ViewBag.Address1 = objFAM.Address1;
            ViewBag.Address2 = objFAM.Address2;
            ViewBag.District = objFAM.District;
            ViewBag.SocietyName = objFAM.SocietyName;
            ViewBag.ARCSName = objFAM.ARCSName;
            ViewBag.ARCSDesignation = objFAM.ARCSDesignation;
            List<SelectListItem> lstSL = new List<SelectListItem>();
            List<SelectListItem> lstI = new List<SelectListItem>();
            lstSL = GetSocietyListForApproval();
            ViewBag.lstSL = lstSL;
            return View();
        }
        #region GenerateUserDetails
        [HttpGet]
        public ActionResult CreatePasswordBackLog()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreatePasswordBackLog(FormCollection fc)
        {
            UserCredential userCredential = new UserCredential();
            SeaechDetail SeaechDetail = new SeaechDetail();
            string ARCSCode = Convert.ToString(Session["ARCSCode"]);
            SeaechDetail.RegistrationNo = fc.Get("oldReg");
            SeaechDetail.SocietyName = fc.Get("SocietyName");            
            userCredential = objARCSD.GetGeneratedDetail(SeaechDetail);
            return View();
        }
       #endregion

        #region BackLog Section 

        #region Upload Section
        [HttpGet]
        public ActionResult UploadExcel()
        {
            //if (UserInfo.CitizenInfo.BrowserId != GenerateHashKeyForCheckBroswerEveryCall())
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            //int i = obj.CheckSessionEveryCall();
            //if (i != 0)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            ARCSData objARCSD = new ARCSData();
            //var store = ARCSData.GetPassword();
            return View();
        }


        //public DataTable getDataTableFromExcel()
        //{
        //    string path = Server.MapPath("~/Upload/");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    using (var pck = new OfficeOpenXml.ExcelPackage())
        //    {
        //        using (var stream = File.OpenRead(path))
        //        {
        //            pck.Load(stream);
        //        }
        //        var ws = pck.Workbook.Worksheets.First();
        //        DataTable tbl = new DataTable();
        //        bool hasHeader = true; // adjust it accordingly( i've mentioned that this is a simple approach)
        //        foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
        //        {
        //            tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
        //        }
        //        var startRow = hasHeader ? 2 : 1;
        //        for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
        //        {
        //            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
        //            var row = tbl.NewRow();
        //            foreach (var cell in wsRow)
        //            {
        //                row[cell.Start.Column - 1] = cell.Text;
        //            }
        //            tbl.Rows.Add(row);
        //        }
        //        return tbl;
        //    }
        //}

        //public ActionResult Upload(FormCollection formCollection)
        //{
        //    if (Request != null)
        //    {
        //        HttpPostedFileBase file = Request.Files["UploadedFile"];
        //        if ((file != null) && (file.ContentLength >= 0) && !string.IsNullOrEmpty(file.FileName))   
        //{
        //            string fileName = file.FileName;
        //            string fileContentType = file.ContentType;
        //            byte[] fileBytes = new byte[file.ContentLength];
        //            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
        //            var usersList = new List & lt;
        //            Users & gt;
        //            ();
        //            using (var package = new ExcelPackage(file.InputStream))
        //            {
        //                var currentSheet = package.Workbook.Worksheets;
        //                var workSheet = currentSheet.First();
        //                var noOfCol = workSheet.Dimension.End.Column;
        //                var noOfRow = workSheet.Dimension.End.Row;
        //                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)   
        //        {
        //                    var user = new Users();
        //                    user.FirstName = workSheet.Cells[rowIterator, 1].Value.ToString();
        //                    user.LastName = workSheet.Cells[rowIterator, 2].Value.ToString();
        //                    usersList.Add(user);
        //                }
        //            }
        //        }
        //    }
        //    return View("Index");
        //}

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase postedFile)
        {

 

            try
            {
                List<ImportExportModels> lstIED = new List<ImportExportModels>();
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Upload/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    if (extension.ToLower() == ".xls" || extension.ToLower() == ".xlsx")
                    {

                        postedFile.SaveAs(filePath);

                        string conString = string.Empty;
                        string excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
                        switch (extension)
                        {
                            case ".xls": //Excel 97-03.
                                conString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                                break;
                            case ".xlsx": //Excel 07 and above.
                                conString = excelConnectionString;// ConfigurationManager.ConnectionStrings["excelConnectionString"].ConnectionString;
                                break;
                        }

                        DataTable dt = new DataTable();
                        conString = string.Format(conString, filePath);

                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();
                                }
                            }
                        }

                        var CurrentDate = DateTime.Now.ToShortDateString();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ImportExportModels objIEM = new ImportExportModels();
                            try
                            {
                                int count = dt.Rows.Count;
                                ViewBag.totalRecord = count - 1;
                                if (i == 37)
                                {

                                }

                                var id = dt.Rows[i][0].ToString();
                                objIEM.SocietyName = dt.Rows[i][0].ToString() == "" ? null : dt.Rows[i][0].ToString();
                                objIEM.OldRedgNo = dt.Rows[i][1].ToString() == "" ? null : dt.Rows[i][1].ToString();
                                objIEM.CreateDate = dt.Rows[i][2].ToString() == "" ? (DateTime?)null : Convert.ToDateTime(dt.Rows[i][2].ToString());
                                objIEM.CreateDate1 = dt.Rows[i][2].ToString() == "" ? null : dt.Rows[i][2].ToString();
                                objIEM.FunctionalorWinding = "w";
                                objIEM.Kind = dt.Rows[i][3].ToString() == "" ? null : dt.Rows[i][3].ToString();
                                objIEM.GH_No = dt.Rows[i][4].ToString() == "" ? null : dt.Rows[i][4].ToString();
                                objIEM.Sector = dt.Rows[i][5].ToString() == "" ? null : dt.Rows[i][5].ToString();
                                if (objIEM.CreateDate == null && objIEM.SocietyName == null && objIEM.OldRedgNo == null && objIEM.FunctionalorWinding == null && objIEM.Kind == null && objIEM.Sector == null && objIEM.GH_No == null)
                                {
                                    objIEM = null;
                                }
                                if (!(objIEM == null))
                                {
                                    if (objIEM.CreateDate == null || objIEM.SocietyName == null || objIEM.OldRedgNo == null || objIEM.FunctionalorWinding == null || objIEM.Kind == null || objIEM.Sector == null || objIEM.GH_No == null)
                                    {
                                        var IsPendingEntry = objARCSD.BackLogPendingRecord(objIEM);
                                        ViewBag.IsPendingExist = "Yes";
                                        objIEM = null;
                                    }
                                }
                                if (objIEM == null)
                                {
                                    //ViewBag.ISnotValid = "NPF";
                                    //return View("~/Views/ARCS/ValidationView.cshtml");
                                    lstIED.Add(new ImportExportModels
                                    {
                                        SocietyName = objIEM.SocietyName,
                                        OldRedgNo = objIEM.OldRedgNo,
                                        CreateDate = Convert.ToDateTime(objIEM.CreateDate),
                                        CreateDate1 = objIEM.CreateDate1,
                                        FunctionalorWinding = objIEM.FunctionalorWinding,
                                        Kind = objIEM.Kind,
                                        GH_No = objIEM.GH_No,
                                        Sector = objIEM.Sector,
                                    });
                                    var productList = new List<ImportExportModels> { objIEM };
                                    ARCSData objARCSD = new ARCSData();
                                    string ARCSCode = Convert.ToString(Session["ARCSCode"]);
                                    var backlogSocietyTransSave = objARCSD.BacklogSocietyTranslog(objIEM, ARCSCode);
                                    if (backlogSocietyTransSave >= 1)
                                    {
                                        var BacklogLogIn = objARCSD.SaveBackLogDetail(objIEM);
                                        ViewBag.ShowGrid = productList;
                                    }
                                }
                                else
                                {
                                    lstIED.Add(new ImportExportModels
                                    {
                                        SocietyName = objIEM.SocietyName,
                                        OldRedgNo = objIEM.OldRedgNo,
                                        CreateDate = Convert.ToDateTime(objIEM.CreateDate),
                                        CreateDate1 = objIEM.CreateDate1,
                                        FunctionalorWinding = objIEM.FunctionalorWinding,
                                        Kind = objIEM.Kind,
                                        GH_No = objIEM.GH_No,
                                        Sector = objIEM.Sector,
                                    });
                                    var productList = new List<ImportExportModels> { objIEM };
                                    ARCSData objARCSD = new ARCSData();
                                    string ARCSCode = Convert.ToString(Session["ARCSCode"]);
                                    var backlogSocietyTransSave = objARCSD.BacklogSocietyTranslog(objIEM, ARCSCode);
                                    if (backlogSocietyTransSave >= 1)
                                    {
                                        var BacklogLogIn = objARCSD.SaveBackLogDetail(objIEM);
                                        ViewBag.ShowGrid = productList;
                                    }
                                    //return View();
                                    // int j = objIED.SaveImportedExcel(objIEM);
                                }
                            }
#pragma warning disable 0168
                            catch (Exception ex)
                            {
                                var IsPendingEntry = objARCSD.BackLogPendingRecord(objIEM);
                                ViewBag.IsPendingExist = "Yes";
                                objIEM = null;
                                ViewBag.IsException = "Yes";
                                //return View(lstIED);
                                //throw ex;
                            }
#pragma warning restore 0168
                        }
                        ViewBag.SaveData = "Yes";
                        ViewBag.PendingData = objARCSD.GetPendingRecord(CurrentDate);
                        return View(lstIED);
                    }
                    ViewBag.ISnotValid = "Yes";
                    return View("~/Views/ARCS/ValidationView.cshtml");
                }
                ViewBag.ISnotValid = "No";
                //return Json("Please Choose Excel File First.");
                //return Json(new { msg = msg }, JsonRequestBehavior.AllowGet);
                //return Json(new { msg = msg }, "text/html", JsonRequestBehavior.AllowGet);
                return View("~/Views/ARCS/ValidationView.cshtml");
            }
            catch (Exception ex)
            {
         //return RedirectToAction("Error", "Unauthorised");
                throw ex;
            }
        }


        public class Search
        {
            public string Sr_no { get; set; }
            public string Name { get; set; }
        }

        public JsonResult GetRecord(string prefix)
        {
            ARCSData objARCSD = new ARCSData();
            DataSet ds = objARCSD.GetName(prefix);
            List<Search> searchlist = new List<Search>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new Search
                {
                    Name = dr["ACT"].ToString(),
                    // Sr_no = dr["Sr_no"].ToString()
                });
            }
            return Json(searchlist, JsonRequestBehavior.AllowGet);
        }

        #region backlogDashBarod
        public ActionResult BackLogDashboard()
        {
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
            List<SelectListItem> lstGD = new List<SelectListItem>();

            lstGD = GetDistrict();
            ViewBag.District = lstGD;
            List<SelectListItem> lstR = new List<SelectListItem>();
            lstR = GetRelationship();
            ViewBag.Relationship = lstR;
            List<SelectListItem> lstMember = new List<SelectListItem>();
            lstMember = GetAllShareTransferMember();
            ViewBag.MemberDetail = lstMember;
            List<SelectListItem> lstMCDCM = new List<SelectListItem>();
            lstMCDCM = GetMemberCommDesignation();
            ViewBag.MemberCommDesignation = lstMCDCM;
            List<SelectListItem> lstARCSCode = new List<SelectListItem>();
            lstARCSCode = GetsubClassSocieties();
            ViewBag.lstARCSCode = lstARCSCode;
            List<SelectListItem> CommunityofSociety = new List<SelectListItem>();
            CommunityofSociety = GetCommunityofSociety();
            ViewBag.CommunityofSociety = CommunityofSociety;
            List<SelectListItem> lstSL = new List<SelectListItem>();
            List<SelectListItem> lstI = new List<SelectListItem>();
            lstSL = GetSocietyList();
            ViewBag.lstSL = lstSL;
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
                List<SelectListItem> lstSL = new List<SelectListItem>();
                List<SelectListItem> lstI = new List<SelectListItem>();
                ARCSData objARCSD = new ARCSData();
                ForwardToIncepector objFTI = new ForwardToIncepector();
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
                int result = objARCSD.ForwardToIncepector(objFTI);
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
                lstI = GetInspectorList();
                ViewBag.BcklstI = lstI;
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
                var TotalCount = Convert.ToInt32(Session["NoOfMembers"]);
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
        #endregion
    }

}
