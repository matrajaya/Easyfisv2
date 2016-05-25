using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepStockTransferController : Controller
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
        public ActionResult StockTransfer(Int32 StockTransferId)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Stock Transfer", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(branch, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var stockTransfers = from d in db.TrnStockTransfers
                                 where d.Id == StockTransferId
                                 select new Models.TrnStockTransfer
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     STNumber = d.STNumber,
                                     STDate = d.STDate.ToShortDateString(),
                                     ToBranchId = d.ToBranchId,
                                     ToBranch = d.MstBranch1.Branch,
                                     Particulars = d.Particulars,
                                     ManualSTNumber = d.ManualSTNumber,
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

            if (stockTransfers.Any())
            {
                String FromBranch = "", ToBranch = "", Particulars = "";
                String STNumber = "", STDate = "";
                String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                foreach (var stockTransfer in stockTransfers)
                {
                    FromBranch = stockTransfer.Branch;
                    ToBranch = stockTransfer.ToBranch;
                    Particulars = stockTransfer.Particulars;
                    STNumber = stockTransfer.STNumber;
                    STDate = stockTransfer.STDate;
                    PreparedBy = stockTransfer.PreparedBy;
                    CheckedBy = stockTransfer.CheckedBy;
                    ApprovedBy = stockTransfer.ApprovedBy;
                }

                PdfPTable tableSubHeader = new PdfPTable(4);
                float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                tableSubHeader.SetWidths(widthscellsSubheader);
                tableSubHeader.WidthPercentage = 100;
                tableSubHeader.AddCell(new PdfPCell(new Phrase("From Branch: ", fontArial11Bold)) { Border = 0, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(FromBranch, fontArial11)) { Border = 0, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("ST Number: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(STNumber, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                tableSubHeader.AddCell(new PdfPCell(new Phrase("To Branch: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(ToBranch, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("ST Date: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(STDate, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                tableSubHeader.AddCell(new PdfPCell(new Phrase("Particulars: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Particulars, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("", fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                document.Add(tableSubHeader);
                document.Add(Chunk.NEWLINE);

                var stockTransferItems = from d in db.TrnStockTransferItems
                                         where d.STId == StockTransferId
                                         select new Models.TrnStockTransferItem
                                         {
                                             Id = d.Id,
                                             STId = d.STId,
                                             ST = d.TrnStockTransfer.STNumber,
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

                PdfPTable tableSTLines = new PdfPTable(6);
                float[] widthscellsSTLines = new float[] { 50f, 60f, 100f, 150f, 100f, 100f };
                tableSTLines.SetWidths(widthscellsSTLines);
                tableSTLines.WidthPercentage = 100;

                tableSTLines.AddCell(new PdfPCell(new Phrase("Quantity", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSTLines.AddCell(new PdfPCell(new Phrase("Unit", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSTLines.AddCell(new PdfPCell(new Phrase("Code", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSTLines.AddCell(new PdfPCell(new Phrase("Item", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSTLines.AddCell(new PdfPCell(new Phrase("Cost", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSTLines.AddCell(new PdfPCell(new Phrase("Amount", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal TotalAmount = 0;
                foreach (var stockTransferItem in stockTransferItems)
                {
                    tableSTLines.AddCell(new PdfPCell(new Phrase(stockTransferItem.Quantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSTLines.AddCell(new PdfPCell(new Phrase(stockTransferItem.Unit, fontArial9)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSTLines.AddCell(new PdfPCell(new Phrase(stockTransferItem.ItemCode, fontArial9)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSTLines.AddCell(new PdfPCell(new Phrase(stockTransferItem.Item, fontArial9)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSTLines.AddCell(new PdfPCell(new Phrase(stockTransferItem.Cost.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSTLines.AddCell(new PdfPCell(new Phrase(stockTransferItem.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    TotalAmount = TotalAmount + stockTransferItem.Amount;
                }

                document.Add(tableSTLines);
                document.Add(Chunk.NEWLINE);

                PdfPTable tableSTLineTotalAmount = new PdfPTable(6);
                float[] widthscellsSTLinesTotalAmount = new float[] { 50f, 60f, 100f, 150f, 100f, 100f };
                tableSTLineTotalAmount.SetWidths(widthscellsSTLinesTotalAmount);
                tableSTLineTotalAmount.WidthPercentage = 100;

                tableSTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableSTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableSTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableSTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableSTLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total:", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableSTLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                document.Add(tableSTLineTotalAmount);

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);

                // Table for Footer
                PdfPTable tableFooter = new PdfPTable(7);
                tableFooter.WidthPercentage = 100;
                float[] widthsCells2 = new float[] { 100f, 20f, 100f, 20f, 100f, 20f, 100f };
                tableFooter.SetWidths(widthsCells2);

                tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase("Received by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });

                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
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
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });

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