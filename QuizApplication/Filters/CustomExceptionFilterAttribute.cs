using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuizApplication.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            // Determine the type of exception
            string exceptionType = context.Exception.GetType().Name;

            context.ExceptionHandled = true; // Mark the exception as handled
            context.Result = new RedirectToActionResult("ExceptionError", "Home", new { errorMessage = $"An error of type {exceptionType} occurred." });
        }
    }
}


