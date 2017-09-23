using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace easyfis.Controllers
{
    public class ApiUserController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // api for innosoft website list user
        [HttpGet]
        [Route("api/user/list")]
        public List<Models.MstUser> listUsers()
        {
            var users = from d in db.MstUsers
                        select new Models.MstUser
                        {
                            Id = d.Id,
                            FullName = d.FullName,
                            UserName = d.UserName,
                        };

            return users.ToList();
        }

        // list ASP User
        [Authorize]
        [HttpGet]
        [Route("api/listAspUser")]
        public List<Models.ApplicationUser> listAspUser()
        {
            var users = from d in db.AspNetUsers
                        select new Models.ApplicationUser
                        {
                            Id = d.Id,
                            FullName = d.FullName,
                            UserName = d.UserName
                        };

            return users.ToList();
        }

        // list MstUser
        [Authorize]
        [HttpGet]
        [Route("api/listUser")]
        public List<Models.MstUser> listUser()
        {
            var users = from d in db.MstUsers
                        select new Models.MstUser
                        {
                            Id = d.Id,
                            FullName = d.FullName,
                            UserName = d.UserName,
                            IsLocked = d.IsLocked,
                            UserId = d.UserId,
                            //CreatedById = d.CreatedById,
                            //CreatedBy = d.MstUser1.FullName,
                            //CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                            //UpdatedById = d.UpdatedById,
                            //UpdatedBy = d.MstUser2.FullName,
                            //UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                        };

            return users.ToList();
        }

        // get MstUser by Id
        [Authorize]
        [HttpGet]
        [Route("api/listUserById/{Id}")]
        public Models.MstUser getMstUserById(String Id)
        {
            var users = from d in db.MstUsers
                        where d.Id == Convert.ToInt32(Id)
                        select new Models.MstUser
                        {
                            Id = d.Id,
                            FullName = d.FullName,
                            UserName = d.UserName,
                            IsLocked = d.IsLocked,
                            UserId = d.UserId,
                            CompanyId = d.CompanyId,
                            Company = d.MstCompany.Company,
                            BranchId = d.BranchId,
                            Branch = d.MstBranch.Branch,
                            IncomeAccountId = d.IncomeAccountId,
                            IncomeAccount = d.MstAccount.Account,
                            SupplierAdvancesAccountId = d.SupplierAdvancesAccountId,
                            SupplierAdvancesAccount = d.MstAccount1.Account,
                            CustomerAdvancesAccountId = d.CustomerAdvancesAccountId,
                            CustomerAdvancesAccount = d.MstAccount2.Account,
                            OfficialReceiptName = d.OfficialReceiptName,
                            InventoryType = d.InventoryType,
                            DefaultSalesInvoiceDiscountId = d.DefaultSalesInvoiceDiscountId,
                            SalesInvoiceName = d.SalesInvoiceName
                            //CreatedById = d.CreatedById,
                            //CreatedBy = d.MstUser1.FullName,
                            //CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                            //UpdatedById = d.UpdatedById,
                            //UpdatedBy = d.MstUser2.FullName,
                            //UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                        };

            return (Models.MstUser)users.FirstOrDefault();
        }

        // get MstUser by Username
        [Authorize]
        [HttpGet]
        [Route("api/listByMstUserId/{userId}")]
        public Models.MstUser getUserByUsername(String userId)
        {
            var mstUserId = (from d in db.MstUsers where d.UserId == userId select d.Id).FirstOrDefault();

            var users = from d in db.MstUsers
                        where d.Id == mstUserId
                        select new Models.MstUser
                        {
                            Id = d.Id,
                            FullName = d.FullName,
                            UserName = d.UserName,
                            IsLocked = d.IsLocked
                        };

            return (Models.MstUser)users.FirstOrDefault();
        }

        // update ASP User
        [Authorize]
        [HttpPut]
        [Route("api/updateAspUser/{id}")]
        public HttpResponseMessage updateAspUser(String id, Models.ApplicationUser aspUser)
        {
            try
            {
                var aspUsers = from d in db.AspNetUsers where d.Id == id select d;
                if (aspUsers.Any())
                {
                    var updateAspUsers = aspUsers.FirstOrDefault();
                    updateAspUsers.FullName = aspUser.FullName;

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

        // update Mst User
        [Authorize]
        [HttpPut]
        [Route("api/updateMstUser/{id}")]
        public HttpResponseMessage updateMstUser(String id, Models.MstUser mstUser)
        {
            try
            {
                var aspNetUSer = from d in db.AspNetUsers where d.Id == id select d;
                if (aspNetUSer.Any())
                {
                    var mstUsers = from d in db.MstUsers where d.UserId == id select d;
                    if (mstUsers.Any())
                    {
                        var updateAspNetUSer = aspNetUSer.FirstOrDefault();
                        updateAspNetUSer.FullName = mstUser.FullName;
                        db.SubmitChanges();

                        var updateMstUsers = mstUsers.FirstOrDefault();
                        updateMstUsers.FullName = mstUser.FullName;
                        updateMstUsers.IsLocked = true;

                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
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

        // update User
        [Authorize]
        [HttpPut]
        [Route("api/updateUser/{id}")]
        public HttpResponseMessage updateUser(String id, Models.MstUser mstUser)
        {
            try
            {
                var mstUsers = from d in db.MstUsers where d.Id == Convert.ToInt32(id) select d;

                var userId = (from d in db.MstUsers where d.Id == Convert.ToInt32(id) select d.UserId).SingleOrDefault();
                var aspUsers = from d in db.AspNetUsers where d.Id == userId select d;

                if (mstUsers.Any())
                {
                    var updateMstUsers = mstUsers.FirstOrDefault();
                    updateMstUsers.FullName = mstUser.FullName;
                    updateMstUsers.IsLocked = true;
                    updateMstUsers.CompanyId = mstUser.CompanyId;
                    updateMstUsers.BranchId = mstUser.BranchId;
                    updateMstUsers.IncomeAccountId = mstUser.IncomeAccountId;
                    updateMstUsers.SupplierAdvancesAccountId = mstUser.SupplierAdvancesAccountId;
                    updateMstUsers.CustomerAdvancesAccountId = mstUser.CustomerAdvancesAccountId;
                    updateMstUsers.OfficialReceiptName = mstUser.OfficialReceiptName;
                    updateMstUsers.InventoryType = mstUser.InventoryType;
                    updateMstUsers.DefaultSalesInvoiceDiscountId = mstUser.DefaultSalesInvoiceDiscountId;
                    updateMstUsers.SalesInvoiceName = mstUser.SalesInvoiceName;
                    db.SubmitChanges();

                    if (aspUsers.Any())
                    {
                        var updateAspUsers = aspUsers.FirstOrDefault();
                        updateAspUsers.FullName = mstUser.FullName;

                        db.SubmitChanges();
                    }

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

        // unlock user
        [Authorize]
        [HttpPut]
        [Route("api/unlockUser/{id}")]
        public HttpResponseMessage unlockUser(String id, Models.MstUser mstUser)
        {
            try
            {
                var mstUsers = from d in db.MstUsers where d.Id == Convert.ToInt32(id) select d;
                if (mstUsers.Any())
                {
                    var updateMstUsers = mstUsers.FirstOrDefault();
                    updateMstUsers.IsLocked = false;

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

        // update defaults
        [Authorize]
        [HttpPut]
        [Route("api/user/updateUserDefaults/byUserId/{userId}")]
        public HttpResponseMessage updateUserDefaults(String userId, Models.MstUser mstUser)
        {
            try
            {
                var userDefaults = from d in db.MstUsers where d.UserId == userId select d;
                if (userDefaults.Any())
                {
                    var updateUserDefaults = userDefaults.FirstOrDefault();
                    updateUserDefaults.BranchId = mstUser.BranchId;
                    updateUserDefaults.OfficialReceiptName = mstUser.OfficialReceiptName;
                    updateUserDefaults.DefaultSalesInvoiceDiscountId = mstUser.DefaultSalesInvoiceDiscountId;
                    updateUserDefaults.SalesInvoiceName = mstUser.SalesInvoiceName;

                    var inventory = from d in db.TrnInventories
                                    select d;

                    if (!inventory.Any())
                    {
                        updateUserDefaults.InventoryType = mstUser.InventoryType;
                    }

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
