using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;

namespace chat
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            setstate(true);
            Ip.Text = "192.168.1.193";
            Point.Text = "8885";
            Name.Text = "奔波儿灞";
        }

        Socket socket = null;
        byte[] by = new byte[258];
        string server;

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            string ip = Ip.Text.Trim();
            int point = int.Parse(Point.Text.Trim());
            ConnectServer cs = new ConnectServer(ip,point);
            if (cs.Connect())
            {
                socket = cs.getsocket();
                if (socket == null)
                    Show(ip + ":" + point.ToString() + "--连接失败");
                else
                {
                    Show(ip + ":" + point.ToString() + "--连接成功");
                    server = socket.RemoteEndPoint.ToString();
                    Servername.Text = socket.LocalEndPoint.ToString() + "@" + server;
                    setstate(false);
                    Receive r = new Receive(socket, Show);
                    r.ReceiveMessage();
                }
            }
            else
                Show(ip + ":" + point.ToString() + "--连接失败");

        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="s"></param>
        private void Show(string s)
        {
            TextBox.Dispatcher.Invoke(new Action(delegate {
                TextBox.Text = TextBox.Text + s + "\n";
                TextBox.ScrollToEnd();
            }));
           
        }

        /// <summary>
        /// 回车发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                sendmessage();

        }

        /// <summary>
        /// 发送消息
        /// </summary>
        private void sendmessage()
        {
            if (Sent.Text.Trim() != "")
            {
                SendMessage sm = new SendMessage(socket);
                sm.Send("("+Name.Text + ")-> " + Sent.Text);
                Sent.Text = "";
            }
        }

        /// <summary>
        /// 更改连接状态
        /// </summary>
        /// <param name="state"></param>
        private void setstate(bool state)
        {
            Name.IsEnabled = state;
            Ip.IsEnabled = state;
            Point.IsEnabled = state;
            Connect.IsEnabled = state;
            @break.IsEnabled = !state;
            if (state)
                Servername.Text = "未连接";
        }

        /// <summary>
        /// 断开按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void break_Click(object sender, RoutedEventArgs e)
        {
            Show("已于服务器： " + server + " 断开连接");
            socket.Close();
            setstate(true);

        }
    }
}
