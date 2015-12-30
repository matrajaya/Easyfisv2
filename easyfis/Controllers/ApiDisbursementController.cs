using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

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

        // ======================
        // GET Disbursement By Id
        // ======================
        [Route("api/disbursement/{id}")]
        public Models.TrnDisbursement GetDisbusementById(String id)
        {
            var disbursement_Id = Convert.ToInt32(id);
            var disbursements = from d in db.TrnDisbursements
                                where d.Id == disbursement_Id
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
            return (Models.TrnDisbursement)disbursements.FirstOrDefault();
        }

        // ==================================
        // GET Disbursement Filter by CV Date
        // ==================================
        [Route("api/listDisbursementFilterByCVDate/{CVDate}")]
        public List<Models.TrnDisbursement> GetDisbusementFilterByCVDate(String CVDate)
        {
            var disbursement_CVDate = Convert.ToDateTime(CVDate);
            var disbursements = from d in db.TrnDisbursements
                                where d.CVDate == disbursement_CVDate
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

        // =================================
        // GET last CVNumber in Disbursement
        // =================================
        [Route("api/disbursementLastCVNumber")]
        public Models.TrnDisbursement GetDisbusementLastCVNumber()
        {
            var disbursements = from d in db.TrnDisbursements.OrderByDescending(d => d.CVNumber)
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
            return (Models.TrnDisbursement)disbursements.FirstOrDefault();
        }

        // ===========================
        // GET last Id in Disbursement
        // ===========================
        [Route("api/disbursementLastId")]
        public Models.TrnDisbursement GetDisbusementLastId()
        {
            var disbursements = from d in db.TrnDisbursements.OrderByDescending(d => d.Id)
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
            return (Models.TrnDisbursement)disbursements.FirstOrDefault();
        }


        // ================
        // ADD Disbursement
        // ================
        [Route("api/addDisbursement")]
        public int Post(Models.TrnDisbursement disbursement)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.TrnDisbursement newDisbursement = new Data.TrnDisbursement();

                newDisbursement.BranchId = disbursement.BranchId;
                newDisbursement.CVNumber = disbursement.CVNumber;
                newDisbursement.CVDate = Convert.ToDateTime(disbursement.CVDate);
                newDisbursement.SupplierId = disbursement.SupplierId;
                newDisbursement.Payee = disbursement.Payee;
                newDisbursement.PayTypeId = disbursement.PayTypeId;
                newDisbursement.BankId = disbursement.BankId;
                newDisbursement.ManualCVNumber = disbursement.ManualCVNumber;
                newDisbursement.Particulars = disbursement.Particulars;
                newDisbursement.CheckNumber = disbursement.CheckNumber;
                newDisbursement.CheckDate = Convert.ToDateTime(disbursement.CheckDate);
                newDisbursement.Amount = disbursement.Amount;
                newDisbursement.IsCrossCheck = disbursement.IsCrossCheck;
                newDisbursement.IsClear = disbursement.IsClear;
                newDisbursement.PreparedById = disbursement.PreparedById;
                newDisbursement.CheckedById = disbursement.CheckedById;
                newDisbursement.ApprovedById = disbursement.ApprovedById;

                newDisbursement.IsLocked = isLocked;
                newDisbursement.CreatedById = mstUserId;
                newDisbursement.CreatedDateTime = date;
                newDisbursement.UpdatedById = mstUserId;
                newDisbursement.UpdatedDateTime = date;

                db.TrnDisbursements.InsertOnSubmit(newDisbursement);
                db.SubmitChanges();

                return newDisbursement.Id;

            }
            catch
            {
                return 0;
            }
        }

        // ===================
        // UPDATE Disbursement
        // ===================
        [Route("api/updateDisbursement/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnDisbursement disbursement)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var disbursement_Id = Convert.ToInt32(id);
                var disbursements = from d in db.TrnDisbursements where d.Id == disbursement_Id select d;

                if (disbursements.Any())
                {
                    var updateDisbursement = disbursements.FirstOrDefault();

                    updateDisbursement.BranchId = disbursement.BranchId;
                    updateDisbursement.CVNumber = disbursement.CVNumber;
                    updateDisbursement.CVDate = Convert.ToDateTime(disbursement.CVDate);
                    updateDisbursement.SupplierId = disbursement.SupplierId;
                    updateDisbursement.Payee = disbursement.Payee;
                    updateDisbursement.PayTypeId = disbursement.PayTypeId;
                    updateDisbursement.BankId = disbursement.BankId;
                    updateDisbursement.ManualCVNumber = disbursement.ManualCVNumber;
                    updateDisbursement.Particulars = disbursement.Particulars;
                    updateDisbursement.CheckNumber = disbursement.CheckNumber;
                    updateDisbursement.CheckDate = Convert.ToDateTime(disbursement.CheckDate);
                    updateDisbursement.Amount = disbursement.Amount;
                    updateDisbursement.IsCrossCheck = disbursement.IsCrossCheck;
                    updateDisbursement.IsClear = disbursement.IsClear;
                    updateDisbursement.PreparedById = disbursement.PreparedById;
                    updateDisbursement.CheckedById = disbursement.CheckedById;
                    updateDisbursement.ApprovedById = disbursement.ApprovedById;

                    updateDisbursement.IsLocked = disbursement.IsLocked;
                    updateDisbursement.UpdatedById = mstUserId;
                    updateDisbursement.UpdatedDateTime = date;

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


        // ===================
        // DELETE Disbursement
        // ===================
        [Route("api/deleteDisbursement/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var disbursement_Id = Convert.ToInt32(id);
                var disbursements = from d in db.TrnDisbursements where d.Id == disbursement_Id select d;

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
