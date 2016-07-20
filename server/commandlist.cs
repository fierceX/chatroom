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
/// 列出所有用户
/// </summary>
namespace server
{
    class commandlist:Handle
    {
        public commandlist(Hashtable connectpool) : base(connectpool) { }
        public override bool handlequest(Action<string,Socket> bc, string s,Socket socket)
        {
            if (s == "/list")
            {
                string a = "\n---------------已连接用户----------------\n";
                foreach (var x in base.connectpool.Keys)
                {
                    a = a + (string)x + "\n";
                }
                a = a + "------------------------------------------\n";
                bc(a,socket);
                return false;
            }
            else if (base.next != null)
                return base.next.handlequest(bc, s,socket);
            else
                return true;
        }
    }
}
