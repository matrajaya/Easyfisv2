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
        public ActionResult ChartOfAccounts()
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Chart of Accounts", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("", fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var accountCategories = from d in db.MstAccountCategories
                                    group d by new
                                    {
                                        AccountCategoryCode = d.AccountCategoryCode,
                                        AccountCategory = d.AccountCategory
                                    } into g
                                    select new Models.MstAccountCategory
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

                    var accountSubCategories = from d in db.MstAccountTypes
                                               where d.MstAccountCategory.AccountCategory == accountCategory.AccountCategory
                                               group d by new
                                               {
                                                   SubCategoryDescription = d.SubCategoryDescription
                                               } into g
                                               select new Models.MstAccountType
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

                            var accountTypes = from d in db.MstAccountTypes
                                               where d.SubCategoryDescription == accountSubCategory.SubCategoryDescription
                                               group d by new
                                               {
                                                   AccountType = d.AccountType
                                               } into g
                                               select new Models.MstAccountType
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

                                    var accounts = from d in db.MstAccounts
                                                   where d.MstAccountType.AccountType == accountType.AccountType
                                                   select new Models.MstAccount
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

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}