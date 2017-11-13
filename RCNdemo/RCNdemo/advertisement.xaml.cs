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
using System.ComponentModel;
using System.IO;

namespace RCNdemo
{
    public partial class advertisement : UserControl
    {

        string path = @"..\resource\ad";
        private List<BitmapImage> image_list = new List<BitmapImage>();


        int cnt = 0;
        public advertisement()
        {
            InitializeComponent();
            load_image();
        }

        private void load_image()
        {
            for (int i = 0; i < GlVar.ad_cnt; i++)
            {
                string str = (i+1).ToString();
                string path_ad = path + str+".jpg";
            
                BitmapImage bmImage = new BitmapImage();
                bmImage.BeginInit();
                bmImage.StreamSource = new MemoryStream(GlFun.TransImageToByte(path_ad));
                bmImage.EndInit();
                bmImage.Freeze();
                image_list.Add(bmImage);
            }
           
        }

        public void ShowImage()
        {           
            int num = cnt % GlVar.ad_cnt;
            adImage.Source = image_list.ElementAt(num);
            cnt = cnt + 1;        
        }

    }
}
