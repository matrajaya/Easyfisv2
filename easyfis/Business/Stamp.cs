//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace easyfis.Business
//{
//    public class Stamp
//    {

//            public MstSecurityUser getCurrentUser() {
//                User user = (User)SecurityContextHolder.getContext().getAuthentication().getPrincipal();
//                String login = user.getUsername(); 
//                return userDao.getUser(login);
//            }

//            public Object stampCreated(Object object) {
//                try {
//                    this.set(object, "CREATED_DATE", new Date());
//                    this.set(object, "CREATED_BY_USER_ID", this.getCurrentUser().getUSER_ID());
//                    this.set(object, "UPDATED_DATE", new Date());
//                    this.set(object, "UPDATED_BY_USER_ID", this.getCurrentUser().getUSER_ID());
//                    this.set(object, "ISDELETED", 0);
//                } catch(Exception e) {
//                }
    	
//                return object;
//            }

//            public Object stampUpdated(Object object) {
//                try {
//                    this.set(object, "UPDATED_DATE", new Date());
//                    this.set(object, "UPDATED_BY_USER_ID", this.getCurrentUser().getUSER_ID());
//                    this.set(object, "ISDELETED", 0);
//                } catch(Exception e) {
//                }
//                return object;
//            }

//            public Object stampDeleted(Object object){
//                try {
//                    this.set(object, "UPDATED_DATE", new Date());
//                    this.set(object, "UPDATED_BY_USER_ID", this.getCurrentUser().getUSER_ID());
//                    this.set(object, "ISDELETED", 1);
//                    this.set(object, "ISDELETED_DATE", new Date());
//                    this.set(object, "ISDELETED_BY_USER_ID", this.getCurrentUser().getUSER_ID());
//                } catch(Exception e) {
//                }
//                return object;
//            }
//        }


//    }
//}