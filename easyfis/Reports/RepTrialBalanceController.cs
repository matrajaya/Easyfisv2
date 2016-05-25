using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepTrialBalanceController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Collection Summary Report
        [Authorize]
        public ActionResult TrialBalance(String StartDate, String EndDate, Int32 CompanyId)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Trial Balance", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // retrieve account category journal
            var journals = from d in db.TrnJournals
                           where d.JournalDate >= Convert.ToDateTime(StartDate)
                           && d.JournalDate <= Convert.ToDateTime(EndDate)
                           && d.MstBranch.CompanyId == CompanyId
                           group d by new
                           {
                               AccountCode = d.MstAccount.AccountCode,
                               Account = d.MstAccount.Account
                           } into g
                           select new Models.TrnJournal
                           {
                               AccountCode = g.Key.AccountCode,
                               Account = g.Key.Account,
                               DebitAmount = g.Sum(d => d.DebitAmount),
                               CreditAmount = g.Sum(d => d.CreditAmount),
                           };

            if (journals.Any())
            {
                document.Add(line);
                PdfPTable tableHeaderDetail = new PdfPTable(4);
                PdfPCell Cell = new PdfPCell();
                float[] widthscellsheader2 = new float[] { 15f, 30f, 15f, 15f };
                tableHeaderDetail.SetWidths(widthscellsheader2);
                tableHeaderDetail.WidthPercentage = 100;
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account Code", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal totalDebitAmount = 0;
                Decimal totalCreditAmount = 0;

                foreach (var journal in journals)
                {
                    // retrieve account category journal
                    var journalsForBalances = from d in db.TrnJournals
                                              where d.JournalDate >= Convert.ToDateTime(StartDate)
                                              && d.JournalDate <= Convert.ToDateTime(EndDate)
                                              && d.MstBranch.CompanyId == CompanyId
                                              group d by new
                                              {
                                                  AccountCategoryId = d.MstAccount.MstAccountType.MstAccountCategory.Id,
                                                  AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
                                              } into g
                                              select new Models.TrnJournal
                                              {
                                                  AccountCategoryId = g.Key.AccountCategoryId,
                                                  AccountCategory = g.Key.AccountCategory,
                                                  DebitAmount = g.Sum(d => d.DebitAmount),
                                                  CreditAmount = g.Sum(d => d.CreditAmount)
                                              };

                    totalDebitAmount = journalsForBalances.Sum(d => d.DebitAmount);
                    totalCreditAmount = journalsForBalances.Sum(d => d.CreditAmount);

                    Decimal balance = 0;
                    foreach (var journalsForBalance in journalsForBalances)
                    {
                        if (journalsForBalance.AccountCategoryId == 1)
                        {
                            balance = journal.DebitAmount - journal.CreditAmount;
                        }
                        else
                        {
                            balance = journal.CreditAmount - journal.DebitAmount;
                        }
                    }

                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.AccountCode, fontArial10)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Account, fontArial10)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                }

                document.Add(tableHeaderDetail);

                document.Add(Chunk.NEWLINE);
                document.Add(line);

                // table Balance Sheet footer in Asset CAtegory
                PdfPTable tableTrialBalanceFooter = new PdfPTable(4);
                float[] widthCellstableTrialBalanceFooter = new float[] { 15f, 30f, 15f, 15f };
                tableTrialBalanceFooter.SetWidths(widthCellstableTrialBalanceFooter);
                tableTrialBalanceFooter.WidthPercentage = 100;
                tableTrialBalanceFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableTrialBalanceFooter.AddCell(new PdfPCell(new Phrase("Totals", fontArial10Bold)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
                tableTrialBalanceFooter.AddCell(new PdfPCell(new Phrase(totalDebitAmount.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableTrialBalanceFooter.AddCell(new PdfPCell(new Phrase(totalCreditAmount.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                document.Add(tableTrialBalanceFooter);
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