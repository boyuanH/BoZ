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
    /// AdPage.xaml 的交互逻辑
    /// </summary>
    public partial class AdPage : UserControl
    {
        private static int CurrentIndex = 0;

        public AdPage()
        {
            InitializeComponent();
            int cnt = 0;
            while (true)
            {
                string str = Process.GetCurrentProcess().MainModule.FileName;
                FileInfo fInfo = new FileInfo(str);
                int order = cnt + 1;
                string fileName = fInfo.DirectoryName + @"\..\resource\ad" + order.ToString() + @".jpg";
                if (!File.Exists(fileName))
                {
                    GlobalVar.AdsCount = cnt;
                    break;
                }
                cnt++;
            }
            this.Ad_Img.Source = LoadImage();
        }

        private BitmapImage LoadImage()
        {
            //Debug.Print(CurrentIndex.ToString());

            
            string str = Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fInfo = new FileInfo(str);
            int order = CurrentIndex % GlobalVar.AdsCount + 1;
            CurrentIndex++;
            string fileName = fInfo.DirectoryName + @"\..\resource\ad" + order.ToString() + @".jpg";
            if (!File.Exists(fileName))
            {
                return null;
            }
            BitmapImage bitmapImage = new BitmapImage(new Uri(fileName, UriKind.RelativeOrAbsolute));
            return bitmapImage;
        }
    }
}
