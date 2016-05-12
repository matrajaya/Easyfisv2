using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class RepAccountsPayableVoucherController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Accounts Payable Voucher
        [Authorize]
        public ActionResult AccountsPayableVoucher(Int32 ReceivingReceiptId)
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
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial11 = FontFactory.GetFont("Arial", 11);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);

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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Accounts Payable Voucher", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(branch, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // receiving receipts
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.Id == ReceivingReceiptId
                                    && d.IsLocked == true
                                    select new Models.TrnReceivingReceipt
                                    {
                                        Id = d.Id,
                                        RRDate = d.RRDate.ToShortDateString(),
                                        RRNumber = d.RRNumber,
                                        Supplier = d.MstArticle.Article,
                                        Term = d.MstTerm.Term,
                                        DocumentReference = d.DocumentReference,
                                        Remarks = d.Remarks,
                                        ReceivedBy = d.MstUser4.FullName,
                                        PreparedBy = d.MstUser3.FullName,
                                        CheckedBy = d.MstUser1.FullName,
                                        ApprovedBy = d.MstUser.FullName,
                                        DueDate = d.RRDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays)).ToShortDateString(),
                                    };

            if (receivingReceipts.Any())
            {
                foreach (var receivingReceipt in receivingReceipts)
                {
                    // Table Sub Header
                    PdfPTable tableSubHeaderPage = new PdfPTable(4);
                    float[] widthsCellsSubHeaderPage = new float[] { 40f, 100f, 100f, 40f };
                    tableSubHeaderPage.SetWidths(widthsCellsSubHeaderPage);
                    tableSubHeaderPage.WidthPercentage = 100;
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase("Supplier: ", fontArial11Bold)) { Border = 0, PaddingTop = 10f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase(receivingReceipt.Supplier, fontArial11)) { Border = 0, PaddingTop = 10f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase("APV Number: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase(receivingReceipt.RRNumber, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase("Terms: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase(receivingReceipt.Term, fontArial11)) { Border = 0, PaddingTop = 3f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase("APV Date: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase(receivingReceipt.RRDate, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase("Due Date: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase(receivingReceipt.DueDate, fontArial11)) { Border = 0, PaddingTop = 3f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase("Document Ref: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase(receivingReceipt.DocumentReference, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase("Remarks: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase(receivingReceipt.Remarks, fontArial11)) { Border = 0, PaddingTop = 3f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeaderPage.AddCell(new PdfPCell(new Phrase("", fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    document.Add(tableSubHeaderPage);
                }

                document.Add(Chunk.NEWLINE);

                // journals 
                var journals = from d in db.TrnJournals
                               where d.RRId == ReceivingReceiptId
                               select new Models.TrnJournal
                               {
                                   Branch = d.MstBranch.Branch,
                                   Account = d.MstAccount.Account,
                                   AccountCode = d.MstAccount.AccountCode,
                                   Article = d.MstArticle.Article,
                                   DebitAmount = d.DebitAmount,
                                   CreditAmount = d.CreditAmount,
                               };

                // APV Lines
                PdfPTable tableAPVLines = new PdfPTable(6);
                float[] widthscellsAPVLines = new float[] { 100f, 60f, 130f, 120f, 100f, 100f };
                tableAPVLines.SetWidths(widthscellsAPVLines);
                tableAPVLines.WidthPercentage = 100;
                tableAPVLines.AddCell(new PdfPCell(new Phrase("Branch", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableAPVLines.AddCell(new PdfPCell(new Phrase("Code", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableAPVLines.AddCell(new PdfPCell(new Phrase("Account", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableAPVLines.AddCell(new PdfPCell(new Phrase("Article", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableAPVLines.AddCell(new PdfPCell(new Phrase("Debit", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableAPVLines.AddCell(new PdfPCell(new Phrase("Credit", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal TotalDebit = 0;
                Decimal TotalCredit = 0;
                foreach (var journal in journals)
                {
                    tableAPVLines.AddCell(new PdfPCell(new Phrase(journal.Branch, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableAPVLines.AddCell(new PdfPCell(new Phrase(journal.AccountCode, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableAPVLines.AddCell(new PdfPCell(new Phrase(journal.Account, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableAPVLines.AddCell(new PdfPCell(new Phrase(journal.Article, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableAPVLines.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableAPVLines.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    TotalDebit = TotalDebit + journal.DebitAmount;
                    TotalCredit = TotalCredit + journal.CreditAmount;
                }

                document.Add(tableAPVLines);
                document.Add(Chunk.NEWLINE);

                PdfPTable tableAPVLineTotalAmount = new PdfPTable(6);
                float[] widthsCellsAPVLinesTotalAmount = new float[] { 100f, 60f, 130f, 120f, 100f, 100f };
                tableAPVLineTotalAmount.SetWidths(widthsCellsAPVLinesTotalAmount);
                tableAPVLineTotalAmount.WidthPercentage = 100;
                tableAPVLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableAPVLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableAPVLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableAPVLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total: ", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableAPVLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalDebit.ToString("#,##0.00"), fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableAPVLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalCredit.ToString("#,##0.00"), fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                document.Add(tableAPVLineTotalAmount);

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);

                foreach (var receivingReceipt in receivingReceipts)
                {
                    // Table User Stamp Security Footer
                    PdfPTable tableUserStampSecurityFooter = new PdfPTable(5);
                    float[] widthsCellsUserStampSecurityFooter = new float[] { 100f, 20f, 100f, 20f, 100f };
                    tableUserStampSecurityFooter.SetWidths(widthsCellsUserStampSecurityFooter);
                    tableUserStampSecurityFooter.WidthPercentage = 100;
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0 });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase("Checked by:", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0 });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase("Approved by:", fontArial10Bold)) { Border = 0, HorizontalAlignment = 0 });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase(receivingReceipt.PreparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase(receivingReceipt.CheckedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableUserStampSecurityFooter.AddCell(new PdfPCell(new Phrase(receivingReceipt.ApprovedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    document.Add(tableUserStampSecurityFooter);
                }
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