using Layer3Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace ShopBase
{
    public enum Status
    {
        Warenkorb = 1,
        Bestellt = 2,
        Versendet = 3,
        Storniert = 4
    }

    public class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public NonCustomer NonCustomer { get; set; }
        public Status Status { get; set; }
        public List<Position> Positions { get; set; }

        public Order()
        {

        }

        public Order(int id, Customer customer, Status status, List<Position> positions)
        {
            Id = id;
            Customer = customer;
            Status = status;
            Positions = positions;
        }

        public Order(Customer customer, Status status, List<Position> positions)
        {
            Customer = customer;
            Status = status;
            Positions = positions;
        }

        public Order(int id, NonCustomer ncustomer, Status status, List<Position> positions)
        {
            Id = id;
            NonCustomer = ncustomer;
            Status = status;
            Positions = positions;
        }

        public Order(NonCustomer ncustomer, Status status, List<Position> positions)
        {
            NonCustomer = ncustomer;
            Status = status;
            Positions = positions;
        }

        public void AddPosition(int menge, int id)
        {
            Positions ??= new List<Position>();


            Positions.Add(new Position(menge, Article.Get(id)));
        }
        public void Insert() => DBObjects.Insert(this);

        public void Change() => DBObjects.Change<Order>(this);

        public void Delete() => DBObjects.Delete<Order>(this);
        public Order MergeBaskets() => DBObjects.MergeBasket(this);
        public override string ToString() => $"{Id,-8} {Positions.Count,-8} Positionen           {Positions.Sum(item => item.Totalsum),-20} EUR {Status}";
        public static Order Get(int id) => DBObjects.ReadAll<Order>(id)[0];
        public static List<Order> GetAllFromCustomer(int cid) => DBObjects.GetAllOrderFromCustomer(cid);
        public static List<Order> GetAllFromNonCustomer(int cid) => DBObjects.GetAllOrderFromNonCustomer(cid);

    }
}
