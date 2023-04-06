using System;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Layer3Objects;

namespace ShopBase
{

    public class Customer : NonCustomer
    {
        public string Password { get; set; }

        public Customer() : base()
        {
            // Same as NonCustomer
        }
        public Customer(int id, string email, string password, string firstname, string lastname, Gender gender, Adress adress) : base(id, email, firstname, lastname, gender, adress)
        {
            // Full Constructor for consiting customer, test customer or set back password
            Password = password;
        }

        public Customer(string email, string password, string firstname, string lastname, Gender gender, Adress adress) : base(email, firstname, lastname, gender, adress)
        {
            // Base + Password
            Password = password;
        }

        public Customer(int id, string email, string firstname, string lastname, Gender gender, Adress adress) : base(id, email, firstname, lastname, gender, adress)
        {

        }

        public override bool Equals(object obj)
        {
            bool isEqaul = false;

            // Important because this line give diconectet item (this.Id != 0 && (obj as Customer).Id != 0)
            Customer c = obj as Customer; // Set c = null if disconnected

            if (c != null)
            {

                if (this.Id != 0 && c.Id != 0)
                {
                    isEqaul = Id == (obj as Customer).Id;
                }
                else
                {
                    isEqaul = EMail == (obj as Customer).EMail;
                }
            }
            return isEqaul;
        }

        public override int GetHashCode()
        {
            if (Id != 0)
            {
                return Id.GetHashCode();
            }
            else
            {
                return EMail.GetHashCode();
            }
        }

        public override void Insert() => DBObjects.Insert(this);

        public override void Delete() => DBObjects.Delete<Customer>(this);

        public override void Change() => DBObjects.Change<Customer>(this);

        public override string ToString() => $"{Id,-8} {Firstname,-30} {Lastname,-59} {EMail,+20}";

        public static int Login(string email, string password) => DBObjects.ReadCustomer(email, password);

        public static new Customer Get(int id) => DBObjects.ReadAll<Customer>(id)[0];

        public static new List<Customer> GetAll() => DBObjects.ReadAll<Customer>();

        public static string GetHash(string input)
        {
            byte[] data = null;
            var sBuilder = new StringBuilder();

            using (SHA256 pwdEncryptor = SHA256.Create())
            {
                data = pwdEncryptor.ComputeHash(Encoding.UTF8.GetBytes(input));

                sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
