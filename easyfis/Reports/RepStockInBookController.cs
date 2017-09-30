using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepStockInBookController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==========================
        // Stock In Book Report - PDF
        // ==========================
        [Authorize]
        public ActionResult StockInBook(String StartDate, String EndDate, Int32 CompanyId, Int32 BranchId)
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
            headerPage.AddCell(new PdfPCell(new Phrase("Stock-In Book", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Date From " + Convert.ToDateTime(StartDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + Convert.ToDateTime(EndDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);
            document.Add(line);

            // ========
            // Get Data
            // ========
            var journals = from d in db.TrnJournals
                           where d.JournalDate >= Convert.ToDateTime(StartDate)
                           && d.JournalDate <= Convert.ToDateTime(EndDate)
                           && d.MstBranch.CompanyId == CompanyId
                           && d.BranchId == BranchId
                           && d.INId != null
                           select new
                           {
                               DocumentReference = d.DocumentReference,
                               AccountCode = d.MstAccount.AccountCode,
                               Account = d.MstAccount.Account,
                               Article = d.MstArticle.Article,
                               Particulars = d.Particulars,
                               DebitAmount = d.DebitAmount,
                               CreditAmount = d.CreditAmount
                           };

            if (journals.Any())
            {
                // ==================
                // Document Reference
                // ==================
                var documentReferences = from d in journals
                                         group d by new
                                         {
                                             d.DocumentReference
                                         } into g
                                         select new
                                         {
                                             DocumentReference = g.Key.DocumentReference
                                         };

                if (documentReferences.Any())
                {
                    // ============
                    // Branch Title
                    // ============
                    PdfPTable branchTitle = new PdfPTable(1);
                    float[] widthCellsBranchTitle = new float[] { 100f };
                    branchTitle.SetWidths(widthCellsBranchTitle);
                    branchTitle.WidthPercentage = 100;
                    PdfPCell branchHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                    branchTitle.AddCell(branchHeaderColspan);
                    document.Add(branchTitle);

                    Decimal totalDebitAmount = 0;
                    Decimal totalCreditAmount = 0;
                    Decimal totalBalance = 0;

                    foreach (var documentReference in documentReferences)
                    {
                        PdfPTable tableDocumentReference = new PdfPTable(1);
                        float[] widthCellsTableDocumentReference = new float[] { 100f };
                        tableDocumentReference.SetWidths(widthCellsTableDocumentReference);
                        tableDocumentReference.WidthPercentage = 100;
                        tableDocumentReference.AddCell(new PdfPCell(new Phrase(documentReference.DocumentReference, fontArial11Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                        document.Add(tableDocumentReference);

                        // ====
                        // Data
                        // ====
                        var journalDatas = from d in journals
                                           where d.DocumentReference.Equals(documentReference.DocumentReference)
                                           select new
                                           {
                                               DocumentReference = d.DocumentReference,
                                               AccountCode = d.AccountCode,
                                               Account = d.Account,
                                               Article = d.Article,
                                               Particulars = d.Particulars,
                                               DebitAmount = d.DebitAmount,
                                               CreditAmount = d.CreditAmount
                                           };

                        if (journalDatas.Any())
                        {
                            PdfPTable tableJournalData = new PdfPTable(6);
                            float[] widthsCellsTableJournalData = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                            tableJournalData.SetWidths(widthsCellsTableJournalData);
                            tableJournalData.WidthPercentage = 100;
                            tableJournalData.AddCell(new PdfPCell(new Phrase("Code", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableJournalData.AddCell(new PdfPCell(new Phrase("Account", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableJournalData.AddCell(new PdfPCell(new Phrase("Article", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableJournalData.AddCell(new PdfPCell(new Phrase("Debit", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableJournalData.AddCell(new PdfPCell(new Phrase("Credit", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableJournalData.AddCell(new PdfPCell(new Phrase("Balance", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                            Decimal subTotalDebitAmount = 0;
                            Decimal subTotalCreditAmount = 0;
                            Decimal subTotalBalance = 0;

                            foreach (var journalData in journalDatas)
                            {
                                Decimal balance = journalData.DebitAmount - journalData.CreditAmount;

                                tableJournalData.AddCell(new PdfPCell(new Phrase(journalData.AccountCode, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJournalData.AddCell(new PdfPCell(new Phrase(journalData.Account, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJournalData.AddCell(new PdfPCell(new Phrase(journalData.Article, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJournalData.AddCell(new PdfPCell(new Phrase(journalData.DebitAmount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJournalData.AddCell(new PdfPCell(new Phrase(journalData.CreditAmount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableJournalData.AddCell(new PdfPCell(new Phrase(balance.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                                subTotalDebitAmount += journalData.DebitAmount;
                                subTotalCreditAmount += journalData.CreditAmount;
                                subTotalBalance += balance;
                            }

                            tableJournalData.AddCell(new PdfPCell(new Phrase("Sub Total", fontArial10Bold)) { Colspan = 3, HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJournalData.AddCell(new PdfPCell(new Phrase(subTotalDebitAmount.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJournalData.AddCell(new PdfPCell(new Phrase(subTotalCreditAmount.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableJournalData.AddCell(new PdfPCell(new Phrase(subTotalBalance.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            document.Add(tableJournalData);

                            totalDebitAmount += subTotalDebitAmount;
                            totalCreditAmount += subTotalCreditAmount;
                            totalBalance += subTotalBalance;
                        }
                    }

                    document.Add(line);

                    // =====
                    // Total
                    // =====
                    PdfPTable tableTotalFooter = new PdfPTable(6);
                    float[] widthCellstableTotalFooter = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                    tableTotalFooter.SetWidths(widthCellstableTotalFooter);
                    tableTotalFooter.WidthPercentage = 100;
                    tableTotalFooter.AddCell(new PdfPCell(new Phrase("Total", fontArial10Bold)) { Colspan = 3, HorizontalAlignment = 2, Border = 0, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 25f });
                    tableTotalFooter.AddCell(new PdfPCell(new Phrase(totalDebitAmount.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 5f, PaddingBottom = 5f });
                    tableTotalFooter.AddCell(new PdfPCell(new Phrase(totalCreditAmount.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 50f });
                    tableTotalFooter.AddCell(new PdfPCell(new Phrase(totalBalance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 5f });
                    document.Add(tableTotalFooter);
                }
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