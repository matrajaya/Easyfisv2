using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace easyfis.ModifiedApiControllers
{
    public class ApiTrnSalesInvoiceController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==================
        // List Sales Invoice
        // ==================
        //[Authorize, HttpGet, Route("api/salesInvoice/list/{startDate}/{endDate}")]
        //public List<Entities.TrnSalesInvoice> ListSalesInvoice(String startDate, String endDate)
        //{
        //    var currentUser = from d in db.MstUsers
        //                      where d.UserId == User.Identity.GetUserId()
        //                      select d;

        //    var branchId = currentUser.FirstOrDefault().BranchId;

        //    var salesInvoices = from d in db.TrnSalesInvoices.OrderByDescending(d => d.Id)
        //                        where d.BranchId == branchId
        //                        && d.SIDate >= Convert.ToDateTime(startDate)
        //                        && d.SIDate <= Convert.ToDateTime(endDate)
        //                        select new Entities.TrnSalesInvoice
        //                        {
        //                            Id = d.Id,
        //                            SINumber = d.SINumber,
        //                            SIDate = d.SIDate.ToShortDateString(),
        //                            Customer = d.MstArticle.Article,
        //                            Remarks = d.Remarks,
        //                            DocumentReference = d.DocumentReference,
        //                            Amount = d.Amount,
        //                            IsLocked = d.IsLocked,
        //                            CreatedBy = d.MstUser2.FullName,
        //                            CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
        //                            UpdatedBy = d.MstUser5.FullName,
        //                            UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
        //                        };

        //    return salesInvoices.ToList();
        //}
    }
}
