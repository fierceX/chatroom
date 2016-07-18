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
/// 向单个用户发送消息
/// </summary>
namespace server
{
    class sentsingle:Handle
    {
        public sentsingle(Hashtable connectpool) : base(connectpool) { }
        public override bool handlequest(Action<string, Socket> bc, string s, Socket socket)
        {
            //判断是否是该命令
            if(s.StartsWith("/sent "))
            {
                char[] f = { ' ' };
                s = s.Substring(6);
                string [] a = s.Split(f);//剪切字符串
                SendMessage sm = new SendMessage(connectpool);
                if (connectpool.ContainsKey(a[0]))//匹配该用户
                {
                    
                    string message="";
                    for (int i = 1; i < a.Length; i++)
                        message = message + a[i] + " ";
                    sm.Send(socket.RemoteEndPoint.ToString()+":: "+message, (Socket)connectpool[a[0]]);
                    sm.Send("--> "+a[0]+" ----\n"+message, socket);
                    return false;
                }
                sm.Send("-->未找到该用户\n " + a[0]+"\n------------", socket);
                return false;
            }
            else if (base.next != null)
                return base.next.handlequest(bc, s, socket);
            else
                return true;
        }
    }
}
