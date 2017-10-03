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
    public class RepSalesController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===================
        // Sales Invoice - PDF
        // ===================
        [Authorize]
        public ActionResult Sales(Int32 SalesId)
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
            var defaultSalesInvoiceName = currentUser.FirstOrDefault().SalesInvoiceName;

            // ==============
            // Company Detail
            // ==============
            var companyName = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.Company).FirstOrDefault();
            var address = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.Address).FirstOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.ContactNumber).FirstOrDefault();
            var branch = (from d in db.MstBranches where d.Id == Convert.ToInt32(currentBranchId) select d.ContactNumber).FirstOrDefault();

            // ===========
            // Header Page
            // ===========
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase(defaultSalesInvoiceName, fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase(branch, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);
            document.Add(line);

            // =============
            // Sales Invoice
            // =============
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.Id == Convert.ToInt32(SalesId)
                                && d.IsLocked == true
                                select d;

            if (salesInvoices.Any())
            {
                String customer = salesInvoices.FirstOrDefault().MstArticle.Article;
                String term = salesInvoices.FirstOrDefault().MstTerm.Term;
                String remarks = salesInvoices.FirstOrDefault().Remarks;
                String sales = salesInvoices.FirstOrDefault().MstUser4.FullName;
                String SINumber = salesInvoices.FirstOrDefault().SINumber;
                String SIDate = salesInvoices.FirstOrDefault().SIDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                String documentReference = salesInvoices.FirstOrDefault().DocumentReference;
                String preparedBy = salesInvoices.FirstOrDefault().MstUser3.FullName;
                String checkedBy = salesInvoices.FirstOrDefault().MstUser1.FullName;
                String approvedBy = salesInvoices.FirstOrDefault().MstUser.FullName;

                PdfPTable tableSalesInvoice = new PdfPTable(4);
                float[] widthscellsTablePurchaseOrder = new float[] { 40f, 130f, 70f, 40f };
                tableSalesInvoice.SetWidths(widthscellsTablePurchaseOrder);
                tableSalesInvoice.WidthPercentage = 100;

                tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Customer: ", fontArial11Bold)) { Border = 0, PaddingTop = 10f });
                tableSalesInvoice.AddCell(new PdfPCell(new Phrase(customer, fontArial11)) { Border = 0, PaddingTop = 10f });
                tableSalesInvoice.AddCell(new PdfPCell(new Phrase("SI Number: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableSalesInvoice.AddCell(new PdfPCell(new Phrase(SINumber, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Term: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSalesInvoice.AddCell(new PdfPCell(new Phrase(term, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSalesInvoice.AddCell(new PdfPCell(new Phrase("SI Date: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSalesInvoice.AddCell(new PdfPCell(new Phrase(SIDate, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Sales: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSalesInvoice.AddCell(new PdfPCell(new Phrase(sales, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSalesInvoice.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Colspan = 2, Border = 0, PaddingTop = 3f });

                tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Document Ref: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSalesInvoice.AddCell(new PdfPCell(new Phrase(documentReference, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSalesInvoice.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Colspan = 2, Border = 0, PaddingTop = 3f });

                tableSalesInvoice.AddCell(new PdfPCell(new Phrase("Remarks: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSalesInvoice.AddCell(new PdfPCell(new Phrase(remarks, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSalesInvoice.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Colspan = 2, Border = 0, PaddingTop = 3f });

                document.Add(tableSalesInvoice);

                document.Add(Chunk.NEWLINE);

                // ======================
                // Get Sales Invoice Item
                // ======================
                var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                        where d.SIId == SalesId
                                        && d.TrnSalesInvoice.IsLocked == true
                                        select new
                                        {
                                            Id = d.Id,
                                            SIId = d.SIId,
                                            SI = d.TrnSalesInvoice.SINumber,
                                            ItemId = d.ItemId,
                                            ItemCode = d.MstArticle.ManualArticleCode,
                                            Item = d.MstArticle.Article,
                                            ItemInventoryId = d.ItemInventoryId,
                                            ItemInventory = d.MstArticleInventory.InventoryCode,
                                            Particulars = d.Particulars,
                                            UnitId = d.UnitId,
                                            Unit = d.MstUnit.Unit,
                                            Quantity = d.Quantity,
                                            Price = d.Price,
                                            DiscountId = d.DiscountId,
                                            Discount = d.MstDiscount.Discount,
                                            DiscountRate = d.DiscountRate,
                                            DiscountAmount = d.DiscountAmount,
                                            NetPrice = d.NetPrice,
                                            Amount = d.Amount,
                                            VATId = d.VATId,
                                            VAT = d.MstTaxType.TaxType,
                                            VATPercentage = d.VATPercentage,
                                            VATAmount = d.VATAmount,
                                            BaseUnitId = d.BaseUnitId,
                                            BaseUnit = d.MstUnit1.Unit,
                                            BaseQuantity = d.BaseQuantity,
                                            BasePrice = d.BasePrice
                                        };

                if (salesInvoiceItems.Any())
                {
                    // ==========
                    // Item Label
                    // ==========
                    PdfPTable tableItemLabel = new PdfPTable(1);
                    float[] widthCellsTableItemLabel = new float[] { 100f };
                    tableItemLabel.SetWidths(widthCellsTableItemLabel);
                    tableItemLabel.WidthPercentage = 100;
                    tableItemLabel.AddCell(new PdfPCell(new Phrase("Items", fontArial13Bold)) { Border = 0, PaddingTop = 5f, PaddingBottom = 10f });
                    document.Add(tableItemLabel);

                    PdfPTable tableSalesInvoiceItems = new PdfPTable(6);
                    float[] widthsCellsSalesInvoiceItems = new float[] { 100f, 70f, 200f, 100f, 100f, 150f };
                    tableSalesInvoiceItems.SetWidths(widthsCellsSalesInvoiceItems);
                    tableSalesInvoiceItems.WidthPercentage = 100;
                    tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase("Price", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase("VAT", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    Decimal totalAmount = 0;

                    foreach (var salesInvoiceItem in salesInvoiceItems)
                    {
                        tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Quantity.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Unit, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Item, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Price.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Amount.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.VAT, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });

                        totalAmount += salesInvoiceItem.Amount;
                    }

                    tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase("Total", fontArial11Bold)) { Colspan = 4, HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase(totalAmount.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableSalesInvoiceItems.AddCell(new PdfPCell(new Phrase(" ")) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    document.Add(tableSalesInvoiceItems);
                }

                document.Add(Chunk.NEWLINE);

                // ============
                // VAT Analysis
                // ============
                var VATItems = from d in salesInvoiceItems
                               group d by new
                               {
                                   VAT = d.VAT
                               } into g
                               select new
                               {
                                   VAT = g.Key.VAT,
                                   Amount = g.Sum(d => d.Amount),
                                   VATAmount = g.Sum(d => d.VATAmount)
                               };

                if (VATItems.Any())
                {
                    // ==================
                    // VAT Analysis Label
                    // ==================
                    PdfPTable tableVATAnalysisLabel = new PdfPTable(1);
                    float[] widthCellsVATAnalysisLabel = new float[] { 100f };
                    tableVATAnalysisLabel.SetWidths(widthCellsVATAnalysisLabel);
                    tableVATAnalysisLabel.WidthPercentage = 100;
                    tableVATAnalysisLabel.AddCell(new PdfPCell(new Phrase("VAT Analysis", fontArial13Bold)) { Border = 0, PaddingBottom = 10f });
                    document.Add(tableVATAnalysisLabel);

                    PdfPTable tableVATAnalysis = new PdfPTable(3);
                    float[] widthsCellsVATItems = new float[] { 200f, 100f, 100f };
                    tableVATAnalysis.SetWidths(widthsCellsVATItems);
                    tableVATAnalysis.HorizontalAlignment = Element.ALIGN_LEFT;
                    tableVATAnalysis.WidthPercentage = 70;
                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase("VAT", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase("VAT Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    Decimal totalVATAmount = 0;
                    Decimal totalVAT = 0;

                    foreach (var VATItem in VATItems)
                    {
                        tableVATAnalysis.AddCell(new PdfPCell(new Phrase(VATItem.VAT, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableVATAnalysis.AddCell(new PdfPCell(new Phrase(VATItem.Amount.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableVATAnalysis.AddCell(new PdfPCell(new Phrase(VATItem.VATAmount.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });

                        totalVATAmount += VATItem.Amount;
                        totalVAT += VATItem.VATAmount;
                    }

                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase("Total", fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase(totalVATAmount.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableVATAnalysis.AddCell(new PdfPCell(new Phrase(totalVAT.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    document.Add(tableVATAnalysis);
                }

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);

                // ==============
                // User Signature
                // ==============
                PdfPTable tableUsers = new PdfPTable(7);
                float[] widthsCellsTableUsers = new float[] { 100f, 20f, 100f, 20f, 100f, 20f, 100f };
                tableUsers.WidthPercentage = 100;
                tableUsers.SetWidths(widthsCellsTableUsers);
                tableUsers.AddCell(new PdfPCell(new Phrase("Prepared by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableUsers.AddCell(new PdfPCell(new Phrase("Checked by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableUsers.AddCell(new PdfPCell(new Phrase("Approved by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableUsers.AddCell(new PdfPCell(new Phrase("Received by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { Colspan = 7, Border = 0, PaddingTop = 15f, PaddingBottom = 15f });
                tableUsers.AddCell(new PdfPCell(new Phrase(preparedBy, fontArial11)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(checkedBy, fontArial11)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(approvedBy, fontArial11)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                tableUsers.AddCell(new PdfPCell(new Phrase("Date Received", fontArial11Bold)) { Border = 1, HorizontalAlignment = 0, PaddingBottom = 5f });
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