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
    public partial class welcome : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        private string m_peoplestring = "";

        public string peopleString
        {
            get
            {
                return m_peoplestring;
            }
            set
            {
                m_peoplestring = value;
                OnPropertyChanged("peopleString");
            }
        }
        private BitmapImage m_wimage = null;
        public BitmapImage wImage
        {
            get
            {
                return m_wimage;
            }
            set 
            {
                m_wimage = value; OnPropertyChanged("wImage");
            }
        }
        public welcome()
        {
            InitializeComponent();
            this.DataContext = this;
            Set_Background();
            return;
        }

        public void Welcome_People(string path)
        {
            byte[] bytes = GlFun.TransImageToByte(path);
            if (bytes == null)
            {
                return;
            }
            peopleString = GlobalVar.people_num.ToString();
            
            BitmapImage bmImage = new BitmapImage();
            bmImage.BeginInit();            
            bmImage.StreamSource = new MemoryStream(bytes);
            bmImage.EndInit();
            bmImage.Freeze();
            wImage = bmImage;
        }

        private void Set_Background()
        {
            string path = @"..\resource\bg2.jpg";
            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            this.Background = bg;            
            return;
        }

    }
}
