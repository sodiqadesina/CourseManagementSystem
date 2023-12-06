using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;

public abstract class AbsractBaseController : Controller
{
    //iniitalizing the context accessor 
    private readonly IHttpContextAccessor _contextAccessor;

    protected AbsractBaseController(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }


    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        SetFirstVisitCookie();
    }

    protected void SetHomeMessage()
    {
        ViewBag.WelcomeMessage = "Hello, welcome to the Course Manager App";
    }



    protected void SetFirstVisitCookie()
    {
        var cookies = _contextAccessor.HttpContext.Request.Cookies;
        if (!cookies.ContainsKey("FirstVisit"))
        {
            var firstVisit = DateTime.UtcNow;
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddYears(1),
                HttpOnly = true,
                Secure = true
            };
            _contextAccessor.HttpContext.Response.Cookies.Append("FirstVisit", JsonConvert.SerializeObject(firstVisit), cookieOptions);
            ViewBag.WelcomeMessage = "Welcome to your first visit!";
        }
        else
        {
            var firstVisit = JsonConvert.DeserializeObject<DateTime>(cookies["FirstVisit"]);
            ViewBag.WelcomeMessage = $"Welcome back! You started using the app on {firstVisit.ToLocalTime():dd MMM yyyy 'at' HH:mm}.";
        }
    }
}
