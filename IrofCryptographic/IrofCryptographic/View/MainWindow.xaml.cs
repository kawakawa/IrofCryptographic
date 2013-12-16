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
using IrofCryptographic.ViewModel;

namespace IrofCryptographic.View
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var vModel = new MainWindowViewModel();

            vModel.SetWebBrowserA = this.setA;
            vModel.SetWebBrowserB = this.setB;
            
            this.DataContext = vModel;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            //var t = new Model.Twitter.TwitterData();
            //t.Open();
            
            //var t2 = t.GetAllTimeLineData("irof");


            //this.log.Text = t2.Count.ToString();
        }

        public void setA(string url)
        {
            this.webBrowserA.Source=new Uri(url);
        }
        public void setB(string url)
        {
            this.webBrowserB.Source = new Uri(url);
        }
    }
}
