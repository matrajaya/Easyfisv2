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

            // Assets
            PdfPTable tableAsset = new PdfPTable(1);
            float[] widthAssetCells = new float[] { 100f };
            tableAsset.SetWidths(widthAssetCells);
            tableAsset.WidthPercentage = 100;
            tableAsset.AddCell(new PdfPCell(new Phrase("Asset", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 10f });
            document.Add(tableAsset);

            PdfPTable tableHeaderAsset = new PdfPTable(5);
            float[] widthHeaderAssetCells = new float[] { 50f, 70f, 100f, 100f, 60f };
            tableHeaderAsset.SetWidths(widthHeaderAssetCells);
            tableHeaderAsset.WidthPercentage = 100;
            tableHeaderAsset.AddCell(new PdfPCell(new Phrase("Document Ref.", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderAsset.AddCell(new PdfPCell(new Phrase("Sub Category", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderAsset.AddCell(new PdfPCell(new Phrase("Account Type", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderAsset.AddCell(new PdfPCell(new Phrase("Account", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderAsset.AddCell(new PdfPCell(new Phrase("Balance", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

            Decimal totalAssets = 0;
            foreach (var asset in assets)
            {
                tableHeaderAsset.AddCell(new PdfPCell(new Phrase(asset.DocumentReference, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderAsset.AddCell(new PdfPCell(new Phrase(asset.SubCategoryDescription, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderAsset.AddCell(new PdfPCell(new Phrase(asset.AccountTypeCode + "-" + asset.AccountType, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderAsset.AddCell(new PdfPCell(new Phrase(asset.AccountCode + "-" + asset.Account, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderAsset.AddCell(new PdfPCell(new Phrase(asset.Balance.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                totalAssets = totalAssets + asset.Balance;
            }
            document.Add(tableHeaderAsset);

            document.Add(line);
            PdfPTable tableTotalAssets = new PdfPTable(5);
            float[] widthTotalAssetsCells = new float[] { 50f, 70f, 100f, 100f, 60f };
            tableTotalAssets.SetWidths(widthTotalAssetsCells);
            tableTotalAssets.WidthPercentage = 100;
            tableTotalAssets.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalAssets.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalAssets.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalAssets.AddCell(new PdfPCell(new Phrase("Total Assets", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalAssets.AddCell(new PdfPCell(new Phrase(totalAssets.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            document.Add(tableTotalAssets);

            document.Add(Chunk.NEWLINE);

            // Liabilities
            PdfPTable tableLiabilities = new PdfPTable(1);
            float[] widthLiabilitiesCells = new float[] { 100f };
            tableLiabilities.SetWidths(widthLiabilitiesCells);
            tableLiabilities.WidthPercentage = 100;
            tableLiabilities.AddCell(new PdfPCell(new Phrase("Liabilities", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 10f });
            document.Add(tableLiabilities);

            PdfPTable tableHeaderLiabilities = new PdfPTable(5);
            float[] widthHeaderLiabilitiesCells = new float[] { 50f, 70f, 100f, 100f, 60f };
            tableHeaderLiabilities.SetWidths(widthHeaderLiabilitiesCells);
            tableHeaderLiabilities.WidthPercentage = 100;
            tableHeaderLiabilities.AddCell(new PdfPCell(new Phrase("Document Ref.", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderLiabilities.AddCell(new PdfPCell(new Phrase("Sub Category", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderLiabilities.AddCell(new PdfPCell(new Phrase("Account Type", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderLiabilities.AddCell(new PdfPCell(new Phrase("Account", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderLiabilities.AddCell(new PdfPCell(new Phrase("Balance", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

            Decimal totalLiabilities = 0;
            foreach (var liability in liabilities)
            {
                tableHeaderLiabilities.AddCell(new PdfPCell(new Phrase(liability.DocumentReference, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderLiabilities.AddCell(new PdfPCell(new Phrase(liability.SubCategoryDescription, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderLiabilities.AddCell(new PdfPCell(new Phrase(liability.AccountTypeCode + "-" + liability.AccountType, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderLiabilities.AddCell(new PdfPCell(new Phrase(liability.AccountCode + "-" + liability.Account, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderLiabilities.AddCell(new PdfPCell(new Phrase(liability.Balance.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                totalLiabilities = totalLiabilities + liability.Balance;
            }
            document.Add(tableHeaderLiabilities);

            document.Add(line);
            PdfPTable tableTotalLiabilities = new PdfPTable(5);
            float[] widthTotalLiabilitiesCells = new float[] { 50f, 70f, 100f, 100f, 60f };
            tableTotalLiabilities.SetWidths(widthTotalLiabilitiesCells);
            tableTotalLiabilities.WidthPercentage = 100;
            tableTotalLiabilities.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalLiabilities.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalLiabilities.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalLiabilities.AddCell(new PdfPCell(new Phrase("Total Liabilities", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalLiabilities.AddCell(new PdfPCell(new Phrase(totalLiabilities.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            document.Add(tableTotalLiabilities);

            document.Add(Chunk.NEWLINE);

            var equitiesWithRetainEarnings = equities.Union(retainedEarnings);

            // Equities
            PdfPTable tableEquities = new PdfPTable(1);
            float[] widthEquitiesCells = new float[] { 100f };
            tableEquities.SetWidths(widthEquitiesCells);
            tableEquities.WidthPercentage = 100;
            tableEquities.AddCell(new PdfPCell(new Phrase("Equities", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 10f });
            document.Add(tableEquities);

            PdfPTable tableHeaderEquities = new PdfPTable(5);
            float[] widthHeaderEquitiesCells = new float[] { 50f, 70f, 100f, 100f, 60f };
            tableHeaderEquities.SetWidths(widthHeaderEquitiesCells);
            tableHeaderEquities.WidthPercentage = 100;
            tableHeaderEquities.AddCell(new PdfPCell(new Phrase("Document Ref.", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderEquities.AddCell(new PdfPCell(new Phrase("Sub Category", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderEquities.AddCell(new PdfPCell(new Phrase("Account Type", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderEquities.AddCell(new PdfPCell(new Phrase("Account", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderEquities.AddCell(new PdfPCell(new Phrase("Balance", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

            Decimal totalEquities = 0;
            foreach (var equitiesWithRetainEarning in equitiesWithRetainEarnings)
            {
                tableHeaderEquities.AddCell(new PdfPCell(new Phrase(equitiesWithRetainEarning.DocumentReference, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderEquities.AddCell(new PdfPCell(new Phrase(equitiesWithRetainEarning.SubCategoryDescription, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderEquities.AddCell(new PdfPCell(new Phrase(equitiesWithRetainEarning.AccountTypeCode + "-" + equitiesWithRetainEarning.AccountType, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderEquities.AddCell(new PdfPCell(new Phrase(equitiesWithRetainEarning.AccountCode + "-" + equitiesWithRetainEarning.Account, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderEquities.AddCell(new PdfPCell(new Phrase(equitiesWithRetainEarning.Balance.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                totalEquities = totalEquities + equitiesWithRetainEarning.Balance;
            }
            document.Add(tableHeaderEquities);

            document.Add(line);
            PdfPTable tableTotalEquities = new PdfPTable(5);
            float[] widthTotalEquitiesCells = new float[] { 50f, 70f, 100f, 100f, 60f };
            tableTotalEquities.SetWidths(widthTotalEquitiesCells);
            tableTotalEquities.WidthPercentage = 100;
            tableTotalEquities.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalEquities.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalEquities.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalEquities.AddCell(new PdfPCell(new Phrase("Total Equities", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            tableTotalEquities.AddCell(new PdfPCell(new Phrase(totalEquities.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            document.Add(tableTotalEquities);

            document.Add(Chunk.NEWLINE);
            document.Add(line);
            Decimal Balance = totalAssets - totalLiabilities - totalEquities;
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

            if (assets.Any())
            {
                String subCategoryDescription = "";
                Boolean subCategoryDescriptionSame = false;
                Decimal totalOverAllAssets = 0;
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
                                            PdfPTable tableAccountTypes = new PdfPTable(3);
                                            float[] widthCellsAccountTypes = new float[] { 50f, 100f, 50f };
                                            tableAccountTypes.SetWidths(widthCellsAccountTypes);
                                            tableAccountTypes.WidthPercentage = 100;
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase(assetAccountTypes.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                                            tableAccountTypes.AddCell(new PdfPCell(new Phrase(assetAccountTypes.Balance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
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
                                        accountTypeSame = true;
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
                            subCategoryDescriptionSame = true;
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

            //// retrieve account sub category journal for Asset
            //var accountTypeSubCategory_Journal_Assets = from d in db.TrnJournals
            //                                            where d.JournalDate <= Convert.ToDateTime(DateAsOf)
            //                                            && d.MstAccount.MstAccountType.MstAccountCategory.Id == 1
            //                                            && d.MstBranch.CompanyId == CompanyId
            //                                            group d by new
            //                                            {
            //                                                AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
            //                                                SubCategoryDescription = d.MstAccount.MstAccountType.SubCategoryDescription
            //                                            } into g
            //                                            select new Models.TrnJournal
            //                                            {
            //                                                AccountCategory = g.Key.AccountCategory,
            //                                                SubCategoryDescription = g.Key.SubCategoryDescription,
            //                                                DebitAmount = g.Sum(d => d.DebitAmount),
            //                                                CreditAmount = g.Sum(d => d.CreditAmount)
            //                                            };

            //// retrieve account type journal for Asset
            //var accountType_Journal_Assets = from d in db.TrnJournals
            //                                 where d.JournalDate <= Convert.ToDateTime(DateAsOf)
            //                                 && d.MstAccount.MstAccountType.MstAccountCategory.Id == 1
            //                                 && d.MstBranch.CompanyId == CompanyId
            //                                 group d by new
            //                                 {
            //                                     AccountType = d.MstAccount.MstAccountType.AccountType
            //                                 } into g
            //                                 select new Models.TrnJournal
            //                                 {
            //                                     AccountType = g.Key.AccountType,
            //                                     DebitAmount = g.Sum(d => d.DebitAmount),
            //                                     CreditAmount = g.Sum(d => d.CreditAmount)
            //                                 };

            //Decimal totalCurrentAsset = 0;
            //Decimal totalCurrentLiabilities = 0;
            //Decimal totalStockHoldersEquity = 0;
            //Decimal balanceEquityCashflowAmount = 0;

            //if (accountTypeSubCategory_Journal_Assets.Any())
            //{
            //    if (accountType_Journal_Assets.Any())
            //    {
            //        foreach (var accountTypeSubCategory_Journal_Asset in accountTypeSubCategory_Journal_Assets)
            //        {
            //            // table Balance Sheet header
            //            PdfPTable tableBalanceSheetHeader = new PdfPTable(3);
            //            float[] widthCellsTableBalanceSheetHeader = new float[] { 50f, 100f, 50f };
            //            tableBalanceSheetHeader.SetWidths(widthCellsTableBalanceSheetHeader);
            //            tableBalanceSheetHeader.WidthPercentage = 100;

            //            document.Add(line);

            //            PdfPCell headerSubCategoryColspan = (new PdfPCell(new Phrase(accountTypeSubCategory_Journal_Asset.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
            //            headerSubCategoryColspan.Colspan = 3;
            //            tableBalanceSheetHeader.AddCell(headerSubCategoryColspan);

            //            document.Add(tableBalanceSheetHeader);
            //            //totalCurrentAsset = accountTypeSubCategory_Journal_Asset.DebitAmount;
            //        }

            //        foreach (var accountType_JournalAsset in accountType_Journal_Assets)
            //        {
            //            Decimal balanceAssetAmount = accountType_JournalAsset.DebitAmount - accountType_JournalAsset.CreditAmount;

            //            // table Balance Sheet
            //            PdfPTable tableBalanceSheetAccounts = new PdfPTable(3);
            //            float[] widthCellsTableBalanceSheetAccounts = new float[] { 50f, 100f, 50f };
            //            tableBalanceSheetAccounts.SetWidths(widthCellsTableBalanceSheetAccounts);
            //            tableBalanceSheetAccounts.WidthPercentage = 100;
            //            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accountType_JournalAsset.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            //            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
            //            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(balanceAssetAmount.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            //            totalCurrentAsset = totalCurrentAsset + balanceAssetAmount;

            //            // retrieve accounts journal Asset
            //            var accounts_JournalAssets = from d in db.TrnJournals
            //                                         where d.JournalDate <= Convert.ToDateTime(DateAsOf)
            //                                         && d.MstAccount.MstAccountType.AccountType == accountType_JournalAsset.AccountType
            //                                         && d.MstAccount.MstAccountType.MstAccountCategory.Id == 1
            //                                         && d.MstBranch.CompanyId == CompanyId
            //                                         group d by new
            //                                         {
            //                                             AccountCode = d.MstAccount.AccountCode,
            //                                             Account = d.MstAccount.Account
            //                                         } into g
            //                                         select new Models.TrnJournal
            //                                         {
            //                                             AccountCode = g.Key.AccountCode,
            //                                             Account = g.Key.Account,
            //                                             DebitAmount = g.Sum(d => d.DebitAmount),
            //                                             CreditAmount = g.Sum(d => d.CreditAmount)
            //                                         };

            //            foreach (var accounts_JournalAsset in accounts_JournalAssets)
            //            {
            //                Decimal balanceAssetAmountForAccounts = accounts_JournalAsset.DebitAmount - accounts_JournalAsset.CreditAmount;

            //                tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalAsset.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            //                tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalAsset.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
            //                tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(balanceAssetAmountForAccounts.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
            //            }

            //            document.Add(tableBalanceSheetAccounts);
            //        }

            //        document.Add(line);
            //        foreach (var accountTypeSubCategory_Journal_Asset_Footer in accountTypeSubCategory_Journal_Assets)
            //        {
            //            // table Balance Sheet footer in Asset CAtegory
            //            PdfPTable tableBalanceSheetFooterAsset = new PdfPTable(4);
            //            float[] widthCellsTableBalanceSheetFooterAsset = new float[] { 20f, 20f, 150f, 30f };
            //            tableBalanceSheetFooterAsset.SetWidths(widthCellsTableBalanceSheetFooterAsset);
            //            tableBalanceSheetFooterAsset.WidthPercentage = 100;
            //            tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            //            tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            //            tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase("Total " + accountTypeSubCategory_Journal_Asset_Footer.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            //            tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase(totalCurrentAsset.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
            //            tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            //            tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            //            tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase("Total Assets", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            //            tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase(totalCurrentAsset.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

            //            document.Add(tableBalanceSheetFooterAsset);
            //            document.Add(Chunk.NEWLINE);
            //        }
            //    }
            //}

            //// retrieve account sub category journal Liabilities
            //var accountTypeSubCategory_JournalLiabilities = from d in db.TrnJournals
            //                                                where d.JournalDate <= Convert.ToDateTime(DateAsOf)
            //                                                && d.MstAccount.MstAccountType.MstAccountCategory.Id == 2
            //                                                && d.MstBranch.CompanyId == CompanyId
            //                                                group d by new
            //                                                {
            //                                                    AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
            //                                                    SubCategoryDescription = d.MstAccount.MstAccountType.SubCategoryDescription
            //                                                } into g
            //                                                select new Models.TrnJournal
            //                                                {
            //                                                    AccountCategory = g.Key.AccountCategory,
            //                                                    SubCategoryDescription = g.Key.SubCategoryDescription,
            //                                                    DebitAmount = g.Sum(d => d.DebitAmount),
            //                                                    CreditAmount = g.Sum(d => d.CreditAmount)
            //                                                };

            //// retrieve account type journal Liabilities
            //var accountTypeJournal_Liabilities = from d in db.TrnJournals
            //                                     where d.JournalDate <= Convert.ToDateTime(DateAsOf)
            //                                     && d.MstAccount.MstAccountType.MstAccountCategory.Id == 2
            //                                     && d.MstBranch.CompanyId == CompanyId
            //                                     group d by new
            //                                     {
            //                                         AccountType = d.MstAccount.MstAccountType.AccountType
            //                                     } into g
            //                                     select new Models.TrnJournal
            //                                     {
            //                                         AccountType = g.Key.AccountType,
            //                                         DebitAmount = g.Sum(d => d.DebitAmount),
            //                                         CreditAmount = g.Sum(d => d.CreditAmount)
            //                                     };

            //if (accountTypeSubCategory_JournalLiabilities.Any())
            //{
            //    if (accountTypeJournal_Liabilities.Any())
            //    {
            //        foreach (var accountTypeSubCategory_JournalsLiability in accountTypeSubCategory_JournalLiabilities)
            //        {
            //            // table Balance Sheet account Type Sub Category liabilities
            //            PdfPTable tableBalanceSheetHeader = new PdfPTable(3);
            //            float[] widthCellsTableBalanceSheetHeader = new float[] { 50f, 100f, 50f };
            //            tableBalanceSheetHeader.SetWidths(widthCellsTableBalanceSheetHeader);
            //            tableBalanceSheetHeader.WidthPercentage = 100;

            //            document.Add(line);

            //            PdfPCell headerSubCategoryColspan = (new PdfPCell(new Phrase(accountTypeSubCategory_JournalsLiability.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
            //            headerSubCategoryColspan.Colspan = 3;
            //            tableBalanceSheetHeader.AddCell(headerSubCategoryColspan);

            //            document.Add(tableBalanceSheetHeader);
            //            //totalCurrentLiabilities = accountTypeSubCategory_JournalsLiability.CreditAmount;
            //        }

            //        foreach (var accountTypeJournal_Liability in accountTypeJournal_Liabilities)
            //        {
            //            Decimal balanceLiabilityAmount = accountTypeJournal_Liability.CreditAmount - accountTypeJournal_Liability.DebitAmount;

            //            // table Balance Sheet liabilities
            //            PdfPTable tableBalanceSheetAccounts = new PdfPTable(3);
            //            float[] widthCellsTableBalanceSheetAccounts = new float[] { 50f, 100f, 50f };
            //            tableBalanceSheetAccounts.SetWidths(widthCellsTableBalanceSheetAccounts);
            //            tableBalanceSheetAccounts.WidthPercentage = 100;
            //            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accountTypeJournal_Liability.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            //            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
            //            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(balanceLiabilityAmount.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            //            totalCurrentLiabilities = totalCurrentLiabilities + balanceLiabilityAmount;

            //            // retrieve accounts journal Liabilities
            //            var accounts_JournalsLiabilities = from d in db.TrnJournals
            //                                               where d.JournalDate <= Convert.ToDateTime(DateAsOf)
            //                                               && d.MstAccount.MstAccountType.AccountType == accountTypeJournal_Liability.AccountType
            //                                               && d.MstAccount.MstAccountType.MstAccountCategory.Id == 2
            //                                               && d.MstBranch.CompanyId == CompanyId
            //                                               group d by new
            //                                               {
            //                                                   AccountCode = d.MstAccount.AccountCode,
            //                                                   Account = d.MstAccount.Account
            //                                               } into g
            //                                               select new Models.TrnJournal
            //                                               {
            //                                                   AccountCode = g.Key.AccountCode,
            //                                                   Account = g.Key.Account,
            //                                                   DebitAmount = g.Sum(d => d.DebitAmount),
            //                                                   CreditAmount = g.Sum(d => d.CreditAmount)
            //                                               };

            //            foreach (var accounts_JournalsLiability in accounts_JournalsLiabilities)
            //            {
            //                Decimal balanceLiabilityAmountForAccounts = accounts_JournalsLiability.CreditAmount - accounts_JournalsLiability.DebitAmount;

            //                tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalsLiability.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            //                tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalsLiability.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
            //                tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(balanceLiabilityAmountForAccounts.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
            //            }

            //            document.Add(tableBalanceSheetAccounts);
            //        }

            //        document.Add(line);

            //        foreach (var accountTypeSubCategory_JournalsLiability_Footer in accountTypeSubCategory_JournalLiabilities)
            //        {
            //            // table Balance Sheet
            //            PdfPTable tableBalanceSheetFooterLiabilities = new PdfPTable(4);
            //            float[] widthCellsTableBalanceSheetFooterLiablities = new float[] { 20f, 20f, 150f, 30f };
            //            tableBalanceSheetFooterLiabilities.SetWidths(widthCellsTableBalanceSheetFooterLiablities);
            //            tableBalanceSheetFooterLiabilities.WidthPercentage = 100;
            //            tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            //            tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            //            tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase("Total " + accountTypeSubCategory_JournalsLiability_Footer.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            //            tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase(totalCurrentLiabilities.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
            //            tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            //            tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            //            tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase("Total Liabilities", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            //            tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase(totalCurrentLiabilities.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

            //            document.Add(tableBalanceSheetFooterLiabilities);
            //            document.Add(Chunk.NEWLINE);
            //        }
            //    }
            //}

            //var accountSubCategoryDescriptions = from d in db.MstAccountTypes
            //                                     where d.AccountCategoryId == 4
            //                                     group d by new
            //                                     {
            //                                         SubCategoryDescription = d.SubCategoryDescription
            //                                     } into g
            //                                     select new Models.TrnJournal
            //                                     {
            //                                         SubCategoryDescription = g.Key.SubCategoryDescription
            //                                     };


            //foreach (var accountSubCategoryDescription in accountSubCategoryDescriptions)
            //{
            //    // table Balance Sheet Equity
            //    PdfPTable tableBalanceSheetHeader = new PdfPTable(3);
            //    float[] widthCellsTableBalanceSheetHeader = new float[] { 50f, 100f, 50f };
            //    tableBalanceSheetHeader.SetWidths(widthCellsTableBalanceSheetHeader);
            //    tableBalanceSheetHeader.WidthPercentage = 100;

            //    document.Add(line);

            //    PdfPCell headerSubCategoryColspan = (new PdfPCell(new Phrase(accountSubCategoryDescription.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
            //    headerSubCategoryColspan.Colspan = 3;
            //    tableBalanceSheetHeader.AddCell(headerSubCategoryColspan);

            //    document.Add(tableBalanceSheetHeader);
            //    //totalStockHoldersEquity = accountTypeSubCategory_JournalEquity.CreditAmount;
            //}

            //var identityUserId = User.Identity.GetUserId();
            //var mstUserId = from d in db.MstUsers where d.UserId == identityUserId select d;
            //var incomeAccount = from d in db.MstAccounts where d.Id == mstUserId.FirstOrDefault().IncomeAccountId select d;

            //var cashFlowIncome = from d in db.TrnJournals
            //                     where d.MstBranch.CompanyId == CompanyId
            //                     && d.JournalDate <= Convert.ToDateTime(DateAsOf)
            //                     && (d.MstAccount.MstAccountType.AccountCategoryId == 5 || d.MstAccount.MstAccountType.AccountCategoryId == 6)
            //                     select new Models.TrnJournal
            //                     {
            //                         AccountCashFlowCode = d.MstAccount.MstAccountCashFlow.AccountCashFlowCode,
            //                         AccountCashFlow = d.MstAccount.MstAccountCashFlow.AccountCashFlow,
            //                         AccountTypeCode = d.MstAccount.MstAccountType.AccountTypeCode,
            //                         AccountType = d.MstAccount.MstAccountType.AccountType,
            //                         AccountCode = d.MstAccount.AccountCode,
            //                         Account = d.MstAccount.Account,
            //                         SubCategoryDescription = d.MstAccount.MstAccountType.SubCategoryDescription,
            //                         DebitAmount = d.DebitAmount,
            //                         CreditAmount = d.CreditAmount
            //                     };

            //var cashFlowBalanceSheet = from d in db.TrnJournals
            //                           where d.MstBranch.CompanyId == CompanyId
            //                           && d.JournalDate <= Convert.ToDateTime(DateAsOf)
            //                           && d.MstAccount.MstAccountType.AccountCategoryId < 5
            //                           && (d.MstAccount.AccountCashFlowId == null ? 4 : d.MstAccount.AccountCashFlowId) <= 3
            //                            && d.MstAccount.MstAccountType.Id == incomeAccount.FirstOrDefault().Id
            //                           select new Models.TrnJournal
            //                           {
            //                               AccountCashFlowCode = d.MstAccount.MstAccountCashFlow.AccountCashFlowCode,
            //                               AccountCashFlow = d.MstAccount.MstAccountCashFlow.AccountCashFlow,
            //                               AccountTypeCode = d.MstAccount.MstAccountType.AccountTypeCode,
            //                               AccountType = d.MstAccount.MstAccountType.AccountType,
            //                               AccountCode = d.MstAccount.AccountCode,
            //                               Account = d.MstAccount.Account,
            //                               SubCategoryDescription = d.MstAccount.MstAccountType.SubCategoryDescription,
            //                               DebitAmount = d.DebitAmount,
            //                               CreditAmount = d.CreditAmount
            //                           };

            //var accountCashFlowGroupFromCashFlowIncome = from d in cashFlowIncome
            //                                             group d by new
            //                                             {
            //                                                 AccountCashFlowCode = d.AccountCashFlowCode,
            //                                                 AccountCashFlow = d.AccountCashFlow,
            //                                                 AccountTypeCode = incomeAccount.FirstOrDefault().MstAccountType.AccountTypeCode,
            //                                                 AccountType = incomeAccount.FirstOrDefault().MstAccountType.AccountType,
            //                                                 AccountCode = "0000",
            //                                                 Account = incomeAccount.FirstOrDefault().Account,
            //                                             } into g
            //                                             select new Models.TrnJournal
            //                                             {
            //                                                 AccountCashFlowCode = g.Key.AccountCashFlowCode,
            //                                                 AccountCashFlow = g.Key.AccountCashFlow,
            //                                                 AccountTypeCode = g.Key.AccountTypeCode,
            //                                                 AccountType = g.Key.AccountType,
            //                                                 AccountCode = g.Key.AccountCode,
            //                                                 Account = g.Key.Account,
            //                                                 DebitAmount = g.Sum(d => d.DebitAmount),
            //                                                 CreditAmount = g.Sum(d => d.CreditAmount),
            //                                                 Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
            //                                             };

            //var accountCashFlowGroupFromCashFlowBalanceSheet = from d in cashFlowBalanceSheet
            //                                                   group d by new
            //                                                   {
            //                                                       AccountCashFlowCode = d.AccountCashFlowCode,
            //                                                       AccountCashFlow = d.AccountCashFlow,
            //                                                       AccountTypeCode = d.AccountTypeCode,
            //                                                       AccountType = d.AccountType,
            //                                                       AccountCode = d.AccountCode,
            //                                                       Account = d.Account
            //                                                   } into g
            //                                                   select new Models.TrnJournal
            //                                                   {
            //                                                       AccountCashFlowCode = g.Key.AccountCashFlowCode,
            //                                                       AccountCashFlow = g.Key.AccountCashFlow,
            //                                                       AccountTypeCode = g.Key.AccountTypeCode,
            //                                                       AccountType = g.Key.AccountType,
            //                                                       AccountCode = g.Key.AccountCode,
            //                                                       Account = g.Key.Account,
            //                                                       DebitAmount = g.Sum(d => d.DebitAmount),
            //                                                       CreditAmount = g.Sum(d => d.CreditAmount),
            //                                                       Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
            //                                                   };

            //var unionAccountCashFlowGroups = accountCashFlowGroupFromCashFlowIncome.Union(accountCashFlowGroupFromCashFlowBalanceSheet).OrderBy(d => d.AccountCashFlowCode);


            //foreach (var accountCashFlowGroupFromCashFlowIncomes in accountCashFlowGroupFromCashFlowIncome)
            //{
            //    balanceEquityCashflowAmount = accountCashFlowGroupFromCashFlowIncomes.Balance;

            //    // table Balance Sheet Equity
            //    PdfPTable tableBalanceSheetEquityAccounts = new PdfPTable(3);
            //    float[] widthCellsTableBalanceSheetEquityAccounts = new float[] { 50f, 100f, 50f };
            //    tableBalanceSheetEquityAccounts.SetWidths(widthCellsTableBalanceSheetEquityAccounts);
            //    tableBalanceSheetEquityAccounts.WidthPercentage = 100;
            //    tableBalanceSheetEquityAccounts.AddCell(new PdfPCell(new Phrase(accountCashFlowGroupFromCashFlowIncomes.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            //    tableBalanceSheetEquityAccounts.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
            //    tableBalanceSheetEquityAccounts.AddCell(new PdfPCell(new Phrase(accountCashFlowGroupFromCashFlowIncomes.Balance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            //    foreach (var unionAccountCashFlowGroup in unionAccountCashFlowGroups)
            //    {
            //       // Decimal balanceEquityAmountForAccounts = accounts_JournalEquity.CreditAmount - accounts_JournalEquity.DebitAmount;

            //        tableBalanceSheetEquityAccounts.AddCell(new PdfPCell(new Phrase(unionAccountCashFlowGroup.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            //        tableBalanceSheetEquityAccounts.AddCell(new PdfPCell(new Phrase(unionAccountCashFlowGroup.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
            //        tableBalanceSheetEquityAccounts.AddCell(new PdfPCell(new Phrase(unionAccountCashFlowGroup.Balance.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
            //    }

            //    document.Add(tableBalanceSheetEquityAccounts);
            //}


            //// retrieve account sub category journal Equity
            //var accountTypeSubCategory_JournalEquities = from d in db.TrnJournals
            //                                             where d.JournalDate <= Convert.ToDateTime(DateAsOf)
            //                                             && d.MstAccount.MstAccountType.MstAccountCategory.Id == 4
            //                                             && d.MstBranch.CompanyId == CompanyId
            //                                             group d by new
            //                                             {
            //                                                 AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
            //                                                 SubCategoryDescription = d.MstAccount.MstAccountType.SubCategoryDescription
            //                                             } into g
            //                                             select new Models.TrnJournal
            //                                             {
            //                                                 AccountCategory = g.Key.AccountCategory,
            //                                                 SubCategoryDescription = g.Key.SubCategoryDescription,
            //                                                 DebitAmount = g.Sum(d => d.DebitAmount),
            //                                                 CreditAmount = g.Sum(d => d.CreditAmount)
            //                                             };

            //// retrieve account type journal Equity
            //var accountTypeJournal_Equities = from d in db.TrnJournals
            //                                  where d.JournalDate <= Convert.ToDateTime(DateAsOf)
            //                                  && d.MstAccount.MstAccountType.MstAccountCategory.Id == 4
            //                                  && d.MstBranch.CompanyId == CompanyId
            //                                  group d by new
            //                                  {
            //                                      AccountType = d.MstAccount.MstAccountType.AccountType
            //                                  } into g
            //                                  select new Models.TrnJournal
            //                                  {
            //                                      AccountType = g.Key.AccountType,
            //                                      DebitAmount = g.Sum(d => d.DebitAmount),
            //                                      CreditAmount = g.Sum(d => d.CreditAmount)
            //                                  };

            //if (accountTypeSubCategory_JournalEquities.Any())
            //{
            //    if (accountTypeJournal_Equities.Any())
            //    {
            //        //foreach (var accountTypeSubCategory_JournalEquity in accountTypeSubCategory_JournalEquities)
            //        //{
            //        //    // table Balance Sheet Equity
            //        //    PdfPTable tableBalanceSheetHeader = new PdfPTable(3);
            //        //    float[] widthCellsTableBalanceSheetHeader = new float[] { 50f, 100f, 50f };
            //        //    tableBalanceSheetHeader.SetWidths(widthCellsTableBalanceSheetHeader);
            //        //    tableBalanceSheetHeader.WidthPercentage = 100;

            //        //    document.Add(line);

            //        //    PdfPCell headerSubCategoryColspan = (new PdfPCell(new Phrase(accountTypeSubCategory_JournalEquity.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
            //        //    headerSubCategoryColspan.Colspan = 3;
            //        //    tableBalanceSheetHeader.AddCell(headerSubCategoryColspan);

            //        //    document.Add(tableBalanceSheetHeader);
            //        //    //totalStockHoldersEquity = accountTypeSubCategory_JournalEquity.CreditAmount;
            //        //}

            //        foreach (var accountTypeJournal_Equity in accountTypeJournal_Equities)
            //        {
            //            Decimal balanceEquityAmount = accountTypeJournal_Equity.CreditAmount - accountTypeJournal_Equity.DebitAmount;

            //            // table Balance Sheet Equity
            //            PdfPTable tableBalanceSheetAccounts = new PdfPTable(3);
            //            float[] widthCellsTableBalanceSheetAccounts = new float[] { 50f, 100f, 50f };
            //            tableBalanceSheetAccounts.SetWidths(widthCellsTableBalanceSheetAccounts);
            //            tableBalanceSheetAccounts.WidthPercentage = 100;
            //            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accountTypeJournal_Equity.AccountType, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            //            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
            //            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(balanceEquityAmount.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            //            totalStockHoldersEquity = totalStockHoldersEquity + balanceEquityAmount;

            //            // retrieve accounts journal Equity
            //            var accounts_JournalEquities = from d in db.TrnJournals
            //                                           where d.JournalDate <= Convert.ToDateTime(DateAsOf)
            //                                           && d.MstAccount.MstAccountType.AccountType == accountTypeJournal_Equity.AccountType
            //                                           && d.MstAccount.MstAccountType.MstAccountCategory.Id == 4
            //                                           && d.MstBranch.CompanyId == CompanyId
            //                                           group d by new
            //                                           {
            //                                               AccountCode = d.MstAccount.AccountCode,
            //                                               Account = d.MstAccount.Account
            //                                           } into g
            //                                           select new Models.TrnJournal
            //                                           {
            //                                               AccountCode = g.Key.AccountCode,
            //                                               Account = g.Key.Account,
            //                                               DebitAmount = g.Sum(d => d.DebitAmount),
            //                                               CreditAmount = g.Sum(d => d.CreditAmount)
            //                                           };

            //            foreach (var accounts_JournalEquity in accounts_JournalEquities)
            //            {
            //                Decimal balanceEquityAmountForAccounts = accounts_JournalEquity.CreditAmount - accounts_JournalEquity.DebitAmount;

            //                tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalEquity.AccountCode, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            //                tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalEquity.Account, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
            //                tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(balanceEquityAmountForAccounts.ToString("#,##0.00"), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
            //            }

            //            document.Add(tableBalanceSheetAccounts);
            //        }

            //        document.Add(line);

            //        foreach (var accountTypeSubCategory_JournalEquity_Footer in accountTypeSubCategory_JournalEquities)
            //        {
            //            // table Balance Sheet
            //            PdfPTable tableBalanceSheetFooterEquity = new PdfPTable(4);
            //            float[] widthCellsTableBalanceSheetFooterEquity = new float[] { 20f, 20f, 150f, 30f };
            //            tableBalanceSheetFooterEquity.SetWidths(widthCellsTableBalanceSheetFooterEquity);
            //            tableBalanceSheetFooterEquity.WidthPercentage = 100;
            //            tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            //            tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            //            tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase("Total " + accountTypeSubCategory_JournalEquity_Footer.SubCategoryDescription, fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            //            tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase(totalStockHoldersEquity.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
            //            tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            //            tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            //            tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase("Total Equities", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            //            tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase(totalStockHoldersEquity.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
            //            document.Add(tableBalanceSheetFooterEquity);
            //            document.Add(Chunk.NEWLINE);
            //        }
            //    }
            //}

            //document.Add(line);

            //Decimal totalLiabilityAndEquity = totalCurrentLiabilities + totalStockHoldersEquity + balanceEquityCashflowAmount;
            //Decimal totalBalance = totalCurrentAsset - totalCurrentLiabilities - totalStockHoldersEquity - balanceEquityCashflowAmount;

            //// table Balance Sheet
            //PdfPTable tableBalanceSheetFooterTotalLiabilityAndEquity = new PdfPTable(4);
            //float[] widthCellsTableBalanceSheetFooterTotalLiabilityAndEquity = new float[] { 20f, 20f, 150f, 30f };
            //tableBalanceSheetFooterTotalLiabilityAndEquity.SetWidths(widthCellsTableBalanceSheetFooterTotalLiabilityAndEquity);
            //tableBalanceSheetFooterTotalLiabilityAndEquity.WidthPercentage = 100;
            //tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            //tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            //tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("Total Equities", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            //tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase(totalLiabilityAndEquity.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
            //tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            //tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            //tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("Balance", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            //tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase(totalBalance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
            //document.Add(tableBalanceSheetFooterTotalLiabilityAndEquity);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}