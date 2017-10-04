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
    public class RepReceivingReceiptController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =======================
        // Receiving Receipt - PDF
        // =======================
        [Authorize]
        public ActionResult ReceivingReceipt(Int32 RRId)
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
            var branch = (from d in db.MstBranches where d.Id == Convert.ToInt32(currentBranchId) select d.ContactNumber).FirstOrDefault();

            // ===========
            // Header Page
            // ===========
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase("Receiving Receipt", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
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

            // =====================
            // Get Receiving Receipt
            // =====================
            var receivingReceipt = from d in db.TrnReceivingReceipts
                                   where d.Id == RRId
                                   && d.IsLocked == true
                                   select d;

            if (receivingReceipt.Any())
            {
                String supplier = receivingReceipt.FirstOrDefault().MstArticle.Article;
                String term = receivingReceipt.FirstOrDefault().MstTerm.Term;
                String dueDate = receivingReceipt.FirstOrDefault().RRDate.AddDays(Convert.ToInt32(receivingReceipt.FirstOrDefault().MstTerm.NumberOfDays)).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                String remarks = receivingReceipt.FirstOrDefault().Remarks;
                String RRNumber = receivingReceipt.FirstOrDefault().RRNumber;
                String RRDate = receivingReceipt.FirstOrDefault().RRDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                String documentRef = receivingReceipt.FirstOrDefault().DocumentReference;
                String preparedBy = receivingReceipt.FirstOrDefault().MstUser3.FullName;
                String checkedBy = receivingReceipt.FirstOrDefault().MstUser1.FullName;
                String approvedBy = receivingReceipt.FirstOrDefault().MstUser.FullName;
                String receivedBy = receivingReceipt.FirstOrDefault().MstUser4.FullName;

                PdfPTable tableReceivingReceipt = new PdfPTable(4);
                float[] widthscellsTableReceivingReceipt = new float[] { 40f, 150f, 70f, 50f };
                tableReceivingReceipt.SetWidths(widthscellsTableReceivingReceipt);
                tableReceivingReceipt.WidthPercentage = 100;
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Customer ", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(supplier, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("RR Number ", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(RRNumber, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Term ", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(term, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("RR Date ", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(RRDate, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Due Date ", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(dueDate, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Document Ref ", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(documentRef, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase("Remarks ", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(remarks, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableReceivingReceipt.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Colspan = 2, Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(tableReceivingReceipt);

                document.Add(spaceTable);

                var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                            where d.RRId == RRId
                                            && d.TrnReceivingReceipt.IsLocked == true
                                            select new
                                            {
                                                Id = d.Id,
                                                RRId = d.RRId,
                                                RR = d.TrnReceivingReceipt.RRNumber,
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
                                                Amount = d.Amount,
                                                VATId = d.VATId,
                                                VAT = d.MstTaxType.TaxType,
                                                VATPercentage = d.VATPercentage,
                                                VATAmount = d.VATAmount,
                                                WTAXId = d.WTAXId,
                                                WTAX = d.MstTaxType1.TaxType,
                                                WTAXPercentage = d.WTAXPercentage,
                                                WTAXAmount = d.WTAXAmount,
                                                BranchId = d.BranchId,
                                                Branch = d.MstBranch.Branch,
                                                BaseUnitId = d.BaseUnitId,
                                                BaseUnit = d.MstUnit1.Unit,
                                                BaseQuantity = d.BaseQuantity,
                                                BaseCost = d.BaseCost
                                            };

                if (receivingReceiptItems.Any())
                {
                    PdfPTable tableReceivingReceiptItems = new PdfPTable(6);
                    float[] widthsCellsReceivingReceiptItems = new float[] { 100f, 70f, 200f, 150f, 100f, 100f };
                    tableReceivingReceiptItems.SetWidths(widthsCellsReceivingReceiptItems);
                    tableReceivingReceiptItems.WidthPercentage = 100;
                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Branch", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Price", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });

                    Decimal totalAmount = 0;

                    foreach (var receivingReceiptItem in receivingReceiptItems)
                    {
                        tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Quantity.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Unit, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Item, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Branch, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Cost.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Amount.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });

                        totalAmount += receivingReceiptItem.Amount;
                    }

                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase("Total", fontArial11Bold)) { Colspan = 5, HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableReceivingReceiptItems.AddCell(new PdfPCell(new Phrase(totalAmount.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    document.Add(tableReceivingReceiptItems);

                    document.Add(spaceTable);
                }

                // ============
                // VAT Analysis
                // ============
                var VATItems = from d in receivingReceiptItems
                               group d by new
                               {
                                   VAT = d.VAT,
                                   WTAX = d.WTAX
                               } into g
                               select new
                               {
                                   VAT = g.Key.VAT,
                                   Amount = g.Sum(d => d.Amount),
                                   VATAmount = g.Sum(d => d.VATAmount),
                                   WTAXAmount = g.Sum(d => d.WTAXAmount)
                               };

                if (VATItems.Any())
                {
                    PdfPTable tableVATAnalysis = new PdfPTable(4);
                    float[] widthsCellsVATItems = new float[] { 200f, 100f, 100f, 100f };
                    tableVATAnalysis.SetWidths(widthsCellsVATItems);
                    tableVATAnalysis.HorizontalAlignment = Element.ALIGN_LEFT;
                    tableVATAnalysis.WidthPercentage = 50;
                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase("VAT", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase("Amount", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase("VAT Amount", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase("WTAX Amount", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });

                    Decimal totalVATAmount = 0;
                    Decimal totalVAT = 0;
                    Decimal totalWTAX = 0;

                    foreach (var VATItem in VATItems)
                    {
                        tableVATAnalysis.AddCell(new PdfPCell(new Phrase(VATItem.VAT, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableVATAnalysis.AddCell(new PdfPCell(new Phrase(VATItem.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableVATAnalysis.AddCell(new PdfPCell(new Phrase(VATItem.VATAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableVATAnalysis.AddCell(new PdfPCell(new Phrase(VATItem.WTAXAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });

                        totalVATAmount += VATItem.Amount;
                        totalVAT += VATItem.VATAmount;
                        totalWTAX += VATItem.WTAXAmount;
                    }

                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase("Total", fontArial9Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase(totalVATAmount.ToString("#,##0.00"), fontArial9Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase(totalVAT.ToString("#,##0.00"), fontArial9Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase(totalWTAX.ToString("#,##0.00"), fontArial9Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });

                    // TODO: Option Settings for VAT Analysis Table
                    //document.Add(tableVATAnalysis);

                    //document.Add(spaceTable);
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
                tableUsers.AddCell(new PdfPCell(new Phrase("Received by", fontArial11Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 50f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 50f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 50f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 50f });
                tableUsers.AddCell(new PdfPCell(new Phrase(preparedBy, fontArial11)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(checkedBy, fontArial11)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(approvedBy, fontArial11)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(receivedBy, fontArial11)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
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