using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Models;

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

                string get = context.HttpContext.Session.GetString("Session");

                Customer cus = JsonConvert.DeserializeObject<Customer>(get);

                if (cus.addresses == null || cus.addresses.Count()==0)
                {
                    context.Result = new RedirectToActionResult("MyAddress", "Account", null);

                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
