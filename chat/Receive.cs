using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace chat
{
    class Receive
    {
        Action<string> bacCallback;
        private Socket s = null;
        private byte[] message = new byte[2048];
        public Receive(Socket s,Action<string> bc)
        {
            this.s = s;
            bacCallback = bc;
        }
        public void ReceiveMessage()
        {

            try
            {
                s.BeginReceive(message, 0, message.Length, SocketFlags.None, new AsyncCallback(back), s);
            }
            catch(Exception e)
            {
                throw e;
            }

        }
        public void SetSocket(Socket s)
        {
            this.s = s;
        }
        private void back(IAsyncResult ts)
        {
            try
            {
                Socket t = (Socket)ts.AsyncState;
                ts.AsyncWaitHandle.Close();
                int num = t.EndReceive(ts);
                string mess = Encoding.UTF8.GetString(message, 0, num);
                bacCallback(mess);
                message = new byte[message.Length];

                t.BeginReceive(message, 0, message.Length, SocketFlags.None, new AsyncCallback(back), s);
            }
            catch (Exception e)
            {
                bacCallback("已从服务器断开连接");
            }
        }
    }
}
