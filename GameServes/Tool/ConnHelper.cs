using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServes.Tool
{
    internal class ConnHelper
    {
        public const string CONNECTIONSTRING = "datasource = 127.0.0.1;port = 4420;database = game01;user=root;pwd=864420";

        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTRING);
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("连接数据库出现异常："+e);
                return null;
            }
            return conn;
        }
        public static void CloseConnection(MySqlConnection conn)
        {
            if (conn != null)
            {
                conn.Close();
            }
            else
            {
                Console.WriteLine("MySqlConnection不能为空");
            }
        }
    }
}
