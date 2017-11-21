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
    /// PictureSettingPage.xaml 的交互逻辑
    /// </summary>
    /// 

    public delegate void PictureSettingPageAdvancedBtnClickDelegate(object sender, EventArgs e);
    public delegate void PictureSettingPageOkBtnClickDelegate(object sender, EventArgs e);
    public delegate void PictureSettingPageBackBtnClickDelegate(object sender, EventArgs e);
    public partial class PictureSettingPage : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PictureSettingPageAdvancedBtnClickDelegate PictureSettingPageAdvancedBtnClickEvent;
        public event PictureSettingPageBackBtnClickDelegate PictureSettingPageBackBtnClickEvent;
        public event PictureSettingPageOkBtnClickDelegate PictureSettingPageOkBtnClickEvent;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string pictureFolder;
        public string PictureFolder
        {
            get
            {
                return pictureFolder;
            }
            set
            {
                pictureFolder = value;
                OnPropertyChanged("PictureFolder");
            }
        }

        private string startHour;
        public string StartHour
        {
            get
            {
                return startHour;
            }
            set
            {
                startHour = value;
                OnPropertyChanged("StartHour");
            }
        }

        private string startMinute;
        public string StartMinute
        {
            get
            {
                return startMinute;
            }
            set
            {
                startMinute = value;
                OnPropertyChanged("StartMinute");
            }
        }

        private string startSecond;
        public string StartSecond
        {
            get
            {
                return startSecond;
            }
            set
            {
                startSecond = value;
                OnPropertyChanged("StartSecond");
            }
        }

        private string endHour;
        public string EndHour
        {
            get
            {
                return endHour;
            }
            set
            {
                endHour = value;
                OnPropertyChanged("EndHour");
            }
        }

        private string endMinute;
        public string EndMinute
        {
            get
            {
                return endMinute;
            }
            set
            {
                endMinute = value;
                OnPropertyChanged("EndMinute");
            }
        }

        private string endSecond;
        public string EndSecond
        {
            get
            {
                return endSecond;
            }
            set
            {
                endSecond = value;
                OnPropertyChanged("EndSecond");
            }
        }

        public PictureSettingPage()
        {
            InitializeComponent();
        }

        private void PictureSettingPageAdvancedBtn_Click(object sender, RoutedEventArgs e)
        {
            if(PictureSettingPageAdvancedBtnClickEvent != null)
            {
                PictureSettingPageAdvancedBtnClickEvent(this, e);
            }
        }

        private void PictureSettingPageBackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PictureSettingPageBackBtnClickEvent != null)
            {
                PictureSettingPageBackBtnClickEvent(this, e);
            }
        }

        private void PictureSettingPageOkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PictureSettingPageOkBtnClickEvent != null)
            {
                PictureSettingPageOkBtnClickEvent(this, e);
            }
        }
    }
}
