using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepStockCountController : Controller
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
        public ActionResult StockCount(Int32 Id)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Stock Count", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(branch, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var stockCounts = from d in db.TrnStockCounts
                              where d.Id == Id
                              select new Models.TrnStockCount
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  SCNumber = d.SCNumber,
                                  SCDate = d.SCDate.ToShortDateString(),
                                  Particulars = d.Particulars,
                                  PreparedBy = d.MstUser3.FullName,
                                  PreparedById = d.PreparedById,
                                  CheckedBy = d.MstUser1.FullName,
                                  CheckedById = d.CheckedById,
                                  ApprovedBy = d.MstUser.FullName,
                                  ApprovedById = d.ApprovedById,
                                  IsLocked = d.IsLocked,
                                  CreatedById = d.CreatedById,
                                  CreatedBy = d.MstUser2.FullName,
                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                  UpdatedById = d.UpdatedById,
                                  UpdatedBy = d.MstUser4.FullName,
                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                              };

            if (stockCounts.Any())
            {
                String Particulars = "";
                String SCNumber = "", SCDate = "";
                String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                foreach (var stockCount in stockCounts)
                {
                    Particulars = stockCount.Particulars;
                    SCNumber = stockCount.SCNumber;
                    SCDate = stockCount.SCDate;
                    PreparedBy = stockCount.PreparedBy;
                    CheckedBy = stockCount.CheckedBy;
                    ApprovedBy = stockCount.ApprovedBy;
                }

                PdfPTable tableSubHeader = new PdfPTable(4);
                float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                tableSubHeader.SetWidths(widthscellsSubheader);
                tableSubHeader.WidthPercentage = 100;
                tableSubHeader.AddCell(new PdfPCell(new Phrase("Particulars: ", fontArial11Bold)) { Border = 0, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Particulars, fontArial11)) { Border = 0, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("SC Number: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(SCNumber, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("", fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("SC Date: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(SCDate, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                document.Add(tableSubHeader);

                document.Add(Chunk.NEWLINE);

                var stockCountItems = from d in db.TrnStockCountItems
                                      where d.SCId == Id
                                      select new Models.TrnStockCountItem
                                      {
                                          Id = d.Id,
                                          SCId = d.SCId,
                                          ItemId = d.ItemId,
                                          Item = d.MstArticle.Article,
                                          ItemCode = d.MstArticle.ManualArticleCode,
                                          Particulars = d.Particulars,
                                          Quantity = d.Quantity,
                                          Unit = d.MstArticle.MstUnit.Unit
                                      };

                PdfPTable tableSCLines = new PdfPTable(4);
                float[] widthscellsSCLines = new float[] { 50f, 60f, 100f, 350f };
                tableSCLines.SetWidths(widthscellsSCLines);
                tableSCLines.WidthPercentage = 100;
                tableSCLines.AddCell(new PdfPCell(new Phrase("Quantity", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSCLines.AddCell(new PdfPCell(new Phrase("Unit", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSCLines.AddCell(new PdfPCell(new Phrase("Code", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSCLines.AddCell(new PdfPCell(new Phrase("Item", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                foreach (var stockCountItem in stockCountItems)
                {
                    tableSCLines.AddCell(new PdfPCell(new Phrase(stockCountItem.Quantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSCLines.AddCell(new PdfPCell(new Phrase(stockCountItem.Unit, fontArial9)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSCLines.AddCell(new PdfPCell(new Phrase(stockCountItem.ItemCode, fontArial9)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSCLines.AddCell(new PdfPCell(new Phrase(stockCountItem.Item, fontArial9)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                }

                document.Add(tableSCLines);

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

                tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0 });

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