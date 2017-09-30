using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepItemListController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ======================
        // Item List Report - PDF
        // ======================
        [Authorize]
        public ActionResult ItemList(Int32 ItemGroupId)
        {
            // ==============================
            // PDF Settings and Customization
            // ==============================
            MemoryStream workStream = new MemoryStream();
            Rectangle rectangle = new Rectangle(PageSize.A3);
            Document document = new Document(rectangle, 72, 72, 72, 72);
            document.SetMargins(30f, 30f, 30f, 30f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();

            // =====
            // Fonts
            // =====
            Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font fontArial11 = FontFactory.GetFont("Arial", 11);
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 4.5F)));

            var identityUserId = User.Identity.GetUserId();
            var currentUser = from d in db.MstUsers where d.UserId == identityUserId select d;
            var currentCompanyId = currentUser.FirstOrDefault().CompanyId;

            // ==============
            // Company Detail
            // ==============
            var companyName = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.Company).FirstOrDefault();
            var address = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.Address).FirstOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.ContactNumber).FirstOrDefault();

            // ===========
            // Header Page
            // ===========
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase("Item List", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);
            document.Add(line);

            // =========
            // Get Items
            // =========
            var items = from d in db.MstArticles
                        where d.ArticleGroupId == ItemGroupId
                        && d.ArticleTypeId == 1
                        select new
                        {
                            Id = d.Id,
                            ArticleCode = d.ArticleCode,
                            ManualArticleCode = d.ManualArticleCode,
                            Article = d.Article,
                            Category = d.Category,
                            ArticleTypeId = d.ArticleTypeId,
                            ArticleType = d.MstArticleType.ArticleType,
                            ArticleGroupId = d.ArticleGroupId,
                            ArticleGroup = d.MstArticleGroup.ArticleGroup,
                            AccountId = d.AccountId,
                            AccountCode = d.MstAccount.AccountCode,
                            Account = d.MstAccount.Account,
                            SalesAccountId = d.SalesAccountId,
                            SalesAccount = d.MstAccount1.Account,
                            CostAccountId = d.CostAccountId,
                            CostAccount = d.MstAccount2.Account,
                            AssetAccountId = d.AssetAccountId,
                            AssetAccount = d.MstAccount3.Account,
                            ExpenseAccountId = d.ExpenseAccountId,
                            ExpenseAccount = d.MstAccount4.Account,
                            UnitId = d.UnitId,
                            Unit = d.MstUnit.Unit,
                            InputTaxId = d.InputTaxId,
                            InputTax = d.MstTaxType.TaxType,
                            OutputTaxId = d.OutputTaxId,
                            OutputTax = d.MstTaxType1.TaxType,
                            WTaxTypeId = d.WTaxTypeId,
                            WTaxType = d.MstTaxType2.TaxType,
                            Price = d.Price,
                            Cost = d.Cost,
                            IsInventory = d.IsInventory,
                            Particulars = d.Particulars,
                            Address = d.Address,
                            TermId = d.TermId,
                            Term = d.MstTerm.Term,
                            ContactNumber = d.ContactNumber,
                            ContactPerson = d.ContactPerson,
                            TaxNumber = d.TaxNumber,
                            CreditLimit = d.CreditLimit,
                            DateAcquired = d.DateAcquired.ToShortDateString(),
                            UsefulLife = d.UsefulLife,
                            SalvageValue = d.SalvageValue,
                            ManualArticleOldCode = d.ManualArticleOldCode
                        };

            if (items.Any())
            {
                var articleGroups = from d in db.MstArticleGroups
                                    where d.Id == Convert.ToInt32(ItemGroupId)
                                    select d;

                if (articleGroups.Any())
                {
                    // ===================
                    // Article Group Title
                    // ===================
                    PdfPTable articleGroupTitle = new PdfPTable(1);
                    float[] widthCellsArticleGroupTitle = new float[] { 100f };
                    articleGroupTitle.SetWidths(widthCellsArticleGroupTitle);
                    articleGroupTitle.WidthPercentage = 100;
                    PdfPCell branchHeaderColspan = (new PdfPCell(new Phrase(articleGroups.FirstOrDefault().ArticleGroup, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 14f });
                    articleGroupTitle.AddCell(branchHeaderColspan);
                    document.Add(articleGroupTitle);

                    PdfPTable tableItems = new PdfPTable(6);
                    float[] widthscellsTableTtems = new float[] { 15f, 20f, 35f, 25f, 20f, 20f };
                    tableItems.SetWidths(widthscellsTableTtems);
                    tableItems.WidthPercentage = 100;
                    tableItems.AddCell(new PdfPCell(new Phrase("Code", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableItems.AddCell(new PdfPCell(new Phrase("Manual Code", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableItems.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableItems.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableItems.AddCell(new PdfPCell(new Phrase("Cost", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableItems.AddCell(new PdfPCell(new Phrase("Price", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    foreach (var item in items)
                    {
                        tableItems.AddCell(new PdfPCell(new Phrase(item.ArticleCode, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableItems.AddCell(new PdfPCell(new Phrase(item.ManualArticleCode, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableItems.AddCell(new PdfPCell(new Phrase(item.Article, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableItems.AddCell(new PdfPCell(new Phrase(item.Unit, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableItems.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(item.Cost).ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableItems.AddCell(new PdfPCell(new Phrase(item.Price.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    }

                    document.Add(tableItems);
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