using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.DAOs;

namespace SWP391_Group3_FinalProject.Filter
{
    public class CustomerFilter : IActionFilter
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


                if (int.TryParse(context.HttpContext.Request.Cookies["role"], out int cookieValue))
                {
                    if (cookieValue == 1)
                    {
                        // Redirect to Dashboard/Index if role is 1
                        context.Result = new RedirectToActionResult("Index", "Dashboard", null);
                    }
                    else if (cookieValue == 0)
                    {
                        var username = context.HttpContext.Request.Cookies["username"];
                        AccountDAO accDAO = new AccountDAO();
                        context.HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(accDAO.GetCustomerByUsername(username)));

                    }
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
