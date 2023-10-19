using Common;
using GameServes.Tool;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServes.Servers
{
    internal class Client
    {
        private Socket _clientSocket;
        private Server _server;
        private Message _msg = new Message();
        private MySqlConnection mySqlConn;

        public Client() { }
        public Client(Socket clientSocket, Server server)
        {
            this._clientSocket = clientSocket;
            this._server = server;
            mySqlConn = ConnHelper.Connect();
        }
        public void Start()
        {
            _clientSocket.BeginReceive(_msg.Data, _msg.StartIndex, _msg.RemainSize, SocketFlags.None, ReceiveCallBack, null);
        }
        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int count = _clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }
                _msg.ReadMessage(count,OnProcessMessage);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }


        }
        private void OnProcessMessage(RequestCode requestCode,ActionCode actionCode,string data)
        {
              _server.HandleRequest(requestCode,actionCode,data,this);
        }
        private void Close()
        {
            ConnHelper.CloseConnection(mySqlConn);
            _clientSocket?.Close();
            _server.RemoveClient(this);
        }
        public void Send(RequestCode requestCode, string data)
        {
            byte[] bytes = Message.PackData(requestCode, data);
            _clientSocket.Send(bytes);
        }
    }
}
