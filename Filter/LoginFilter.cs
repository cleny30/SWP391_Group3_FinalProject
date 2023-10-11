using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.DAOs;

namespace SWP391_Group3_FinalProject.Filter
{
    public class LoginFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                //Get action (1. manager, 0. customer)
                var serializedAction = context.HttpContext.Session.GetString("action");
                int? action = null;
                if (!string.IsNullOrEmpty(serializedAction))
                {
                    action = JsonConvert.DeserializeObject<int>(serializedAction);
                    // Use the deserialized integer 'action' here
                }


                if (action == null)
                {
                    context.Result = new RedirectToActionResult("Index", "Login", null);
                }
                else if (action == 1) // If manager != null means that is admin/staff therefore redirect to dashboard
                {
                    context.Result = new RedirectToActionResult("Index", "Dashboard", null);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
