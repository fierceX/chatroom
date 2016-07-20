using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class help:Handle
    {
        public help(Hashtable connectpool) : base(connectpool) { }
        public override bool handlequest(Action<string, Socket> bc, string s, Socket socket)
        {
            if (s == "/help")
            {
                string message = "-------已支持命令------\n"
                    + "/help  --获得该帮助命令\n"
                    + "/list    --获取用户列表\n"
                    + "/s xxx   --搜索xxx用户\n"
                    + "/sent xxx yyy\n"
                    + "--向xxx用户单独发送消息yyy(xxx为ip:端口号)\n"
                    + "-------已支持命令------\n";
                bc(message,socket);
                return false;
            }
            else if (base.next != null)
                return base.next.handlequest(bc, s, socket);
            else
                return true;
        }
    }
}
