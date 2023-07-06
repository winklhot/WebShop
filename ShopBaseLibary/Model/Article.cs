using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer3Objects;


namespace ShopBase
{
    public class Article
    {
        public int Id { get; set; }
        public bool Active { get; set; } = false;

        // Name may not be emty, desc can be emty according to database setup
        public string Name { get => _name; set => _name = value.Length < 31 && value.Trim().Length > 0 ? value.Trim() : throw new Exception("Name zu lang oder keine Eingabe"); }
        public string Description { get => _desc; set => _desc = value.Length < 71 ? value : throw new Exception("Bezeichnung ist zu lang");}
        // Price range limited by database setup
        public decimal Price 
        { 
            get => _price;
            set
            {
                _price = value > 0.00m && value < 9999.99m ? value : throw new Exception("Preis gültig zwischen 0,00 und 9999,99");
            }
        }
        public int Count { get; set; }

        private string _name;

        private string _desc;

        private decimal _price;
        

        public Article()
        {

        }

        public Article(int id, bool active, string name, string description, decimal price, int count)
        {
            Id = id;
            Active = active;
            Name = name;
            Description = description;
            Price = price;
            Count = count;
        }

        public Article(bool active, string name, string description, decimal price, int count)
        {
            Active = active;
            Name = name;
            Description = description;
            Price = price;
            Count = count;
        }

        public int Insert() => DBObjects.Insert(this);

        public void Change() => DBObjects.Change<Article>(this);

        public void Delete() => DBObjects.Delete<Article>(this);

        public string ToCSV() => $"{Id};{Name};{Description};{Price}";

        public override string ToString() => $"{Id,-6} {Name,-30} {Description,-70} {Price,+8:#0.00}";

        public override bool Equals(object? obj) => obj is Article artikel && artikel.Id == Id;

        public override int GetHashCode()
        {
            if (Id == 0)
            {
                return 0;
            }
            else
            {
                return Id.GetHashCode();
            }
        }

        public static Article Get(int id) => DBObjects.ReadAll<Article>(id)[0];

        public static List<Article> GetAll() => DBObjects.ReadAll<Article>();

    }
}
