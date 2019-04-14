using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PracticeWeb1.Entities;
using PracticeWeb1.Models;

namespace PracticeWeb1.Custom_Authorization
{
    public class roleAuth : AuthorizeAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //public string AccessLevel { get; set; }
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    var isAuthorized = base.AuthorizeCore(httpContext);
        //    if (!isAuthorized)
        //    {
        //        return false;
        //    }

        //    string roleName = db.tblRoles.Where(x => x.Id.Equals(1)).Single().Name;

        //    string privilegeLevels = string.Join("", GetUserRights(roleName)); // Call another method to get rights of the user from DB

        //    return privilegeLevels.Contains(AccessLevel);
        //}

        //protected override bool AuthorizeCore

        //private string GetUserRights(string v)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    filterContext.Result = new RedirectToRouteResult(
        //                new RouteValueDictionary(
        //                    new
        //                    {
        //                        controller = "UserAccount",
        //                        action = "Login"
        //                    })
        //                );
        //}


        private readonly string[] allowedroles;

        public roleAuth(params string[] roles)
        {
            allowedroles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;

            if (httpContext.Session["role"] != null)
            {
                if (httpContext.Session["role"].ToString() == "admin")
                {
                    authorize = true; /* return true if Entity has current user(active) with specific role */
                }
            }

            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "UserAccount",
                                action = "Login",
                                Area = ""
                            })
                        );
        }

    }
}