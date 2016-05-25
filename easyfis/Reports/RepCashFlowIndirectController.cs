using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepCashFlowIndirectController : Controller
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
        public ActionResult CashFlowIndirect(String StartDate, String EndDate, Int32 CompanyId)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Cash Flow (Indirect)", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var identityUserId = User.Identity.GetUserId();
            var mstUserId = from d in db.MstUsers where d.UserId == identityUserId select d;
            var incomeAccount = from d in db.MstAccounts where d.Id == mstUserId.FirstOrDefault().IncomeAccountId select d;

            var cashFlowIncome = from d in db.TrnJournals
                                 where d.MstBranch.CompanyId == CompanyId
                                 && d.JournalDate >= Convert.ToDateTime(StartDate)
                                 && d.JournalDate <= Convert.ToDateTime(EndDate)
                                 && (d.MstAccount.MstAccountType.AccountCategoryId == 5 || d.MstAccount.MstAccountType.AccountCategoryId == 6)
                                 select new Models.TrnJournal
                                 {
                                     AccountCashFlowCode = d.MstAccount.MstAccountCashFlow.AccountCashFlowCode,
                                     AccountCashFlow = d.MstAccount.MstAccountCashFlow.AccountCashFlow,
                                     AccountTypeCode = d.MstAccount.MstAccountType.AccountTypeCode,
                                     AccountType = d.MstAccount.MstAccountType.AccountType,
                                     AccountCode = d.MstAccount.AccountCode,
                                     Account = d.MstAccount.Account,
                                     DebitAmount = d.DebitAmount,
                                     CreditAmount = d.CreditAmount
                                 };

            var cashFlowBalanceSheet = from d in db.TrnJournals
                                       where d.MstBranch.CompanyId == CompanyId
                                       && d.JournalDate >= Convert.ToDateTime(StartDate)
                                       && d.JournalDate <= Convert.ToDateTime(EndDate)
                                       && d.MstAccount.MstAccountType.AccountCategoryId < 5
                                       && (d.MstAccount.AccountCashFlowId == null ? 4 : d.MstAccount.AccountCashFlowId) <= 3
                                       select new Models.TrnJournal
                                       {
                                           AccountCashFlowCode = d.MstAccount.MstAccountCashFlow.AccountCashFlowCode,
                                           AccountCashFlow = d.MstAccount.MstAccountCashFlow.AccountCashFlow,
                                           AccountTypeCode = d.MstAccount.MstAccountType.AccountTypeCode,
                                           AccountType = d.MstAccount.MstAccountType.AccountType,
                                           AccountCode = d.MstAccount.AccountCode,
                                           Account = d.MstAccount.Account,
                                           DebitAmount = d.DebitAmount,
                                           CreditAmount = d.CreditAmount
                                       };

            var accountCashFlowGroupFromCashFlowIncome = from d in cashFlowIncome
                                                         group d by new
                                                         {
                                                             AccountCashFlowCode = d.AccountCashFlowCode,
                                                             AccountCashFlow = d.AccountCashFlow
                                                         } into g
                                                         select new Models.TrnJournal
                                                         {
                                                             AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                             AccountCashFlow = g.Key.AccountCashFlow
                                                         };

            var accountCashFlowGroupFromCashFlowBalanceSheet = from d in cashFlowBalanceSheet
                                                               group d by new
                                                               {
                                                                   AccountCashFlowCode = d.AccountCashFlowCode,
                                                                   AccountCashFlow = d.AccountCashFlow
                                                               } into g
                                                               select new Models.TrnJournal
                                                               {
                                                                   AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                                   AccountCashFlow = g.Key.AccountCashFlow
                                                               };

            var unionAccountCashFlowGroups = accountCashFlowGroupFromCashFlowIncome.Union(accountCashFlowGroupFromCashFlowBalanceSheet).OrderBy(d => d.AccountCashFlowCode);
            if (unionAccountCashFlowGroups.Any())
            {
                Decimal totalBalanceAmountOfAllBranches = 0;
                foreach (var unionAccountCashFlowGroup in unionAccountCashFlowGroups)
                {
                    PdfPTable tableCashFlow = new PdfPTable(3);
                    float[] widthCellstableTableCashFlow = new float[] { 50f, 150f, 30f };
                    tableCashFlow.SetWidths(widthCellstableTableCashFlow);
                    tableCashFlow.WidthPercentage = 100;

                    document.Add(line);

                    PdfPCell cashFlowColspan = (new PdfPCell(new Phrase(unionAccountCashFlowGroup.AccountCashFlow, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    cashFlowColspan.Colspan = 3;
                    tableCashFlow.AddCell(cashFlowColspan);

                    var accountTypeGroupFromCashFlowIncome = from d in cashFlowIncome
                                                             where d.AccountCashFlow == unionAccountCashFlowGroup.AccountCashFlow
                                                             group d by new
                                                             {
                                                                 AccountCashFlowCode = d.AccountCashFlowCode,
                                                                 AccountCashFlow = d.AccountCashFlow,
                                                                 AccountTypeCode = incomeAccount.FirstOrDefault().MstAccountType.AccountTypeCode,
                                                                 AccountType = incomeAccount.FirstOrDefault().MstAccountType.AccountType,
                                                                 AccountCode = "0000",
                                                                 Account = incomeAccount.FirstOrDefault().Account,
                                                             } into g
                                                             select new Models.TrnJournal
                                                             {
                                                                 AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                                 AccountCashFlow = g.Key.AccountCashFlow,
                                                                 AccountTypeCode = g.Key.AccountTypeCode,
                                                                 AccountType = g.Key.AccountType,
                                                                 AccountCode = g.Key.AccountCode,
                                                                 Account = g.Key.Account,
                                                                 DebitAmount = g.Sum(d => d.DebitAmount),
                                                                 CreditAmount = g.Sum(d => d.CreditAmount),
                                                                 Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                             };

                    var accountTypeGroupFromCashFlowBalanceSheet = from d in cashFlowBalanceSheet
                                                                   where d.AccountCashFlow == unionAccountCashFlowGroup.AccountCashFlow
                                                                   group d by new
                                                                   {
                                                                       AccountCashFlowCode = d.AccountCashFlowCode,
                                                                       AccountCashFlow = d.AccountCashFlow,
                                                                       AccountTypeCode = d.AccountTypeCode,
                                                                       AccountType = d.AccountType,
                                                                       AccountCode = d.AccountCode,
                                                                       Account = d.Account
                                                                   } into g
                                                                   select new Models.TrnJournal
                                                                   {
                                                                       AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                                       AccountCashFlow = g.Key.AccountCashFlow,
                                                                       AccountTypeCode = g.Key.AccountTypeCode,
                                                                       AccountType = g.Key.AccountType,
                                                                       AccountCode = g.Key.AccountCode,
                                                                       Account = g.Key.Account,
                                                                       DebitAmount = g.Sum(d => d.DebitAmount),
                                                                       CreditAmount = g.Sum(d => d.CreditAmount),
                                                                       Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                   };

                    var unionAccountTypeGroups = accountTypeGroupFromCashFlowIncome.Union(accountTypeGroupFromCashFlowBalanceSheet).OrderBy(d => d.AccountCode);

                    Decimal totalBalanceAmount = 0;
                    foreach (var unionAccountTypeGroup in unionAccountTypeGroups)
                    {
                        var accountGroupFromCashFlowIncome = from d in accountTypeGroupFromCashFlowIncome
                                                             where d.AccountCode == unionAccountTypeGroup.AccountCode
                                                             group d by new
                                                             {
                                                                 AccountCashFlowCode = d.AccountCashFlowCode,
                                                                 AccountCashFlow = d.AccountCashFlow,
                                                                 AccountTypeCode = incomeAccount.FirstOrDefault().MstAccountType.AccountTypeCode,
                                                                 AccountType = incomeAccount.FirstOrDefault().MstAccountType.AccountType,
                                                                 AccountCode = "0000",
                                                                 Account = incomeAccount.FirstOrDefault().Account,
                                                             } into g
                                                             select new Models.TrnJournal
                                                             {
                                                                 AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                                 AccountCashFlow = g.Key.AccountCashFlow,
                                                                 AccountTypeCode = g.Key.AccountTypeCode,
                                                                 AccountType = g.Key.AccountType,
                                                                 AccountCode = g.Key.AccountCode,
                                                                 Account = g.Key.Account,
                                                                 DebitAmount = g.Sum(d => d.DebitAmount),
                                                                 CreditAmount = g.Sum(d => d.CreditAmount),
                                                                 Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                             };

                        var accountGroupFromCashFlowBalanceSheet = from d in accountTypeGroupFromCashFlowBalanceSheet
                                                                   where d.AccountCode == unionAccountTypeGroup.AccountCode
                                                                   group d by new
                                                                   {
                                                                       AccountCashFlowCode = d.AccountCashFlowCode,
                                                                       AccountCashFlow = d.AccountCashFlow,
                                                                       AccountTypeCode = d.AccountTypeCode,
                                                                       AccountType = d.AccountType,
                                                                       AccountCode = d.AccountCode,
                                                                       Account = d.Account
                                                                   } into g
                                                                   select new Models.TrnJournal
                                                                   {
                                                                       AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                                       AccountCashFlow = g.Key.AccountCashFlow,
                                                                       AccountTypeCode = g.Key.AccountTypeCode,
                                                                       AccountType = g.Key.AccountType,
                                                                       AccountCode = g.Key.AccountCode,
                                                                       Account = g.Key.Account,
                                                                       DebitAmount = g.Sum(d => d.DebitAmount),
                                                                       CreditAmount = g.Sum(d => d.CreditAmount),
                                                                       Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                   };

                        PdfPCell accountColspan = (new PdfPCell(new Phrase(unionAccountTypeGroup.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                        accountColspan.Colspan = 3;
                        tableCashFlow.AddCell(accountColspan);

                        var unionAccountGroups = accountGroupFromCashFlowIncome.Union(accountGroupFromCashFlowBalanceSheet).OrderBy(d => d.AccountCode);
                        foreach (var unionAccountGroup in unionAccountGroups)
                        {
                            tableCashFlow.AddCell(new PdfPCell(new Phrase(unionAccountGroup.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                            tableCashFlow.AddCell(new PdfPCell(new Phrase(unionAccountGroup.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                            tableCashFlow.AddCell(new PdfPCell(new Phrase(unionAccountGroup.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });

                            totalBalanceAmount = totalBalanceAmount + unionAccountGroup.Balance;
                        }
                    }

                    document.Add(tableCashFlow);

                    document.Add(line);
                    PdfPTable tableCashFlowTotal = new PdfPTable(3);
                    float[] widthCellstableTableCashFlowTotal = new float[] { 50f, 150f, 30f };
                    tableCashFlowTotal.SetWidths(widthCellstableTableCashFlowTotal);
                    tableCashFlowTotal.WidthPercentage = 100;
                    tableCashFlowTotal.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                    tableCashFlowTotal.AddCell(new PdfPCell(new Phrase("Total " + unionAccountCashFlowGroup.AccountCashFlow, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                    tableCashFlowTotal.AddCell(new PdfPCell(new Phrase(totalBalanceAmount.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    document.Add(tableCashFlowTotal);
                    document.Add(Chunk.NEWLINE);

                    totalBalanceAmountOfAllBranches = totalBalanceAmountOfAllBranches + totalBalanceAmount;
                }

                document.Add(line);
                PdfPTable tableCashFlowTotalAllBranches = new PdfPTable(3);
                float[] widthCellstableTableCashFlowTotalAllBranches = new float[] { 50f, 150f, 30f };
                tableCashFlowTotalAllBranches.SetWidths(widthCellstableTableCashFlowTotalAllBranches);
                tableCashFlowTotalAllBranches.WidthPercentage = 100;
                tableCashFlowTotalAllBranches.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableCashFlowTotalAllBranches.AddCell(new PdfPCell(new Phrase("All Branches Cash Balance ", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 20f });
                tableCashFlowTotalAllBranches.AddCell(new PdfPCell(new Phrase(totalBalanceAmountOfAllBranches.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 5f, PaddingBottom = 5f });
                document.Add(tableCashFlowTotalAllBranches);
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