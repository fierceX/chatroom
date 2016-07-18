using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections;
/// <summary>
/// 发送信息
/// </summary>
namespace server
{
    class SendMessage
    {
        Hashtable connectpool;
        public SendMessage(Hashtable connectpool)
        {
            this.connectpool = connectpool;
        }
        /// <summary>
        /// 向单个连接发送消息
        /// </summary>
        /// <param name="b"></param>
        /// <param name="s"></param>
        public void Send(string b,Socket s)
        {
            byte[] a = Encoding.UTF8.GetBytes(b);
            s.BeginSend(a, 0, a.Length, 0, new AsyncCallback(sentback), s);
        }
        /// <summary>
        /// 向所有连接池里的连接发送消息
        /// </summary>
        /// <param name="b"></param>
        public void sendgroup(string b)
        {
            byte[] a = Encoding.UTF8.GetBytes(b);
            foreach(var x in connectpool.Values)
            {
                Socket s = (Socket)x;
                s.BeginSend(a, 0, a.Length, 0, new AsyncCallback(sentback), s);
            }
        }
        /// <summary>
        /// 异步发送回调函数
        /// </summary>
        /// <param name="s"></param>
        private void sentback(IAsyncResult s)
        {
            try
            {
                Socket handler = (Socket)s.AsyncState;
                int num = handler.EndSend(s);

            }
            catch (Exception e)
            {

            }
        }
        
    }
}
