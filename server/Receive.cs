using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections;
using System.Collections.ObjectModel;
/// <summary>
/// 接受信息
/// </summary>
namespace server
{
    class Receive
    {
        Action<string> bacCallback;
        private Socket s = null;
        private byte[] message = new byte[2048];
        Hashtable connectpool;
        Action<string> delete;
        public Receive(Socket s, Action<string> bc, Hashtable connectpool, Action<string> delete)
        {
            this.s = s;
            bacCallback = bc;
            this.connectpool = connectpool;
            this.delete = delete;
        }
        /// <summary>
        /// 异步接收
        /// </summary>
        public void ReceiveMessage()
        {

            try
            {
                s.BeginReceive(message, 0, message.Length, SocketFlags.None, new AsyncCallback(back), s);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public void SetSocket(Socket s)
        {
            this.s = s;
        }
        /// <summary>
        /// 接受回调
        /// </summary>
        /// <param name="ts"></param>
        private void back(IAsyncResult ts)
        {
            Socket t = (Socket)ts.AsyncState;
            try
            {

                ts.AsyncWaitHandle.Close();
                    int num = t.EndReceive(ts);
                    string mess = Encoding.UTF8.GetString(message, 0, num);
                    bacCallback(mess);
                    message = new byte[message.Length];
                    t.BeginReceive(message, 0, message.Length, SocketFlags.None, new AsyncCallback(back), s);
            }
            catch (Exception e)
            {
                try
                {
                    connectpool.Remove(t.RemoteEndPoint.ToString());
                    delete(t.RemoteEndPoint.ToString());
                    SendMessage sm = new SendMessage(connectpool);
                    sm.sendgroup(t.RemoteEndPoint.ToString() + ":已断开");
                    t.Close();
                }
                catch(Exception x)
                {

                }
            }
        }
    }
}
