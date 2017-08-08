using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepCollectionDetailReportController : Controller
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        [Authorize]
        public ActionResult CollectionDetailReport(String StartDate, String EndDate, String CompanyId, String BranchId)
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
            tableHeader.AddCell(new PdfPCell(new Phrase("Collection Detail Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeader);
            document.Add(line);

            Decimal totalAmount = 0;
            var collectionLines = from d in db.TrnCollectionLines
                                  where d.TrnCollection.BranchId == Convert.ToInt32(BranchId)
                                  && d.TrnCollection.ORDate >= Convert.ToDateTime(StartDate)
                                  && d.TrnCollection.ORDate <= Convert.ToDateTime(EndDate)
                                  && d.TrnCollection.IsLocked == true
                                  select new Models.TrnCollectionLine
                                  {
                                      Id = d.Id,
                                      OR = d.TrnCollection.ORNumber,
                                      ORDate = d.TrnCollection.ORDate.ToShortDateString(),
                                      SI = d.TrnSalesInvoice.SINumber,
                                      Amount = d.Amount,
                                      DepositoryBank = d.MstArticle1.Article,
                                      PayType = d.MstPayType.PayType
                                  };

            if (collectionLines.Any())
            {
                PdfPTable tableBranchSubHeader = new PdfPTable(1);
                float[] widthCellsTableBranchSubHeader = new float[] { 100f };
                tableBranchSubHeader.SetWidths(widthCellsTableBranchSubHeader);
                tableBranchSubHeader.WidthPercentage = 100;
                PdfPCell branchSubHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial10Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                tableBranchSubHeader.AddCell(branchSubHeaderColspan);
                document.Add(tableBranchSubHeader);

                PdfPTable tableCollectionLinesData = new PdfPTable(6);
                float[] widthsCellsTableCollectionLinesData = new float[] { 15f, 10f, 30f, 30f, 30f, 20f };
                tableCollectionLinesData.SetWidths(widthsCellsTableCollectionLinesData);
                tableCollectionLinesData.WidthPercentage = 100;
                tableCollectionLinesData.AddCell(new PdfPCell(new Phrase("OR Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCollectionLinesData.AddCell(new PdfPCell(new Phrase("OR Date", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCollectionLinesData.AddCell(new PdfPCell(new Phrase("Pay Type", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCollectionLinesData.AddCell(new PdfPCell(new Phrase("SI Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCollectionLinesData.AddCell(new PdfPCell(new Phrase("Depository Bank", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCollectionLinesData.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                foreach (var collectionLine in collectionLines)
                {
                    tableCollectionLinesData.AddCell(new PdfPCell(new Phrase(collectionLine.OR, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCollectionLinesData.AddCell(new PdfPCell(new Phrase(collectionLine.ORDate, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCollectionLinesData.AddCell(new PdfPCell(new Phrase(collectionLine.PayType, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCollectionLinesData.AddCell(new PdfPCell(new Phrase(collectionLine.SI, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCollectionLinesData.AddCell(new PdfPCell(new Phrase(collectionLine.DepositoryBank, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCollectionLinesData.AddCell(new PdfPCell(new Phrase(collectionLine.Amount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    totalAmount = totalAmount + collectionLine.Amount;
                }

                document.Add(tableCollectionLinesData);
                document.Add(Chunk.NEWLINE);

                PdfPTable tableORLineTotalFooter = new PdfPTable(6);
                float[] widthscellsTableORLineTotalFooter = new float[] { 15f, 10f, 30f, 30f, 30f, 20f };
                tableORLineTotalFooter.SetWidths(widthscellsTableORLineTotalFooter);
                tableORLineTotalFooter.WidthPercentage = 100;
                tableORLineTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableORLineTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableORLineTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableORLineTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableORLineTotalFooter.AddCell(new PdfPCell(new Phrase("Total:", fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableORLineTotalFooter.AddCell(new PdfPCell(new Phrase(totalAmount.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });

                document.Add(tableORLineTotalFooter);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}