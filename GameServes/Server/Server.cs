using Common;
using GameServes.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServes.Servers
{
    internal class Server
    {
        private IPEndPoint _ipEndPoint;
        private Socket _serverSocket;
        public List<Client> clientList = new List<Client>();
        private ControllerManager _controllerManager;

        public Server()
        {
        }
        public Server(string ip, int port)
        {
            _controllerManager = new ControllerManager(this);
            SetIpAndPort(ip, port);
        }
        private void SetIpAndPort(string ip, int port)
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }
        public void Start()
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(_ipEndPoint);
            _serverSocket.Listen(0);
            _serverSocket.BeginAccept(AcceptCallBack, null);
        }
        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket _clientSocket = _serverSocket.EndAccept(ar);
            Client client = new Client(_clientSocket, this);
            client.Start();
            clientList.Add(client);
        }
        public void RemoveClient(Client client)
        {
            lock (clientList)
            {
                clientList.Remove(client);
            }
        }
        public void SendResponse(Client client, RequestCode requestCode, string data)
        {
            client.Send(requestCode, data);
        }
        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            _controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }
    }
}
