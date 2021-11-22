using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Lemon
{
    public class SQLiteHelper : IDisposable
    {
        private readonly SQLiteConnection _connection;
        private readonly SQLiteCommand _command;

        public SQLiteHelper(IConfiguration configuration)
            : this(configuration.GetConnectionString("SQLite"))
        {
            
        }

        public SQLiteHelper(string connectionString, string password = null)
        {
            // "Data Source=D:\\data.db3";
            // "Data Source=D:\\data.db3;Password=123456";
            _connection = new SQLiteConnection(connectionString);
            if (!string.IsNullOrWhiteSpace(password))
            {
                _connection.SetPassword(password);
            }
            _connection.Open();
            _command = new SQLiteCommand(_connection);
        }

        public void ChangePassword(string newPassword)
        {
            _connection.ChangePassword(newPassword);
        }

        /// <summary>  
        /// 创建SQLite数据库文件 
        /// </summary>  
        public async Task CreateDb()
        {
            await ExecuteNonQueryAsync("CREATE TABLE Test(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE)");
            await ExecuteNonQueryAsync("DROP TABLE Test");
        }

        /// <summary>  
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。 
        /// </summary>  
        /// <param name="sql"> 要执行的增删改的SQL语句 </param> 
        /// <param name="parameters"> 执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准 </param> 
        /// <returns></returns>  
        public async Task<int> ExecuteNonQueryAsync(string sql, SQLiteParameter[] parameters = null)
        {
            _command.CommandText = sql;
            if (parameters != null && parameters.Length > 0)
            {
                _command.Parameters.AddRange(parameters);
            }

            return await _command.ExecuteNonQueryAsync();
        }

        /// <summary>  
        /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例 
        /// </summary>  
        /// <param name="sql"> 要执行的查询语句 </param> 
        /// <param name="parameters"> 执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准 </param> 
        /// <returns></returns>  
        public async Task<DbDataReader> ExecuteReaderAsync(string sql, SQLiteParameter[] parameters = null)
        {
            _command.CommandText = sql;
            if (parameters != null && parameters.Length > 0)
            {
                _command.Parameters.AddRange(parameters);
            }

            return await _command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        /// <summary>  
        /// 执行一个查询语句，返回一个包含查询结果的DataTable 
        /// </summary>  
        /// <param name="sql"> 要执行的查询语句 </param> 
        /// <param name="parameters"> 执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准 </param> 
        /// <returns></returns>  
        public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters = null)
        {
            _command.CommandText = sql;
            if (parameters != null && parameters.Length > 0)
            {
                _command.Parameters.AddRange(parameters);
            }

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(_command);
            DataTable data = new DataTable();
            adapter.Fill(data);
            return data;
        }

        public void Dispose()
        {
            _command.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }
}