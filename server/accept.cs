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

namespace server
{
    class accept
    {
        Socket serversocket;
        Hashtable connectpool;
        Action<string> update;
        Action<string> delete;

        /// <summary>
        /// 构建socket，绑定端口
        /// </summary>
        /// <param name="poin"></param>
        /// <param name="num"></param>
        /// <param name="connectpool"></param>
        /// <param name="update"></param>
        /// <param name="delete"></param>
        public accept(int poin,int num, Hashtable connectpool, Action<string> update, Action<string> delete)
        {
            IPAddress ip = IPAddress.Any;
            serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serversocket.Bind(new IPEndPoint(ip, poin));
            serversocket.Listen(num);
            this.connectpool = connectpool;
            this.update = update;
            this.delete = delete;
        }

        /// <summary>
        /// 开始异步监听
        /// </summary>
        public void beginaccept()
        {
            serversocket.BeginAccept(new AsyncCallback(acceptcallback), serversocket);
        }

        /// <summary>
        /// 异步监听回调
        /// </summary>
        /// <param name="s"></param>
        private void acceptcallback(IAsyncResult s)
        {
            try
            {
                Socket socket = (Socket)s.AsyncState;
                Socket client = socket.EndAccept(s);
                //添加该连接到连接池
                connectpool.Add(client.RemoteEndPoint.ToString(), client);
                //更新服务器用户列表
                update(client.RemoteEndPoint.ToString());
                //继续异步监听
                socket.BeginAccept(new AsyncCallback(acceptcallback), serversocket);
                SendMessage sm = new SendMessage(connectpool);
                sm.sendgroup(client.RemoteEndPoint.ToString() + "：已连接");
                string message = "\n---------------已支持命令----------------\n"
                    + "/help    ---获得该帮助命令\n"
                    + "/list      ---获取用户列表\n"
                    + "/s xxx   ---搜索xxx用户\n"
                    + "/sent xxx yyy\n"
                    + "---向xxx用户单独发送消息yyy\n"
                    + "---(xxx为ip:端口号)\n"
                    + "------------------------------------------\n";
                sm.Send(message, client);
                Command comm = new Command(connectpool,client);
                //开始异步接收信息，并将 信息处理 作为回调函数
                Receive r = new Receive(client,comm.command, connectpool,delete);
                r.ReceiveMessage();
            }
            catch(Exception e)
            {

            }
        }
        public Socket getserver()
        {
            return serversocket;
        }
    }
}
