using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuizApplication.Filters
{
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private string _allowedUser;
        public AuthorizationAttribute(string allowedUser) 
        {
            _allowedUser = allowedUser;
        }
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var sessionUser = filterContext.HttpContext.Session;
            if(sessionUser == null || sessionUser.GetString("user_type")!= _allowedUser.ToString())
            {
                filterContext.Result = new RedirectToActionResult("AuthorizationError", "Home", null);
            }
        }
    }
}
