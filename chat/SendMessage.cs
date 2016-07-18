using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace chat
{
    class SendMessage
    {
        private Socket s = null;
        public SendMessage(Socket s)
        {
            this.s = s;
        }
        public bool Send(string b)
        {
            try
            {
                byte[] a = Encoding.UTF8.GetBytes(b);
                s.BeginSend(a, 0, a.Length, 0, new AsyncCallback(sentback), s);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
            
        }
        private void sentback(IAsyncResult s)
        {
            try
            {
                Socket handler = (Socket)s.AsyncState;
                int num = handler.EndSend(s);
               
            }
            catch(Exception e)
            {

            }
        }
        public void SetSocket(Socket s)
        {
            this.s = s;
        }
    }
}
