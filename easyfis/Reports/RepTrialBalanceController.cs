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
    public class RepTrialBalanceController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==========================
        // Trial Balance Report - PDF
        // ==========================
        [Authorize]
        public ActionResult TrialBalance(String StartDate, String EndDate, Int32 CompanyId, Int32 BranchId)
        {
            // ============
            // PDF Settings
            // ============
            MemoryStream workStream = new MemoryStream();
            Rectangle rectangle = new Rectangle(PageSize.A3);
            Document document = new Document(rectangle, 72, 72, 72, 72);
            document.SetMargins(30f, 30f, 30f, 30f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();

            // ===================
            // Fonts Customization
            // ===================
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
            var companyName = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.MstCompany.Company).FirstOrDefault();
            var address = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.MstCompany.Address).FirstOrDefault();
            var contactNo = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.MstCompany.ContactNumber).FirstOrDefault();
            var branch = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.Branch).FirstOrDefault();

            // ===========
            // Header Page
            // ===========
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase("Trial Balance", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Date From " + Convert.ToDateTime(StartDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + Convert.ToDateTime(EndDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);
            document.Add(line);

            // ============
            // Journal Data
            // ============
            var journals = from d in db.TrnJournals
                           where d.JournalDate >= Convert.ToDateTime(StartDate)
                           && d.JournalDate <= Convert.ToDateTime(EndDate)
                           && d.MstBranch.CompanyId == Convert.ToInt32(CompanyId)
                           && d.BranchId == Convert.ToInt32(BranchId)
                           group d by new
                           {
                               AccountCode = d.MstAccount.AccountCode,
                               Account = d.MstAccount.Account,
                               AccountCategoryId = d.MstAccount.MstAccountType.MstAccountCategory.Id,
                               AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
                           } into g
                           select new Models.TrnJournal
                           {
                               AccountCode = g.Key.AccountCode,
                               Account = g.Key.Account,
                               AccountCategoryId = g.Key.AccountCategoryId,
                               AccountCategory = g.Key.AccountCategory,
                               DebitAmount = g.Sum(d => d.DebitAmount),
                               CreditAmount = g.Sum(d => d.CreditAmount),
                           };

            if (journals.Any())
            {
                // ============
                // Branch Title
                // ============
                PdfPTable branchTitle = new PdfPTable(1);
                float[] widthCellsBranchTitle = new float[] { 100f };
                branchTitle.SetWidths(widthCellsBranchTitle);
                branchTitle.WidthPercentage = 100;
                branchTitle.AddCell(new PdfPCell(new Phrase(branch, fontArial12Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 14f, Border = 0 });
                document.Add(branchTitle);

                // ====
                // Data
                // ====
                PdfPTable tableData = new PdfPTable(4);
                float[] widthsCellsTableData = new float[] { 15f, 30f, 15f, 15f };
                tableData.SetWidths(widthsCellsTableData);
                tableData.WidthPercentage = 100;
                tableData.AddCell(new PdfPCell(new Phrase("Account Code", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Account", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Debit", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Credit", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal totalDebitAmount = 0;
                Decimal totalCreditAmount = 0;

                foreach (var journal in journals)
                {
                    totalDebitAmount = journals.Sum(d => d.DebitAmount);
                    totalCreditAmount = journals.Sum(d => d.CreditAmount);

                    tableData.AddCell(new PdfPCell(new Phrase(journal.AccountCode, fontArial10)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(journal.Account, fontArial10)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                }

                tableData.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { Colspan = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableData.AddCell(new PdfPCell(new Phrase(totalDebitAmount.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableData.AddCell(new PdfPCell(new Phrase(totalCreditAmount.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(tableData);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}