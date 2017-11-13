using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace RCNdemo
{
    /// <summary>
    /// PleaseSignPage.xaml 的交互逻辑
    /// </summary>
    public partial class PleaseSignPage : UserControl
    {
        private string plsSignYourName = @"\..\resource\PlsSignYourName.png";
        private string bg2 = @"\..\resource\PlsSign_bg.jpg";

        public PleaseSignPage()
        {
            InitializeComponent();
            string str = Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fInfo = new FileInfo(str);
            this.Background = new ImageBrush(LoadImage(fInfo.Directory + bg2));
            PlsSignYourName_Img.Source = LoadImage(fInfo.Directory + plsSignYourName);
        }

        private BitmapImage LoadImage(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            BitmapImage bitmapImg = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)); ;
            return bitmapImg;
        }

    }
}
