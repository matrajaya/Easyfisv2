using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepIncomeStatementController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===========================
        // Income Statement PDF Report
        // ===========================  
        [Authorize]
        public ActionResult IncomeStatement(String StartDate, String EndDate, Int32 CompanyId)
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
            header.AddCell(new PdfPCell(new Phrase("Income Statement", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            header.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            header.AddCell(new PdfPCell(new Phrase("Date from " + Convert.ToDateTime(StartDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + Convert.ToDateTime(EndDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
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

            Decimal totalOverallIncomes = 0;
            Decimal totalOverallExpenses = 0;

            // ==========
            // Get Income
            // ==========
            var incomes = from d in db.TrnJournals
                          where d.JournalDate >= Convert.ToDateTime(StartDate)
                          && d.JournalDate <= Convert.ToDateTime(EndDate)
                          && d.MstAccount.MstAccountType.MstAccountCategory.Id == 5
                          && d.MstBranch.CompanyId == CompanyId
                          group d by d.MstAccount into g
                          select new Models.TrnJournal
                          {
                              DocumentReference = "5 - incomes",
                              AccountCategoryCode = g.Key.MstAccountType.MstAccountCategory.AccountCategoryCode,
                              AccountCategory = g.Key.MstAccountType.MstAccountCategory.AccountCategory,
                              SubCategoryDescription = g.Key.MstAccountType.SubCategoryDescription,
                              AccountTypeCode = g.Key.MstAccountType.AccountTypeCode,
                              AccountType = g.Key.MstAccountType.AccountType,
                              AccountCode = g.Key.AccountCode,
                              Account = g.Key.Account,
                              DebitAmount = g.Sum(d => d.DebitAmount),
                              CreditAmount = g.Sum(d => d.CreditAmount),
                              Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                          };

            if (incomes.Any())
            {
                // ===============================
                // Income Sub Category Description
                // ===============================
                var incomeSubCategoryDescriptions = from d in incomes
                                                    group d by new
                                                    {
                                                        SubCategoryDescription = d.SubCategoryDescription
                                                    } into g
                                                    select new
                                                    {
                                                        SubCategoryDescription = g.Key.SubCategoryDescription,
                                                        Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                    };

                if (incomeSubCategoryDescriptions.Any())
                {
                    Decimal totalAllIncomes = 0;
                    foreach (var incomeSubCategoryDescription in incomeSubCategoryDescriptions)
                    {
                        PdfPTable incomeSubCategoryDescriptionTable = new PdfPTable(1);
                        float[] widthCellsIncomeSubCategoryDescriptionTable = new float[] { 100f };
                        incomeSubCategoryDescriptionTable.SetWidths(widthCellsIncomeSubCategoryDescriptionTable);
                        incomeSubCategoryDescriptionTable.WidthPercentage = 100;
                        incomeSubCategoryDescriptionTable.AddCell(new PdfPCell(new Phrase(incomeSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        document.Add(incomeSubCategoryDescriptionTable);

                        // ====================
                        // Income Account Types
                        // ====================
                        var incomeAccountTypes = from d in incomes
                                                 where d.SubCategoryDescription.Equals(incomeSubCategoryDescription.SubCategoryDescription)
                                                 group d by new
                                                 {
                                                     AccountType = d.AccountType
                                                 } into g
                                                 select new
                                                 {
                                                     AccountType = g.Key.AccountType,
                                                     Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                 };

                        if (incomeAccountTypes.Any())
                        {
                            Decimal totalCurrentIncomes = 0;
                            foreach (var incomeAccountType in incomeAccountTypes)
                            {
                                totalCurrentIncomes += incomeAccountType.Balance;

                                PdfPTable incomeAccountTypeTable = new PdfPTable(3);
                                float[] widthCellsIncomeAccountTypeTable = new float[] { 50f, 100f, 50f };
                                incomeAccountTypeTable.SetWidths(widthCellsIncomeAccountTypeTable);
                                incomeAccountTypeTable.WidthPercentage = 100;
                                incomeAccountTypeTable.AddCell(new PdfPCell(new Phrase(incomeAccountType.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                incomeAccountTypeTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                incomeAccountTypeTable.AddCell(new PdfPCell(new Phrase(incomeAccountType.Balance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                document.Add(incomeAccountTypeTable);

                                // ===============
                                // Income Accounts
                                // ===============
                                var incomeAccounts = from d in incomes
                                                     where d.AccountType.Equals(incomeAccountType.AccountType)
                                                     group d by new
                                                     {
                                                         AccountCode = d.AccountCode,
                                                         Account = d.Account
                                                     } into g
                                                     select new
                                                     {
                                                         AccountCode = g.Key.AccountCode,
                                                         Account = g.Key.Account,
                                                         DebitAmount = g.Sum(d => d.DebitAmount),
                                                         CreditAmount = g.Sum(d => d.CreditAmount),
                                                         Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                     };

                                if (incomeAccounts.Any())
                                {
                                    foreach (var incomeAccount in incomeAccounts)
                                    {
                                        totalAllIncomes += incomeAccount.Balance;

                                        PdfPTable incomeAccountTable = new PdfPTable(3);
                                        float[] widthCellsIncomeAccountTable = new float[] { 50f, 100f, 50f };
                                        incomeAccountTable.SetWidths(widthCellsIncomeAccountTable);
                                        incomeAccountTable.WidthPercentage = 100;
                                        incomeAccountTable.AddCell(new PdfPCell(new Phrase(incomeAccount.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                        incomeAccountTable.AddCell(new PdfPCell(new Phrase(incomeAccount.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        incomeAccountTable.AddCell(new PdfPCell(new Phrase(incomeAccount.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                        document.Add(incomeAccountTable);
                                    }
                                }
                            }

                            // =====================
                            // Total Current Incomes
                            // =====================
                            document.Add(line);
                            PdfPTable totalCurrentIncomesTable = new PdfPTable(5);
                            float[] widthCellsTotalCurrentIncomesTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                            totalCurrentIncomesTable.SetWidths(widthCellsTotalCurrentIncomesTable);
                            totalCurrentIncomesTable.WidthPercentage = 100;
                            totalCurrentIncomesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentIncomesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentIncomesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentIncomesTable.AddCell(new PdfPCell(new Phrase("Total " + incomeSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentIncomesTable.AddCell(new PdfPCell(new Phrase(totalCurrentIncomes.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(totalCurrentIncomesTable);
                        }
                    }

                    // =================
                    // Total All Incomes
                    // =================
                    document.Add(line);
                    PdfPTable totalAllIncomesTable = new PdfPTable(5);
                    float[] widthCellsTotalAllIncomesTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                    totalAllIncomesTable.SetWidths(widthCellsTotalAllIncomesTable);
                    totalAllIncomesTable.WidthPercentage = 100;
                    totalAllIncomesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllIncomesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllIncomesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllIncomesTable.AddCell(new PdfPCell(new Phrase("Total Income", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllIncomesTable.AddCell(new PdfPCell(new Phrase(totalAllIncomes.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    document.Add(totalAllIncomesTable);

                    totalOverallIncomes += totalAllIncomes;
                    document.Add(Chunk.NEWLINE);
                }
            }

            // ============
            // Get Expenses
            // ============
            var expenses = from d in db.TrnJournals
                           where d.JournalDate >= Convert.ToDateTime(StartDate)
                           && d.JournalDate <= Convert.ToDateTime(EndDate)
                           && d.MstAccount.MstAccountType.MstAccountCategory.Id == 6
                           && d.MstBranch.CompanyId == CompanyId
                           group d by d.MstAccount into g
                           select new Models.TrnJournal
                           {
                               DocumentReference = "6 - Expenses",
                               AccountCategoryCode = g.Key.MstAccountType.MstAccountCategory.AccountCategoryCode,
                               AccountCategory = g.Key.MstAccountType.MstAccountCategory.AccountCategory,
                               SubCategoryDescription = g.Key.MstAccountType.SubCategoryDescription,
                               AccountTypeCode = g.Key.MstAccountType.AccountTypeCode,
                               AccountType = g.Key.MstAccountType.AccountType,
                               AccountCode = g.Key.AccountCode,
                               Account = g.Key.Account,
                               DebitAmount = g.Sum(d => d.DebitAmount),
                               CreditAmount = g.Sum(d => d.CreditAmount),
                               Balance = g.Sum(d => d.DebitAmount - d.CreditAmount)
                           };

            if (expenses.Any())
            {
                // ================================
                // Expense Sub Category Description
                // ================================
                var expenseSubCategoryDescriptions = from d in expenses
                                                     group d by new
                                                     {
                                                         SubCategoryDescription = d.SubCategoryDescription
                                                     } into g
                                                     select new
                                                     {
                                                         SubCategoryDescription = g.Key.SubCategoryDescription,
                                                         Balance = g.Sum(d => d.DebitAmount - d.CreditAmount)
                                                     };

                if (expenseSubCategoryDescriptions.Any())
                {
                    Decimal totalAllExpenses = 0;
                    foreach (var expenseSubCategoryDescription in expenseSubCategoryDescriptions)
                    {
                        PdfPTable expenseSubCategoryDescriptionTable = new PdfPTable(1);
                        float[] widthCellsExpenseSubCategoryDescriptionTable = new float[] { 100f };
                        expenseSubCategoryDescriptionTable.SetWidths(widthCellsExpenseSubCategoryDescriptionTable);
                        expenseSubCategoryDescriptionTable.WidthPercentage = 100;
                        expenseSubCategoryDescriptionTable.AddCell(new PdfPCell(new Phrase(expenseSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        document.Add(expenseSubCategoryDescriptionTable);

                        // =====================
                        // Expense Account Types
                        // =====================
                        var expenseAccountTypes = from d in expenses
                                                  where d.SubCategoryDescription.Equals(expenseSubCategoryDescription.SubCategoryDescription)
                                                  group d by new
                                                  {
                                                      AccountType = d.AccountType
                                                  } into g
                                                  select new
                                                  {
                                                      AccountType = g.Key.AccountType,
                                                      Balance = g.Sum(d => d.DebitAmount - d.CreditAmount)
                                                  };

                        if (expenseAccountTypes.Any())
                        {
                            Decimal totalCurrentExpenses = 0;
                            foreach (var expenseAccountType in expenseAccountTypes)
                            {
                                totalCurrentExpenses += expenseAccountType.Balance;

                                PdfPTable expenseAccountTypeTable = new PdfPTable(3);
                                float[] widthCellsExpenseAccountTypeTable = new float[] { 50f, 100f, 50f };
                                expenseAccountTypeTable.SetWidths(widthCellsExpenseAccountTypeTable);
                                expenseAccountTypeTable.WidthPercentage = 100;
                                expenseAccountTypeTable.AddCell(new PdfPCell(new Phrase(expenseAccountType.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                expenseAccountTypeTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                expenseAccountTypeTable.AddCell(new PdfPCell(new Phrase(expenseAccountType.Balance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                document.Add(expenseAccountTypeTable);

                                // ================
                                // Expense Accounts
                                // ================
                                var expenseAccounts = from d in expenses
                                                      where d.AccountType.Equals(expenseAccountType.AccountType)
                                                      group d by new
                                                      {
                                                          AccountCode = d.AccountCode,
                                                          Account = d.Account
                                                      } into g
                                                      select new
                                                      {
                                                          AccountCode = g.Key.AccountCode,
                                                          Account = g.Key.Account,
                                                          DebitAmount = g.Sum(d => d.DebitAmount),
                                                          CreditAmount = g.Sum(d => d.CreditAmount),
                                                          Balance = g.Sum(d => d.DebitAmount - d.CreditAmount)
                                                      };

                                if (expenseAccounts.Any())
                                {
                                    foreach (var expenseAccount in expenseAccounts)
                                    {
                                        totalAllExpenses += expenseAccount.Balance;

                                        PdfPTable expenseAccountTable = new PdfPTable(3);
                                        float[] widthCellsExpenseAccountTable = new float[] { 50f, 100f, 50f };
                                        expenseAccountTable.SetWidths(widthCellsExpenseAccountTable);
                                        expenseAccountTable.WidthPercentage = 100;
                                        expenseAccountTable.AddCell(new PdfPCell(new Phrase(expenseAccount.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                        expenseAccountTable.AddCell(new PdfPCell(new Phrase(expenseAccount.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        expenseAccountTable.AddCell(new PdfPCell(new Phrase(expenseAccount.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                        document.Add(expenseAccountTable);
                                    }
                                }
                            }

                            // ======================
                            // Total Current Expenses
                            // ======================
                            document.Add(line);
                            PdfPTable totalCurrentExpensesTable = new PdfPTable(5);
                            float[] widthCellsTotalCurrentExpensesTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                            totalCurrentExpensesTable.SetWidths(widthCellsTotalCurrentExpensesTable);
                            totalCurrentExpensesTable.WidthPercentage = 100;
                            totalCurrentExpensesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentExpensesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentExpensesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentExpensesTable.AddCell(new PdfPCell(new Phrase("Total " + expenseSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentExpensesTable.AddCell(new PdfPCell(new Phrase(totalCurrentExpenses.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(totalCurrentExpensesTable);
                        }
                    }

                    // ==================
                    // Total All Expenses
                    // ==================
                    document.Add(line);
                    PdfPTable totalAllExpensesTable = new PdfPTable(5);
                    float[] widthCellsTotalAllExpensesTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                    totalAllExpensesTable.SetWidths(widthCellsTotalAllExpensesTable);
                    totalAllExpensesTable.WidthPercentage = 100;
                    totalAllExpensesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllExpensesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllExpensesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllExpensesTable.AddCell(new PdfPCell(new Phrase("Total Expense", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllExpensesTable.AddCell(new PdfPCell(new Phrase(totalAllExpenses.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    document.Add(totalAllExpensesTable);

                    totalOverallExpenses += totalAllExpenses;
                    document.Add(Chunk.NEWLINE);
                }
            }

            document.Add(Chunk.NEWLINE);

            // ==========
            // Net Income
            // ==========
            Decimal NetIncome = totalOverallIncomes - totalOverallExpenses;

            document.Add(line);
            PdfPTable netIncomeTable = new PdfPTable(5);
            float[] widthCellsNetIncomeTable = new float[] { 50f, 70f, 100f, 100f, 60f };
            netIncomeTable.SetWidths(widthCellsNetIncomeTable);
            netIncomeTable.WidthPercentage = 100;
            netIncomeTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            netIncomeTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            netIncomeTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            netIncomeTable.AddCell(new PdfPCell(new Phrase("Net Income (Loss)", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            netIncomeTable.AddCell(new PdfPCell(new Phrase(NetIncome.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            document.Add(netIncomeTable);

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}