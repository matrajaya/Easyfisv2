using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepSalesDetailReportController : Controller
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        [Authorize]
        public ActionResult SalesDetailReport(String StartDate, String EndDate, String CompanyId, String BranchId)
        {
            MemoryStream workStream = new MemoryStream();
            Rectangle rectangle = new Rectangle(PageSize.A3);
            Document document = new Document(rectangle, 72, 72, 72, 72);
            document.SetMargins(30f, 30f, 30f, 30f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();

            Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font fontArial11 = FontFactory.GetFont("Arial", 11);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            var companyName = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.MstCompany.Company).SingleOrDefault();
            var address = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.MstCompany.Address).SingleOrDefault();
            var contactNo = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.MstCompany.ContactNumber).SingleOrDefault();
            var branch = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.Branch).SingleOrDefault();

            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthsCellsTableHeader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthsCellsTableHeader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Sales Detail Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeader);
            document.Add(line);

            Decimal totalAmount = 0;
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    where d.TrnSalesInvoice.BranchId == Convert.ToInt32(BranchId)
                                    && d.TrnSalesInvoice.MstBranch.CompanyId == Convert.ToInt32(CompanyId)
                                    && d.TrnSalesInvoice.SIDate >= Convert.ToDateTime(StartDate)
                                    && d.TrnSalesInvoice.SIDate <= Convert.ToDateTime(EndDate)
                                    && d.TrnSalesInvoice.IsLocked == true
                                    select new Models.TrnSalesInvoiceItem
                                    {
                                        SIId = d.SIId,
                                        Id = d.Id,
                                        SI = d.TrnSalesInvoice.SINumber,
                                        SIDate = d.TrnSalesInvoice.SIDate.ToShortDateString(),
                                        Item = d.MstArticle.Article,
                                        ItemInventory = d.MstArticleInventory.InventoryCode,
                                        Unit = d.MstUnit.Unit,
                                        Quantity = d.Quantity,
                                        Amount = d.Amount,
                                        Price = d.Price,
                                        Customer = d.TrnSalesInvoice.MstArticle.Article
                                    };

            if (salesInvoiceItems.Any())
            {
                PdfPTable tableBranchSubHeader = new PdfPTable(1);
                float[] widthCellsTableBranchSubHeader = new float[] { 100f };
                tableBranchSubHeader.SetWidths(widthCellsTableBranchSubHeader);
                tableBranchSubHeader.WidthPercentage = 100;
                PdfPCell branchSubHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial10Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                tableBranchSubHeader.AddCell(branchSubHeaderColspan);
                document.Add(tableBranchSubHeader);

                PdfPTable tableSalesInvoiceItemData = new PdfPTable(9);
                float[] widthsCellsTableSalesInvoiceItemData = new float[] { 18f, 15f, 30f, 30f, 30f, 20f, 20f, 20f, 20f };
                tableSalesInvoiceItemData.SetWidths(widthsCellsTableSalesInvoiceItemData);
                tableSalesInvoiceItemData.WidthPercentage = 100;
                tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase("SI Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase("SI Date", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase("Customer", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase("Inventory Code", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase("Price", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                foreach (var salesInvoiceItem in salesInvoiceItems)
                {
                    tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.SI, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.SIDate, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Customer, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Item, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.ItemInventory, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Price.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Quantity.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Unit, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceItemData.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Amount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    totalAmount = totalAmount + salesInvoiceItem.Amount;
                }

                document.Add(tableSalesInvoiceItemData);
                document.Add(Chunk.NEWLINE);

                PdfPTable tableTotalAmountFooter = new PdfPTable(9);
                float[] widthsCellsTableTotalAmountFooter = new float[] { 18f, 15f, 30f, 30f, 30f, 20f, 20f, 20f, 20f };
                tableTotalAmountFooter.SetWidths(widthsCellsTableTotalAmountFooter);
                tableTotalAmountFooter.WidthPercentage = 100;
                tableTotalAmountFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAmountFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAmountFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAmountFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAmountFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAmountFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAmountFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAmountFooter.AddCell(new PdfPCell(new Phrase("Total", fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAmountFooter.AddCell(new PdfPCell(new Phrase(totalAmount.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });

                document.Add(tableTotalAmountFooter);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}