using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiDisbursementController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =================
        // LIST Disbursement
        // =================
        [Route("api/listDisbursement")]
        public List<Models.TrnDisbursement> Get()
        {
            var disbursements = from d in db.TrnDisbursements
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
            return disbursements.ToList();
        }

        // ===================
        // DELETE Disbursement
        // ===================
        [Route("api/deleteDisbursement/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var disbursementId = Convert.ToInt32(id);
                var disbursements = from d in db.TrnDisbursements where d.Id == disbursementId select d;

                if (disbursements.Any())
                {
                    db.TrnDisbursements.DeleteOnSubmit(disbursements.First());
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
    }
}
