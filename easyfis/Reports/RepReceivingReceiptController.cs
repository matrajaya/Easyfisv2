using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepReceivingReceiptController : Controller
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
        public ActionResult ReceivingReceipt(Int32 RRId)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Receiving Receipt", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(branch, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var receivingReceiptHeaders = from d in db.TrnReceivingReceipts
                                          where d.Id == RRId
                                          && d.IsLocked == true
                                          select new Models.TrnReceivingReceipt
                                          {
                                              Id = d.Id,
                                              BranchId = d.BranchId,
                                              Branch = d.MstBranch.Branch,
                                              RRDate = d.RRDate.ToShortDateString(),
                                              RRNumber = d.RRNumber,
                                              SupplierId = d.SupplierId,
                                              Supplier = d.MstArticle.Article,
                                              TermId = d.TermId,
                                              Term = d.MstTerm.Term,
                                              DueDate = d.RRDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays)).ToShortDateString(),
                                              DocumentReference = d.DocumentReference,
                                              ManualRRNumber = d.ManualRRNumber,
                                              Remarks = d.Remarks,
                                              Amount = d.Amount,
                                              WTaxAmount = d.WTaxAmount,
                                              PaidAmount = d.PaidAmount,
                                              AdjustmentAmount = d.AdjustmentAmount,
                                              BalanceAmount = d.BalanceAmount,
                                              ReceivedById = d.ReceivedById,
                                              ReceivedBy = d.MstUser4.FullName,
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
                                              UpdatedBy = d.MstUser5.FullName,
                                              UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                          };

            if (receivingReceiptHeaders.Any())
            {
                String Supplier = "", Term = "", DueDate = "", Remarks = "";
                String RRNumber = "", RRDate = "", DocumentRef = "";
                String PreparedBy = "", CheckedBy = "", ApprovedBy = "", ReceivedBy = "";

                foreach (var receivingReceiptHeader in receivingReceiptHeaders)
                {
                    Supplier = receivingReceiptHeader.Supplier;
                    Term = receivingReceiptHeader.Term;
                    DueDate = receivingReceiptHeader.DueDate;
                    Remarks = receivingReceiptHeader.Remarks;
                    RRNumber = receivingReceiptHeader.RRNumber;
                    RRDate = receivingReceiptHeader.RRDate;
                    DocumentRef = receivingReceiptHeader.DocumentReference;
                    PreparedBy = receivingReceiptHeader.PreparedBy;
                    CheckedBy = receivingReceiptHeader.CheckedBy;
                    ApprovedBy = receivingReceiptHeader.ApprovedBy;
                    ReceivedBy = receivingReceiptHeader.ReceivedBy;
                }

                PdfPTable tableSubHeader = new PdfPTable(4);
                float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                tableSubHeader.SetWidths(widthscellsSubheader);
                tableSubHeader.WidthPercentage = 100;
                tableSubHeader.AddCell(new PdfPCell(new Phrase("Supplier: ", fontArial11Bold)) { Border = 0, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Supplier, fontArial11)) { Border = 0, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("RR Number: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(RRNumber, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                tableSubHeader.AddCell(new PdfPCell(new Phrase("Term: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Term, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("RR date: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(RRDate, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                tableSubHeader.AddCell(new PdfPCell(new Phrase("Due Date: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(DueDate, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("Document Ref: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(DocumentRef, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                tableSubHeader.AddCell(new PdfPCell(new Phrase("Remarks: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Remarks, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("", fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                document.Add(tableSubHeader);
                document.Add(Chunk.NEWLINE);

                var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                            where d.RRId == RRId
                                            && d.TrnReceivingReceipt.IsLocked == true
                                            select new Models.TrnReceivingReceiptItem
                                            {
                                                Id = d.Id,
                                                RRId = d.RRId,
                                                RR = d.TrnReceivingReceipt.RRNumber,
                                                POId = d.POId,
                                                PO = d.TrnPurchaseOrder.PONumber,
                                                ItemId = d.ItemId,
                                                Item = d.MstArticle.Article,
                                                ItemCode = d.MstArticle.ManualArticleCode,
                                                Particulars = d.Particulars,
                                                UnitId = d.UnitId,
                                                Unit = d.MstUnit.Unit,
                                                Quantity = d.Quantity,
                                                Cost = d.Cost,
                                                Amount = d.Amount,
                                                VATId = d.VATId,
                                                VAT = d.MstTaxType.TaxType,
                                                VATPercentage = d.VATPercentage,
                                                VATAmount = d.VATAmount,
                                                WTAXId = d.WTAXId,
                                                WTAX = d.MstTaxType1.TaxType,
                                                WTAXPercentage = d.WTAXPercentage,
                                                WTAXAmount = d.WTAXAmount,
                                                BranchId = d.BranchId,
                                                Branch = d.MstBranch.Branch,
                                                BaseUnitId = d.BaseUnitId,
                                                BaseUnit = d.MstUnit1.Unit,
                                                BaseQuantity = d.BaseQuantity,
                                                BaseCost = d.BaseCost
                                            };

                PdfPTable tableRRLines = new PdfPTable(6);
                float[] widthscellsRRLines = new float[] { 50f, 60f, 150f, 130f, 100f, 100f };
                tableRRLines.SetWidths(widthscellsRRLines);
                tableRRLines.WidthPercentage = 100;

                tableRRLines.AddCell(new PdfPCell(new Phrase("Quantity", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRLines.AddCell(new PdfPCell(new Phrase("Unit", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRLines.AddCell(new PdfPCell(new Phrase("Item", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRLines.AddCell(new PdfPCell(new Phrase("Branch", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRLines.AddCell(new PdfPCell(new Phrase("Price", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRLines.AddCell(new PdfPCell(new Phrase("Amount", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal TotalAmount = 0;
                foreach (var receivingReceiptItem in receivingReceiptItems)
                {
                    tableRRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Quantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Unit, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Item, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Branch, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Price.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    TotalAmount = TotalAmount + receivingReceiptItem.Amount;
                }

                document.Add(tableRRLines);

                document.Add(Chunk.NEWLINE);

                PdfPTable tableRRLineTotalAmount = new PdfPTable(6);
                float[] widthscellsRRLinesTotalAmount = new float[] { 50f, 60f, 150f, 130f, 100f, 100f };
                tableRRLineTotalAmount.SetWidths(widthscellsRRLinesTotalAmount);
                tableRRLineTotalAmount.WidthPercentage = 100;

                tableRRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableRRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableRRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableRRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableRRLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total:", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableRRLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                document.Add(tableRRLineTotalAmount);

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
                tableFooter.AddCell(new PdfPCell(new Phrase(ReceivedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });

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