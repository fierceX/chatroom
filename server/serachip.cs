using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Collections;
using System.Collections.ObjectModel;
/// <summary>
/// 查找用户命令
/// </summary>
namespace server
{
    class serachip:Handle
    {
        public serachip(Hashtable connectpool) : base(connectpool) { }
        public override bool handlequest(Action<string, Socket> bc, string s, Socket socket)
        {
            if (s.StartsWith("/s "))
            {
                bool b = true;
                char[] f = { 's', ' ' };
                string a = s.Split(f)[2];
                string mess = "\n----------------查找用户-----------------\n";
                foreach (var x in connectpool.Keys)
                {
                    string v = (string)x;
                    if (v.Contains(a))
                    {
                        mess = mess + v + "\n";
                        b = false;
                    }
                }
                if (b)
                    mess = mess + "没有找到该IP用户\n";
                mess = mess + "------------------------------------------\n";
                bc(mess, socket);
                return false;
            }
            else if (base.next != null)
                return base.next.handlequest(bc, s, socket);
            else
                return true;
        }
    }
}
