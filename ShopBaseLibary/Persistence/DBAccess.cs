using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using ShopBase;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Data.Sqlite;
using System.Threading;

namespace Layer3Access
{
    public static class DBAccess
    {
        private static string _liteSQLPath = @"../../../../ShopBaseLibary/Persistence/WebShop.db";

        public static bool CreateDefaultDataBase()
        {
            bool isCreated = false;

            if (!File.Exists(_liteSQLPath))
            {
                FileStream fs = File.Create(_liteSQLPath);
                fs.Close();

                isCreated = true;
            }

            return isCreated;
        }
        public static bool DeleteDataBase()
        {
            bool isDeleted = false;

            if (File.Exists(_liteSQLPath)) // does not work because process block the file??
            {
                GC.Collect();
                Thread.Sleep(2000);
                File.Delete(_liteSQLPath); //Exception thrown
                isDeleted = true;
            }

            string sql = "Drop Database WebShop";

            ExecuteNonQuery(sql);

            return isDeleted;
        } // Not in use does not work see Todo
        public static SqliteConnection OpenDB()
        {
            SqliteConnection con = null;

            SqliteConnectionStringBuilder cs = new SqliteConnectionStringBuilder()
            {
                DataSource = _liteSQLPath
            };

            con = new(cs + "");
            con.Open();

            return con;
        }
        public static int ExecuteNonQuery(String? sql)
        {
            int lastId = -1;
            if (sql != null)
            {

                using (SqliteConnection con = OpenDB())
                {
                    SqliteCommand cmd = new(sql, con);
                    cmd.ExecuteNonQuery();

                    if (sql.ToUpper().Contains("INSERT"))
                    {
                        cmd = new("SELECT LAST_INSERT_ROWID();", con);
                        lastId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                }
            }
            return lastId;
        }
        public static int ExecuteNonQuery(String? sql, SqliteConnection con)
        {
            int lastId = -1;
            if (sql != null && sql != "")
            {
                SqliteCommand cmd = new(sql, con);
                cmd.ExecuteNonQuery();


                if (sql.ToUpper().Contains("INSERT"))
                {
                    cmd = new("SELECT LAST_INSERT_ROWID();", con);
                    lastId = Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
            return lastId;
        }
        public static int ExecuteNonQuery(String sql, SqliteConnection con, SqliteTransaction t)
        {
            int lastId = -1;
            if (sql != null)
            {
                SqliteCommand cmd = new(sql, con, t);
                cmd.ExecuteNonQuery();


                if (sql.ToUpper().Contains("INSERT"))
                {
                    cmd = new("SELECT LAST_INSERT_ROWID();", con, t);
                    lastId = Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
            return lastId;
        }
        public static SqliteDataReader ExecuteReader(string sql, SqliteConnection con)
        {
            SqliteCommand cmd = new(sql, con);
            return cmd.ExecuteReader();
        }
        public static bool CreateArticleTable()
        {
            bool isCreated = false;

            string sql =       "Create Table TArticle" +
                               "(" +
                               "Id integer primary key autoincrement," +
                               "Active bool default false," +
                               "Name VARCHAR(30) not null," +
                               "Description VARCHAR(60)," +
                               "Price Decimal(6,2)," +
                               "Count int not null" +
                               ");";

            try
            {
                DBAccess.ExecuteNonQuery(sql);
                isCreated = true;
            }
            catch (Exception)
            {
            }

            return isCreated;
        }
        public static bool CreateAdressTable()
        {
            bool isCreated = false;

            string sql = "Create Table TAdress" +
                                "(" +
                                "Id integer primary key autoincrement," +
                                "Score int default 500," +
                                "Street VARCHAR(50) not null," +
                                "HouseNumber int not null," +
                                "PostalCode VARCHAR(50) not null," +
                                "City VARCHAR(50) not null," +
                                "Country VARCHAR(50) not null," +
                                "UNIQUE(Street, HouseNumber, PostalCode, City, Country)" +
                                ");";

            try
            {
                DBAccess.ExecuteNonQuery(sql);
                isCreated = true;
            }
            catch (Exception)
            {
            }

            return isCreated;
        }
        public static bool CreateAdressNodeTable()
        {
            bool isCreated = false;

            string sql =        "Create Table TNodeAdress" +
                                "(" +
                                "Id integer primary key autoincrement," +
                                "CId int default null," +
                                "CNId int default null," +
                                "AId int not null," +
                                "Adresstype Text not null Check(Adresstype in('Invoice', 'Delivery'))" +
                                //"Foreign Key(CId) References TCustomer(CId) on Delete Cascade On Update no action," +
                                //"Foreign Key(CNId) References TNonCustomer(CId) on Delete Cascade On Update no action," +
                                //"Foreign Key(AId) References TAdress(AId)" +
                                ");";

            try
            {
                DBAccess.ExecuteNonQuery(sql);
                isCreated = true;
            }
            catch (Exception)
            {
            }

            return isCreated;
        }
        public static bool CreateCustomerTable()
        {
            bool isCreated = false;

            // Not Working See Todo

            //using (SQLiteConnection con = new(_liteSQLPath))
            //{
            //    isCreated = CreateTableResult.Created == con.CreateTable<Customer>();
            //}


            string sql = "Create Table TCustomer" +
                            "(" +
                            "Id integer primary key autoincrement," +
                            "CreditScore int default 1000," +
                            "EMail VARCHAR(60) unique not null," +
                            "Password TEXT not null," +
                            "Firstname VARCHAR(30) not null," +
                            "Lastname VARCHAR(30) not null," +
                            "Gender Text not Null Check(Gender in('male','female','divers'))" +
                            ");";

            try
            {
                DBAccess.ExecuteNonQuery(sql);
                isCreated = true;
            }
            catch (Exception)
            {
            }

            return isCreated;
        } 
        public static bool CreateNonCustomerTable()
        {
            bool isCreated = false;

            // Not Working See Todo

            //using (SQLiteConnection con = new(_liteSQLPath))
            //{
            //    isCreated = CreateTableResult.Created == con.CreateTable<Customer>();
            //}


            string sql =        "Create Table TNonCustomer" +
                                "(" +
                                "Id integer primary key autoincrement," +
                                "EMail VARCHAR(50) not null," +
                                "Firstname VARCHAR(50) not null," +
                                "Lastname VARCHAR(50) not null," +
                                "Gender Text not Null Check(Gender in('male','female','divers'))" +
                                ");";

            try
            {
                DBAccess.ExecuteNonQuery(sql);
                isCreated = true;
            }
            catch (Exception)
            {
            }

            return isCreated;
        }
        public static bool CreatePictureTable()
        {
            bool isCreated = false;

            // Not Working See Todo

            //using (SQLiteConnection con = new(_liteSQLPath))
            //{
            //    isCreated = CreateTableResult.Created == con.CreateTable<Customer>();
            //}


            string sql =        "Create Table TPicture" +
                                "(" +
                                "Id integer primary key autoincrement," +
                                "AId int default null," +
                                "Filename Varchar(30) not null," +
                                "Data Blob not null," +
                                "Length int not null" +
                                //"Foreign Key(AId) References TArticle(AId) on Delete Cascade on Update no action" +
                                ");";

            try
            {
                DBAccess.ExecuteNonQuery(sql);
                isCreated = true;
            }
            catch (Exception)
            {
            }

            return isCreated;
        } 
        public static bool CreateOrderTable()
        {
            bool isCreated = false;

            // Not Working See Todo

            //using (SQLiteConnection con = new(_liteSQLPath))
            //{
            //    isCreated = CreateTableResult.Created == con.CreateTable<Customer>();
            //}


            string sql =        "Create Table TOrder" +
                                "(" +
                                "Id integer primary key autoincrement," +
                                "CId int default null," +
                                "CNId int default null," +
                                "Status Text not null Check(Status in ('Warenkorb', 'Bestellt','Versendet','Storniert'))" +
                                //"Foreign Key(CId) References TCustomer(CId) on Delete Cascade On Update Cascade," +
                                //"Foreign Key(CNId) References TNonCustomer(CId) on Delete Cascade On Update Cascade" +
                                ");";

            try
            {
                DBAccess.ExecuteNonQuery(sql);
                isCreated = true;
            }
            catch (Exception)
            {
            }

            return isCreated;
        }
        public static bool CreatePositionTable()
        {
            bool isCreated = false;

            // Not Working See Todo

            //using (SQLiteConnection con = new(_liteSQLPath))
            //{
            //    isCreated = CreateTableResult.Created == con.CreateTable<Customer>();
            //}


            string sql =        "Create Table TPosition" +
                               "(" +
                               "Id integer primary key autoincrement," +
                               "Oid int not null," +
                               "Aid int not null," +
                               "Count int not null" +
                               //"Foreign Key(OId) References TOrder(OId) on Delete Cascade On Update Cascade," +
                               //"Foreign Key(AId) References TArticle(AId)" +
                               ");";

            try
            {
                DBAccess.ExecuteNonQuery(sql);
                isCreated = true;
            }
            catch (Exception)
            {
            }

            return isCreated;
        }
    }
}
