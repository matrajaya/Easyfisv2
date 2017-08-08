using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepSalesSummaryReportController : Controller
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        [Authorize]
        public ActionResult SalesSummaryReport(String StartDate, String EndDate, String CompanyId, String BranchId)
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
            float[] widthsCellstableHeader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthsCellstableHeader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Sales Summary Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeader);
            document.Add(line);

            Decimal totalAmount = 0;
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.BranchId == Convert.ToInt32(BranchId)
                                && d.MstBranch.CompanyId == Convert.ToInt32(CompanyId)
                                && d.SIDate >= Convert.ToDateTime(StartDate)
                                && d.SIDate <= Convert.ToDateTime(EndDate)
                                && d.IsLocked == true
                                select new Models.TrnSalesInvoice
                                {
                                    Id = d.Id,
                                    Branch = d.MstBranch.Branch,
                                    SINumber = d.SINumber,
                                    SIDate = d.SIDate.ToShortDateString(),
                                    Customer = d.MstArticle.Article,
                                    Remarks = d.Remarks,
                                    SoldBy = d.MstUser4.FullName,
                                    Amount = d.Amount
                                };

            if (salesInvoices.Any())
            {
                PdfPTable tableBranchSubHeader = new PdfPTable(1);
                float[] widthCellsTableBranchSubHeader = new float[] { 100f };
                tableBranchSubHeader.SetWidths(widthCellsTableBranchSubHeader);
                tableBranchSubHeader.WidthPercentage = 100;
                PdfPCell branchSubHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial10Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                tableBranchSubHeader.AddCell(branchSubHeaderColspan);
                document.Add(tableBranchSubHeader);

                PdfPTable tableSalesInvoiceData = new PdfPTable(7);
                float[] widthsCellsTableSalesInvoiceData = new float[] { 25f, 10f, 15f, 25f, 25f, 20f, 20f };
                tableSalesInvoiceData.SetWidths(widthsCellsTableSalesInvoiceData);
                tableSalesInvoiceData.WidthPercentage = 100;
                tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase("Branch", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase("SI Date", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase("SI Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase("Customer", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase("Sold By", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase("Remarks", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                foreach (var salesInvoice in salesInvoices)
                {
                    tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase(salesInvoice.Branch, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase(salesInvoice.SIDate, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase(salesInvoice.SINumber, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase(salesInvoice.Customer, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase(salesInvoice.SoldBy, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase(salesInvoice.Remarks, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSalesInvoiceData.AddCell(new PdfPCell(new Phrase(salesInvoice.Amount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    totalAmount = totalAmount + salesInvoice.Amount;
                }

                document.Add(tableSalesInvoiceData);
                document.Add(Chunk.NEWLINE);

                PdfPTable tableTotalAmountFooter = new PdfPTable(7);
                float[] widthsCellsTableTotalAmountFooter = new float[] { 25f, 5f, 10f, 10f, 20f, 50f, 20f };
                tableTotalAmountFooter.SetWidths(widthsCellsTableTotalAmountFooter);
                tableTotalAmountFooter.WidthPercentage = 100;
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