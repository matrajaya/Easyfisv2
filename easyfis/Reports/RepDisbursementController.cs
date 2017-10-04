using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepDisbursementController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==================
        // Disbursement - PDF
        // ==================
        [Authorize]
        public ActionResult Disbursement(Int32 DisbursementId)
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
            var branch = (from d in db.MstBranches where d.Id == Convert.ToInt32(currentBranchId) select d.ContactNumber).FirstOrDefault();

            // ===========
            // Header Page
            // ===========
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase("Check / Cash Voucher", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
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

            // ================
            // Get Disbursement
            // ================
            var disbursements = from d in db.TrnDisbursements
                                where d.Id == DisbursementId
                                && d.IsLocked == true
                                select d;

            if (disbursements.Any())
            {
                String payee = disbursements.FirstOrDefault().Payee;
                String bank = disbursements.FirstOrDefault().MstArticle1.Article;
                String particulars = disbursements.FirstOrDefault().Particulars;
                String checkNo = disbursements.FirstOrDefault().CheckNumber;
                String CVNumber = disbursements.FirstOrDefault().CVNumber;
                String CVDate = disbursements.FirstOrDefault().CVDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                String checkDate = disbursements.FirstOrDefault().CheckDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                String preparedBy = disbursements.FirstOrDefault().MstUser3.FullName;
                String checkedBy = disbursements.FirstOrDefault().MstUser1.FullName;
                String approvedBy = disbursements.FirstOrDefault().MstUser.FullName;

                PdfPTable tableDisbursement = new PdfPTable(4);
                float[] widthscellsTableDisbursement = new float[] { 40f, 150f, 70f, 50f };
                tableDisbursement.SetWidths(widthscellsTableDisbursement);
                tableDisbursement.WidthPercentage = 100;

                tableDisbursement.AddCell(new PdfPCell(new Phrase("Payee", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableDisbursement.AddCell(new PdfPCell(new Phrase(payee, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableDisbursement.AddCell(new PdfPCell(new Phrase("CV Number", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableDisbursement.AddCell(new PdfPCell(new Phrase(CVNumber, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableDisbursement.AddCell(new PdfPCell(new Phrase("Particulars", fontArial11Bold)) { Rowspan = 3, Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                var particularsPhrase = new Phrase();
                particularsPhrase.Add(new Chunk(particulars, fontArial11));

                Paragraph particularsParagraph = new Paragraph();
                particularsParagraph.Add(particularsPhrase);

                PdfPCell particularsCell = new PdfPCell();
                particularsCell.AddElement(particularsParagraph);

                tableDisbursement.AddCell(new PdfPCell(particularsCell) { Rowspan = 3, Border = 0, PaddingTop = 0f, PaddingLeft = 5f, PaddingRight = 5f });
                tableDisbursement.AddCell(new PdfPCell(new Phrase("Check No.", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableDisbursement.AddCell(new PdfPCell(new Phrase(checkNo, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableDisbursement.AddCell(new PdfPCell(new Phrase("Check Date", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableDisbursement.AddCell(new PdfPCell(new Phrase(checkDate, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableDisbursement.AddCell(new PdfPCell(new Phrase("Bank", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                tableDisbursement.AddCell(new PdfPCell(new Phrase(bank, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                document.Add(tableDisbursement);

                document.Add(spaceTable);

                // ===========
                // Get Journal
                // ===========
                var journals = from d in db.TrnJournals
                               where d.CVId == DisbursementId
                               select new
                               {
                                   Id = d.Id,
                                   JournalDate = d.JournalDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture),
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

                if (journals.Any())
                {
                    PdfPTable tableJournal = new PdfPTable(6);
                    float[] widthscellsJournal = new float[] { 120f, 90f, 130f, 150f, 100f, 100f };
                    tableJournal.SetWidths(widthscellsJournal);
                    tableJournal.WidthPercentage = 100;
                    tableJournal.AddCell(new PdfPCell(new Phrase("Branch", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableJournal.AddCell(new PdfPCell(new Phrase("Code", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableJournal.AddCell(new PdfPCell(new Phrase("Account", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableJournal.AddCell(new PdfPCell(new Phrase("Article", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableJournal.AddCell(new PdfPCell(new Phrase("Debit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableJournal.AddCell(new PdfPCell(new Phrase("Credit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });

                    Decimal totalDebitAmount = 0;
                    Decimal totalCreditAmount = 0;

                    foreach (var journal in journals)
                    {
                        tableJournal.AddCell(new PdfPCell(new Phrase(journal.Branch, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableJournal.AddCell(new PdfPCell(new Phrase(journal.AccountCode, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableJournal.AddCell(new PdfPCell(new Phrase(journal.Account, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableJournal.AddCell(new PdfPCell(new Phrase(journal.Article, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableJournal.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableJournal.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });

                        totalDebitAmount += journal.DebitAmount;
                        totalCreditAmount += journal.CreditAmount;
                    }

                    tableJournal.AddCell(new PdfPCell(new Phrase("Total", fontArial11Bold)) { Colspan = 4, HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableJournal.AddCell(new PdfPCell(new Phrase(totalDebitAmount.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableJournal.AddCell(new PdfPCell(new Phrase(totalCreditAmount.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    document.Add(tableJournal);

                    document.Add(spaceTable);
                }

                // ======================
                // Get Disbursement Lines
                // ======================
                var disbursementLines = from d in db.TrnDisbursementLines
                                        where d.CVId == DisbursementId
                                        && d.TrnDisbursement.IsLocked == true
                                        select new
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
                                            RR = d.TrnReceivingReceipt,
                                            Particulars = d.Particulars,
                                            Amount = d.Amount
                                        };

                if (disbursementLines.Any())
                {
                    PdfPTable tableDisbursementLines = new PdfPTable(4);
                    float[] widthscellsDisbursementLines = new float[] { 120f, 80f, 140f, 100f };
                    tableDisbursementLines.SetWidths(widthscellsDisbursementLines);
                    tableDisbursementLines.WidthPercentage = 100;
                    tableDisbursementLines.AddCell(new PdfPCell(new Phrase("RR Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableDisbursementLines.AddCell(new PdfPCell(new Phrase("RR Date", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableDisbursementLines.AddCell(new PdfPCell(new Phrase("Particulars", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                    tableDisbursementLines.AddCell(new PdfPCell(new Phrase("Paid Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });

                    Decimal totalPaidAmount = 0;

                    foreach (var disbursementLine in disbursementLines)
                    {
                        String RRNumber = " ", RRDate = " ";
                        if (disbursementLine.RRId != null)
                        {
                            RRNumber = disbursementLine.RR.RRNumber;
                            RRDate = disbursementLine.RR.RRDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                        }

                        tableDisbursementLines.AddCell(new PdfPCell(new Phrase(RRNumber, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableDisbursementLines.AddCell(new PdfPCell(new Phrase(RRDate, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableDisbursementLines.AddCell(new PdfPCell(new Phrase(disbursementLine.Particulars, fontArial11)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableDisbursementLines.AddCell(new PdfPCell(new Phrase(disbursementLine.Amount.ToString("#,##0.00"), fontArial11)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 7f, PaddingLeft = 5f, PaddingRight = 5f });

                        totalPaidAmount += disbursementLine.Amount;
                    }

                    tableDisbursementLines.AddCell(new PdfPCell(new Phrase("Total", fontArial11Bold)) { Colspan = 3, HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableDisbursementLines.AddCell(new PdfPCell(new Phrase(totalPaidAmount.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 5f, PaddingBottom = 9f, PaddingLeft = 5f, PaddingRight = 5f });
                    document.Add(tableDisbursementLines);

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

                document.Add(spaceTable);

                // ================
                // Money Word Table
                // ================
                PdfPTable tableMoneyWord = new PdfPTable(3)
                {
                    WidthPercentage = 100
                };
                float[] widthsSignFooterCells = new float[] { 40f, 100f, 140f };
                tableMoneyWord.SetWidths(widthsSignFooterCells);
                tableMoneyWord.AddCell(new PdfPCell(new Phrase("Check No.", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableMoneyWord.AddCell(new PdfPCell(new Phrase(checkNo, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });


                String paidAmount = Convert.ToString(Math.Round(disbursements.FirstOrDefault().Amount * 100) / 100);
                var amountTablePhrase = new Phrase();
                var amountString = "ZERO";

                if (Convert.ToDecimal(paidAmount) != 0)
                {
                    amountString = GetMoneyWord(paidAmount).ToUpper();
                }

                amountTablePhrase.Add(new Chunk("Representing Payment from " + companyName + "the amount of ", fontArial11));
                amountTablePhrase.Add(new Chunk(amountString, fontArial11Bold));

                Paragraph paragraphAmountTable = new Paragraph();
                paragraphAmountTable.SetLeading(0, 1.4f);
                paragraphAmountTable.Add(amountTablePhrase);

                PdfPCell chunkyAmountTable = new PdfPCell();
                chunkyAmountTable.AddElement(paragraphAmountTable);
                chunkyAmountTable.BorderWidth = PdfPCell.NO_BORDER;

                tableMoneyWord.AddCell(new PdfPCell(chunkyAmountTable) { Rowspan = 3, Border = 0, PaddingTop = 0f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 0 });
                tableMoneyWord.AddCell(new PdfPCell(new Phrase("Check Date", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableMoneyWord.AddCell(new PdfPCell(new Phrase(checkDate, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableMoneyWord.AddCell(new PdfPCell(new Phrase("Bank", fontArial11Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableMoneyWord.AddCell(new PdfPCell(new Phrase(bank, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(tableMoneyWord);

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);

                // ===============
                // Signature Table
                // ===============
                PdfPTable tableSignature = new PdfPTable(4)
                {
                    WidthPercentage = 100
                };
                float[] widthCellsSignature = new float[] { 115f, 50f, 10f, 50f };
                tableSignature.SetWidths(widthCellsSignature);
                tableSignature.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Border = 0 });
                tableSignature.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Border = 0 });
                tableSignature.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Border = 0 });
                tableSignature.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Border = 0 });
                tableSignature.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, });
                tableSignature.AddCell(new PdfPCell(new Phrase("Signature Over Printed Name", fontArial11Bold)) { Border = 1, HorizontalAlignment = 1 });
                tableSignature.AddCell(new PdfPCell(new Phrase(" ", fontArial11Bold)) { Border = 0 });
                tableSignature.AddCell(new PdfPCell(new Phrase("Date", fontArial11Bold)) { Border = 1, HorizontalAlignment = 1 });
                document.Add(tableSignature);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        // =====================
        // Convert To Money Word
        // =====================
        public static String GetMoneyWord(String input)
        {
            String decimals = "";
            if (input.Contains("."))
            {
                decimals = input.Substring(input.IndexOf(".") + 1);
                input = input.Remove(input.IndexOf("."));
            }

            String strWords = GetMoreThanThousandNumberWords(input);
            if (decimals.Length > 0)
            {
                if (Convert.ToDecimal(decimals) > 0)
                {
                    String getFirstRoundedDecimals = new String(decimals.Take(2).ToArray());
                    strWords += " Pesos And " + GetMoreThanThousandNumberWords(getFirstRoundedDecimals) + " Cents Only";
                }
                else
                {
                    strWords += " Pesos Only";
                }
            }
            else
            {
                strWords += " Pesos Only";
            }

            return strWords;
        }

        // ===================================
        // Get More Than Thousand Number Words
        // ===================================
        private static String GetMoreThanThousandNumberWords(string input)
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
                    _3digits = GetHundredNumberWords(no);

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

        // =====================================
        // Get From Ones to Hundred Number Words
        // =====================================
        private static String GetHundredNumberWords(Int32 no)
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