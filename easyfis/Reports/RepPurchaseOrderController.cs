using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepPurchaseOrderController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ====================
        // Purchase Order - PDF
        // ====================
        [Authorize]
        public ActionResult PurchaseOrder(Int32 POId)
        {
            // ==============================
            // PDF Settings and Customization
            // ==============================
            MemoryStream workStream = new MemoryStream();
            Rectangle rectangle = new Rectangle(PageSize.A3);
            Document document = new Document(rectangle, 72, 72, 72, 72);
            document.SetMargins(30f, 30f, 30f, 30f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();

            // =====
            // Fonts
            // =====
            Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font fontArial11 = FontFactory.GetFont("Arial", 11);
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font fontArial13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 4.5F)));

            var identityUserId = User.Identity.GetUserId();
            var currentUser = from d in db.MstUsers where d.UserId == identityUserId select d;
            var currentCompanyId = currentUser.FirstOrDefault().CompanyId;
            var currentBranchId = currentUser.FirstOrDefault().BranchId;

            // ==============
            // Company Detail
            // ==============
            var companyName = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.Company).FirstOrDefault();
            var address = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.Address).FirstOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.ContactNumber).FirstOrDefault();
            var branch = (from d in db.MstBranches where d.Id == Convert.ToInt32(currentBranchId) select d.Branch).FirstOrDefault();

            // ===========
            // Header Page
            // ===========
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase("Purchase Order", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase(branch, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);

            // =====
            // Space
            // =====
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { Border = 0, PaddingTop = 5f });

            document.Add(line);

            // ===================
            // Get Purchase Orders
            // ===================
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 where d.Id == Convert.ToInt32(POId)
                                 && d.IsLocked == true
                                 select d;

            if (purchaseOrders.Any())
            {
                String supplier = purchaseOrders.FirstOrDefault().MstArticle.Article;
                String PONumber = purchaseOrders.FirstOrDefault().PONumber;
                String term = purchaseOrders.FirstOrDefault().MstArticle.MstTerm.Term;
                String PODate = purchaseOrders.FirstOrDefault().PODate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                String dateNeeded = purchaseOrders.FirstOrDefault().DateNeeded.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                String requestNo = purchaseOrders.FirstOrDefault().ManualRequestNumber;
                String remarks = purchaseOrders.FirstOrDefault().Remarks;
                String preparedBy = purchaseOrders.FirstOrDefault().MstUser3.FullName;
                String checkedBy = purchaseOrders.FirstOrDefault().MstUser1.FullName;
                String approvedBy = purchaseOrders.FirstOrDefault().MstUser.FullName;
                String requestedBy = purchaseOrders.FirstOrDefault().MstUser4.FullName;

                PdfPTable tablePurchaseOrder = new PdfPTable(4);
                float[] widthscellsTablePurchaseOrder = new float[] { 40f, 150f, 70f, 50f };
                tablePurchaseOrder.SetWidths(widthscellsTablePurchaseOrder);
                tablePurchaseOrder.WidthPercentage = 100;

                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase("Supplier", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase(supplier, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase("PO Number", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase(PONumber, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase("Term", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase(term, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase("PO Date", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase(PODate, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase("Date Needed", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase(dateNeeded, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase("Request No.", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase(requestNo, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase("Remarks ", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase(remarks, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tablePurchaseOrder.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Colspan = 2, Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(tablePurchaseOrder);

                document.Add(spaceTable);

                // ========================
                // Get Purchase Order Items
                // ========================
                var purchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                         where d.POId == POId
                                         && d.TrnPurchaseOrder.IsLocked == true
                                         select new
                                         {
                                             PO = d.TrnPurchaseOrder.PONumber,
                                             Item = d.MstArticle.Article,
                                             Particulars = d.Particulars,
                                             Unit = d.MstUnit.Unit,
                                             Quantity = d.Quantity,
                                             Cost = d.Cost,
                                             Amount = d.Amount
                                         };

                if (purchaseOrderItems.Any())
                {
                    PdfPTable tablePurchaseOrderLines = new PdfPTable(6);
                    float[] widthscellsPOLines = new float[] { 100f, 70f, 200f, 150f, 100f, 100f };
                    tablePurchaseOrderLines.SetWidths(widthscellsPOLines);
                    tablePurchaseOrderLines.WidthPercentage = 100;
                    tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase("Particulars", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase("Price", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });

                    Decimal totalAmount = 0;

                    foreach (var purchaseOrderItem in purchaseOrderItems)
                    {
                        tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Quantity.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Unit, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Item, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Particulars, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Cost.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Amount.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });

                        totalAmount += purchaseOrderItem.Amount;
                    }

                    tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase("Total", fontArial11Bold)) { Colspan = 5, HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tablePurchaseOrderLines.AddCell(new PdfPCell(new Phrase(totalAmount.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    document.Add(tablePurchaseOrderLines);

                    document.Add(spaceTable);
                }

                // ==============
                // User Signature
                // ==============
                PdfPTable tableUsers = new PdfPTable(4);
                float[] widthsCellsTableUsers = new float[] { 100f, 100f, 100f, 100f };
                tableUsers.WidthPercentage = 100;
                tableUsers.SetWidths(widthsCellsTableUsers);
                tableUsers.AddCell(new PdfPCell(new Phrase("Prepared by", fontArial11Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase("Checked by", fontArial11Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase("Approved by", fontArial11Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase("Requested by", fontArial11Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 50f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 50f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 50f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 50f });
                tableUsers.AddCell(new PdfPCell(new Phrase(preparedBy, fontArial11)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(checkedBy, fontArial11)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(approvedBy, fontArial11)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(requestedBy, fontArial11)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(tableUsers);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}