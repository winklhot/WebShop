using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ShopBase;
using System;
using WebApp;

namespace WebApp.Pages
{
    [BindProperties]
    public class NonCustomer2Model : PageModel
    {
        public string Firstname { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Gender { get; set; } = "male";
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string AGBsCheck { get; set; }
        public string ResultText { get; set; }

        public void OnGet()
        {
        }

        public void OnPostCheck()
        {
            if (Convert.ToBoolean(AGBsCheck))
            {
                try
                {
                    Gender gender = (Gender)Enum.Parse(typeof(Gender), Gender);

                    NonCustomer n = new NonCustomer(EMail, Firstname, Name, gender, Adress.GetOrCreateIfNotExists(Street, HouseNumber, PostalCode, City, Country));

                    n.Insert();

                    Order sessionBasket = HttpContext.Session.GetObject<Order>("sessionBasket");

                    sessionBasket.NonCustomer = n;

                    sessionBasket.Insert();


                    HttpContext.Session.SetObject("sessionBasket", sessionBasket);


                    Response.Redirect("/Basket");
                }
                catch (Exception)
                {
                    ResultText = "Es ist ein Fehler aufgetreten - bitte Felder überprüfen";
                }
            }
            else
            {
                ResultText = "Es ist ein Fehler aufgetreten - bitte Felder überprüfen";
            }
        }


    }
}
