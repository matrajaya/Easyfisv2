using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepIncomeStatementController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // PDF Collection Summary Report
        [Authorize]
        public ActionResult IncomeStatement(String StartDate, String EndDate, Int32 CompanyId)
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
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // table main header
            PdfPTable tableHeaderPage = new PdfPTable(2);
            float[] widthsCellsheaderPage = new float[] { 100f, 75f };
            tableHeaderPage.SetWidths(widthsCellsheaderPage);
            tableHeaderPage.WidthPercentage = 100;
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Income Statement", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // Income
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

            Decimal totalOverAllIncome = 0;
            if (incomes.Any())
            {
                String subCategoryDescription = "";
                Boolean subCategoryDescriptionSame = false;
                foreach (var incomeSubCategoryDescription in incomes)
                {
                    if (subCategoryDescriptionSame == false)
                    {
                        if (subCategoryDescription.Equals(incomeSubCategoryDescription.SubCategoryDescription) == false)
                        {
                            Decimal totalAllIncome = 0;
                            subCategoryDescription = incomeSubCategoryDescription.SubCategoryDescription;
                            PdfPTable tableSubCategoryDescriptionIncome = new PdfPTable(1);
                            float[] widthSubCategoryDescriptionIncomeCells = new float[] { 100f };
                            tableSubCategoryDescriptionIncome.SetWidths(widthSubCategoryDescriptionIncomeCells);
                            tableSubCategoryDescriptionIncome.WidthPercentage = 100;
                            document.Add(line);
                            tableSubCategoryDescriptionIncome.AddCell(new PdfPCell(new Phrase(incomeSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            document.Add(tableSubCategoryDescriptionIncome);

                            String accountType = "";
                            Boolean accountTypeSame = false;
                            foreach (var incomeAccountTypes in incomes)
                            {
                                if (accountTypeSame == false)
                                {
                                    if (accountType.Equals(incomeAccountTypes.AccountType) == false)
                                    {
                                        accountType = incomeAccountTypes.AccountType;
                                        if (incomeAccountTypes.SubCategoryDescription.Equals(subCategoryDescription) == true)
                                        {
                                            Decimal accountTypeTotalBalance = 0;
                                            foreach (var accountTypeBalance in incomes)
                                            {
                                                if (accountTypeBalance.AccountType.Equals(accountType) == true)
                                                {
                                                    accountTypeTotalBalance = accountTypeTotalBalance + accountTypeBalance.Balance;
                                                }
                                            }

                                            PdfPTable tableAccountTypes = new PdfPTable(3);
                                            float[] widthCellsAccountTypes = new float[] { 50f, 100f, 50f };
                                            tableAccountTypes.SetWidths(widthCellsAccountTypes);
                                            tableAccountTypes.WidthPercentage = 100;
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase(incomeAccountTypes.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase(accountTypeTotalBalance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                            document.Add(tableAccountTypes);

                                            String account = "";
                                            Boolean accountSame = false;
                                            foreach (var incomeAccounts in incomes)
                                            {
                                                if (accountSame == false)
                                                {
                                                    if (account.Equals(incomeAccounts.Account) == false)
                                                    {
                                                        account = incomeAccounts.Account;
                                                        if (incomeAccounts.AccountType.Equals(accountType) == true)
                                                        {
                                                            PdfPTable tableAccounts = new PdfPTable(3);
                                                            float[] widthCellsAccounts = new float[] { 50f, 100f, 50f };
                                                            tableAccounts.SetWidths(widthCellsAccounts);
                                                            tableAccounts.WidthPercentage = 100;
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(incomeAccounts.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(incomeAccounts.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(incomeAccounts.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                                            document.Add(tableAccounts);

                                                            totalAllIncome = totalAllIncome + incomeAccounts.Balance;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    accountSame = false;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        accountTypeSame = true;
                                    }
                                }
                                else
                                {
                                    accountTypeSame = false;
                                }
                            }

                            document.Add(line);
                            PdfPTable tableTotalAllIncomes = new PdfPTable(5);
                            float[] widthTotalTotalAllIncomesCells = new float[] { 50f, 70f, 100f, 100f, 60f };
                            tableTotalAllIncomes.SetWidths(widthTotalTotalAllIncomesCells);
                            tableTotalAllIncomes.WidthPercentage = 100;
                            tableTotalAllIncomes.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllIncomes.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllIncomes.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllIncomes.AddCell(new PdfPCell(new Phrase("Total " + incomeSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllIncomes.AddCell(new PdfPCell(new Phrase(totalAllIncome.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(tableTotalAllIncomes);

                            totalOverAllIncome = totalOverAllIncome + totalAllIncome;
                        }
                        else
                        {
                            subCategoryDescriptionSame = true;
                        }
                    }
                    else
                    {
                        subCategoryDescriptionSame = false;
                    }
                }

                document.Add(line);
                PdfPTable tableTotalOverAllIncome = new PdfPTable(5);
                float[] widthTotalTotalOverAllIncomeCells = new float[] { 50f, 70f, 100f, 100f, 60f };
                tableTotalOverAllIncome.SetWidths(widthTotalTotalOverAllIncomeCells);
                tableTotalOverAllIncome.WidthPercentage = 100;
                tableTotalOverAllIncome.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllIncome.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllIncome.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllIncome.AddCell(new PdfPCell(new Phrase("Total Income", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllIncome.AddCell(new PdfPCell(new Phrase(totalOverAllIncome.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                document.Add(tableTotalOverAllIncome);
            }

            // expenses
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

            document.Add(Chunk.NEWLINE);
            Decimal totalOverAllExpense = 0;
            if (expenses.Any())
            {
                String subCategoryDescription = "";
                Boolean subCategoryDescriptionSame = false;
                foreach (var expensesSubCategoryDescription in expenses)
                {
                    if (subCategoryDescriptionSame == false)
                    {
                        if (subCategoryDescription.Equals(expensesSubCategoryDescription.SubCategoryDescription) == false)
                        {
                            Decimal totalAllExpenses = 0;
                            subCategoryDescription = expensesSubCategoryDescription.SubCategoryDescription;
                            PdfPTable tableSubCategoryDescriptionExpenses = new PdfPTable(1);
                            float[] widthSubCategoryDescriptionExpensesCells = new float[] { 100f };
                            tableSubCategoryDescriptionExpenses.SetWidths(widthSubCategoryDescriptionExpensesCells);
                            tableSubCategoryDescriptionExpenses.WidthPercentage = 100;
                            document.Add(line);
                            tableSubCategoryDescriptionExpenses.AddCell(new PdfPCell(new Phrase(expensesSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            document.Add(tableSubCategoryDescriptionExpenses);

                            String accountType = "";
                            Boolean accountTypeSame = false;
                            foreach (var expensesAccountTypes in expenses)
                            {
                                if (accountTypeSame == false)
                                {
                                    if (accountType.Equals(expensesAccountTypes.AccountType) == false)
                                    {
                                        accountType = expensesAccountTypes.AccountType;
                                        if (expensesAccountTypes.SubCategoryDescription.Equals(subCategoryDescription) == true)
                                        {
                                            Decimal accountTypeTotalBalance = 0;
                                            foreach (var accountTypeBalance in expenses)
                                            {
                                                if (accountTypeBalance.AccountType.Equals(accountType) == true)
                                                {
                                                    accountTypeTotalBalance = accountTypeTotalBalance + accountTypeBalance.Balance;
                                                }
                                            }

                                            PdfPTable tableAccountTypes = new PdfPTable(3);
                                            float[] widthCellsAccountTypes = new float[] { 50f, 100f, 50f };
                                            tableAccountTypes.SetWidths(widthCellsAccountTypes);
                                            tableAccountTypes.WidthPercentage = 100;
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase(expensesAccountTypes.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase(accountTypeTotalBalance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                            document.Add(tableAccountTypes);

                                            String account = "";
                                            Boolean accountSame = false;
                                            foreach (var expensesAccounts in expenses)
                                            {
                                                if (accountSame == false)
                                                {
                                                    if (account.Equals(expensesAccounts.Account) == false)
                                                    {
                                                        account = expensesAccounts.Account;
                                                        if (expensesAccounts.AccountType.Equals(accountType) == true)
                                                        {
                                                            PdfPTable tableAccounts = new PdfPTable(3);
                                                            float[] widthCellsAccounts = new float[] { 50f, 100f, 50f };
                                                            tableAccounts.SetWidths(widthCellsAccounts);
                                                            tableAccounts.WidthPercentage = 100;
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(expensesAccounts.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(expensesAccounts.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(expensesAccounts.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                                            document.Add(tableAccounts);

                                                            totalAllExpenses = totalAllExpenses + expensesAccounts.Balance;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    accountSame = false;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        accountTypeSame = true;
                                    }
                                }
                                else
                                {
                                    accountTypeSame = false;
                                }
                            }

                            document.Add(line);
                            PdfPTable tableTotalAllIncomes = new PdfPTable(5);
                            float[] widthTotalTotalAllIncomesCells = new float[] { 50f, 70f, 100f, 100f, 60f };
                            tableTotalAllIncomes.SetWidths(widthTotalTotalAllIncomesCells);
                            tableTotalAllIncomes.WidthPercentage = 100;
                            tableTotalAllIncomes.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllIncomes.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllIncomes.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllIncomes.AddCell(new PdfPCell(new Phrase("Total " + expensesSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllIncomes.AddCell(new PdfPCell(new Phrase(totalAllExpenses.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(tableTotalAllIncomes);

                            totalOverAllExpense = totalOverAllExpense + totalAllExpenses;
                        }
                        else
                        {
                            subCategoryDescriptionSame = true;
                        }
                    }
                    else
                    {
                        subCategoryDescriptionSame = false;
                    }
                }

                document.Add(line);
                PdfPTable tableTotalOverAllExpense = new PdfPTable(5);
                float[] widthTotalTotalOverAllExpenseCells = new float[] { 50f, 70f, 100f, 100f, 60f };
                tableTotalOverAllExpense.SetWidths(widthTotalTotalOverAllExpenseCells);
                tableTotalOverAllExpense.WidthPercentage = 100;
                tableTotalOverAllExpense.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllExpense.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllExpense.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllExpense.AddCell(new PdfPCell(new Phrase("Total Expense", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllExpense.AddCell(new PdfPCell(new Phrase(totalOverAllExpense.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                document.Add(tableTotalOverAllExpense);
            }

            document.Add(Chunk.NEWLINE);

            Decimal NetIncome = totalOverAllIncome - totalOverAllExpense;
            document.Add(line);
            PdfPTable tableNetIncome = new PdfPTable(5);
            float[] widthTotalNetIncomeCells = new float[] { 50f, 70f, 100f, 100f, 60f };
            tableNetIncome.SetWidths(widthTotalNetIncomeCells);
            tableNetIncome.WidthPercentage = 100;
            tableNetIncome.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableNetIncome.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableNetIncome.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableNetIncome.AddCell(new PdfPCell(new Phrase("Net Income (Loss)", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableNetIncome.AddCell(new PdfPCell(new Phrase(NetIncome.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            document.Add(tableNetIncome);
            
            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}