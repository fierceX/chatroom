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
/// 接受发送请求，组装命令责任链
/// </summary>
namespace server
{
    class Command
    {
        Hashtable connectpool;
        Handle list;
        Handle serachip;
        Handle sentsingle;
        Handle help;
        Socket socket;
        public Command(Hashtable connectpool,Socket s)
        {
            this.connectpool = connectpool;
            list = new commandlist(connectpool);
            serachip = new serachip(connectpool);
            sentsingle = new sentsingle(connectpool);
            help = new help(connectpool);
            list.next = serachip;
            serachip.next = sentsingle;
            sentsingle.next = help;
            socket = s;
        }
        public void command(string s)
        {
            char[] f = { '-', '>' };
            string a = s.Split(f)[2].Trim();
            SendMessage sm = new SendMessage(connectpool);
            if (a.StartsWith("/"))
            {
                if (list.handlequest(sm.Send, a, socket))
                    sm.Send("\n-----------------------------------------", socket);
            }
            else
                sm.sendgroup(s);
        }
    }
}
