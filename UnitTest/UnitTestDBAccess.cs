using Layer3Access;
using Xunit;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTest
{
    public class UnitTestDBAccess
    {
        [Fact]
        public void TestCreateDefaultDataBase()
        {
            bool isPassed = true;

            try
            {
                if (!File.Exists(@"../../../../ShopBaseLibary/Persistence/WebShop.db"))
                {
                    isPassed = DBAccess.CreateDefaultDataBase(true);
                }
            }
            catch (System.Exception)
            {
                isPassed = false;
            }

            Assert.True(isPassed);
        }
        [Fact]
        public void TestOpenDB()
        {
            bool isPassed = true;

            try
            {
                for (int i = 0; i < 1000; i++)
                {
                    SqliteConnection con = DBAccess.OpenDB();
                }
            }
            catch (System.Exception)
            {
                isPassed = false;
            }

            Assert.True(isPassed);
        }
        [Fact]
        public void TestExecuteNonQuery()
        {
            List<string> sqls = new List<string>();
            bool isPassed = true;

            string sql1 = "Create Table TTest" +
                          "(" +
                          "Id integer primary key autoincrement," +
                          "Testint1 int," +
                          "Testint2 int not null," +
                          "TestVarChar varchar(30) not null," +
                          "UNIQUE(Testint1, TestVarChar)" +
                          ");";

            string sql2 = "Drop Table TTest;";

            sqls.Add(sql1);
            sqls.Add(sql2);

            try
            {
                sqls.ForEach(item => DBAccess.ExecuteNonQuery(item));

                using (SqliteConnection con = DBAccess.OpenDB())
                {
                    sqls.ForEach(item => DBAccess.ExecuteNonQuery(item, con));
                }

                // Test overload with transaction Code right

                using (SqliteConnection con = DBAccess.OpenDB())
                {
                    using (SqliteTransaction t = con.BeginTransaction())
                    {
                        sqls.ForEach(item => DBAccess.ExecuteNonQuery(item, con, t));

                        t.Commit();
                    }
                }
                // Test overload with transaction Code wrong

                using (SqliteConnection con = DBAccess.OpenDB())
                {
                    using (SqliteTransaction t = con.BeginTransaction())
                    {
                        DBAccess.ExecuteNonQuery(sql1, con, t);

                        try
                        {
                            DBAccess.ExecuteNonQuery(sql2.Replace("op", "zz"), con, t); // Code is wrong
                            t.Commit();

                        }
                        catch (Exception)
                        {
                        }

                    }
                }

                using (SqliteConnection con = DBAccess.OpenDB())
                {
                    SqliteCommand com = new("Select name from sqlite_master where type='table' and name='TTest'", con);

                    using (SqliteDataReader r = com.ExecuteReader())
                    {
                        r.Read();
                        string? s = null;

                        try
                        {
                            s = r.GetString(0);

                        }
                        catch (Exception)
                        {
                        }

                        if (s != null && s != "")
                        {
                            throw new Exception();
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                isPassed = false;
            }

            Assert.True(isPassed);
        }

        [Fact]
        public void TestCreateTables()
        {
            bool isPassed = true;

            try
            {
                string[] nameTables = new string[] { "TArticle", "TAdress", "TNodeAdress", "TCustomer", "TNonCustomer", "TPicture", "TOrder", "TPosition" };
                DBAccess.CreateArticleTable();
                DBAccess.CreateAdressTable();
                DBAccess.CreateAdressNodeTable();
                DBAccess.CreateCustomerTable();
                DBAccess.CreateNonCustomerTable();
                DBAccess.CreatePictureTable();
                DBAccess.CreateOrderTable();
                DBAccess.CreatePositionTable();

                using (SqliteConnection con = DBAccess.OpenDB())
                {
                    for (int i = 0; i < 7; i++)
                    {
                        SqliteCommand com = new($"Select name from sqlite_master where type='table' and name='{nameTables[i]}'", con);

                        using (SqliteDataReader r = com.ExecuteReader())
                        {
                            r.Read();
                            string? s = null;

                            try
                            {
                                s = r.GetString(0);

                            }
                            catch (Exception)
                            {
                            }

                            if (s == null || s == "")
                            {
                                throw new Exception();
                            }
                        }
                    }

                }


            }
            catch (System.Exception)
            {
                isPassed = false;
            }

            Assert.True(isPassed);
        }
        public void Test()
        {
            bool isPassed = true;

            try
            {

            }
            catch (System.Exception)
            {
                isPassed = false;
            }

            Assert.True(isPassed);
        }

        //.. and so on ..
    }
}