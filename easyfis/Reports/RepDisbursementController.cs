using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepDisbursementController : Controller
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
        public ActionResult Disbursement(Int32 DisbursementId)
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
            Font fontArial12 = FontFactory.GetFont("Arial", 12);

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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Check/Cash Voucher", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(branch, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var disbursementHeaders = from d in db.TrnDisbursements
                                      where d.Id == DisbursementId
                                      && d.IsLocked == true
                                      select new Models.TrnDisbursement
                                      {
                                          Id = d.Id,
                                          BranchId = d.BranchId,
                                          Branch = d.MstBranch.Branch,
                                          CVNumber = d.CVNumber,
                                          CVDate = d.CVDate.ToShortDateString(),
                                          SupplierId = d.SupplierId,
                                          Supplier = d.MstArticle.Article,
                                          Payee = d.Payee,
                                          PayTypeId = d.PayTypeId,
                                          PayType = d.MstPayType.PayType,
                                          BankId = d.BankId,
                                          Bank = d.MstArticle1.Article,
                                          ManualCVNumber = d.ManualCVNumber,
                                          Particulars = d.Particulars,
                                          CheckNumber = d.CheckNumber,
                                          CheckDate = d.CheckDate.ToShortDateString(),
                                          Amount = d.Amount,
                                          IsCrossCheck = d.IsCrossCheck,
                                          IsClear = d.IsClear,
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

            if (disbursementHeaders.Any())
            {
                String Payee = "", Bank = "", Particulars = "", CheckNo = "";
                String CVNumber = "", CVDate = "", CheckDate = "";
                String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                foreach (var disbursementHeader in disbursementHeaders)
                {
                    Payee = disbursementHeader.Payee;
                    Bank = disbursementHeader.Bank;
                    CheckNo = disbursementHeader.CheckNumber;
                    CheckDate = disbursementHeader.CheckDate;
                    Particulars = disbursementHeader.Particulars;
                    CVNumber = disbursementHeader.CVNumber;
                    CVDate = disbursementHeader.CVDate;
                    PreparedBy = disbursementHeader.PreparedBy;
                    CheckedBy = disbursementHeader.CheckedBy;
                    ApprovedBy = disbursementHeader.ApprovedBy;
                }

                PdfPTable tableSubHeader = new PdfPTable(4);
                float[] widthscellsSubheader = new float[] { 50f, 70f, 50f, 50f };
                tableSubHeader.SetWidths(widthscellsSubheader);
                tableSubHeader.WidthPercentage = 100;
                tableSubHeader.AddCell(new PdfPCell(new Phrase("Payee: ", fontArial11Bold)) { Border = 0, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Payee, fontArial11)) { Border = 0, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("CV Number: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(CVNumber, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                tableSubHeader.AddCell(new PdfPCell(new Phrase("Particulars: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Particulars, fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("Check No.: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(CheckNo, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                tableSubHeader.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("", fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("Check Date: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(CheckDate, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                tableSubHeader.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("", fontArial11)) { Border = 0, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase("Bank:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                tableSubHeader.AddCell(new PdfPCell(new Phrase(Bank, fontArial11)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                document.Add(tableSubHeader);
                document.Add(Chunk.NEWLINE);

                var journals = from d in db.TrnJournals
                               where d.CVId == DisbursementId
                               select new Models.TrnJournal
                               {
                                   Id = d.Id,
                                   JournalDate = d.JournalDate.ToShortDateString(),
                                   BranchId = d.BranchId,
                                   Branch = d.MstBranch.Branch,
                                   AccountId = d.AccountId,
                                   Account = d.MstAccount.Account,
                                   AccountCode = d.MstAccount.AccountCode,
                                   ArticleId = d.ArticleId,
                                   Article = d.MstArticle.Article,
                                   Particulars = d.Particulars,
                                   DebitAmount = d.DebitAmount,
                                   CreditAmount = d.CreditAmount,
                                   ORId = d.ORId,
                                   CVId = d.CVId,
                                   JVId = d.JVId,
                                   RRId = d.RRId,
                                   SIId = d.SIId,
                                   INId = d.INId,
                                   OTId = d.OTId,
                                   STId = d.STId,
                                   DocumentReference = d.DocumentReference,
                                   APRRId = d.APRRId,
                                   ARSIId = d.ARSIId,
                               };

                PdfPTable tableCVJournal = new PdfPTable(6);
                float[] widthscellsCVJournal = new float[] { 120f, 90f, 130f, 150f, 100f, 100f };
                tableCVJournal.SetWidths(widthscellsCVJournal);
                tableCVJournal.WidthPercentage = 100;

                tableCVJournal.AddCell(new PdfPCell(new Phrase("Branch", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVJournal.AddCell(new PdfPCell(new Phrase("Code", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVJournal.AddCell(new PdfPCell(new Phrase("Account", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVJournal.AddCell(new PdfPCell(new Phrase("Article", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVJournal.AddCell(new PdfPCell(new Phrase("Debit", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVJournal.AddCell(new PdfPCell(new Phrase("Credit", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal TotalDebit = 0;
                Decimal TotalCredit = 0;

                foreach (var journal in journals)
                {
                    tableCVJournal.AddCell(new PdfPCell(new Phrase(journal.Branch, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVJournal.AddCell(new PdfPCell(new Phrase(journal.AccountCode, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVJournal.AddCell(new PdfPCell(new Phrase(journal.Account, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVJournal.AddCell(new PdfPCell(new Phrase(journal.Article, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVJournal.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVJournal.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    TotalDebit = TotalDebit + journal.DebitAmount;
                    TotalCredit = TotalCredit + journal.CreditAmount;
                }

                document.Add(tableCVJournal);
                document.Add(Chunk.NEWLINE);

                PdfPTable tableCVLineTotalDebitCredit = new PdfPTable(6);
                float[] widthscellsCVLinesTotalDebitCredit = new float[] { 120f, 90f, 130f, 150f, 100f, 100f };
                tableCVLineTotalDebitCredit.SetWidths(widthscellsCVLinesTotalDebitCredit);
                tableCVLineTotalDebitCredit.WidthPercentage = 100;

                tableCVLineTotalDebitCredit.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableCVLineTotalDebitCredit.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableCVLineTotalDebitCredit.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableCVLineTotalDebitCredit.AddCell(new PdfPCell(new Phrase("Total: ", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableCVLineTotalDebitCredit.AddCell(new PdfPCell(new Phrase(TotalDebit.ToString("#,##0.00"), fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableCVLineTotalDebitCredit.AddCell(new PdfPCell(new Phrase(TotalCredit.ToString("#,##0.00"), fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                document.Add(tableCVLineTotalDebitCredit);

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);

                var disbursementLines = from d in db.TrnDisbursementLines
                                        where d.CVId == DisbursementId
                                        && d.RRId != null
                                        && d.TrnDisbursement.IsLocked == true
                                        select new Models.TrnDisbursementLine
                                        {
                                            Id = d.Id,
                                            CVId = d.CVId,
                                            CV = d.TrnDisbursement.CVNumber,
                                            BranchId = d.BranchId,
                                            Branch = d.MstBranch.Branch,
                                            AccountId = d.AccountId,
                                            Account = d.MstAccount.Account,
                                            ArticleId = d.ArticleId,
                                            Article = d.MstArticle.Article,
                                            RRId = d.RRId,
                                            RR = d.TrnReceivingReceipt.RRNumber,
                                            RRDate = d.TrnReceivingReceipt.RRDate.ToShortDateString(),
                                            Particulars = d.Particulars,
                                            Amount = d.Amount
                                        };

                PdfPTable tableCVLines = new PdfPTable(4);
                float[] widthscellsCVLines = new float[] { 120f, 80f, 140f, 100f };
                tableCVLines.SetWidths(widthscellsCVLines);
                tableCVLines.WidthPercentage = 100;
                tableCVLines.AddCell(new PdfPCell(new Phrase("RR Number", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVLines.AddCell(new PdfPCell(new Phrase("RR Date", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVLines.AddCell(new PdfPCell(new Phrase("Particulars", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableCVLines.AddCell(new PdfPCell(new Phrase("Paid Amount", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal TotalAmount = 0;

                foreach (var disbursementLine in disbursementLines)
                {
                    tableCVLines.AddCell(new PdfPCell(new Phrase(disbursementLine.RR, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLines.AddCell(new PdfPCell(new Phrase(disbursementLine.RRDate, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLines.AddCell(new PdfPCell(new Phrase(disbursementLine.Particulars, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLines.AddCell(new PdfPCell(new Phrase(disbursementLine.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });

                    TotalAmount = TotalAmount + disbursementLine.Amount;
                }

                document.Add(tableCVLines);

                document.Add(Chunk.NEWLINE);

                PdfPTable tableCVLineTotalAmount = new PdfPTable(4);
                float[] widthscellsCVLinesTotalAmount = new float[] { 120f, 80f, 140f, 100f };
                tableCVLineTotalAmount.SetWidths(widthscellsCVLinesTotalAmount);
                tableCVLineTotalAmount.WidthPercentage = 100;

                tableCVLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableCVLineTotalAmount.AddCell(new PdfPCell(new Phrase("", fontArial9)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableCVLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total:", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableCVLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), fontArial9)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                document.Add(tableCVLineTotalAmount);

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
                tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });

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

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(line);

                string paidAmount = Convert.ToString(Math.Round(TotalAmount * 100) / 100);

                document.Add(Chunk.NEWLINE);
                // Table for sign Footer
                PdfPTable tableSignFooter = new PdfPTable(3);
                tableSignFooter.WidthPercentage = 100;
                float[] widthsSignFooterCells = new float[] { 50f, 100f, 150f };
                tableSignFooter.SetWidths(widthsSignFooterCells);
                tableSignFooter.AddCell(new PdfPCell(new Phrase("Check No.:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableSignFooter.AddCell(new PdfPCell(new Phrase(CheckNo, fontArial11)) { Border = 0, });
                tableSignFooter.AddCell(new PdfPCell(new Phrase("Representing Payment from " + companyName + "the amount of", fontArial11)) { Border = 0, HorizontalAlignment = 0 });

                tableSignFooter.AddCell(new PdfPCell(new Phrase("Check Date:", fontArial11Bold)) { Border = 0, });
                tableSignFooter.AddCell(new PdfPCell(new Phrase(CheckDate, fontArial11)) { Border = 0, });
                tableSignFooter.AddCell(new PdfPCell(new Phrase(getMoneyWord(paidAmount).ToUpper(), fontArial12Bold)) { Border = 0, Rowspan = 2, });

                tableSignFooter.AddCell(new PdfPCell(new Phrase("Bank:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
                tableSignFooter.AddCell(new PdfPCell(new Phrase(Bank, fontArial11)) { Border = 0, });
                tableSignFooter.AddCell(new PdfPCell(new Phrase("")) { Border = 0, HorizontalAlignment = 1 });
                document.Add(tableSignFooter);

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);

                // Table for signature Footer
                PdfPTable tableSignatureFooter = new PdfPTable(4);
                tableSignatureFooter.WidthPercentage = 100;
                float[] widthsSignatureFooterCells = new float[] { 115f, 50f, 10f, 50f };
                tableSignatureFooter.SetWidths(widthsSignatureFooterCells);

                tableSignatureFooter.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Border = 0 });
                tableSignatureFooter.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Border = 0 });
                tableSignatureFooter.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Border = 0 });
                tableSignatureFooter.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Border = 0 });

                tableSignatureFooter.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, });
                tableSignatureFooter.AddCell(new PdfPCell(new Phrase("Signature Over Printed Name", fontArial11Bold)) { Border = 1, HorizontalAlignment = 1 });
                tableSignatureFooter.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Border = 0 });
                tableSignatureFooter.AddCell(new PdfPCell(new Phrase("Date", fontArial11Bold)) { Border = 1, HorizontalAlignment = 1 });

                document.Add(tableSignatureFooter);
            }

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        public static String getMoneyWord(String input)
        {
            String decimals = "";
            if (input.Contains("."))
            {
                decimals = input.Substring(input.IndexOf(".") + 1);
                input = input.Remove(input.IndexOf("."));
            }

            String strWords = GetWords(input);
            if (decimals.Length > 0)
            {
                strWords += "And " + GetWords(decimals) + " Cents ";
            }

            return strWords;
        }

        private static String GetWords(string input)
        {
            try
            {
                String[] seperators = { "", " Thousand ", " Million ", " Billion " };

                int i = 0;

                String strWords = "";

                while (input.Length > 0)
                {
                    String _3digits = input.Length < 3 ? input : input.Substring(input.Length - 3);
                    input = input.Length < 3 ? "" : input.Remove(input.Length - 3);

                    Int32 no = Int32.Parse(_3digits);
                    _3digits = GetWord(no);

                    _3digits += seperators[i];
                    strWords = _3digits + strWords;

                    i++;
                }


                return strWords;
            }
            catch
            {
                return "Invalid Amount";
            }
        }

        private static String GetWord(Int32 no)
        {
            String[] Ones =
            {
                "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
                "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen"
            };

            String[] Tens = { "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninty" };
            String word = "";

            if (no > 99 && no < 1000)
            {
                Int32 i = no / 100;
                word = word + Ones[i - 1] + " Hundred ";
                no = no % 100;
            }

            if (no > 19 && no < 100)
            {
                Int32 i = no / 10;
                word = word + Tens[i - 1] + " ";
                no = no % 10;
            }

            if (no > 0 && no < 20)
            {
                word = word + Ones[no - 1];
            }

            return word;
        }
    }
}