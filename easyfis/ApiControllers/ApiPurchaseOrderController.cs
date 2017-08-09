using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.Net.Mail;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace easyfis.Controllers
{
    public class ApiPurchaseOrderController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.BranchId).SingleOrDefault();
        }

        public String zeroFill(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        // get purchase order amount by POId
        public Decimal getPurchaseOrderAmountByPOId(Int32 POId)
        {
            Decimal totalAmount;

            var purchaseOrderItems = from d in db.TrnPurchaseOrderItems where d.POId == POId select d;
            if (!purchaseOrderItems.Any())
            {
                totalAmount = 0;
            }
            else
            {
                totalAmount = purchaseOrderItems.Sum(d => d.Amount);
            }

            return totalAmount;
        }

        // list purchase order
        [Authorize]
        [HttpGet]
        [Route("api/listPurchaseOrder")]
        public List<Models.TrnPurchaseOrder> listPurchaseOrder()
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders.OrderByDescending(d => d.Id)
                                 where d.BranchId == currentBranchId()
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     Amount = getPurchaseOrderAmountByPOId(d.Id),
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
                                     PreparedById = d.PreparedById,
                                     PreparedBy = d.MstUser3.FullName,
                                     CheckedById = d.CheckedById,
                                     CheckedBy = d.MstUser1.FullName,
                                     ApprovedById = d.ApprovedById,
                                     ApprovedBy = d.MstUser.FullName,
                                     IsLocked = d.IsLocked,
                                     CreatedById = d.CreatedById,
                                     CreatedBy = d.MstUser2.FullName,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedById = d.UpdatedById,
                                     UpdatedBy = d.MstUser5.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };

            return purchaseOrders.ToList();
        }

        // get purchase order by Id
        [Authorize]
        [HttpGet]
        [Route("api/purchaseOrder/{id}")]
        public Models.TrnPurchaseOrder getPurchaseOrderById(String id)
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 where d.Id == Convert.ToInt32(id)
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     Amount = getPurchaseOrderAmountByPOId(d.Id),
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
                                     PreparedById = d.PreparedById,
                                     PreparedBy = d.MstUser3.FullName,
                                     CheckedById = d.CheckedById,
                                     CheckedBy = d.MstUser1.FullName,
                                     ApprovedById = d.ApprovedById,
                                     ApprovedBy = d.MstUser.FullName,
                                     IsLocked = d.IsLocked,
                                     CreatedById = d.CreatedById,
                                     CreatedBy = d.MstUser2.FullName,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedById = d.UpdatedById,
                                     UpdatedBy = d.MstUser5.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };

            return (Models.TrnPurchaseOrder)purchaseOrders.FirstOrDefault();
        }

        // list purchase order by PODate
        [Authorize]
        [HttpGet]
        [Route("api/listPurchaseOrderFilterByPODate/{PODate}/{POEndDate}")]
        public List<Models.TrnPurchaseOrder> listPurchaseOrderByPODate(String PODate, String POEndDate)
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders.OrderByDescending(d => d.Id)
                                 where d.PODate >= Convert.ToDateTime(PODate)
                                 && d.PODate <= Convert.ToDateTime(POEndDate)
                                 && d.BranchId == currentBranchId()
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     Amount = getPurchaseOrderAmountByPOId(d.Id),
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
                                     PreparedById = d.PreparedById,
                                     PreparedBy = d.MstUser3.FullName,
                                     CheckedById = d.CheckedById,
                                     CheckedBy = d.MstUser1.FullName,
                                     ApprovedById = d.ApprovedById,
                                     ApprovedBy = d.MstUser.FullName,
                                     IsLocked = d.IsLocked,
                                     CreatedById = d.CreatedById,
                                     CreatedBy = d.MstUser2.FullName,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedById = d.UpdatedById,
                                     UpdatedBy = d.MstUser5.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };

            return purchaseOrders.ToList();
        }

        // list purchase order by supplier Id
        [Authorize]
        [HttpGet]
        [Route("api/listPurchaseOrderBySupplierId/{supplierId}")]
        public List<Models.TrnPurchaseOrder> listPurchaseOrderBySupplierId(String supplierId)
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 where d.SupplierId == Convert.ToInt32(supplierId)
                                 && d.BranchId == currentBranchId()
                                 && d.IsLocked == true
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     Amount = getPurchaseOrderAmountByPOId(d.Id),
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
                                     PreparedById = d.PreparedById,
                                     PreparedBy = d.MstUser3.FullName,
                                     CheckedById = d.CheckedById,
                                     CheckedBy = d.MstUser1.FullName,
                                     ApprovedById = d.ApprovedById,
                                     ApprovedBy = d.MstUser.FullName,
                                     IsLocked = d.IsLocked,
                                     CreatedById = d.CreatedById,
                                     CreatedBy = d.MstUser2.FullName,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedById = d.UpdatedById,
                                     UpdatedBy = d.MstUser5.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };

            return purchaseOrders.ToList();
        }

        // get last purchase order PONumber
        [Authorize]
        [HttpGet]
        [Route("api/purchaseOrderLastPONumber")]
        public Models.TrnPurchaseOrder getPurchaseOrderLastPONumber()
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders.OrderByDescending(d => d.PONumber)
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     Amount = getPurchaseOrderAmountByPOId(d.Id),
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
                                     PreparedById = d.PreparedById,
                                     PreparedBy = d.MstUser3.FullName,
                                     CheckedById = d.CheckedById,
                                     CheckedBy = d.MstUser1.FullName,
                                     ApprovedById = d.ApprovedById,
                                     ApprovedBy = d.MstUser.FullName,
                                     IsLocked = d.IsLocked,
                                     CreatedById = d.CreatedById,
                                     CreatedBy = d.MstUser2.FullName,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedById = d.UpdatedById,
                                     UpdatedBy = d.MstUser5.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };

            return (Models.TrnPurchaseOrder)purchaseOrders.FirstOrDefault();
        }

        // add purchase order
        [Authorize]
        [HttpPost]
        [Route("api/addPurchaseOrder")]
        public Int32 insertPurchaseOrder(Models.TrnPurchaseOrder purchaseOrder)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var PONumberResult = "0000000001";
                var lastPONumber = from d in db.TrnPurchaseOrders.OrderByDescending(d => d.Id) where d.BranchId == currentBranchId() select d;
                if (lastPONumber.Any())
                {
                    var PONumber = Convert.ToInt32(lastPONumber.FirstOrDefault().PONumber) + 0000000001;
                    PONumberResult = zeroFill(PONumber, 10);
                }

                Data.TrnPurchaseOrder newPurchaseOrder = new Data.TrnPurchaseOrder();
                newPurchaseOrder.BranchId = currentBranchId();
                newPurchaseOrder.PONumber = PONumberResult;
                newPurchaseOrder.PODate = DateTime.Today;
                newPurchaseOrder.SupplierId = (from d in db.MstArticles where d.ArticleTypeId == 3 select d.Id).FirstOrDefault();
                newPurchaseOrder.TermId = (from d in db.MstTerms select d.Id).FirstOrDefault();
                newPurchaseOrder.ManualRequestNumber = "NA";
                newPurchaseOrder.ManualPONumber = "NA";
                newPurchaseOrder.DateNeeded = DateTime.Today;
                newPurchaseOrder.Remarks = "NA";
                newPurchaseOrder.IsClose = false;
                newPurchaseOrder.RequestedById = userId;
                newPurchaseOrder.PreparedById = userId;
                newPurchaseOrder.CheckedById = userId;
                newPurchaseOrder.ApprovedById = userId;
                newPurchaseOrder.IsLocked = false;
                newPurchaseOrder.CreatedById = userId;
                newPurchaseOrder.CreatedDateTime = DateTime.Now;
                newPurchaseOrder.UpdatedById = userId;
                newPurchaseOrder.UpdatedDateTime = DateTime.Now;

                db.TrnPurchaseOrders.InsertOnSubmit(newPurchaseOrder);
                db.SubmitChanges();

                return newPurchaseOrder.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update purchase order
        [Authorize]
        [HttpPut]
        [Route("api/updatePurchaseOrder/{id}")]
        public HttpResponseMessage updatePurchaseOrder(String id, Models.TrnPurchaseOrder purchaseOrder)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var purchaseOrders = from d in db.TrnPurchaseOrders where d.Id == Convert.ToInt32(id) select d;
                if (purchaseOrders.Any())
                {
                    var updatePurchaseOrder = purchaseOrders.FirstOrDefault();
                    updatePurchaseOrder.BranchId = purchaseOrder.BranchId;
                    updatePurchaseOrder.PONumber = purchaseOrder.PONumber;
                    updatePurchaseOrder.PODate = Convert.ToDateTime(purchaseOrder.PODate);
                    updatePurchaseOrder.SupplierId = purchaseOrder.SupplierId;
                    updatePurchaseOrder.TermId = purchaseOrder.TermId;
                    updatePurchaseOrder.ManualRequestNumber = purchaseOrder.ManualRequestNumber;
                    updatePurchaseOrder.ManualPONumber = purchaseOrder.ManualPONumber;
                    updatePurchaseOrder.DateNeeded = Convert.ToDateTime(purchaseOrder.DateNeeded);
                    updatePurchaseOrder.Remarks = purchaseOrder.Remarks;
                    updatePurchaseOrder.IsClose = purchaseOrder.IsClose;
                    updatePurchaseOrder.RequestedById = purchaseOrder.RequestedById;
                    updatePurchaseOrder.PreparedById = purchaseOrder.PreparedById;
                    updatePurchaseOrder.CheckedById = purchaseOrder.CheckedById;
                    updatePurchaseOrder.ApprovedById = purchaseOrder.ApprovedById;
                    updatePurchaseOrder.IsLocked = true;
                    updatePurchaseOrder.UpdatedById = userId;
                    updatePurchaseOrder.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // unlock purchase order
        [Authorize]
        [HttpPut]
        [Route("api/updatePurchaseOrderIsLocked/{id}")]
        public HttpResponseMessage unlockPurchaseOrder(String id, Models.TrnPurchaseOrder purchaseOrder)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var purchaseOrders = from d in db.TrnPurchaseOrders where d.Id == Convert.ToInt32(id) select d;
                if (purchaseOrders.Any())
                {
                    var unlockPurchaseOrder = purchaseOrders.FirstOrDefault();
                    unlockPurchaseOrder.IsLocked = false;
                    unlockPurchaseOrder.UpdatedById = userId;
                    unlockPurchaseOrder.UpdatedDateTime = DateTime.Now; ;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // delete purchase order
        [Authorize]
        [HttpDelete]
        [Route("api/deletePO/{id}")]
        public HttpResponseMessage deletePurchaseOrder(String id)
        {
            try
            {
                var purchaseOrders = from d in db.TrnPurchaseOrders where d.Id == Convert.ToInt32(id) select d;
                if (purchaseOrders.Any())
                {
                    db.TrnPurchaseOrders.DeleteOnSubmit(purchaseOrders.First());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        public byte[] PODetailReport(Int32 POId)
        {
            // PDF settings
            MemoryStream workStream = new MemoryStream();
            Rectangle rectangle = new Rectangle(PageSize.A3);
            Document document = new Document(rectangle, 72, 72, 72, 72);
            document.SetMargins(30f, 30f, 30f, 30f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font fontArial11 = FontFactory.GetFont("Arial", 11);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);

            // line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // Company Detail
            var companyName = (from d in db.MstBranches where d.Id == currentBranchId() select d.MstCompany.Company).SingleOrDefault();
            var address = (from d in db.MstBranches where d.Id == currentBranchId() select d.MstCompany.Address).SingleOrDefault();
            var contactNo = (from d in db.MstBranches where d.Id == currentBranchId() select d.MstCompany.ContactNumber).SingleOrDefault();
            var branch = (from d in db.MstBranches where d.Id == currentBranchId() select d.Branch).SingleOrDefault();

            // table main header
            PdfPTable tableHeaderPage = new PdfPTable(2);
            float[] widthsCellsheaderPage = new float[] { 100f, 75f };
            tableHeaderPage.SetWidths(widthsCellsheaderPage);
            tableHeaderPage.WidthPercentage = 100;
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Purchase Order", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(branch, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var purchaseOrderHeaders = from d in db.TrnPurchaseOrders
                                       where d.Id == POId
                                       && d.IsLocked == true
                                       select new Models.TrnPurchaseOrder
                                       {
                                           Id = d.Id,
                                           BranchId = d.BranchId,
                                           Branch = d.MstBranch.Branch,
                                           PONumber = d.PONumber,
                                           PODate = d.PODate.ToShortDateString(),
                                           SupplierId = d.SupplierId,
                                           Supplier = d.MstArticle.Article,
                                           TermId = d.TermId,
                                           Term = d.MstTerm.Term,
                                           ManualRequestNumber = d.ManualRequestNumber,
                                           ManualPONumber = d.ManualPONumber,
                                           DateNeeded = d.DateNeeded.ToShortDateString(),
                                           Remarks = d.Remarks,
                                           IsClose = d.IsClose,
                                           RequestedById = d.RequestedById,
                                           RequestedBy = d.MstUser4.FullName,
                                           PreparedById = d.PreparedById,
                                           PreparedBy = d.MstUser3.FullName,
                                           CheckedById = d.CheckedById,
                                           CheckedBy = d.MstUser1.FullName,
                                           ApprovedById = d.ApprovedById,
                                           ApprovedBy = d.MstUser.FullName
                                       };

            if (purchaseOrderHeaders.Any())
            {


                String Supplier = "", Term = "", DateNeeded = "", Remarks = "";
                String PONumber = "", PODate = "", RequestNo = "";
                String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                foreach (var purchaseOrderHeader in purchaseOrderHeaders)
                {
                    Supplier = purchaseOrderHeader.Supplier;
                    Term = purchaseOrderHeader.Term;
                    DateNeeded = purchaseOrderHeader.DateNeeded;
                    Remarks = purchaseOrderHeader.Remarks;
                    PONumber = purchaseOrderHeader.PONumber;
                    PODate = purchaseOrderHeader.PODate;
                    RequestNo = purchaseOrderHeader.ManualRequestNumber;
                    PreparedBy = purchaseOrderHeader.PreparedBy;
                    CheckedBy = purchaseOrderHeader.CheckedBy;
                    ApprovedBy = purchaseOrderHeader.ApprovedBy;
                }

                PdfPTable tableSubHeader = new PdfPTable(4);
                float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                tableSubHeader.SetWidths(widthscellsSubheader);
                tableSubHeader.WidthPercentage = 100;
                tableSubHeader.AddCell(new PdfPCell(new Phrase("Supplier: ", fontArial11Bold)) { Border = 0, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Supplier, fontArial11)) { Border = 0, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("PO Number: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(PONumber, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                tableSubHeader.AddCell(new PdfPCell(new Phrase("Term: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Term, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("PO date: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(PODate, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                tableSubHeader.AddCell(new PdfPCell(new Phrase("Date Needed: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(DateNeeded, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("Request No: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(RequestNo, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                tableSubHeader.AddCell(new PdfPCell(new Phrase("Remarks: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Remarks, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("", fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                document.Add(tableSubHeader);
                document.Add(Chunk.NEWLINE);

                var purchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                         where d.POId == POId
                                         && d.TrnPurchaseOrder.IsLocked == true
                                         select new Models.TrnPurchaseOrderItem
                                         {
                                             Id = d.Id,
                                             POId = d.POId,
                                             PO = d.TrnPurchaseOrder.PONumber,
                                             ItemId = d.ItemId,
                                             Item = d.MstArticle.Article,
                                             ItemCode = d.MstArticle.ManualArticleCode,
                                             Particulars = d.Particulars,
                                             UnitId = d.UnitId,
                                             Unit = d.MstUnit.Unit,
                                             Quantity = d.Quantity,
                                             Cost = d.Cost,
                                             Amount = d.Amount
                                         };

                PdfPTable tablePOLines = new PdfPTable(6);
                float[] widthscellsPOLines = new float[] { 50f, 60f, 130f, 150f, 100f, 100f };
                tablePOLines.SetWidths(widthscellsPOLines);
                tablePOLines.WidthPercentage = 100;

                tablePOLines.AddCell(new PdfPCell(new Phrase("Quantity", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOLines.AddCell(new PdfPCell(new Phrase("Unit", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOLines.AddCell(new PdfPCell(new Phrase("Item", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOLines.AddCell(new PdfPCell(new Phrase("Particulars", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOLines.AddCell(new PdfPCell(new Phrase("Price", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOLines.AddCell(new PdfPCell(new Phrase("Amount", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal TotalAmount = 0;
                foreach (var purchaseOrderItem in purchaseOrderItems)
                {
                    tablePOLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Quantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Unit, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Item, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Particulars, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Cost.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    TotalAmount = TotalAmount + purchaseOrderItem.Amount;
                }

                document.Add(tablePOLines);

                document.Add(Chunk.NEWLINE);

                PdfPTable tablePOLineTotalAmount = new PdfPTable(6);
                float[] widthscellsPOLinesTotalAmount = new float[] { 50f, 60f, 130f, 150f, 100f, 100f };
                tablePOLineTotalAmount.SetWidths(widthscellsPOLinesTotalAmount);
                tablePOLineTotalAmount.WidthPercentage = 100;

                tablePOLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tablePOLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tablePOLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tablePOLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tablePOLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total:", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tablePOLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                document.Add(tablePOLineTotalAmount);

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);

                // Table for Footer
                PdfPTable tableFooter = new PdfPTable(7);
                tableFooter.WidthPercentage = 100;
                float[] widthsCells2 = new float[] { 100f, 20f, 100f, 20f, 100f, 20f, 100f };
                tableFooter.SetWidths(widthsCells2);
                tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase("Received by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });

                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });

                tableFooter.AddCell(new PdfPCell(new Phrase(PreparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(CheckedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(ApprovedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase("Date Received: ", fontArial11Bold)) { Border = 1, HorizontalAlignment = 0, PaddingBottom = 5f });
                document.Add(tableFooter);
            }

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return byteInfo;
        }

        // send email purchase order    

        [Authorize]
        [HttpPut]
        [Route("api/purchaseOrder/sendEmail/{POId}/{SupplierId}")]
        public HttpResponseMessage sendEmail(Int32 POId, Int32 SupplierId)
        {
            try
            {
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                var supplierEmailAddress = from d in db.MstArticles where d.Id == SupplierId select d;

                if (supplierEmailAddress.Any())
                {
                    MailMessage mm = new MailMessage("easyfisv2@gmail.com", supplierEmailAddress.FirstOrDefault().EmailAddress);
                    mm.Subject = "Purchase Order";
                    mm.Body = "Purchase Order";
                    mm.Attachments.Add(new Attachment(new MemoryStream(PODetailReport(POId)), "PurchaseOrder.pdf"));
                    mm.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;

                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = "easyfisv2@gmail.com";
                    NetworkCred.Password = "@innosoft123";
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
              
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
