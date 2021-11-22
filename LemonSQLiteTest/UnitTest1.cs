using System.IO;
using Lemon;
using NUnit.Framework;

namespace LemonSQLiteTest
{

    public class Tests
    {
        private SQLiteHelper _sqLiteHelper;
        [SetUp]
        public void Setup()
        {
            string dbDirectory = Path.Combine(Directory.GetCurrentDirectory(), "data");
            string dbPath = Path.Combine(dbDirectory, "data.db3");
            if(!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }
            string connectionString = $"Data Source={dbPath}";
            _sqLiteHelper = new SQLiteHelper(connectionString);
        }

        [Test]
        public void Test1()
        {
            _sqLiteHelper.CreateDb();
            _sqLiteHelper.ExecuteNonQuery("CREATE TABLE Test(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE)");
            Assert.Pass();
        }
    }
}