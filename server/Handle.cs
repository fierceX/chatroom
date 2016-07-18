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
/// 命令责任链父类
/// </summary>
namespace server
{
    abstract class Handle
    {
        public Handle next=null;
        protected Hashtable connectpool;
        public abstract bool handlequest(Action<string,Socket> bc, string s,Socket socket);
        public Handle(Hashtable connectpool)
        {
            this.connectpool = connectpool;
        }
    }
}
