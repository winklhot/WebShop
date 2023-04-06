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
        public Order sessionBasket { get; set; }
        public bool OrderSubmitted { get; set; } = false;
        public string ResultMessage { get; set; }
        public string OrderNumber { get; set; }
        public NonCustomer Customer { get; set; }

        public void OnGet()
        {
            sessionBasket = HttpContext.Session.GetObject<Order>("sessionBasket");

            sessionBasket = sessionBasket.MergeBaskets();

            HttpContext.Session.SetObject("sessionBasket", sessionBasket);
        }

        public void OnPostDelete(int id)
        {
            sessionBasket = HttpContext.Session.GetObject<Order>("sessionBasket");
            sessionBasket.Positions.RemoveAt(id);
            HttpContext.Session.SetObject("sessionBasket", sessionBasket);
        }

        public void OnPostOrder()
        {
            sessionBasket = HttpContext.Session.GetObject<Order>("sessionBasket");

            if (sessionBasket != null && sessionBasket.Positions.Count > 0 && (sessionBasket.Customer != null || sessionBasket.NonCustomer != null))
            {

                sessionBasket.Status = ShopBase.Status.Bestellt;
                sessionBasket.Change();
                Customer = sessionBasket.Customer ?? sessionBasket.NonCustomer;
                OrderNumber = sessionBasket.Id.ToString();
                sessionBasket.Positions = sessionBasket.Positions;

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
