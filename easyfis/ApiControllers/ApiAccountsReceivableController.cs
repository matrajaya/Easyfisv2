using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiAccountsReceivableController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        public Decimal computeAge(Int32 Age, Int32 Elapsed, Decimal Amount)
        {
            Decimal returnValue = 0;

            if (Age == 0)
            {
                if (Elapsed < 30)
                {
                    returnValue = Amount;
                }
            }
            else if (Age == 1)
            {
                if (Elapsed >= 30 && Elapsed < 60)
                {
                    returnValue = Amount;
                }
            }
            else if (Age == 2)
            {
                if (Elapsed >= 60 && Elapsed < 90)
                {
                    returnValue = Amount;
                }
            }
            else if (Age == 3)
            {
                if (Elapsed >= 90 && Elapsed < 120)
                {
                    returnValue = Amount;
                }
            }
            else if (Age == 4)
            {
                if (Elapsed >= 120)
                {
                    returnValue = Amount;
                }
            }
            else
            {
                returnValue = 0;
            }

            return returnValue;
        }

        // accounts receivable list
        [HttpGet]
        [Route("api/accountsReceivable/list/{dateAsOf}/{companyId}/")]
        public List<Models.TrnSalesInvoice> accountsReceivableList(String dateAsOf, String companyId)
        {
            try
            {
                var salesInvoice = from d in db.TrnSalesInvoices
                                   where d.SIDate <= Convert.ToDateTime(dateAsOf)
                                   && d.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                   && d.BalanceAmount > 0
                                   && d.IsLocked == true
                                   select new Models.TrnSalesInvoice
                                   {
                                       Id = d.Id,
                                       AccountId = d.MstArticle.AccountId,
                                       AccountCode = d.MstArticle.MstAccount.AccountCode,
                                       Account = d.MstArticle.MstAccount.Account,
                                       SINumber = d.SINumber,
                                       SIDate = d.SIDate.ToShortDateString(),
                                       DocumentReference = d.DocumentReference,
                                       BalanceAmount = d.BalanceAmount,
                                       CustomerId = d.CustomerId,
                                       Customer = d.MstArticle.Article,
                                       DueDate = d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays)).ToShortDateString(),
                                       NumberOfDaysFromDueDate = Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days,
                                       CurrentAmount = computeAge(0, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                       Age30Amount = computeAge(1, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                       Age60Amount = computeAge(2, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                       Age90Amount = computeAge(3, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                       Age120Amount = computeAge(4, Convert.ToDateTime(dateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount)
                                   };

                return salesInvoice.ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
