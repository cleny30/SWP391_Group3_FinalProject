using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.DAOs;

namespace SWP391_Group3_FinalProject.Filter
{
    public class CheckOutFilter: IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                string getCountItemInCart = context.HttpContext.Session.GetString("Count");

                var count = JsonConvert.DeserializeObject<int>(getCountItemInCart);
                if (count == 0)
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
