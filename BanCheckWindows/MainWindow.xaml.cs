using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Newtonsoft.Json;


namespace BanCheckWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonCheck_Click(object sender, RoutedEventArgs e)
        {
            string url = textBoxLinkInput.Text.Trim();
            //  //string[] splited = url.Split('/');
            //  //MessageBox.Show(splited[splited.Length - 1]);
            //  XmlDocument doc = new XmlDocument();
            ////  XmlDocument doc = new XmlDocument();
            //  doc.Load(url + "?xml=1");
            //  string xpathID64 = "//profile/steamID64";
            //  XmlNode nodeID64 = doc.SelectSingleNode(xpathID64);
            //  string id = nodeID64.InnerText;
            //  MessageBox.Show(id);
            // BanCheck check = new BanCheck(url);
            // textBlockResult.Text = check.GetHttpResponse(url + "?key=" + App.SteamAPIKey + "&steamids=76561198118220136,");
            BanCheck check = new BanCheck(url);
            textBlockResult.Text = check.SetResultUI();
        }
    }
}
