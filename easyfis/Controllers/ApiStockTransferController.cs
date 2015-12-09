using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiStockTransferController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===================
        // LIST Stock Transfer
        // ===================
        [Route("api/listStockTransfer")]
        public List<Models.TrnStockTransfer> Get()
        {
            var stockTransfer = from d in db.TrnStockTransfers
                                select new Models.TrnStockTransfer
                                {
                                    Id = d.Id,
                                    //Userid = d.Userid,
                                    //User = d.User,
                                    //Audidate = d.Audidate,
                                    TableInformation = d.TableInformation,
                                    RecordInformation = d.RecordInformation,
                                    FormInformation = d.FormInformation,
                                    ActionInformation = d.ActionInformation
                                };
            return stockTransfer.ToList();
        }
    }
}
