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

namespace BoZPreparation_Tool
{
    /// <summary>
    /// AdvancedSettingPage.xaml 的交互逻辑
    /// </summary>
    /// 

    public delegate void AdvancedSettingPageBackBtnClickDelegate(object sender, EventArgs e);
    public delegate void AdvancedSettingPageOkBtnClickDelegatte(object sender, EventArgs e);

    public partial class AdvancedSettingPage : UserControl
    {
        public AdvancedSettingPage()
        {
            InitializeComponent();
        }

        private void AdvancedSettingPageBackBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AdvancedSettingPageOKBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
