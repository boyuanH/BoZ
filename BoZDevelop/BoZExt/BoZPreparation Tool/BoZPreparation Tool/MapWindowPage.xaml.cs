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

namespace BoZPreparation_Tool
{
    /// <summary>
    /// MapWindowPage.xaml 的交互逻辑
    /// </summary>
    /// 

    public delegate void MapWindowPageCancelBtnClickDelegate(object sender, EventArgs e);
    public delegate void MapWindowPageOkBtnClickDelegate(object sender, EventArgs e);

    public partial class MapWindowPage : UserControl,INotifyPropertyChanged
    {
        public event MapWindowPageCancelBtnClickDelegate MapWindowPageCancelBtnClickEvent;
        public event MapWindowPageOkBtnClickDelegate MapWindowPageOkBtnClickEvent;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string mapLongitude;
        public string MapLongitude
        {
            get
            {
                return mapLongitude;
            }
            set
            {
                mapLongitude = value;
                OnPropertyChanged("MapLongitude");
            }
        }

        private string mapLatitude;
        public string MapLatitude
        {
            get
            {
                return mapLatitude;
            }
            set
            {
                mapLatitude = value;
                OnPropertyChanged("MapLatitude");
            }
        }


        public MapWindowPage()
        {
            InitializeComponent();
            SetCoordinate("0", "0");
        }

        public void SetCoordinate(string longitude, string latitude)
        {
            MapLongitude = longitude.ToString();
            MapLatitude = latitude.ToString();
        }

        private void MapWindowPageCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            MapWindowPageCancelBtnClickEvent?.Invoke(this, e);
        }

        private void MapWindowPageOKBtn_Click(object sender, RoutedEventArgs e)
        {
            MapWindowPageOkBtnClickEvent?.Invoke(this, e);
        }
    }
}
