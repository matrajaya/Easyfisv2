using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class RepReceivingReceiptDetailReportController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =====================
        // Preview and Print PDF
        // =====================
        [Authorize]
        public ActionResult ReceivingReceiptDetailReport(String StartDate, String EndDate, String CompanyId, String BranchId)
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
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 4.5F)));

            // ==============
            // Company Detail
            // ==============
            var companyName = (from d in db.MstCompanies where d.Id == Convert.ToInt32(CompanyId) select d.Company).FirstOrDefault();
            var address = (from d in db.MstCompanies where d.Id == Convert.ToInt32(CompanyId) select d.Address).FirstOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == Convert.ToInt32(CompanyId) select d.ContactNumber).FirstOrDefault();
            var branch = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.Branch).FirstOrDefault();

            // ===========
            // Header Page
            // ===========
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase("Receiving Receipt Detail Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Date From " + Convert.ToDateTime(StartDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + Convert.ToDateTime(EndDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);
            document.Add(line);

            // ==============================
            // Data (Receiving Receipt Items)
            // ==============================
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.TrnReceivingReceipt.RRDate >= Convert.ToDateTime(StartDate)
                                        && d.TrnReceivingReceipt.RRDate <= Convert.ToDateTime(EndDate)
                                        && d.TrnReceivingReceipt.MstBranch.CompanyId == Convert.ToInt32(CompanyId)
                                        && d.TrnReceivingReceipt.BranchId == Convert.ToInt32(BranchId)
                                        && d.TrnReceivingReceipt.IsLocked == true
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            RRId = d.RRId,
                                            Id = d.Id,
                                            RRDate = d.TrnReceivingReceipt.RRDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture),
                                            RR = d.TrnReceivingReceipt.RRNumber,
                                            Supplier = d.TrnReceivingReceipt.MstArticle.Article,
                                            PO = d.TrnPurchaseOrder.PONumber,
                                            Item = d.MstArticle.Article,
                                            Price = d.MstArticle.Price,
                                            Unit = d.MstUnit.Unit,
                                            Quantity = d.Quantity,
                                            Cost = d.Cost,
                                            Amount = d.Amount,
                                            Branch = d.MstBranch.Branch
                                        };

            if (receivingReceiptItems.Any())
            {
                // ============
                // Branch Title
                // ============
                PdfPTable branchTitle = new PdfPTable(1);
                float[] widthCellsBranchTitle = new float[] { 100f };
                branchTitle.SetWidths(widthCellsBranchTitle);
                branchTitle.WidthPercentage = 100;
                PdfPCell branchHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial12Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 14f, Border = 0 });
                branchTitle.AddCell(branchHeaderColspan);
                document.Add(branchTitle);

                // ====
                // Data
                // ====
                PdfPTable data = new PdfPTable(9);
                float[] widthsCellsData = new float[] { 20f, 18f, 25f, 20f, 25f, 20f, 15f, 20f, 20f };
                data.SetWidths(widthsCellsData);
                data.WidthPercentage = 100;
                data.AddCell(new PdfPCell(new Phrase("RR Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("RR Date", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Supplier", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("PO Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Price", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal total = 0;
                foreach (var receivingReceiptItem in receivingReceiptItems)
                {
                    data.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.RR, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                    data.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.RRDate, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                    data.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Supplier, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                    data.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.PO, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                    data.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Item, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                    data.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Price.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                    data.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Unit, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                    data.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Quantity.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                    data.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Amount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                    total += receivingReceiptItem.Amount;
                }

                // =====
                // Total
                // =====
                data.AddCell(new PdfPCell(new Phrase("Total", fontArial10Bold)) { Colspan = 8, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingRight = 10f, PaddingLeft = 10f });
                data.AddCell(new PdfPCell(new Phrase(total.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                document.Add(data);
            }

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}