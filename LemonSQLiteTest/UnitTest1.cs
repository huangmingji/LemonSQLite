using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using Lemon;
using NUnit.Framework;

namespace LemonSQLiteTest
{

    public class Tests
    {
        private SQLiteHelper _sqLiteHelper;
        private string _dbPath;
        [SetUp]
        public void Setup()
        {
            string dbDirectory = Path.Combine(Directory.GetCurrentDirectory(), "data");
            _dbPath = Path.Combine(dbDirectory, "data.db3");
            if(!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }
            string connectionString = $"Data Source={_dbPath}";
            // string connectionString = $"Data Source={_dbPath};Password=123456";
            _sqLiteHelper = new SQLiteHelper(connectionString);
        }

        [Test]
        public void CreateDbTest()
        {
            _sqLiteHelper.CreateDb();
            Assert.IsTrue(File.Exists(_dbPath));
        }

        [Test]
        public void CreateTableTest()
        {
            _sqLiteHelper.ExecuteNonQueryAsync("CREATE TABLE Test(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,title varchar(200),item blob,addTime datetime,updateTime Date)");
            Assert.Pass();
        }

        [Test]
        public void InsertTest()
        {
            string sql = "INSERT INTO Test( title, item, addTime, updateTime ) values ( @title, @item, @addTime, @updateTime)";
            for (char c = 'A'; c <= 'Z'; c++)
            {
                for (int i = 0; i < 100; i++)
                {
                    SQLiteParameter[] parameters = new SQLiteParameter[]
                    { 
                        new SQLiteParameter ("@title",c+i.ToString()), 
                        new SQLiteParameter ("@item",c.ToString()), 
                        new SQLiteParameter ("@addTime", DateTime.Now), 
                        new SQLiteParameter ("@updateTime", DateTime.Now.Date)
                    };
                    _sqLiteHelper.ExecuteNonQueryAsync(sql, parameters);
                }
            }
            Assert.Pass();
        }

        [Test]
        public void SelectTest()
        {
            string sql = "select * from Test order by id desc limit 1 offset 1";
            var data = _sqLiteHelper.ExecuteDataTable(sql);
            Console.WriteLine(data.Rows.Count);
            Assert.IsTrue(data.Rows.Count > 0);
        }

        [Test]
        public async Task DeleteTest()
        {
            string sql = "delete from Test";
            var result = await _sqLiteHelper.ExecuteNonQueryAsync(sql);
            Console.WriteLine(result);
            Assert.IsTrue(result > 0);
        }
    }
}