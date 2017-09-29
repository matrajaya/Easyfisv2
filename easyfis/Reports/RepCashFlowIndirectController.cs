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
    public class RepCashFlowIndirectController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =============================
        // Cash Flow Indirect PDF Report
        // =============================
        [Authorize]
        public ActionResult CashFlowIndirect(String StartDate, String EndDate, Int32 CompanyId)
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

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 4.5F)));

            // ==============
            // Company Detail
            // ==============
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // ======
            // Header
            // ======
            PdfPTable header = new PdfPTable(2);
            float[] widthsCellsHeader = new float[] { 100f, 75f };
            header.SetWidths(widthsCellsHeader);
            header.WidthPercentage = 100;
            header.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            header.AddCell(new PdfPCell(new Phrase("Cash Flow (Indirect)", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            header.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            header.AddCell(new PdfPCell(new Phrase("Date From " + Convert.ToDateTime(StartDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + Convert.ToDateTime(EndDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            header.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            header.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(header);
            document.Add(line);

            // =====
            // Space
            // =====
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { Border = 0, PaddingTop = 5f });
            document.Add(spaceTable);

            // ======================
            // Default Income Account
            // ======================
            var identityUserId = User.Identity.GetUserId();
            var mstUserId = from d in db.MstUsers where d.UserId == identityUserId select d;
            var incomeAccount = from d in db.MstAccounts where d.Id == mstUserId.FirstOrDefault().IncomeAccountId select d;

            // ================
            // Cash Flow Income
            // ================
            var cashFlowIncome = from d in db.TrnJournals
                                 where d.MstBranch.CompanyId == CompanyId
                                 && d.JournalDate >= Convert.ToDateTime(StartDate)
                                 && d.JournalDate <= Convert.ToDateTime(EndDate)
                                 && (d.MstAccount.MstAccountType.AccountCategoryId == 5 || d.MstAccount.MstAccountType.AccountCategoryId == 6)
                                 select new
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

            // =======================
            // Cash Flow Balance Sheet
            // =======================
            var cashFlowBalanceSheet = from d in db.TrnJournals
                                       where d.MstBranch.CompanyId == CompanyId
                                       && d.JournalDate >= Convert.ToDateTime(StartDate)
                                       && d.JournalDate <= Convert.ToDateTime(EndDate)
                                       && d.MstAccount.MstAccountType.AccountCategoryId < 5
                                       && d.MstAccount.AccountCashFlowId <= 3
                                       select new
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

            // ==========
            // Cash Flows
            // ==========
            var groupedCashFlowIncome = from d in cashFlowIncome
                                        group d by new
                                        {
                                            AccountCashFlowCode = d.AccountCashFlowCode,
                                            AccountCashFlow = d.AccountCashFlow
                                        } into g
                                        select new
                                        {
                                            AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                            AccountCashFlow = g.Key.AccountCashFlow
                                        };

            var groupedCashFlowBalanceSheet = from d in cashFlowBalanceSheet
                                              group d by new
                                              {
                                                  AccountCashFlowCode = d.AccountCashFlowCode,
                                                  AccountCashFlow = d.AccountCashFlow
                                              } into g
                                              select new
                                              {
                                                  AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                  AccountCashFlow = g.Key.AccountCashFlow
                                              };

            // ================
            // Union Cash Flows
            // ================
            var unionCashFlows = groupedCashFlowIncome.Union(groupedCashFlowBalanceSheet).OrderBy(d => d.AccountCashFlowCode);

            if (unionCashFlows.Any())
            {
                Decimal totalCashFlowOfAllBranches = 0;

                foreach (var unionCashFlow in unionCashFlows)
                {
                    PdfPTable tableCashFlow = new PdfPTable(3);
                    float[] widthCellsTableCashFlow = new float[] { 50f, 100f, 50f };
                    tableCashFlow.SetWidths(widthCellsTableCashFlow);
                    tableCashFlow.WidthPercentage = 100;
                    tableCashFlow.AddCell(new PdfPCell(new Phrase(unionCashFlow.AccountCashFlow, fontArial10Bold)) { Colspan = 3, Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    document.Add(tableCashFlow);

                    // =============
                    // Account Types
                    // =============
                    var groupedAccountTypesIncome = from d in cashFlowIncome
                                                    group d by new
                                                    {
                                                        AccountCashFlowCode = d.AccountCashFlowCode,
                                                        AccountCashFlow = d.AccountCashFlow,
                                                        AccountTypeCode = incomeAccount.FirstOrDefault().MstAccountType.AccountTypeCode,
                                                        AccountType = incomeAccount.FirstOrDefault().MstAccountType.AccountType,
                                                        AccountCode = "0000",
                                                        Account = incomeAccount.FirstOrDefault().Account,
                                                    } into g
                                                    select new
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

                    var groupedAccountTypesBalanceSheet = from d in cashFlowBalanceSheet
                                                          group d by new
                                                          {
                                                              AccountCashFlowCode = d.AccountCashFlowCode,
                                                              AccountCashFlow = d.AccountCashFlow,
                                                              AccountTypeCode = d.AccountTypeCode,
                                                              AccountType = d.AccountType,
                                                              AccountCode = d.AccountCode,
                                                              Account = d.Account
                                                          } into g
                                                          select new
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

                    var unionAccountTypes = groupedAccountTypesIncome.Union(groupedAccountTypesBalanceSheet).Where(d => d.AccountCashFlow.Equals(unionCashFlow.AccountCashFlow)).OrderBy(d => d.AccountCode);

                    if (unionAccountTypes.Any())
                    {
                        Decimal totalCashFlow = 0;

                        foreach (var unionAccountType in unionAccountTypes)
                        {
                            PdfPTable tableAccountType = new PdfPTable(3);
                            float[] widthCellsTableAccountType = new float[] { 50f, 100f, 50f };
                            tableAccountType.SetWidths(widthCellsTableAccountType);
                            tableAccountType.WidthPercentage = 100;
                            tableAccountType.AddCell(new PdfPCell(new Phrase(unionAccountType.AccountType, fontArial10Bold)) { Colspan = 3, Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                            document.Add(tableAccountType);

                            // ========
                            // Accounts
                            // ========
                            var groupedAccountIncome = from d in groupedAccountTypesIncome
                                                       where d.AccountCode.Equals(unionAccountType.AccountCode)
                                                       group d by new
                                                       {
                                                           AccountCashFlowCode = d.AccountCashFlowCode,
                                                           AccountCashFlow = d.AccountCashFlow,
                                                           AccountTypeCode = incomeAccount.FirstOrDefault().MstAccountType.AccountTypeCode,
                                                           AccountType = incomeAccount.FirstOrDefault().MstAccountType.AccountType,
                                                           AccountCode = "0000",
                                                           Account = incomeAccount.FirstOrDefault().Account,
                                                       } into g
                                                       select new
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

                            var groupedAccountBalanceSheet = from d in groupedAccountTypesBalanceSheet
                                                             where d.AccountCode.Equals(unionAccountType.AccountCode)
                                                             group d by new
                                                             {
                                                                 AccountCashFlowCode = d.AccountCashFlowCode,
                                                                 AccountCashFlow = d.AccountCashFlow,
                                                                 AccountTypeCode = d.AccountTypeCode,
                                                                 AccountType = d.AccountType,
                                                                 AccountCode = d.AccountCode,
                                                                 Account = d.Account
                                                             } into g
                                                             select new
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

                            var unionAccountGroups = groupedAccountIncome.Union(groupedAccountBalanceSheet).OrderBy(d => d.AccountCode);

                            if (unionAccountGroups.Any())
                            {
                                foreach (var unionAccountGroup in unionAccountGroups)
                                {
                                    PdfPTable tableAccount = new PdfPTable(3);
                                    float[] widthCellsTableAccount = new float[] { 50f, 100f, 50f };
                                    tableAccount.SetWidths(widthCellsTableAccount);
                                    tableAccount.WidthPercentage = 100;
                                    tableAccount.AddCell(new PdfPCell(new Phrase(unionAccountGroup.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                    tableAccount.AddCell(new PdfPCell(new Phrase(unionAccountGroup.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                    tableAccount.AddCell(new PdfPCell(new Phrase(unionAccountGroup.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                    document.Add(tableAccount);

                                    totalCashFlow += unionAccountGroup.Balance;
                                }
                            }
                        }

                        // ===============
                        // Total Cash Flow
                        // ===============
                        document.Add(line);

                        PdfPTable tableTotalCashFlow = new PdfPTable(5);
                        float[] widthCellsTableTotalCashFlow = new float[] { 50f, 70f, 100f, 100f, 60f };
                        tableTotalCashFlow.SetWidths(widthCellsTableTotalCashFlow);
                        tableTotalCashFlow.WidthPercentage = 100;
                        tableTotalCashFlow.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                        tableTotalCashFlow.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                        tableTotalCashFlow.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                        tableTotalCashFlow.AddCell(new PdfPCell(new Phrase("Total " + unionCashFlow.AccountCashFlow, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                        tableTotalCashFlow.AddCell(new PdfPCell(new Phrase(totalCashFlow.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        document.Add(tableTotalCashFlow);
                        document.Add(Chunk.NEWLINE);

                        totalCashFlowOfAllBranches = totalCashFlowOfAllBranches + totalCashFlow;
                    }
                }

                // ===============================
                // Total Cash Flow of All Branches
                // ===============================
                document.Add(line);

                PdfPTable tableCashFlowTotalAllBranches = new PdfPTable(5);
                float[] widthCellsTableCashFlowTotalAllBranches = new float[] { 50f, 70f, 100f, 100f, 60f };
                tableCashFlowTotalAllBranches.SetWidths(widthCellsTableCashFlowTotalAllBranches);
                tableCashFlowTotalAllBranches.WidthPercentage = 100;
                tableCashFlowTotalAllBranches.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableCashFlowTotalAllBranches.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableCashFlowTotalAllBranches.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableCashFlowTotalAllBranches.AddCell(new PdfPCell(new Phrase("All Branches Cash Balance ", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 20f });
                tableCashFlowTotalAllBranches.AddCell(new PdfPCell(new Phrase(totalCashFlowOfAllBranches.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 5f, PaddingBottom = 5f });
                document.Add(tableCashFlowTotalAllBranches);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}