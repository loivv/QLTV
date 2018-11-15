using LB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LB.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MyValidateAccess : ActionFilterAttribute
    {

        ssofty_com_thuvienEntities db = new ssofty_com_thuvienEntities();

        public string code { get; set; }

        public int edit { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string userName = HttpContext.Current.User.Identity.Name;

                var findUser = db.AspNetUsers.Where(p => p.UserName == userName).FirstOrDefault();

                if (findUser == null || findUser.UType == "SADMIN")
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult()
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = new ResultInfo()
                            {
                                error = 1,
                                msg = "Bạn không có quyền truy cập"
                            }
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/error/relogin");
                    }
                    return;
                }

                // neu là admin thi duoc vao
                if (findUser.UType == "ADMIN")
                {
                    base.OnActionExecuting(filterContext);
                    return;
                }

                var groupId = findUser.UserGroup;

                var getAccess = db.USER_CHECKACCESS(groupId, code).FirstOrDefault();

                if (getAccess == null)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult()
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = new ResultInfo()
                            {
                                error = 1,
                                msg = "Bạn không có quyền truy cập"
                            }
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/error/relogin");
                    }
                    return;
                }

                if (getAccess.CanEdit == edit)
                {
                    base.OnActionExecuting(filterContext);
                    return;
                }
            }

        }
    }
}