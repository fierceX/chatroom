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
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections;
using System.Collections.ObjectModel;

namespace server
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Socket server=null;
        Hashtable pool = new Hashtable();
        ObservableCollection<string> connlist = new ObservableCollection<string>();
        public MainWindow()
        {
            
            InitializeComponent();
            Ip.Text = "192.168.1.193";
            Point.Text = "8885";
            setstate(true);
            ListView.ItemsSource = connlist;
        }
        /// <summary>
        /// 开启服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect_Click(object sender, RoutedEventArgs e)
        {

            accept ap = new accept(int.Parse(Point.Text), 100, pool,updatainvoke,deleteinvoke);
            server = ap.getserver();
            ap.beginaccept();
            setstate(false);
        }
        /// <summary>
        /// 停止服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void break_Click(object sender, RoutedEventArgs e)
        {
            server.Close();
            setstate(true);
            foreach(Socket x in pool.Values)
            {
                x.Close();
            }
            connlist.Clear();
            pool.Clear();
        }
        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="state"></param>
        private void setstate(bool state)
        {
            Ip.IsEnabled = state;
            Point.IsEnabled = state;
            Bind.IsEnabled = state;
            kill.IsEnabled = !state;
            @break.IsEnabled = !state;
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        /// <param name="s"></param>
        private void updatainvoke(string s)
        {
            try
            {
                Action<string> updata = this.updata;
                ListView.Dispatcher.Invoke(updata,s);
            }
            catch(Exception e)
            {

            }
        }
        /// <summary>
        /// 删除列表内容
        /// </summary>
        /// <param name="s"></param>
        private void deleteinvoke(string s)
        {
            try
            {
                Action<string> delete = this.delete;
                ListView.Dispatcher.Invoke(delete, s);
            }
            catch(Exception e)
            {

            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="s"></param>
        public void updata(string s)
        {
            connlist.Add(s);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="s"></param>
        public void delete(string s)
        {
            connlist.Remove(s);
        }

        /// <summary>
        /// 踢掉用户事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kill_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string a = ListView.SelectedItem.ToString();
                Socket s = (Socket)pool[a];
                pool.Remove(a);
                delete(a);
                s.Close();
            }
            catch(Exception x)
            {

            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sent_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                SendMessage sm = new SendMessage(pool);
                if((bool)ALL.IsChecked)
                {
                    sm.sendgroup("服务器-> "+Sent.Text);
                    TextBox.Text = TextBox.Text + "-----> ALL -----\n" + Sent.Text + "\n";
                }
                else if((bool)select.IsChecked)
                {
                    try
                    {
                        string a = ListView.SelectedItem.ToString();
                        Socket s = (Socket)pool[a];
                        sm.Send("服务器-> " + Sent.Text, s);
                        TextBox.Text = TextBox.Text + "-----> "+a+"-----\n"+ Sent.Text+"\n";
                    }
                    catch(Exception x)
                    {
                        TextBox.Text = TextBox.Text + "未选中任何用户\n";
                    }
                }
            }
        }
    }
}
