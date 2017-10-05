using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepStockOutController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===============
        // Stock Out - PDF
        // ===============
        [Authorize]
        public ActionResult StockOut(Int32 StockOutId)
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
            Font fontArial13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 4.5F)));

            var identityUserId = User.Identity.GetUserId();
            var currentUser = from d in db.MstUsers where d.UserId == identityUserId select d;
            var currentCompanyId = currentUser.FirstOrDefault().CompanyId;
            var currentBranchId = currentUser.FirstOrDefault().BranchId;

            // ==============
            // Company Detail
            // ==============
            var companyName = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.Company).FirstOrDefault();
            var address = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.Address).FirstOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.ContactNumber).FirstOrDefault();
            var branch = (from d in db.MstBranches where d.Id == Convert.ToInt32(currentBranchId) select d.Branch).FirstOrDefault();

            // ===========
            // Header Page
            // ===========
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase("Stock Out", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase(branch, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);

            // =====
            // Space
            // =====
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { Border = 0, PaddingTop = 5f });

            document.Add(line);

            var stockOuts = from d in db.TrnStockOuts
                            where d.Id == StockOutId
                            && d.IsLocked == true
                            select d;

            if (stockOuts.Any())
            {
                String account = stockOuts.FirstOrDefault().MstAccount.Account;
                String particulars = stockOuts.FirstOrDefault().Particulars;
                String OUTNumber = stockOuts.FirstOrDefault().OTNumber;
                String OUTDate = stockOuts.FirstOrDefault().OTDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture); ;
                String preparedBy = stockOuts.FirstOrDefault().MstUser3.FullName;
                String checkedBy = stockOuts.FirstOrDefault().MstUser1.FullName;
                String approvedBy = stockOuts.FirstOrDefault().MstUser.FullName;

                PdfPTable tableStockOuts = new PdfPTable(4);
                float[] widthscellsTablePurchaseOrder = new float[] { 40f, 150f, 70f, 50f };
                tableStockOuts.SetWidths(widthscellsTablePurchaseOrder);
                tableStockOuts.WidthPercentage = 100;

                tableStockOuts.AddCell(new PdfPCell(new Phrase("Account", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableStockOuts.AddCell(new PdfPCell(new Phrase(account, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableStockOuts.AddCell(new PdfPCell(new Phrase("OUT Number", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableStockOuts.AddCell(new PdfPCell(new Phrase(OUTNumber, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableStockOuts.AddCell(new PdfPCell(new Phrase("Particulars", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableStockOuts.AddCell(new PdfPCell(new Phrase(particulars, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableStockOuts.AddCell(new PdfPCell(new Phrase("OUT Date", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableStockOuts.AddCell(new PdfPCell(new Phrase(OUTDate, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                document.Add(tableStockOuts);

                document.Add(spaceTable);

                // ===================
                // Get Stock Out Items
                // ===================
                var stockOutItems = from d in db.TrnStockOutItems
                                    where d.OTId == StockOutId
                                    && d.TrnStockOut.IsLocked == true
                                    select new
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

                if (stockOutItems.Any())
                {
                    PdfPTable tableStockOutItems = new PdfPTable(6);
                    float[] widthscellsPOLines = new float[] { 100f, 70f, 150f, 200f, 100f, 100f };
                    tableStockOutItems.SetWidths(widthscellsPOLines);
                    tableStockOutItems.WidthPercentage = 100;
                    tableStockOutItems.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableStockOutItems.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableStockOutItems.AddCell(new PdfPCell(new Phrase("Code", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableStockOutItems.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableStockOutItems.AddCell(new PdfPCell(new Phrase("Cost", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableStockOutItems.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });

                    Decimal totalAmount = 0;

                    foreach (var stockOutItem in stockOutItems)
                    {
                        tableStockOutItems.AddCell(new PdfPCell(new Phrase(stockOutItem.Quantity.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableStockOutItems.AddCell(new PdfPCell(new Phrase(stockOutItem.Unit, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableStockOutItems.AddCell(new PdfPCell(new Phrase(stockOutItem.ItemCode, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableStockOutItems.AddCell(new PdfPCell(new Phrase(stockOutItem.Item, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableStockOutItems.AddCell(new PdfPCell(new Phrase(stockOutItem.Cost.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableStockOutItems.AddCell(new PdfPCell(new Phrase(stockOutItem.Amount.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });

                        totalAmount += stockOutItem.Amount;
                    }

                    tableStockOutItems.AddCell(new PdfPCell(new Phrase("Total", fontArial11Bold)) { Colspan = 5, HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableStockOutItems.AddCell(new PdfPCell(new Phrase(totalAmount.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    document.Add(tableStockOutItems);

                    document.Add(spaceTable);
                }

                // ==============
                // User Signature
                // ==============
                PdfPTable tableUsers = new PdfPTable(3);
                float[] widthsCellsTableUsers = new float[] { 100f, 100f, 100f };
                tableUsers.WidthPercentage = 100;
                tableUsers.SetWidths(widthsCellsTableUsers);
                tableUsers.AddCell(new PdfPCell(new Phrase("Prepared by", fontArial11Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase("Checked by", fontArial11Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase("Approved by", fontArial11Bold)) { PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 50f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 50f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 50f });
                tableUsers.AddCell(new PdfPCell(new Phrase(preparedBy, fontArial11)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(checkedBy, fontArial11)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(approvedBy, fontArial11)) { HorizontalAlignment = 1, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(tableUsers);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}