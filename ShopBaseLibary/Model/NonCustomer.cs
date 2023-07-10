using Layer3Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBase
{
      public enum Gender
    {
        male = 1,
        female = 2,
        divers = 3
    }
    public class NonCustomer
    {
        public int Id { get; set; }
        public string? EMail { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public Gender Gender { get; set; }
        public Adress? Adress { get; set; }


        public NonCustomer()
        {

        }

        public NonCustomer(int id, string email, string firstname, string lastname, Gender gender, Adress adress)
        {
            // Full Constructor for consiting customer, test customer or set back password
            Id = id;
            EMail = email;
            Firstname = firstname;
            Lastname = lastname;
            Gender = gender;
            Adress = adress;
        }

        public NonCustomer(string email, string firstname, string lastname, Gender gender, Adress adress)
        {
            EMail = email;
            Firstname = firstname;
            Lastname = lastname;
            Gender = gender;
            Adress = adress;
        }

        public override bool Equals(object? obj)
        {
            bool isEqaul = false;
            Customer? c = obj as Customer;

            if (this.Id != 0 && c != null && c.Id != 0)
            {
                isEqaul = Id == c.Id;
            }
            else if(EMail != null && c != null && c.EMail != null)
            {
                isEqaul = EMail == c.EMail;
            }

            return isEqaul;
        }

        public override int GetHashCode()
        {
            if (Id != 0)
            {
                return Id.GetHashCode();
            }
            else if(EMail != null)
            {
                return EMail.GetHashCode();
            }
            else
            {
                throw new NoNullAllowedException();
            }
        }

        public virtual void Insert() => DBObjects.Insert(this);

        public virtual void Delete() => DBObjects.Delete<NonCustomer>(this);

        public virtual void Change() => DBObjects.Change<NonCustomer>(this);

        public override string ToString() => $"{Id,-8} {Firstname,-30} {Lastname,-59} {EMail,+20}";

        public static NonCustomer Get(int id) => DBObjects.ReadAll<NonCustomer>(id)[0];

        public static List<NonCustomer> GetAll() => DBObjects.ReadAll<NonCustomer>();
        
    }
}
