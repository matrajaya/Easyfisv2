//using easyfis.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Http;
//using System.Web.Mvc;
//using Microsoft.AspNet.Identity;
//using System.Reflection;

//namespace easyfis.Business
//{
//    public class StampSecurity : Controller
//    {
//        public Boolean set(Object obj, String fieldName, Object fieldValue) 
//        {
//            var objClass = obj.GetType();
//            if (objClass != null)
//            {
//                try
//                {
//                    // Get the PropertyInfo object:
//                    var properties = objClass.GetProperty(fieldName);
//                    properties.SetValue(obj, fieldValue);
//                    return true; 
//                }
//                catch 
//                {
//                    return false;
//                }
//            }
           
//            return false;
//        }

//        public Object stampCreated(Object obj)
//        {
//            var userId = User.Identity.GetUserId();
//            var username = User.Identity.Name;

//            try
//            {
//                this.set(obj, "CreatedById", userId);
//                this.set(obj, "CreatedDateTime", new DateTime());
//                this.set(obj, "UpdatedById", userId);
//                this.set(obj, "UpdatedDateTime", new DateTime());
//            }
//            catch 
//            {

//            }
//            return obj;
//        }

//        public Object stampUpdated(Object obj)
//        {
//            var userId = User.Identity.GetUserId();
//            var username = User.Identity.Name;

//            try
//            {
//                this.set(obj, "UpdatedById", userId);
//                this.set(obj, "UpdatedDateTime", new DateTime());
//            }
//            catch
//            {

//            }
//            return obj;
//        }
//    }

//}