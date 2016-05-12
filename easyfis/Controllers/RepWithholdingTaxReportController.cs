using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace easyfis.Controllers
{
    public class RepWithholdingTaxReportController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Withholding Tax Report
        [Authorize]
        public ActionResult WithholdingTaxReport(String StartDate, String EndDate)
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
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);

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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Withholding Tax Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Period From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);
            document.Add(Chunk.NEWLINE);

            // receiving Receipt Items
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.TrnReceivingReceipt.RRDate >= Convert.ToDateTime(StartDate)
                                        && d.TrnReceivingReceipt.RRDate <= Convert.ToDateTime(EndDate)
                                        && d.WTAXAmount > 0
                                        && d.TrnReceivingReceipt.IsLocked == true
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            Id = d.Id,
                                            Supplier = d.TrnReceivingReceipt.MstArticle.Article,
                                            SupplierTIN = d.TrnReceivingReceipt.MstArticle.TaxNumber,
                                            Amount = d.Amount,
                                            WTAX = d.MstTaxType1.TaxType,
                                            WTAXPercentage = d.WTAXPercentage,
                                            WTAXAmount = d.WTAXAmount,
                                        };

            if (receivingReceiptItems.Any())
            {
                PdfPTable tableWTRLines = new PdfPTable(7);
                float[] widthsCellsWTRLines = new float[] { 120f, 150f, 130f, 110f, 90f, 90f, 90f };
                tableWTRLines.SetWidths(widthsCellsWTRLines);
                tableWTRLines.WidthPercentage = 100;
                tableWTRLines.AddCell(new PdfPCell(new Phrase("TIN", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableWTRLines.AddCell(new PdfPCell(new Phrase("Supplier Name", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableWTRLines.AddCell(new PdfPCell(new Phrase("WT Code", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableWTRLines.AddCell(new PdfPCell(new Phrase("Nature of Payment", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableWTRLines.AddCell(new PdfPCell(new Phrase("Amount", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableWTRLines.AddCell(new PdfPCell(new Phrase("Tax Rate", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableWTRLines.AddCell(new PdfPCell(new Phrase("Withheld Amount", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal TotalAmount = 0;
                Decimal TotalWTAXAmount = 0;

                foreach (var receivingReceiptItem in receivingReceiptItems)
                {
                    tableWTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.SupplierTIN, fontArial9)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableWTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Supplier, fontArial9)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableWTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.WTAX, fontArial9)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableWTRLines.AddCell(new PdfPCell(new Phrase("Purchases", fontArial9)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableWTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableWTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.WTAXPercentage.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableWTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.WTAXAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    TotalAmount = TotalAmount + receivingReceiptItem.Amount;
                    TotalWTAXAmount = TotalWTAXAmount + receivingReceiptItem.WTAXAmount;
                }

                document.Add(tableWTRLines);
                document.Add(Chunk.NEWLINE);

                PdfPTable tableWTRLineTotalAmount = new PdfPTable(7);
                float[] widthscellsWTRLinesTotalAmount = new float[] { 120f, 150f, 130f, 110f, 90f, 90f, 90f };
                tableWTRLineTotalAmount.SetWidths(widthscellsWTRLinesTotalAmount);
                tableWTRLineTotalAmount.WidthPercentage = 100;
                tableWTRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableWTRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableWTRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableWTRLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total: ", fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableWTRLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableWTRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableWTRLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalWTAXAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                document.Add(tableWTRLineTotalAmount);
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