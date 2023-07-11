using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using ShopBase;
using System.Threading;
using System.Threading.Tasks;
using WebApp;

namespace WebApp.Pages
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        public string? EMail { get; set; }
        public string? HashedPwd { get; set; }
        public string? ErrorMessage { get; set; }
        public Order? sessionBasket { get; set; }

        public void OnGet()
        {

        }

        public void OnPostCheck()
        {
            sessionBasket = HttpContext.Session.GetObject<Order>("sessionBasket");
            //Iam not sure how long the js needs to compute hash so it should wait 10ms before requesting the value
            Thread.Sleep(10);
            HashedPwd = Request.Form["HashedPwd"].ToString();
            int customerId = -1;

            if (EMail != null && HashedPwd != null)
            {
                customerId = Customer.Login(EMail, HashedPwd);
            }

            // Customer unique Main and hashed js pwd is found
            if (customerId != -1)
            {

                sessionBasket ??= new Order();
                sessionBasket.Customer = Customer.Get(customerId);

                if (sessionBasket.Positions == null && sessionBasket.Customer.Id == 1)
                {
                    Order? o = Order.GetAllFromCustomer(sessionBasket.Customer.Id).Find(x => x.Status == Status.Warenkorb);

                    if (o != null && o.Customer != null)
                    {
                        sessionBasket = o;
                    }
                }

                // New sample data for Tina test

                // Back to Orderoview

                // Save the new basket in session
                HttpContext.Session.SetObject("sessionBasket", sessionBasket);

                Response.Redirect("/Basket");
            }
            else
            {
                ErrorMessage = "Da ist etwas falsch";
            }
        }

        public void OnPostLogout()
        {
            sessionBasket = new();
            sessionBasket.Customer = null;
            HttpContext.Session.SetObject("sessionBasket", sessionBasket);
        }

    }
}
