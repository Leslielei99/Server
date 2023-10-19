using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLServes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //连接路径
            string connStr = "Database=mygamedb;Data Source=127.0.0.1;port=4420;User Id= root;Password=864420";
            MySqlConnection conn = new MySqlConnection(connStr);//创建连接

            conn.Open();//开启连接
            #region 查询
            //MySqlCommand cmd = new MySqlCommand("select * from user", conn);//创建查询方法
            //MySqlDataReader mySqlDataReader = cmd.ExecuteReader();//执行

            //while (mySqlDataReader.Read())
            //{ //自动读取一行

            //    string username = mySqlDataReader.GetString("username");
            //    string password = mySqlDataReader.GetString("password");
            //    Console.WriteLine(username + " : " + password);

            //}
            //mySqlDataReader.Close();//关闭Read
            #endregion

            #region 插入
            //string usern = "rtuyrtytr";string passd = "99999";
            //MySqlCommand cmd = new MySqlCommand("insert into user set username = @un , password = @pwd", conn);

            //cmd.Parameters.AddWithValue("un", usern);  
            //cmd.Parameters.AddWithValue ("pwd", passd);

            //cmd.ExecuteNonQuery();

            #endregion

            #region 删除
            //MySqlCommand cmd = new MySqlCommand("delete from user where id = @did", conn);

            //cmd.Parameters.AddWithValue("did", 8);
            //cmd.ExecuteNonQuery();
            #endregion

            #region 更新
            MySqlCommand cmd = new MySqlCommand("update user set password = @pwd where id between 1 and 3", conn);

            cmd.Parameters.AddWithValue("pwd", 11111);
            cmd.ExecuteNonQuery();
            #endregion

            conn.Close();
            Console.ReadKey();
        }
    }
}
