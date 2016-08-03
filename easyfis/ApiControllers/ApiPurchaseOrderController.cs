using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.Net.Mail;


namespace easyfis.Controllers
{
    public class ApiPurchaseOrderController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.BranchId).SingleOrDefault();
        }

        public String zeroFill(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        // get purchase order amount by POId
        public Decimal getPurchaseOrderAmountByPOId(Int32 POId)
        {
            Decimal totalAmount;

            var purchaseOrderItems = from d in db.TrnPurchaseOrderItems where d.POId == POId select d;
            if (!purchaseOrderItems.Any())
            {
                totalAmount = 0;
            }
            else
            {
                totalAmount = purchaseOrderItems.Sum(d => d.Amount);
            }

            return totalAmount;
        }

        // list purchase order
        [Authorize]
        [HttpGet]
        [Route("api/listPurchaseOrder")]
        public List<Models.TrnPurchaseOrder> listPurchaseOrder()
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders.OrderByDescending(d => d.Id)
                                 where d.BranchId == currentBranchId()
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     Amount = getPurchaseOrderAmountByPOId(d.Id),
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
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

            return purchaseOrders.ToList();
        }

        // get purchase order by Id
        [Authorize]
        [HttpGet]
        [Route("api/purchaseOrder/{id}")]
        public Models.TrnPurchaseOrder getPurchaseOrderById(String id)
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 where d.Id == Convert.ToInt32(id)
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     Amount = getPurchaseOrderAmountByPOId(d.Id),
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
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

            return (Models.TrnPurchaseOrder)purchaseOrders.FirstOrDefault();
        }

        // list purchase order by PODate
        [Authorize]
        [HttpGet]
        [Route("api/listPurchaseOrderFilterByPODate/{PODate}")]
        public List<Models.TrnPurchaseOrder> listPurchaseOrderByPODate(String PODate)
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders.OrderByDescending(d => d.Id)
                                 where d.PODate == Convert.ToDateTime(PODate)
                                 && d.BranchId == currentBranchId()
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     Amount = getPurchaseOrderAmountByPOId(d.Id),
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
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

            return purchaseOrders.ToList();
        }

        // list purchase order by supplier Id
        [Authorize]
        [HttpGet]
        [Route("api/listPurchaseOrderBySupplierId/{supplierId}")]
        public List<Models.TrnPurchaseOrder> listPurchaseOrderBySupplierId(String supplierId)
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 where d.SupplierId == Convert.ToInt32(supplierId)
                                 && d.IsLocked == true
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     Amount = getPurchaseOrderAmountByPOId(d.Id),
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
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

            return purchaseOrders.ToList();
        }

        // get last purchase order PONumber
        [Authorize]
        [HttpGet]
        [Route("api/purchaseOrderLastPONumber")]
        public Models.TrnPurchaseOrder getPurchaseOrderLastPONumber()
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders.OrderByDescending(d => d.PONumber)
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     Amount = getPurchaseOrderAmountByPOId(d.Id),
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
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

            return (Models.TrnPurchaseOrder)purchaseOrders.FirstOrDefault();
        }

        // add purchase order
        [Authorize]
        [HttpPost]
        [Route("api/addPurchaseOrder")]
        public Int32 insertPurchaseOrder(Models.TrnPurchaseOrder purchaseOrder)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var lastPONumber = from d in db.TrnPurchaseOrders.OrderByDescending(d => d.Id) select d;
                var PONumberResult = "0000000001";

                if (lastPONumber.Any())
                {
                    var PONumber = Convert.ToInt32(lastPONumber.FirstOrDefault().PONumber) + 0000000001;
                    PONumberResult = zeroFill(PONumber, 10);
                }

                Data.TrnPurchaseOrder newPurchaseOrder = new Data.TrnPurchaseOrder();
                newPurchaseOrder.BranchId = currentBranchId();
                newPurchaseOrder.PONumber = PONumberResult;
                newPurchaseOrder.PODate = DateTime.Today;
                newPurchaseOrder.SupplierId = (from d in db.MstArticles where d.ArticleTypeId == 3 select d.Id).FirstOrDefault();
                newPurchaseOrder.TermId = (from d in db.MstTerms select d.Id).FirstOrDefault();
                newPurchaseOrder.ManualRequestNumber = "NA";
                newPurchaseOrder.ManualPONumber = "NA";
                newPurchaseOrder.DateNeeded = DateTime.Today;
                newPurchaseOrder.Remarks = "NA";
                newPurchaseOrder.IsClose = false;
                newPurchaseOrder.RequestedById = userId;
                newPurchaseOrder.PreparedById = userId;
                newPurchaseOrder.CheckedById = userId;
                newPurchaseOrder.ApprovedById = userId;
                newPurchaseOrder.IsLocked = false;
                newPurchaseOrder.CreatedById = userId;
                newPurchaseOrder.CreatedDateTime = DateTime.Now;
                newPurchaseOrder.UpdatedById = userId;
                newPurchaseOrder.UpdatedDateTime = DateTime.Now;

                db.TrnPurchaseOrders.InsertOnSubmit(newPurchaseOrder);
                db.SubmitChanges();

                return newPurchaseOrder.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update purchase order
        [Authorize]
        [HttpPut]
        [Route("api/updatePurchaseOrder/{id}")]
        public HttpResponseMessage updatePurchaseOrder(String id, Models.TrnPurchaseOrder purchaseOrder)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var purchaseOrders = from d in db.TrnPurchaseOrders where d.Id == Convert.ToInt32(id) select d;
                if (purchaseOrders.Any())
                {
                    var updatePurchaseOrder = purchaseOrders.FirstOrDefault();
                    updatePurchaseOrder.BranchId = purchaseOrder.BranchId;
                    updatePurchaseOrder.PONumber = purchaseOrder.PONumber;
                    updatePurchaseOrder.PODate = Convert.ToDateTime(purchaseOrder.PODate);
                    updatePurchaseOrder.SupplierId = purchaseOrder.SupplierId;
                    updatePurchaseOrder.TermId = purchaseOrder.TermId;
                    updatePurchaseOrder.ManualRequestNumber = purchaseOrder.ManualRequestNumber;
                    updatePurchaseOrder.ManualPONumber = purchaseOrder.ManualPONumber;
                    updatePurchaseOrder.DateNeeded = Convert.ToDateTime(purchaseOrder.DateNeeded);
                    updatePurchaseOrder.Remarks = purchaseOrder.Remarks;
                    updatePurchaseOrder.IsClose = purchaseOrder.IsClose;
                    updatePurchaseOrder.RequestedById = purchaseOrder.RequestedById;
                    updatePurchaseOrder.PreparedById = purchaseOrder.PreparedById;
                    updatePurchaseOrder.CheckedById = purchaseOrder.CheckedById;
                    updatePurchaseOrder.ApprovedById = purchaseOrder.ApprovedById;
                    updatePurchaseOrder.IsLocked = true;
                    updatePurchaseOrder.UpdatedById = userId;
                    updatePurchaseOrder.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // unlock purchase order
        [Authorize]
        [HttpPut]
        [Route("api/updatePurchaseOrderIsLocked/{id}")]
        public HttpResponseMessage unlockPurchaseOrder(String id, Models.TrnPurchaseOrder purchaseOrder)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var purchaseOrders = from d in db.TrnPurchaseOrders where d.Id == Convert.ToInt32(id) select d;
                if (purchaseOrders.Any())
                {
                    var unlockPurchaseOrder = purchaseOrders.FirstOrDefault();
                    unlockPurchaseOrder.IsLocked = false;
                    unlockPurchaseOrder.UpdatedById = userId;
                    unlockPurchaseOrder.UpdatedDateTime = DateTime.Now; ;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        
        // delete purchase order
        [Authorize]
        [HttpDelete]
        [Route("api/deletePO/{id}")]
        public HttpResponseMessage deletePurchaseOrder(String id)
        {
            try
            {
                var purchaseOrders = from d in db.TrnPurchaseOrders where d.Id == Convert.ToInt32(id) select d;
                if (purchaseOrders.Any())
                {
                    db.TrnPurchaseOrders.DeleteOnSubmit(purchaseOrders.First());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        
        // send email purchase order


        [Authorize]
        [HttpPut]
        [Route("api/purchaseOrder/sendEmail")]
        public HttpResponseMessage sendEmail()
        {
            try
            {              
                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSL = true;
               
                

                string emailFrom = "easyfisv2@gmail.com";
                string password = "@innosoft123";
                string emailTo = "mahilumphil@gmail.com";
                string subject = "Hello";
                string body = "";

               


                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    // Can set to false, if you are sending pure text.

                    //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
                    //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));
                    

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
