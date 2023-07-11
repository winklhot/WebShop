using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ShopBase;
using WebApp;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Generic;

namespace WebApp.Pages
{
    [BindProperties]
    public class sessionBasketModel : PageModel
    {
        public Order? sessionBasket { get; set; }
        public bool OrderSubmitted { get; set; } = false;
        public string? ResultMessage { get; set; }

        public void OnGet()
        {
            sessionBasket = HttpContext.Session.GetObject<Order>("sessionBasket") ?? new Order();


            if (sessionBasket != null)
            {
                sessionBasket = sessionBasket.MergeBaskets();
                HttpContext.Session.SetObject("sessionBasket", sessionBasket);
            }


            HttpContext.Session.SetObject("lastUrl", "Basket");
        }

        public void OnPostDelete(int id)
        {
            sessionBasket = HttpContext.Session.GetObject<Order>("sessionBasket");



            if (sessionBasket != null && sessionBasket.Positions != null && sessionBasket.Positions.Count != 0)
            {
                Position? p = null;
                if (sessionBasket != null)
                {
                    p = sessionBasket.Positions[id];

                    if (sessionBasket.Customer != null)
                    {
                        sessionBasket.DeletePosition(p);
                        sessionBasket = Order.GetAllFromCustomer(sessionBasket.Customer.Id).Find(x => x.Status == Status.Warenkorb);
                    }
                    else
                    {
                        if (sessionBasket != null && sessionBasket.Positions != null && p != null)
                            sessionBasket.Positions.Remove(p);
                    }

                    if (sessionBasket != null)
                        HttpContext.Session.SetObject("sessionBasket", sessionBasket);
                }

            }

            sessionBasket = HttpContext.Session.GetObject<Order>("sessionBasket");
        }

        public void OnPostOrder()
        {
            sessionBasket = HttpContext.Session.GetObject<Order>("sessionBasket");

            if (sessionBasket != null && sessionBasket.Positions != null && sessionBasket.Positions.Count > 0 && (sessionBasket.Customer != null || sessionBasket.NonCustomer != null))
            {

                sessionBasket.Status = ShopBase.Status.Bestellt;
                if (sessionBasket.Customer != null)
                {
                    sessionBasket.Change();
                    sessionBasket.Positions = sessionBasket.Positions;

                }
                else
                {
                    sessionBasket.Insert();
                }

                OrderSubmitted = true;

            }
            else
            {
                ResultMessage = "Der sessionBasket ist leer - bitte im Marktplatz Artikel auswählen";
            }
        }

        public void OnPostCashier()
        {
            sessionBasket = HttpContext.Session.GetObject<Order>("sessionBasket");

            if (sessionBasket.Positions.Count > 0)
            {
                Response.Redirect("/CheckoutDecision");
            }
            else
            {
                ResultMessage = "Der sessionBasket ist leer - bitte im Marktplatz Artikel auswählen oder einloggen";
            }


        }

        public void OnPostBack()
        {
            Response.Redirect("/Marketplace");
        }

    }
}
