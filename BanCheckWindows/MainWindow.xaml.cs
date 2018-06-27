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
        Player[] Players;
        BanCheck check;
        int playerIndex = 0;
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
           

            if (url.EndsWith(","))
              url = url.TrimEnd(',');

            check = new BanCheck(url);
            //   Players = check.Players;
            textBoxResult.Text = check.SetResultUI(0);
            buttonPrevious.IsEnabled = true;
            buttonNext.IsEnabled = true;
            //  textBoxResult.Text = check.SetResultUI();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            buttonPrevious.Content = "<";
            buttonNext.Content = ">";
            buttonPrevious.IsEnabled = false;
            buttonNext.IsEnabled = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //BanCheck chk = new BanCheck();
            //textBoxResult.Text = chk.SpiltURLtoAPIParam(textBoxLinkInput.Text);
        }

        private void buttonPrevious_Click(object sender, RoutedEventArgs e)
        {
            playerIndex--;
            string result= check.SetResultUI(playerIndex);
            if (result != "")
                textBoxResult.Text = result;
            else
                MessageBox.Show("已经是第一项");
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            playerIndex++;
            string result = check.SetResultUI(playerIndex);
            if (result != "")
                textBoxResult.Text = result;
            else
                MessageBox.Show("已经是最后一项");
        }
    }
}
