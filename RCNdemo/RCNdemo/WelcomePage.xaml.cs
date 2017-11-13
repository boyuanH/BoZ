using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// WelcomePage.xaml 的交互逻辑
    /// </summary>
    public partial class WelcomePage : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string peopleString = "";

        public string PeopleString
        {
            get
            {
                return peopleString;
            }
            set
            {
                peopleString = value;
                OnPropertyChanged("peopleString");
            }
        }

        private BitmapImage signatureImage = null;
        public BitmapImage SignatureImage
        {
            get
            {
                return signatureImage;
            }
            set
            {
                signatureImage = value; OnPropertyChanged("SignatureImage");
            }
        }

        public WelcomePage(int currentNumber,BitmapImage signatureImg)
        {
            InitializeComponent();
            this.SignatureImage = signatureImg;
            LoadBg();
            PeopleString = currentNumber.ToString();
            this.DataContext = this;
        }

        private void LoadBg()
        {
            string path = @"..\resource\Welcome_bg.jpg";
            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            this.Background = bg;        
        }
    }
}
