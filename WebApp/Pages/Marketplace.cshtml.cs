using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopBase;
using System.Web;
using System.IO;
using WebApp;

namespace WebApp.Pages
{
    [BindProperties]
    public class Index : PageModel
    {
        public List<Article> lArtikel { get; set; }
        public string Suchbegriff { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            lArtikel ??= Article.GetAll();
            Order sessionBasket = HttpContext.Session.GetObject<Order>("sessionBasket") ?? new Order();
            sessionBasket.Status = Status.Warenkorb;

            HttpContext.Session.SetObject("sessionBasket", sessionBasket);
        }

        public void OnPostSuche()
        {
            lArtikel = Article.GetAll();

            if (Suchbegriff != null)
            {
                lArtikel = lArtikel.FindAll(a => a.Name.ToLower().Trim().Contains(Suchbegriff.ToLower()) || a.Description.ToLower().Trim().Contains(Suchbegriff.ToLower()) || a.Id.ToString() == Suchbegriff);
                
                // First Element when article number matches
                if (int.TryParse(Suchbegriff, out int id))
                {
                    Article a = lArtikel.Find(item => item.Id == id);

                    if (a != null)
                    {
                        lArtikel.Remove(a);

                        lArtikel.Insert(0, a);
                    }

                }
            }
        }

        public void OnPostAdd(int id, int menge)
        {
            if (menge <= Article.Get(id).Count)
            {
                Order sessionBasket = HttpContext.Session.GetObject<Order>("sessionBasket");
                sessionBasket.AddPosition(menge, id);
                HttpContext.Session.SetObject("sessionBasket", sessionBasket);
                Response.Redirect("/Basket");
            }
            else
            {
                ErrorMessage = $"Die gewünschte Anzahl ist nicht auf Lager ({Article.Get(id).Name})";
                Suchbegriff = Article.Get(id).Name;
                OnPostSuche();
            }

        }

    }
}
