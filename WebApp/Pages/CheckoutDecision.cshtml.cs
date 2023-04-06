using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class CheckOutNonCustomer1Model : PageModel
    {
        public void OnGet()
        {
        }

        public void OnPostToLogin()
        {
            HttpContext.Response.Redirect("/Login");
        }

        public void OnPostToRegister()
        {
            HttpContext.Response.Redirect("/NonCustomerDataEntry");
        }
    }
}
