using easyfis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace easyfis.Business
{
    public class Stamp : Controller
    {
        public Boolean set(Object obj, String fieldName, Object fieldValue) 
        {
	        return false;
        }

        public Object stampCreated(Object stampCreatedObject)
        {
            var userId = User.Identity.GetUserId();
            var username = User.Identity.Name;

            this.set(stampCreatedObject, "CreatedById", userId);
            this.set(stampCreatedObject, "CreatedDateTime", new DateTime());
            this.set(stampCreatedObject, "UpdatedById", userId);
            this.set(stampCreatedObject, "UpdatedDateTime", new DateTime());

            return stampCreatedObject;
        }

        public Object stampUpdated(Object stampUpdatedObject)
        {
            var userId = User.Identity.GetUserId();
            var username = User.Identity.Name;

            this.set(stampUpdatedObject, "UpdatedById", userId);
            this.set(stampUpdatedObject, "UpdatedDateTime", new DateTime());

            return stampUpdatedObject;
        }
    }

}