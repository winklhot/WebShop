using Layer3Objects;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBase
{
    //
    public enum AdressType
    {
        Invoice = 1,
        Delivery = 2
    }
    public class Adress
    {
        public int Id { get; set; }
        public Customer? Customer { get; set; }  
        public string? Street { get; set; }
        public int HouseNumber { get; set; }
        public int PostalCode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public AdressType AdressType { get; set; } = AdressType.Invoice;

        public Adress()
        {

        }

        public Adress(int id, string street, int houseNumber, int postalCode, string city, string country)
        {
            Id = id;
            Street = street;
            HouseNumber = houseNumber;
            PostalCode = postalCode;
            City = city;
            Country = country;
        }

        public Adress(string street, int houseNumber, int postalCode, string city, string country)
        {
            Street = street;
            HouseNumber = houseNumber;
            PostalCode = postalCode;
            City = city;
            Country = country;
        }
        public void Insert() => DBObjects.Insert(this);
        public static Adress GetFromCustomer<T>(int id) => DBObjects.ReadAll<Adress>(DBObjects.GetAdressID<T>(id))[0];
        public static Adress Get(int id) => DBObjects.ReadAll<Adress>(id)[0];
        public static Adress GetOrCreateIfNotExists(string street, int houseNumber, int postalCode, string city, string country) => DBObjects.GetAdressWithoutID(new Adress(street,houseNumber, postalCode, city, country));
    }
}
