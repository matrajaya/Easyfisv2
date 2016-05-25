using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepStockOutController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Item List
        [Authorize]
        public ActionResult StockOut(Int32 StockOutId)
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
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);

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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Stock Out", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(branch, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var stockOuts = from d in db.TrnStockOuts
                            where d.Id == StockOutId
                            select new Models.TrnStockOut
                            {
                                Id = d.Id,
                                BranchId = d.BranchId,
                                Branch = d.MstBranch.Branch,
                                OTNumber = d.OTNumber,
                                OTDate = d.OTDate.ToShortDateString(),
                                AccountId = d.AccountId,
                                Account = d.MstAccount.Account,
                                ArticleId = d.ArticleId,
                                Article = d.MstArticle.Article,
                                Particulars = d.Particulars,
                                ManualOTNumber = d.ManualOTNumber,
                                PreparedById = d.PreparedById,
                                PreparedBy = d.MstUser3.FullName,
                                CheckedById = d.CheckedById,
                                CheckedBy = d.MstUser1.FullName,
                                ApprovedById = d.ApprovedById,
                                ApprovedBy = d.MstUser.FullName,
                                IsLocked = d.IsLocked,
                                CreatedById = d.CreatedById,
                                CreatedBy = d.MstUser2.FullName,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedById = d.UpdatedById,
                                UpdatedBy = d.MstUser4.FullName,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

            if (stockOuts.Any())
            {
                String Account = "", Particulars = "";
                String OUTNumber = "", OUTDate = "";
                String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                foreach (var stockOut in stockOuts)
                {
                    Account = stockOut.Account;
                    Particulars = stockOut.Particulars;
                    OUTNumber = stockOut.OTNumber;
                    OUTDate = stockOut.OTDate;
                    PreparedBy = stockOut.PreparedBy;
                    CheckedBy = stockOut.CheckedBy;
                    ApprovedBy = stockOut.ApprovedBy;
                }

                PdfPTable tableSubHeader = new PdfPTable(4);
                float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                tableSubHeader.SetWidths(widthscellsSubheader);
                tableSubHeader.WidthPercentage = 100;
                tableSubHeader.AddCell(new PdfPCell(new Phrase("Account: ", fontArial11Bold)) { Border = 0, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Account, fontArial11)) { Border = 0, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("OUT Number: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(OUTNumber, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                tableSubHeader.AddCell(new PdfPCell(new Phrase("Particulars: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Particulars, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("OUT Date: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(OUTDate, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                document.Add(tableSubHeader);
                document.Add(Chunk.NEWLINE);

                var stockOutItems = from d in db.TrnStockOutItems
                                    where d.OTId == StockOutId
                                    select new Models.TrnStockOutItem
                                    {
                                        Id = d.Id,
                                        OTId = d.OTId,
                                        OT = d.TrnStockOut.OTNumber,
                                        ExpenseAccountId = d.ExpenseAccountId,
                                        ExpenseAccount = d.MstAccount.Account,
                                        ItemId = d.ItemId,
                                        ItemCode = d.MstArticle.ManualArticleCode,
                                        Item = d.MstArticle.Article,
                                        ItemInventoryId = d.ItemInventoryId,
                                        ItemInventory = d.MstArticleInventory.InventoryCode,
                                        Particulars = d.Particulars,
                                        UnitId = d.UnitId,
                                        Unit = d.MstUnit.Unit,
                                        Quantity = d.Quantity,
                                        Cost = d.Cost,
                                        Amount = d.Amount,
                                        BaseUnitId = d.BaseUnitId,
                                        BaseUnit = d.MstUnit1.Unit,
                                        BaseQuantity = d.BaseQuantity,
                                        BaseCost = d.BaseCost
                                    };

                PdfPTable tableOUTLines = new PdfPTable(6);
                float[] widthscellsOUTLines = new float[] { 50f, 60f, 100f, 150f, 100f, 100f };
                tableOUTLines.SetWidths(widthscellsOUTLines);
                tableOUTLines.WidthPercentage = 100;

                tableOUTLines.AddCell(new PdfPCell(new Phrase("Quantity", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableOUTLines.AddCell(new PdfPCell(new Phrase("Unit", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableOUTLines.AddCell(new PdfPCell(new Phrase("Code", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableOUTLines.AddCell(new PdfPCell(new Phrase("Item", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableOUTLines.AddCell(new PdfPCell(new Phrase("Cost", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableOUTLines.AddCell(new PdfPCell(new Phrase("Amount", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal TotalAmount = 0;
                foreach (var stockOutItem in stockOutItems)
                {
                    tableOUTLines.AddCell(new PdfPCell(new Phrase(stockOutItem.Quantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableOUTLines.AddCell(new PdfPCell(new Phrase(stockOutItem.Unit, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableOUTLines.AddCell(new PdfPCell(new Phrase(stockOutItem.ItemCode, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableOUTLines.AddCell(new PdfPCell(new Phrase(stockOutItem.Item, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableOUTLines.AddCell(new PdfPCell(new Phrase(stockOutItem.Cost.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableOUTLines.AddCell(new PdfPCell(new Phrase(stockOutItem.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    TotalAmount = TotalAmount + stockOutItem.Amount;
                }

                document.Add(tableOUTLines);

                document.Add(Chunk.NEWLINE);

                PdfPTable tableOUTLineTotalAmount = new PdfPTable(6);
                float[] widthscellsOUTLinesTotalAmount = new float[] { 50f, 60f, 100f, 150f, 100f, 100f };
                tableOUTLineTotalAmount.SetWidths(widthscellsOUTLinesTotalAmount);
                tableOUTLineTotalAmount.WidthPercentage = 100;

                tableOUTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableOUTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableOUTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableOUTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableOUTLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total:", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableOUTLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                document.Add(tableOUTLineTotalAmount);

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);

                // Table for Footer
                PdfPTable tableFooter = new PdfPTable(5);
                tableFooter.WidthPercentage = 100;
                float[] widthsCells2 = new float[] { 100f, 20f, 100f, 20f, 100f };
                tableFooter.SetWidths(widthsCells2);

                tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });

                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });

                tableFooter.AddCell(new PdfPCell(new Phrase(PreparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(CheckedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(ApprovedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });

                document.Add(tableFooter);
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