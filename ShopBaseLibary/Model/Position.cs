using Layer3Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShopBase
{
    public class Position
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public decimal Totalsum { get => Article.Price * Count; }
        public Article Article { get; set; }

        public Position()
        {

        }

        public Position(int id, Article article, int count)
        {
            Id = id;
            Count = count;
            Article = article;
        }
        public Position(int count, Article article)
        {
            Count = count;
            Article = article;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Position other = (Position)obj;

            return this.Article.Id == other.Article.Id;   
        }

        public override int GetHashCode()
        {
            return this.Article.Id;
        }
        public static List<Position> GetAll(int oid) => DBObjects.ReadAll<Position>(oid);
        public static List<Position> GetAll() => DBObjects.ReadAll<Position>();

    }
}
