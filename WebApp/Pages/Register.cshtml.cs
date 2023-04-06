using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using ShopBase;

namespace WebApp.Pages
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        public string Firstname { get; set; }
        public string Gender { get; set; } = "male";
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; } 
        public string Password { get; set; }
        public string Password2 { get; set; }
        public string IsChecked { get; set; }
        public string ResultMessage { get; set; }



        public void OnGet()
        {
        }

        public void OnPostRegister()
        {
            if (Convert.ToBoolean(IsChecked))
            {
                if (Password == Password2)
                {
                    try
                    {
                        Gender gender = (Gender)Enum.Parse(typeof(Gender), Gender);
                                                                                             // Gets existing Adress or insert and get the new one
                        Customer c = new Customer(EMail, Password, Firstname, Name, gender, Adress.GetOrCreateIfNotExists(Street, HouseNumber, PostalCode, City, Country));

                        c.Insert();

                        ResultMessage = $"{Firstname} {Name} wurde erfolgreich registriert";


                    }
                    catch (Exception)
                    {
                        ResultMessage = "Fehler - bitte alle Felder überprüfen";
                    }
                }
                else
                {
                    ResultMessage = "Fehler - Die Passwörter stimmen nicht überein";
                }
            }
            else
            {
                ResultMessage = "Fehler - bestätigen Sie die AGBs";
            }

        }
    }
}
