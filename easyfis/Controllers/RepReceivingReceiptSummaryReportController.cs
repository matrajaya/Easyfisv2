using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

namespace easyfis.Controllers
{
    public class RepReceivingReceiptSummaryReportController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Receiving Receipt Summary Report
        [Authorize]
        public ActionResult ReceivingReceiptSummaryReport(String StartDate, String EndDate)
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
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);

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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Receiving Receipt Summary Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // Receiving Receipts 
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.BranchId == currentBranchId()
                                    && d.RRDate >= Convert.ToDateTime(StartDate)
                                    && d.RRDate <= Convert.ToDateTime(EndDate)
                                    && d.IsLocked == true
                                    select new Models.TrnReceivingReceipt
                                    {
                                        Id = d.Id,
                                        Branch = d.MstBranch.Branch,
                                        RRDate = d.RRDate.ToShortDateString(),
                                        RRNumber = d.RRNumber,
                                        Supplier = d.MstArticle.Article,
                                        DocumentReference = d.DocumentReference,
                                        Remarks = d.Remarks,
                                        Amount = d.Amount
                                    };

            Decimal total = 0;

            if (receivingReceipts.Any())
            {
                // table branch header
                PdfPTable tableBranchHeader = new PdfPTable(1);
                float[] widthCellsTableBranchHeader = new float[] { 100f };
                tableBranchHeader.SetWidths(widthCellsTableBranchHeader);
                tableBranchHeader.WidthPercentage = 100;
                PdfPCell branchHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial10Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                tableBranchHeader.AddCell(branchHeaderColspan);
                document.Add(tableBranchHeader);

                // Table RR Data
                PdfPTable tableRData = new PdfPTable(7);
                float[] widthsCellsTableRRData = new float[] { 25f, 10f, 15f, 15f, 25f, 30f, 20f };
                tableRData.SetWidths(widthsCellsTableRRData);
                tableRData.WidthPercentage = 100;
                tableRData.AddCell(new PdfPCell(new Phrase("Branch", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRData.AddCell(new PdfPCell(new Phrase("RR Date", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRData.AddCell(new PdfPCell(new Phrase("RR Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRData.AddCell(new PdfPCell(new Phrase("Doc. Ref", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRData.AddCell(new PdfPCell(new Phrase("Supplier", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRData.AddCell(new PdfPCell(new Phrase("Remarks", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRData.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                foreach (var receivingReceipt in receivingReceipts)
                {
                    tableRData.AddCell(new PdfPCell(new Phrase(receivingReceipt.Branch, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRData.AddCell(new PdfPCell(new Phrase(receivingReceipt.RRDate, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRData.AddCell(new PdfPCell(new Phrase(receivingReceipt.RRNumber, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRData.AddCell(new PdfPCell(new Phrase(receivingReceipt.DocumentReference, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRData.AddCell(new PdfPCell(new Phrase(receivingReceipt.Supplier, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRData.AddCell(new PdfPCell(new Phrase(receivingReceipt.Remarks, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRData.AddCell(new PdfPCell(new Phrase(receivingReceipt.Amount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    total = total + receivingReceipt.Amount;
                }

                document.Add(tableRData);
                document.Add(Chunk.NEWLINE);

                // Table RR Total Footer
                PdfPTable tableRRFooter = new PdfPTable(7);
                float[] widthscellsTableRRFooter = new float[] { 25f, 5f, 10f, 10f, 20f, 50f, 20f };
                tableRRFooter.SetWidths(widthscellsTableRRFooter);
                tableRRFooter.WidthPercentage = 100;
                tableRRFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRFooter.AddCell(new PdfPCell(new Phrase("Total", fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRFooter.AddCell(new PdfPCell(new Phrase(total.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                document.Add(tableRRFooter);
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