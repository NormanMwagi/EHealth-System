﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SHERIA.Models;

public class RBACAttribute : ActionFilterAttribute
{
    public DBHandler dbhandler { get; set; }

    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        //Create permission string based on the requested controller name and action name in the format 'controllername-action'
        string requiredPermission = String.Format("{0}-{1}", filterContext.RouteData.Values["Controller"], filterContext.RouteData.Values["action"]);

        //Create an instance of our custom user authorization object passing requesting user's 'Windows Username' into constructor
        RBACUser requestingUser = new RBACUser(filterContext.HttpContext.Session.GetString("email")!);
        //Check if the requesting user has the permission to run the controller's action
        if (!requestingUser.HasPermission(requiredPermission) & !requestingUser.IsSysAdmin)
        {
            //User doesn't have the required permission and is not a SysAdmin, return our custom “401 Unauthorized” access error
            //Since we are setting filterContext.Result to contain an ActionResult page, the controller's action will not be run.
            //The custom “401 Unauthorized” access error will be returned to the browser in response to the initial request.
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "Unauthorised" } });
        }
        //If the user has the permission to run the controller's action, then filterContext.Result will be uninitialized and
        //executing the controller's action is dependant on whether filterContext.Result is uninitialized.
    }
}
