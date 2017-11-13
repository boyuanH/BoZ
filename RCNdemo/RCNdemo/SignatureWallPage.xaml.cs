using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// SignatureWallPage.xaml 的交互逻辑
    /// </summary>
    public partial class SignatureWallPage : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private double coverOpacity;
        public double CoverOpacity
        {
            get { return coverOpacity; }
            set { coverOpacity = value; OnPropertyChanged("CoverOpacity"); }
        }


        private string LogoPath = @"\..\resource\SignatureWall_logo.jpg";
        private string BgPath = @"\..\resource\SignatureWall_bg.jpg";

        public SignatureWallPage(BitmapImage sigatureWallImg)
        {
            InitializeComponent();

            string str = Process.GetCurrentProcess().MainModule.FileName;
            FileInfo fInfo = new FileInfo(str);
            this.CoverOpacity = GlobalVar.opacity;
            Logo_Img.Source = LoadImage(fInfo.Directory + LogoPath);
            Background_Img.Source = LoadImage(fInfo.Directory + BgPath);
            SignatureWall_Img.Source = sigatureWallImg;

            this.DataContext = this;
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
