using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace easyfis.Controllers
{
    public class RepDisbursementDetailReportController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Disbursement Detail Report
        [Authorize]
        public ActionResult DisbursementDetailReport(String StartDate, String EndDate)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Disbursement Detail Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // disbursement Lines
            var disbursementLines = from d in db.TrnDisbursementLines
                                    where d.TrnDisbursement.BranchId == currentBranchId()
                                    && d.TrnDisbursement.CVDate >= Convert.ToDateTime(StartDate)
                                    && d.TrnDisbursement.CVDate <= Convert.ToDateTime(EndDate)
                                    && d.TrnDisbursement.IsLocked == true
                                    select new Models.TrnDisbursementLine
                                    {
                                        Id = d.Id,
                                        CV = d.TrnDisbursement.CVNumber,
                                        CVDate = d.TrnDisbursement.CVDate.ToShortDateString(),
                                        Branch = d.MstBranch.Branch,
                                        Account = d.MstAccount.Account,
                                        Article = d.MstArticle.Article,
                                        RR = d.TrnReceivingReceipt.RRNumber,
                                        Particulars = d.Particulars,
                                        Amount = d.Amount
                                    };

            Decimal total = 0;

            if (disbursementLines.Any())
            {
                // table branch header
                PdfPTable tableBranchHeader = new PdfPTable(1);
                float[] widthCellsTableBranchHeader = new float[] { 100f };
                tableBranchHeader.SetWidths(widthCellsTableBranchHeader);
                tableBranchHeader.WidthPercentage = 100;
                PdfPCell branchHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial10Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                tableBranchHeader.AddCell(branchHeaderColspan);
                document.Add(tableBranchHeader);

                // table CV Lines Data
                PdfPTable tableCVLinesData = new PdfPTable(7);
                float[] widthsCellsTableCVLinesData = new float[] { 15f, 10f, 25f, 25f, 15f, 30f, 20f };
                tableCVLinesData.SetWidths(widthsCellsTableCVLinesData);
                tableCVLinesData.WidthPercentage = 100;
                tableCVLinesData.AddCell(new PdfPCell(new Phrase("CV Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVLinesData.AddCell(new PdfPCell(new Phrase("CV Date", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVLinesData.AddCell(new PdfPCell(new Phrase("Account", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVLinesData.AddCell(new PdfPCell(new Phrase("Article", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVLinesData.AddCell(new PdfPCell(new Phrase("RR Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVLinesData.AddCell(new PdfPCell(new Phrase("Particulars", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVLinesData.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                foreach (var disbursementLine in disbursementLines)
                {
                    tableCVLinesData.AddCell(new PdfPCell(new Phrase(disbursementLine.CV, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLinesData.AddCell(new PdfPCell(new Phrase(disbursementLine.CVDate, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLinesData.AddCell(new PdfPCell(new Phrase(disbursementLine.Account, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLinesData.AddCell(new PdfPCell(new Phrase(disbursementLine.Article, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLinesData.AddCell(new PdfPCell(new Phrase(disbursementLine.RR, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLinesData.AddCell(new PdfPCell(new Phrase(disbursementLine.Particulars, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLinesData.AddCell(new PdfPCell(new Phrase(disbursementLine.Amount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    total = total + disbursementLine.Amount;
                }

                document.Add(tableCVLinesData);
                document.Add(Chunk.NEWLINE);

                // table CV Line total Footer
                PdfPTable tableCVLineFooter = new PdfPTable(7);
                float[] widthscellsTableCVLineFooter = new float[] { 15f, 10f, 25f, 25f, 15f, 30f, 20f };
                tableCVLineFooter.SetWidths(widthscellsTableCVLineFooter);
                tableCVLineFooter.WidthPercentage = 100;
                tableCVLineFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableCVLineFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableCVLineFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableCVLineFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableCVLineFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableCVLineFooter.AddCell(new PdfPCell(new Phrase("Total:", fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableCVLineFooter.AddCell(new PdfPCell(new Phrase(total.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                document.Add(tableCVLineFooter);
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