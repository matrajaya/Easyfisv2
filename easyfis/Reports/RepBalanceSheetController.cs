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
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===================
        // Balance Sheet - PDF
        // ===================
        [Authorize]
        public ActionResult BalanceSheet(String DateAsOf, Int32 CompanyId)
        {
            // ============
            // PDF settings
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

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

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
            header.AddCell(new PdfPCell(new Phrase("Balance Sheet", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            header.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            header.AddCell(new PdfPCell(new Phrase("Date as of " + DateAsOf, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            header.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            header.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(header);
            document.Add(line);

            Decimal totalOverallAssets = 0;
            Decimal totalOverallLiabilities = 0;
            Decimal totalOverallEquities = 0;

            // ==========
            // Get Assets
            // ==========
            var assets = from d in db.TrnJournals
                         where d.JournalDate <= Convert.ToDateTime(DateAsOf) &&
                         d.MstAccount.MstAccountType.MstAccountCategory.Id == 1 &&
                         d.MstBranch.CompanyId == CompanyId
                         group d by d.MstAccount into g
                         select new
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

            if (assets.Any())
            {
                // ==============================
                // Asset Sub Category Description
                // ==============================
                var assetSubCategoryDescriptions = from d in assets
                                                   group d by new
                                                   {
                                                       SubCategoryDescription = d.SubCategoryDescription
                                                   } into g
                                                   select new
                                                   {
                                                       SubCategoryDescription = g.Key.SubCategoryDescription,
                                                       Balance = g.Sum(d => d.DebitAmount - d.CreditAmount)
                                                   };

                if (assetSubCategoryDescriptions.Any())
                {
                    Decimal totalAllAssets = 0;
                    foreach (var assetSubCategoryDescription in assetSubCategoryDescriptions)
                    {
                        document.Add(line);
                        PdfPTable assetSubCategoryDescriptionTable = new PdfPTable(1);
                        float[] widthCellsAssetSubCategoryDescriptionTable = new float[] { 100f };
                        assetSubCategoryDescriptionTable.SetWidths(widthCellsAssetSubCategoryDescriptionTable);
                        assetSubCategoryDescriptionTable.WidthPercentage = 100;
                        assetSubCategoryDescriptionTable.AddCell(new PdfPCell(new Phrase(assetSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        document.Add(assetSubCategoryDescriptionTable);

                        // ===================
                        // Asset Account Types
                        // ===================
                        var assetAccountTypes = from d in assets
                                                where d.SubCategoryDescription.Equals(assetSubCategoryDescription.SubCategoryDescription)
                                                group d by new
                                                {
                                                    AccountType = d.AccountType
                                                } into g
                                                select new
                                                {
                                                    AccountType = g.Key.AccountType,
                                                    Balance = g.Sum(d => d.DebitAmount - d.CreditAmount)
                                                };

                        if (assetAccountTypes.Any())
                        {
                            Decimal totalCurrentAssets = 0;
                            foreach (var assetAccountType in assetAccountTypes)
                            {
                                totalCurrentAssets += assetAccountType.Balance;

                                PdfPTable assetAccountTypeTable = new PdfPTable(3);
                                float[] widthCellsAssetAccountTypeTable = new float[] { 50f, 100f, 50f };
                                assetAccountTypeTable.SetWidths(widthCellsAssetAccountTypeTable);
                                assetAccountTypeTable.WidthPercentage = 100;
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase(assetAccountType.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                assetAccountTypeTable.AddCell(new PdfPCell(new Phrase(assetAccountType.Balance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                document.Add(assetAccountTypeTable);

                                // ==============
                                // Asset Accounts
                                // ==============
                                var assetAccounts = from d in assets
                                                    where d.AccountType.Equals(assetAccountType.AccountType)
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

                                if (assetAccounts.Any())
                                {
                                    foreach (var assetAccount in assetAccounts)
                                    {
                                        totalAllAssets += assetAccount.Balance;

                                        PdfPTable assetAccountTable = new PdfPTable(3);
                                        float[] widthCellsAssetAccountTable = new float[] { 50f, 100f, 50f };
                                        assetAccountTable.SetWidths(widthCellsAssetAccountTable);
                                        assetAccountTable.WidthPercentage = 100;
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(assetAccount.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(assetAccount.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        assetAccountTable.AddCell(new PdfPCell(new Phrase(assetAccount.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                        document.Add(assetAccountTable);
                                    }
                                }
                            }

                            // ====================
                            // Total Current Assets
                            // ====================
                            document.Add(line);
                            PdfPTable totalCurrentAssetsTable = new PdfPTable(5);
                            float[] widthCellsTotalCurrentAssetsTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                            totalCurrentAssetsTable.SetWidths(widthCellsTotalCurrentAssetsTable);
                            totalCurrentAssetsTable.WidthPercentage = 100;
                            totalCurrentAssetsTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentAssetsTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentAssetsTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentAssetsTable.AddCell(new PdfPCell(new Phrase("Total " + assetSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentAssetsTable.AddCell(new PdfPCell(new Phrase(totalCurrentAssets.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(totalCurrentAssetsTable);
                        }
                    }

                    // ================
                    // Total All Assets
                    // ================
                    document.Add(line);
                    PdfPTable totalAllAssetsTable = new PdfPTable(5);
                    float[] widthCellsTotalAllAssetsTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                    totalAllAssetsTable.SetWidths(widthCellsTotalAllAssetsTable);
                    totalAllAssetsTable.WidthPercentage = 100;
                    totalAllAssetsTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllAssetsTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllAssetsTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllAssetsTable.AddCell(new PdfPCell(new Phrase("Total Asset", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllAssetsTable.AddCell(new PdfPCell(new Phrase(totalAllAssets.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    document.Add(totalAllAssetsTable);

                    totalOverallAssets += totalAllAssets;
                    document.Add(Chunk.NEWLINE);
                }
            }

            // ===============
            // Get Liabilities
            // ===============
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

            if (liabilities.Any())
            {
                // ==================================
                // Liability Sub Category Description
                // ==================================
                var liabilitySubCategoryDescriptions = from d in liabilities
                                                       group d by new
                                                       {
                                                           SubCategoryDescription = d.SubCategoryDescription
                                                       } into g
                                                       select new
                                                       {
                                                           SubCategoryDescription = g.Key.SubCategoryDescription,
                                                           Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                       };

                if (liabilitySubCategoryDescriptions.Any())
                {
                    Decimal totalAllLiabilities = 0;
                    foreach (var liabilitySubCategoryDescription in liabilitySubCategoryDescriptions)
                    {
                        document.Add(line);
                        PdfPTable liabilitySubCategoryDescriptionTable = new PdfPTable(1);
                        float[] widthCellsLiabilitySubCategoryDescriptionTable = new float[] { 100f };
                        liabilitySubCategoryDescriptionTable.SetWidths(widthCellsLiabilitySubCategoryDescriptionTable);
                        liabilitySubCategoryDescriptionTable.WidthPercentage = 100;
                        liabilitySubCategoryDescriptionTable.AddCell(new PdfPCell(new Phrase(liabilitySubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        document.Add(liabilitySubCategoryDescriptionTable);

                        // =======================
                        // Liability Account Types
                        // =======================
                        var liabilityAccountTypes = from d in liabilities
                                                    where d.SubCategoryDescription.Equals(liabilitySubCategoryDescription.SubCategoryDescription)
                                                    group d by new
                                                    {
                                                        AccountType = d.AccountType
                                                    } into g
                                                    select new
                                                    {
                                                        AccountType = g.Key.AccountType,
                                                        Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                    };

                        if (liabilityAccountTypes.Any())
                        {
                            Decimal totalCurrentLiabilities = 0;
                            foreach (var liabilityAccountType in liabilityAccountTypes)
                            {
                                totalCurrentLiabilities += liabilityAccountType.Balance;

                                PdfPTable liabilityAccountTypeTable = new PdfPTable(3);
                                float[] widthCellsLiabilityAccountTypeTable = new float[] { 50f, 100f, 50f };
                                liabilityAccountTypeTable.SetWidths(widthCellsLiabilityAccountTypeTable);
                                liabilityAccountTypeTable.WidthPercentage = 100;
                                liabilityAccountTypeTable.AddCell(new PdfPCell(new Phrase(liabilityAccountType.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                liabilityAccountTypeTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                liabilityAccountTypeTable.AddCell(new PdfPCell(new Phrase(liabilityAccountType.Balance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                document.Add(liabilityAccountTypeTable);

                                // ==================
                                // Liability Accounts
                                // ==================
                                var liabilityAccounts = from d in liabilities
                                                        where d.AccountType.Equals(liabilityAccountType.AccountType)
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

                                if (liabilityAccounts.Any())
                                {
                                    foreach (var liabilityAccount in liabilityAccounts)
                                    {
                                        totalAllLiabilities += liabilityAccount.Balance;

                                        PdfPTable liabilityAccountTable = new PdfPTable(3);
                                        float[] widthCellsLiabilityAccountTable = new float[] { 50f, 100f, 50f };
                                        liabilityAccountTable.SetWidths(widthCellsLiabilityAccountTable);
                                        liabilityAccountTable.WidthPercentage = 100;
                                        liabilityAccountTable.AddCell(new PdfPCell(new Phrase(liabilityAccount.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                        liabilityAccountTable.AddCell(new PdfPCell(new Phrase(liabilityAccount.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        liabilityAccountTable.AddCell(new PdfPCell(new Phrase(liabilityAccount.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                        document.Add(liabilityAccountTable);
                                    }
                                }
                            }

                            // =========================
                            // Total Current Liabilities
                            // =========================
                            document.Add(line);
                            PdfPTable totalCurrentLiabilitiesTable = new PdfPTable(5);
                            float[] widthCellsTotalCurrentLiabilitiesTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                            totalCurrentLiabilitiesTable.SetWidths(widthCellsTotalCurrentLiabilitiesTable);
                            totalCurrentLiabilitiesTable.WidthPercentage = 100;
                            totalCurrentLiabilitiesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentLiabilitiesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentLiabilitiesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentLiabilitiesTable.AddCell(new PdfPCell(new Phrase("Total " + liabilitySubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentLiabilitiesTable.AddCell(new PdfPCell(new Phrase(totalCurrentLiabilities.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(totalCurrentLiabilitiesTable);
                        }
                    }

                    // =====================
                    // Total All Liabilities
                    // =====================
                    document.Add(line);
                    PdfPTable totalAllLiabilitiesTable = new PdfPTable(5);
                    float[] widthCellsTotalAllLiabilitiesTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                    totalAllLiabilitiesTable.SetWidths(widthCellsTotalAllLiabilitiesTable);
                    totalAllLiabilitiesTable.WidthPercentage = 100;
                    totalAllLiabilitiesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllLiabilitiesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllLiabilitiesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllLiabilitiesTable.AddCell(new PdfPCell(new Phrase("Total Liability", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllLiabilitiesTable.AddCell(new PdfPCell(new Phrase(totalAllLiabilities.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    document.Add(totalAllLiabilitiesTable);

                    totalOverallLiabilities += totalAllLiabilities;
                    document.Add(Chunk.NEWLINE);
                }
            }

            // ===================
            // Get Profit and Loss
            // ===================
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

            // ============
            // Get Equities
            // ============
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

            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
            var incomeAccount = from d in db.MstAccounts where d.Id == currentUser.FirstOrDefault().IncomeAccountId select d;

            var retainedEarnings = from d in profitAndLoss
                                   group d by d.DocumentReference into g
                                   select new Models.TrnJournal
                                   {
                                       DocumentReference = "3 - Equity",
                                       AccountCategoryCode = incomeAccount.FirstOrDefault().MstAccountType.MstAccountCategory.AccountCategoryCode,
                                       AccountCategory = incomeAccount.FirstOrDefault().MstAccountType.MstAccountCategory.AccountCategory,
                                       SubCategoryDescription = incomeAccount.FirstOrDefault().MstAccountType.SubCategoryDescription,
                                       AccountTypeCode = incomeAccount.FirstOrDefault().MstAccountType.AccountTypeCode,
                                       AccountType = incomeAccount.FirstOrDefault().MstAccountType.AccountType,
                                       AccountCode = incomeAccount.FirstOrDefault().AccountCode,
                                       Account = incomeAccount.FirstOrDefault().Account,
                                       DebitAmount = g.Sum(d => d.DebitAmount),
                                       CreditAmount = g.Sum(d => d.CreditAmount),
                                       Balance = g.Sum(d => d.Balance),
                                   };

            var unionEquitiesWithRetainEarnings = equities.Union(retainedEarnings);

            if (unionEquitiesWithRetainEarnings.Any())
            {
                // ================================
                // Equity Sub Category Descriptions
                // ================================
                var equitySubCategoryDescriptions = from d in unionEquitiesWithRetainEarnings
                                                    group d by new
                                                    {
                                                        SubCategoryDescription = d.SubCategoryDescription
                                                    } into g
                                                    select new
                                                    {
                                                        SubCategoryDescription = g.Key.SubCategoryDescription,
                                                        Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                    };

                if (equitySubCategoryDescriptions.Any())
                {
                    Decimal totalAllEquities = 0;
                    foreach (var equitySubCategoryDescription in equitySubCategoryDescriptions)
                    {
                        document.Add(line);
                        PdfPTable equitySubCategoryDescriptionTable = new PdfPTable(1);
                        float[] widthCellsEquitySubCategoryDescriptionTable = new float[] { 100f };
                        equitySubCategoryDescriptionTable.SetWidths(widthCellsEquitySubCategoryDescriptionTable);
                        equitySubCategoryDescriptionTable.WidthPercentage = 100;
                        equitySubCategoryDescriptionTable.AddCell(new PdfPCell(new Phrase(equitySubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        document.Add(equitySubCategoryDescriptionTable);

                        // ====================
                        // Equity Account Types
                        // ====================
                        var equityAccountTypes = from d in unionEquitiesWithRetainEarnings
                                                 where d.SubCategoryDescription.Equals(equitySubCategoryDescription.SubCategoryDescription)
                                                 group d by new
                                                 {
                                                     AccountType = d.AccountType
                                                 } into g
                                                 select new
                                                 {
                                                     AccountType = g.Key.AccountType,
                                                     Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                 };

                        if (equityAccountTypes.Any())
                        {
                            Decimal totalCurrentEquities = 0;
                            foreach (var equityAccountType in equityAccountTypes)
                            {
                                totalCurrentEquities += equityAccountType.Balance;

                                PdfPTable equityAccountTypeTable = new PdfPTable(3);
                                float[] widthCellsEquityAccountTypeTable = new float[] { 50f, 100f, 50f };
                                equityAccountTypeTable.SetWidths(widthCellsEquityAccountTypeTable);
                                equityAccountTypeTable.WidthPercentage = 100;
                                equityAccountTypeTable.AddCell(new PdfPCell(new Phrase(equityAccountType.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                equityAccountTypeTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                equityAccountTypeTable.AddCell(new PdfPCell(new Phrase(equityAccountType.Balance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                                document.Add(equityAccountTypeTable);

                                // ===============
                                // Equity Accounts
                                // ===============
                                var equityAccounts = from d in unionEquitiesWithRetainEarnings
                                                     where d.AccountType.Equals(equityAccountType.AccountType)
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

                                if (equityAccounts.Any())
                                {
                                    foreach (var equityAccount in equityAccounts)
                                    {
                                        totalAllEquities += equityAccount.Balance;

                                        PdfPTable equityAccountTable = new PdfPTable(3);
                                        float[] widthCellsEquityAccountTable = new float[] { 50f, 100f, 50f };
                                        equityAccountTable.SetWidths(widthCellsEquityAccountTable);
                                        equityAccountTable.WidthPercentage = 100;
                                        equityAccountTable.AddCell(new PdfPCell(new Phrase(equityAccount.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                        equityAccountTable.AddCell(new PdfPCell(new Phrase(equityAccount.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                        equityAccountTable.AddCell(new PdfPCell(new Phrase(equityAccount.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                                        document.Add(equityAccountTable);
                                    }
                                }
                            }

                            // ======================
                            // Total Current Equities
                            // ======================
                            document.Add(line);
                            PdfPTable totalCurrentEquitiesTable = new PdfPTable(5);
                            float[] widthCellsTotalCurrentEquitiesTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                            totalCurrentEquitiesTable.SetWidths(widthCellsTotalCurrentEquitiesTable);
                            totalCurrentEquitiesTable.WidthPercentage = 100;
                            totalCurrentEquitiesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentEquitiesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentEquitiesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentEquitiesTable.AddCell(new PdfPCell(new Phrase("Total " + equitySubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            totalCurrentEquitiesTable.AddCell(new PdfPCell(new Phrase(totalCurrentEquities.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            document.Add(totalCurrentEquitiesTable);
                        }
                    }

                    // ==================
                    // Total All Equities
                    // ==================
                    document.Add(line);
                    PdfPTable totalAllEquitiesTable = new PdfPTable(5);
                    float[] widthCellsTotalAllEquitiesTable = new float[] { 50f, 70f, 100f, 100f, 60f };
                    totalAllEquitiesTable.SetWidths(widthCellsTotalAllEquitiesTable);
                    totalAllEquitiesTable.WidthPercentage = 100;
                    totalAllEquitiesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllEquitiesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllEquitiesTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllEquitiesTable.AddCell(new PdfPCell(new Phrase("Total Equity", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    totalAllEquitiesTable.AddCell(new PdfPCell(new Phrase(totalAllEquities.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    document.Add(totalAllEquitiesTable);

                    totalOverallEquities += totalAllEquities;
                    document.Add(Chunk.NEWLINE);
                }
            }

            document.Add(Chunk.NEWLINE);

            // ==============================
            // Total Liabilities and Equities
            // ==============================
            Decimal totalLiabilityAndEquity = totalOverallLiabilities + totalOverallEquities;

            document.Add(line);
            PdfPTable tableTotalLiabilityAndEquityTable = new PdfPTable(5);
            float[] widthCellsTableTotalLiabilityAndEquityTable = new float[] { 50f, 70f, 100f, 100f, 60f };
            tableTotalLiabilityAndEquityTable.SetWidths(widthCellsTableTotalLiabilityAndEquityTable);
            tableTotalLiabilityAndEquityTable.WidthPercentage = 100;
            tableTotalLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase("Total Liability and Equity", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalLiabilityAndEquityTable.AddCell(new PdfPCell(new Phrase(totalLiabilityAndEquity.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            document.Add(tableTotalLiabilityAndEquityTable);

            // =======
            // Balance
            // =======
            Decimal Balance = totalOverallAssets - totalLiabilityAndEquity;

            document.Add(line);
            PdfPTable balanceTable = new PdfPTable(5);
            float[] widthCellsBalanceTable = new float[] { 50f, 70f, 100f, 100f, 60f };
            balanceTable.SetWidths(widthCellsBalanceTable);
            balanceTable.WidthPercentage = 100;
            balanceTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            balanceTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            balanceTable.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            balanceTable.AddCell(new PdfPCell(new Phrase("Balance", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            balanceTable.AddCell(new PdfPCell(new Phrase(Balance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            document.Add(balanceTable);

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}