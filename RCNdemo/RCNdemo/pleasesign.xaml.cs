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
using System.IO;

namespace RCNdemo
{
    /// <summary>
    /// Interaction logic for advertisement.xaml
    /// </summary>
    public partial class pleasesign : UserControl
    {
        private string ad = @"..\resource\advertisment.png";
        private string bg = @"..\resource\bg2.jpg";

        public pleasesign()
        {
            InitializeComponent();

            this.Background = new ImageBrush(Image_Loaded(bg));
            advertImage.Source = Image_Loaded(ad);
        }

        private ImageSource Image_Loaded(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            ImageSource imgSrc = null;
            using (BinaryReader loader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                FileInfo fd = new FileInfo(path);
                int Length = (int)fd.Length;
                byte[] buf = new byte[Length];
                buf = loader.ReadBytes((int)fd.Length);
                loader.Dispose();
                loader.Close();
                BitmapImage bim = new BitmapImage();
                bim.BeginInit();
                bim.StreamSource = new MemoryStream(buf);
                bim.EndInit();
                imgSrc = bim;
                GC.Collect(); 
            }

            return imgSrc;
        }
    }
}
