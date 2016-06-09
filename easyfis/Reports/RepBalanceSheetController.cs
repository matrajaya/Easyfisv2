using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepBalanceSheetController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // PDF Collection Summary Report
        [Authorize]
        public ActionResult BalanceSheet(String DateAsOf, Int32 CompanyId)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Balance Sheet", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date as of " + DateAsOf, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // Assets
            var assets = from d in db.TrnJournals
                         where d.JournalDate <= Convert.ToDateTime(DateAsOf) &&
                         d.MstAccount.MstAccountType.MstAccountCategory.Id == 1 &&
                         d.MstBranch.CompanyId == CompanyId
                         group d by d.MstAccount into g
                         select new Models.TrnJournal
                         {
                             DocumentReference = "1 - Asset",
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

            Decimal totalOverAllAssets = 0;
            if (assets.Any())
            {
                String subCategoryDescription = "";
                Boolean subCategoryDescriptionSame = false;
                foreach (var assetSubCategoryDescription in assets)
                {
                    if (subCategoryDescriptionSame == false)
                    {
                        if (subCategoryDescription.Equals(assetSubCategoryDescription.SubCategoryDescription) == false)
                        {
                            Decimal totalAllAssets = 0;
                            subCategoryDescription = assetSubCategoryDescription.SubCategoryDescription;
                            PdfPTable tableSubCategoryDescriptionAsset = new PdfPTable(1);
                            float[] widthSubCategoryDescriptionAssetCells = new float[] { 100f };
                            tableSubCategoryDescriptionAsset.SetWidths(widthSubCategoryDescriptionAssetCells);
                            tableSubCategoryDescriptionAsset.WidthPercentage = 100;
                            document.Add(line);
                            tableSubCategoryDescriptionAsset.AddCell(new PdfPCell(new Phrase(assetSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            document.Add(tableSubCategoryDescriptionAsset);

                            String accountType = "";
                            Boolean accountTypeSame = false;
                            foreach (var assetAccountTypes in assets)
                            {
                                if (accountTypeSame == false)
                                {
                                    if (accountType.Equals(assetAccountTypes.AccountType) == false)
                                    {
                                        accountType = assetAccountTypes.AccountType;
                                        if (assetAccountTypes.SubCategoryDescription.Equals(subCategoryDescription) == true)
                                        {
                                            Decimal accountTypeTotalBalance = 0;
                                            foreach (var accountTypeBalance in assets)
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
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase(assetAccountTypes.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase(accountTypeTotalBalance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                            document.Add(tableAccountTypes);

                                            String account = "";
                                            Boolean accountSame = false;
                                            foreach (var assetAccounts in assets)
                                            {
                                                if (accountSame == false)
                                                {
                                                    if (account.Equals(assetAccounts.Account) == false)
                                                    {
                                                        account = assetAccounts.Account;
                                                        if (assetAccounts.AccountType.Equals(accountType) == true)
                                                        {
                                                            PdfPTable tableAccounts = new PdfPTable(3);
                                                            float[] widthCellsAccounts = new float[] { 50f, 100f, 50f };
                                                            tableAccounts.SetWidths(widthCellsAccounts);
                                                            tableAccounts.WidthPercentage = 100;
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(assetAccounts.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(assetAccounts.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(assetAccounts.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                                            document.Add(tableAccounts);

                                                            totalAllAssets = totalAllAssets + assetAccounts.Balance;
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
                                        accountTypeSame = false;
                                    }
                                }
                                else
                                {
                                    accountTypeSame = false;
                                }
                            }

                            document.Add(line);
                            PdfPTable tableTotalAllAssets = new PdfPTable(5);
                            float[] widthTotalTotalAllAssetsCells = new float[] { 50f, 70f, 100f, 100f, 60f };
                            tableTotalAllAssets.SetWidths(widthTotalTotalAllAssetsCells);
                            tableTotalAllAssets.WidthPercentage = 100;
                            tableTotalAllAssets.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllAssets.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllAssets.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllAssets.AddCell(new PdfPCell(new Phrase("Total " + assetSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllAssets.AddCell(new PdfPCell(new Phrase(totalAllAssets.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(tableTotalAllAssets);

                            totalOverAllAssets = totalOverAllAssets + totalAllAssets;
                        }
                        else
                        {
                            subCategoryDescriptionSame = false;
                        }
                    }
                    else
                    {
                        subCategoryDescriptionSame = false;
                    }
                }

                document.Add(line);
                PdfPTable tableTotalOverAllAssets = new PdfPTable(5);
                float[] widthTotalTotalOverAllAssetsCells = new float[] { 50f, 70f, 100f, 100f, 60f };
                tableTotalOverAllAssets.SetWidths(widthTotalTotalOverAllAssetsCells);
                tableTotalOverAllAssets.WidthPercentage = 100;
                tableTotalOverAllAssets.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllAssets.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllAssets.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllAssets.AddCell(new PdfPCell(new Phrase("Total Asset", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllAssets.AddCell(new PdfPCell(new Phrase(totalOverAllAssets.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                document.Add(tableTotalOverAllAssets);
            }

            // Liabilities
            var liabilities = from d in db.TrnJournals
                              where d.JournalDate <= Convert.ToDateTime(DateAsOf) &&
                              d.MstAccount.MstAccountType.MstAccountCategory.Id == 2 &&
                              d.MstBranch.CompanyId == CompanyId
                              group d by d.MstAccount into g
                              select new Models.TrnJournal
                              {
                                  DocumentReference = "2 - Liability",
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

            document.Add(Chunk.NEWLINE);

            Decimal totalOverAllLiabilities = 0;
            if (liabilities.Any())
            {
                String subCategoryDescription = "";
                Boolean subCategoryDescriptionSame = false;
                foreach (var liabilitySubCategoryDescription in liabilities)
                {
                    if (subCategoryDescriptionSame == false)
                    {
                        if (subCategoryDescription.Equals(liabilitySubCategoryDescription.SubCategoryDescription) == false)
                        {
                            Decimal totalAllLiabilities = 0;
                            subCategoryDescription = liabilitySubCategoryDescription.SubCategoryDescription;
                            PdfPTable tableSubCategoryDescriptionLiability = new PdfPTable(1);
                            float[] widthSubCategoryDescriptionLiabilityCells = new float[] { 100f };
                            tableSubCategoryDescriptionLiability.SetWidths(widthSubCategoryDescriptionLiabilityCells);
                            tableSubCategoryDescriptionLiability.WidthPercentage = 100;
                            document.Add(line);
                            tableSubCategoryDescriptionLiability.AddCell(new PdfPCell(new Phrase(liabilitySubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            document.Add(tableSubCategoryDescriptionLiability);

                            String accountType = "";
                            Boolean accountTypeSame = false;
                            foreach (var liabilityAccountTypes in liabilities)
                            {
                                if (accountTypeSame == false)
                                {
                                    if (accountType.Equals(liabilityAccountTypes.AccountType) == false)
                                    {
                                        accountType = liabilityAccountTypes.AccountType;
                                        if (liabilityAccountTypes.SubCategoryDescription.Equals(subCategoryDescription) == true)
                                        {
                                            Decimal accountTypeTotalBalance = 0;
                                            foreach (var accountTypeBalance in liabilities)
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
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase(liabilityAccountTypes.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase(accountTypeTotalBalance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                            document.Add(tableAccountTypes);

                                            String account = "";
                                            Boolean accountSame = false;
                                            foreach (var liabilityAccounts in liabilities)
                                            {
                                                if (accountSame == false)
                                                {
                                                    if (account.Equals(liabilityAccounts.Account) == false)
                                                    {
                                                        account = liabilityAccounts.Account;
                                                        if (liabilityAccounts.AccountType.Equals(accountType) == true)
                                                        {
                                                            PdfPTable tableAccounts = new PdfPTable(3);
                                                            float[] widthCellsAccounts = new float[] { 50f, 100f, 50f };
                                                            tableAccounts.SetWidths(widthCellsAccounts);
                                                            tableAccounts.WidthPercentage = 100;
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(liabilityAccounts.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(liabilityAccounts.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(liabilityAccounts.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                                            document.Add(tableAccounts);

                                                            totalAllLiabilities = totalAllLiabilities + liabilityAccounts.Balance;
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
                                        accountTypeSame = false;
                                    }
                                }
                                else
                                {
                                    accountTypeSame = false;
                                }
                            }

                            document.Add(line);
                            PdfPTable tableTotalAllLiability = new PdfPTable(5);
                            float[] widthTotalTotalAllLiabilityCells = new float[] { 50f, 70f, 100f, 100f, 60f };
                            tableTotalAllLiability.SetWidths(widthTotalTotalAllLiabilityCells);
                            tableTotalAllLiability.WidthPercentage = 100;
                            tableTotalAllLiability.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllLiability.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllLiability.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllLiability.AddCell(new PdfPCell(new Phrase("Total " + liabilitySubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllLiability.AddCell(new PdfPCell(new Phrase(totalAllLiabilities.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(tableTotalAllLiability);

                            totalOverAllLiabilities = totalOverAllLiabilities + totalAllLiabilities;
                        }
                        else
                        {
                            subCategoryDescriptionSame = false;
                        }
                    }
                    else
                    {
                        subCategoryDescriptionSame = false;
                    }
                }

                document.Add(line);
                PdfPTable tableTotalOverAllLiability = new PdfPTable(5);
                float[] widthTotalTotalOverAllLiabilityCells = new float[] { 50f, 70f, 100f, 100f, 60f };
                tableTotalOverAllLiability.SetWidths(widthTotalTotalOverAllLiabilityCells);
                tableTotalOverAllLiability.WidthPercentage = 100;
                tableTotalOverAllLiability.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllLiability.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllLiability.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllLiability.AddCell(new PdfPCell(new Phrase("Total Liability", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllLiability.AddCell(new PdfPCell(new Phrase(totalOverAllLiabilities.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                document.Add(tableTotalOverAllLiability);
            }

            // Equities
            var profitAndLoss = from d in db.TrnJournals
                                where d.JournalDate <= Convert.ToDateTime(DateAsOf)
                                && d.MstAccount.MstAccountType.MstAccountCategory.Id == 5 || d.MstAccount.MstAccountType.MstAccountCategory.Id == 6
                                && d.MstBranch.CompanyId == CompanyId
                                group d by d.MstAccount into g
                                select new Models.TrnJournal
                                {
                                    DocumentReference = "ProfitAndLoss",
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

            var identityUserId = User.Identity.GetUserId();
            var mstUserId = from d in db.MstUsers where d.UserId == identityUserId select d;
            var incomeAccount = from d in db.MstAccounts where d.Id == mstUserId.FirstOrDefault().IncomeAccountId select d;

            var equities = from d in db.TrnJournals
                           where d.JournalDate <= Convert.ToDateTime(DateAsOf)
                           && d.MstAccount.MstAccountType.MstAccountCategory.Id == 4
                           && d.MstBranch.CompanyId == CompanyId
                           group d by d.MstAccount into g
                           select new Models.TrnJournal
                           {
                               DocumentReference = "3 - Equity",
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

            var retainedEarnings = from d in profitAndLoss
                                   group d by d.DocumentReference into g
                                   select new Models.TrnJournal
                                   {
                                       DocumentReference = "3 - Equity",
                                       AccountCategoryCode = incomeAccount.First().MstAccountType.MstAccountCategory.AccountCategoryCode,
                                       AccountCategory = incomeAccount.First().MstAccountType.MstAccountCategory.AccountCategory,
                                       SubCategoryDescription = incomeAccount.First().MstAccountType.SubCategoryDescription,
                                       AccountTypeCode = incomeAccount.First().MstAccountType.AccountTypeCode,
                                       AccountType = incomeAccount.First().MstAccountType.AccountType,
                                       AccountCode = incomeAccount.First().AccountCode,
                                       Account = incomeAccount.First().Account,
                                       DebitAmount = g.Sum(d => d.DebitAmount),
                                       CreditAmount = g.Sum(d => d.CreditAmount),
                                       Balance = g.Sum(d => d.Balance),
                                   };

            var equitiesWithRetainEarnings = equities.Union(retainedEarnings);

            document.Add(Chunk.NEWLINE);

            Decimal totalOverAllEquities = 0;
            if (equitiesWithRetainEarnings.Any())
            {
                String subCategoryDescription = "";
                Boolean subCategoryDescriptionSame = false;
                foreach (var equitySubCategoryDescription in equitiesWithRetainEarnings)
                {
                    if (subCategoryDescriptionSame == false)
                    {
                        if (subCategoryDescription.Equals(equitySubCategoryDescription.SubCategoryDescription) == false)
                        {
                            Decimal totalAllEquities = 0;
                            subCategoryDescription = equitySubCategoryDescription.SubCategoryDescription;
                            PdfPTable tableSubCategoryDescriptionEquity = new PdfPTable(1);
                            float[] widthSubCategoryDescriptionEquityCells = new float[] { 100f };
                            tableSubCategoryDescriptionEquity.SetWidths(widthSubCategoryDescriptionEquityCells);
                            tableSubCategoryDescriptionEquity.WidthPercentage = 100;
                            document.Add(line);
                            tableSubCategoryDescriptionEquity.AddCell(new PdfPCell(new Phrase(equitySubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            document.Add(tableSubCategoryDescriptionEquity);

                            String accountType = "";
                            Boolean accountTypeSame = false;
                            foreach (var equityAccountTypes in equitiesWithRetainEarnings)
                            {
                                if (accountTypeSame == false)
                                {
                                    if (accountType.Equals(equityAccountTypes.AccountType) == false)
                                    {
                                        accountType = equityAccountTypes.AccountType;
                                        if (equityAccountTypes.SubCategoryDescription.Equals(subCategoryDescription) == true)
                                        {
                                            Decimal accountTypeTotalBalance = 0;
                                            foreach (var accountTypeBalance in equitiesWithRetainEarnings)
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
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase(equityAccountTypes.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase(accountTypeTotalBalance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                            document.Add(tableAccountTypes);

                                            String account = "";
                                            Boolean accountSame = false;
                                            foreach (var equityAccounts in equitiesWithRetainEarnings)
                                            {
                                                if (accountSame == false)
                                                {
                                                    if (account.Equals(equityAccounts.Account) == false)
                                                    {
                                                        account = equityAccounts.Account;
                                                        if (equityAccounts.AccountType.Equals(accountType) == true)
                                                        {
                                                            PdfPTable tableAccounts = new PdfPTable(3);
                                                            float[] widthCellsAccounts = new float[] { 50f, 100f, 50f };
                                                            tableAccounts.SetWidths(widthCellsAccounts);
                                                            tableAccounts.WidthPercentage = 100;
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(equityAccounts.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(equityAccounts.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                                            tableAccounts.AddCell(new PdfPCell(new Phrase(equityAccounts.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                                            document.Add(tableAccounts);

                                                            totalAllEquities = totalAllEquities + equityAccounts.Balance;
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
                                        accountTypeSame = false;
                                    }
                                }
                                else
                                {
                                    accountTypeSame = false;
                                }
                            }

                            document.Add(line);
                            PdfPTable tableTotalAllEquity = new PdfPTable(5);
                            float[] widthTotalTotalAllEquityCells = new float[] { 50f, 70f, 100f, 100f, 60f };
                            tableTotalAllEquity.SetWidths(widthTotalTotalAllEquityCells);
                            tableTotalAllEquity.WidthPercentage = 100;
                            tableTotalAllEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllEquity.AddCell(new PdfPCell(new Phrase("Total " + equitySubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableTotalAllEquity.AddCell(new PdfPCell(new Phrase(totalAllEquities.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(tableTotalAllEquity);

                            totalOverAllEquities = totalOverAllEquities + totalAllEquities;
                        }
                        else
                        {
                            subCategoryDescriptionSame = false;
                        }
                    }
                    else
                    {
                        subCategoryDescriptionSame = false;
                    }
                }

                document.Add(line);
                PdfPTable tableTotalOverAllEquity = new PdfPTable(5);
                float[] widthTotalTotalOverAllEquityCells = new float[] { 50f, 70f, 100f, 100f, 60f };
                tableTotalOverAllEquity.SetWidths(widthTotalTotalOverAllEquityCells);
                tableTotalOverAllEquity.WidthPercentage = 100;
                tableTotalOverAllEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllEquity.AddCell(new PdfPCell(new Phrase("Total Equity", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableTotalOverAllEquity.AddCell(new PdfPCell(new Phrase(totalOverAllEquities.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                document.Add(tableTotalOverAllEquity);
            }

            document.Add(Chunk.NEWLINE);

            Decimal totalLiabilityAndEquity = totalOverAllLiabilities + totalOverAllEquities;
            document.Add(line);
            PdfPTable tableTotalLiabilityAndEquity = new PdfPTable(5);
            float[] widthTotalLiabilityAndEquityCells = new float[] { 50f, 70f, 100f, 100f, 60f };
            tableTotalLiabilityAndEquity.SetWidths(widthTotalLiabilityAndEquityCells);
            tableTotalLiabilityAndEquity.WidthPercentage = 100;
            tableTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("Total Liability and Equity", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase(totalLiabilityAndEquity.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            document.Add(tableTotalLiabilityAndEquity);

            Decimal Balance = totalOverAllAssets - totalOverAllLiabilities - totalOverAllEquities;
            document.Add(line);
            PdfPTable tableBalance = new PdfPTable(5);
            float[] widthTotalBalanceCells = new float[] { 50f, 70f, 100f, 100f, 60f };
            tableBalance.SetWidths(widthTotalBalanceCells);
            tableBalance.WidthPercentage = 100;
            tableBalance.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableBalance.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableBalance.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableBalance.AddCell(new PdfPCell(new Phrase("Balance", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableBalance.AddCell(new PdfPCell(new Phrase(Balance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            document.Add(tableBalance);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}