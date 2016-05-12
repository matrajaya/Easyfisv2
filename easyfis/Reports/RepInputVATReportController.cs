using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class RepInputVATReportController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Input VAT Report
        [Authorize]
        public ActionResult InputVATReport(String StartDate, String EndDate)
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
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);

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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Input Value Added Tax Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Period From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);
            document.Add(Chunk.NEWLINE);

            // receiving receipt Items
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.TrnReceivingReceipt.RRDate >= Convert.ToDateTime(StartDate)
                                        && d.TrnReceivingReceipt.RRDate <= Convert.ToDateTime(EndDate)
                                        && d.VATAmount > 0
                                        && d.TrnReceivingReceipt.IsLocked == true
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            Id = d.Id,
                                            RR = d.TrnReceivingReceipt.RRNumber,
                                            RRDate = d.TrnReceivingReceipt.RRDate.ToShortDateString(),
                                            Supplier = d.TrnReceivingReceipt.MstArticle.Article,
                                            SupplierTIN = d.TrnReceivingReceipt.MstArticle.TaxNumber,
                                            SupplierAddress = d.TrnReceivingReceipt.MstArticle.Address,
                                            Amount = d.Amount,
                                            VATAmount = d.VATAmount
                                        };

            if (receivingReceiptItems.Any())
            {
                // Lines
                PdfPTable tableIVTRLines = new PdfPTable(12);
                float[] widthsCellsIVTRLines = new float[] { 80f, 90f, 100f, 120f, 150f, 90f, 90f, 90f, 90f, 90f, 90f, 90f };
                tableIVTRLines.SetWidths(widthsCellsIVTRLines);
                tableIVTRLines.WidthPercentage = 100;
                tableIVTRLines.AddCell(new PdfPCell(new Phrase("Date", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableIVTRLines.AddCell(new PdfPCell(new Phrase("RR No.", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableIVTRLines.AddCell(new PdfPCell(new Phrase("TIN", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableIVTRLines.AddCell(new PdfPCell(new Phrase("Supplier Name", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableIVTRLines.AddCell(new PdfPCell(new Phrase("Address", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableIVTRLines.AddCell(new PdfPCell(new Phrase("Amount of Gross Purchase", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableIVTRLines.AddCell(new PdfPCell(new Phrase("Amount of Taxable Purchase", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableIVTRLines.AddCell(new PdfPCell(new Phrase("Amount of Purchase of Services", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableIVTRLines.AddCell(new PdfPCell(new Phrase("Amount of Purchase of Capital Goods", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableIVTRLines.AddCell(new PdfPCell(new Phrase("Amount of Purchase of Goods other than Capital Goods", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableIVTRLines.AddCell(new PdfPCell(new Phrase("Amount of Input Tax", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableIVTRLines.AddCell(new PdfPCell(new Phrase("Amount of Gross Taxable Purchase", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal TotalAmountofGrossPurchase = 0;
                Decimal TotalAmountofTaxablePurchase = 0;
                Decimal TotalAmountofPurchaseofServices = 0;
                Decimal TotalAmountofPurchaseofCapitalGoods = 0;
                Decimal TotalAmountofPurchaseofGoodsotherthanCapitalGoods = 0;
                Decimal TotalAmounAmountofInputTax = 0;
                Decimal TotalAmountofGrossTaxablePurchase = 0;

                foreach (var receivingReceiptItem in receivingReceiptItems)
                {
                    tableIVTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.RRDate, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableIVTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.RR, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableIVTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.SupplierTIN, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableIVTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Supplier, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableIVTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.SupplierAddress, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableIVTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableIVTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableIVTRLines.AddCell(new PdfPCell(new Phrase("0.00", fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableIVTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableIVTRLines.AddCell(new PdfPCell(new Phrase("0.00", fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableIVTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.VATAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableIVTRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    TotalAmountofGrossPurchase = TotalAmountofGrossPurchase + receivingReceiptItem.Amount;
                    TotalAmountofTaxablePurchase = TotalAmountofTaxablePurchase + receivingReceiptItem.Amount;
                    TotalAmountofPurchaseofServices = TotalAmountofPurchaseofServices + 0;
                    TotalAmountofPurchaseofCapitalGoods = TotalAmountofPurchaseofCapitalGoods + receivingReceiptItem.Amount;
                    TotalAmountofPurchaseofGoodsotherthanCapitalGoods = TotalAmountofPurchaseofGoodsotherthanCapitalGoods + 0;
                    TotalAmounAmountofInputTax = TotalAmounAmountofInputTax + receivingReceiptItem.VATAmount;
                    TotalAmountofGrossTaxablePurchase = TotalAmountofGrossTaxablePurchase + receivingReceiptItem.Amount;
                }

                document.Add(tableIVTRLines);
                document.Add(Chunk.NEWLINE);

                // Footer Total Amount
                PdfPTable tableIVTRLineTotalAmount = new PdfPTable(12);
                float[] widthscellsIVTRLinesTotalAmount = new float[] { 80f, 90f, 100f, 120f, 150f, 90f, 90f, 90f, 90f, 90f, 90f, 90f };
                tableIVTRLineTotalAmount.SetWidths(widthscellsIVTRLinesTotalAmount);
                tableIVTRLineTotalAmount.WidthPercentage = 100;
                tableIVTRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableIVTRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableIVTRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableIVTRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableIVTRLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total: ", fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableIVTRLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmountofGrossPurchase.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableIVTRLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmountofTaxablePurchase.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableIVTRLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmountofPurchaseofServices.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableIVTRLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmountofPurchaseofCapitalGoods.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableIVTRLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmountofPurchaseofGoodsotherthanCapitalGoods.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableIVTRLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmounAmountofInputTax.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableIVTRLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmountofGrossTaxablePurchase.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                document.Add(tableIVTRLineTotalAmount);
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