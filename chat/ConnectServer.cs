using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace chat
{
    class ConnectServer
    {
        private IPAddress ip;
        private Socket clientSocket;
        private int point;
        public ConnectServer(string ip,int point)
        {
            this.ip = IPAddress.Parse(ip);
            this.point = point;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public bool Connect()
        {
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, point));
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public Socket getsocket()
        {
            return clientSocket;
        }
    }
}
