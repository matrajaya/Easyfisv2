using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepChartOfAccountsController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==============================
        // Chart of Accounts Report - PDF
        // ==============================
        [Authorize]
        public ActionResult ChartOfAccounts()
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

            var identityUserId = User.Identity.GetUserId();
            var currentUser = from d in db.MstUsers where d.UserId == identityUserId select d;
            var currentBranchId = currentUser.FirstOrDefault().BranchId;

            // ==============
            // Company Detail
            // ==============
            var companyName = (from d in db.MstBranches where d.Id == currentBranchId select d.MstCompany.Company).SingleOrDefault();
            var address = (from d in db.MstBranches where d.Id == currentBranchId select d.MstCompany.Address).SingleOrDefault();
            var contactNo = (from d in db.MstBranches where d.Id == currentBranchId select d.MstCompany.ContactNumber).SingleOrDefault();
            var branch = (from d in db.MstBranches where d.Id == currentBranchId select d.Branch).SingleOrDefault();

            // ===========
            // Header Page
            // ===========
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase("Chart of Accounts", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase(branch, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);
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

            // ==================
            // Account Categories
            // ==================
            var accountCategories = from d in db.MstAccountCategories
                                    group d by new
                                    {
                                        AccountCategoryCode = d.AccountCategoryCode,
                                        AccountCategory = d.AccountCategory
                                    } into g
                                    select new
                                    {
                                        AccountCategoryCode = g.Key.AccountCategoryCode,
                                        AccountCategory = g.Key.AccountCategory
                                    };

            if (accountCategories.Any())
            {
                foreach (var accountCategory in accountCategories)
                {
                    PdfPTable tableAccountCategory = new PdfPTable(1);
                    float[] widthscellsTableAccountCategory = new float[] { 100f };
                    tableAccountCategory.SetWidths(widthscellsTableAccountCategory);
                    tableAccountCategory.WidthPercentage = 100;
                    tableAccountCategory.AddCell(new PdfPCell(new Phrase(accountCategory.AccountCategory, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, Border = 0, BackgroundColor = BaseColor.LIGHT_GRAY });
                    document.Add(tableAccountCategory);

                    // ======================
                    // Account Sub Categories
                    // ======================
                    var accountSubCategories = from d in db.MstAccountTypes
                                               where d.MstAccountCategory.AccountCategory.Equals(accountCategory.AccountCategory)
                                               group d by new
                                               {
                                                   SubCategoryDescription = d.SubCategoryDescription
                                               } into g
                                               select new
                                               {
                                                   SubCategoryDescription = g.Key.SubCategoryDescription
                                               };

                    if (accountSubCategories.Any())
                    {
                        foreach (var accountSubCategory in accountSubCategories)
                        {
                            PdfPTable tableSubAccountCategory = new PdfPTable(1);
                            float[] widthscellsTableSubAccountCategory = new float[] { 100f };
                            tableSubAccountCategory.SetWidths(widthscellsTableSubAccountCategory);
                            tableSubAccountCategory.WidthPercentage = 100;
                            tableSubAccountCategory.AddCell(new PdfPCell(new Phrase(accountSubCategory.SubCategoryDescription, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f, Border = 0 });
                            document.Add(tableSubAccountCategory);

                            // =============
                            // Account Types
                            // =============
                            var accountTypes = from d in db.MstAccountTypes
                                               where d.SubCategoryDescription.Equals(accountSubCategory.SubCategoryDescription)
                                               group d by new
                                               {
                                                   AccountType = d.AccountType
                                               } into g
                                               select new
                                               {
                                                   AccountType = g.Key.AccountType
                                               };

                            if (accountTypes.Any())
                            {
                                foreach (var accountType in accountTypes)
                                {
                                    PdfPTable tableAccountType = new PdfPTable(1);
                                    float[] widthscellsTableAccountType = new float[] { 100f };
                                    tableAccountType.SetWidths(widthscellsTableAccountType);
                                    tableAccountType.WidthPercentage = 100;
                                    tableAccountType.AddCell(new PdfPCell(new Phrase(accountType.AccountType, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 40f, Border = 0 });
                                    document.Add(tableAccountType);

                                    // ========
                                    // Accounts
                                    // ========
                                    var accounts = from d in db.MstAccounts
                                                   where d.MstAccountType.AccountType.Equals(accountType.AccountType)
                                                   select new
                                                   {
                                                       AccountCode = d.AccountCode,
                                                       Account = d.Account
                                                   };

                                    if (accounts.Any())
                                    {
                                        foreach (var account in accounts)
                                        {
                                            PdfPTable tableAccount = new PdfPTable(2);
                                            float[] widthscellsTableAccount = new float[] { 20f, 100f };
                                            tableAccount.SetWidths(widthscellsTableAccount);
                                            tableAccount.WidthPercentage = 100;
                                            tableAccount.AddCell(new PdfPCell(new Phrase(account.AccountCode, fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 60f, Border = 0 });
                                            tableAccount.AddCell(new PdfPCell(new Phrase(account.Account, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 10f, Border = 0 });
                                            document.Add(tableAccount);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}