using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serves01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartServeAsync();
            Console.ReadKey();
        }
       static void StartServeAsync()//异步
        {
            Socket serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress iPAddress = IPAddress.Parse("10.18.100.24");
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 88);
            serversocket.Bind(iPEndPoint);
            serversocket.Listen(50);
            serversocket.BeginAccept(AcceptCallBack,serversocket);
        }
        static byte[] dateBuffer = new byte[1024];
        static Message msg = new Message();
        static void AcceptCallBack(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);
            //向客户端发送一条消息
            string msgptr = "Hello ! 你好！！我是服务端";
            byte[] data = Encoding.UTF8.GetBytes(msgptr);
            clientSocket.Send(data);
            //等待接受
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, clientSocket);//接收消息后回调
            //继续处理下一个客户端连接
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }

        static  void ReceiveCallback(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);//完成接收
                if(count == 0)//正常关闭客户端
                {
                    clientSocket.Close();
                    return;
                }
                msg.AddCount(count);
                msg.ReadMessage();
                //string msgget = Encoding.UTF8.GetString(dateBuffer, 0, count);
                //Console.WriteLine("从客户端接受：" + msgget);
                //接收消息后输出控制台，然后接着接收，一个循环了
                //clientSocket.BeginReceive(dateBuffer, 0, 1024, SocketFlags.None, ReceiveCallback, clientSocket);//接收消息后回调
                clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, clientSocket);//接收消息后回调
            }
            catch (Exception e)//异常关闭客户端
            {
                Console.WriteLine(e);
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }
            }
          
        }
















        void StartServeSync()//同步
        {
            //ipv4,数据流，TCP协议 / ipv4 ,Dgram数据包,UDP
            Socket serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //本机IP : 10.18.100.24   127.0.0.1
            //绑定ip: ipAddress  xxx.xxx.xxx.xxx  IpEndPoint xxx.xxx.xxx.xxx:port

            // IPAddress iPAddress = new IPAddress(new byte[] {10,18,100,24});
            IPAddress iPAddress = IPAddress.Parse("10.18.100.24");
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 88);
            serversocket.Bind(iPEndPoint); //绑定（申请）IP和端口号

            serversocket.Listen(50);//50个客户端(队列)数量，不代表最大连接数，0表示不限制数量,开始监听端口好
            Socket clientSocket = serversocket.Accept();//程序暂停，直到接受一个客户端连接

            //向客户端发送一条消息
            string msg = "Hello ! 你好！！";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            clientSocket.Send(data);

            //接受客户端的一条消息
            byte[] dateBuffer = new byte[1024];
            int count = clientSocket.Receive(dateBuffer);//接受到的字节数量
            string msgReceive = System.Text.Encoding.UTF8.GetString(dateBuffer, 0, count);
            Console.WriteLine(msgReceive);

            Console.ReadLine();
            //关闭跟客户端的连接，关闭自身的连接
            clientSocket.Close();
            serversocket.Close();
        }
    }
}
