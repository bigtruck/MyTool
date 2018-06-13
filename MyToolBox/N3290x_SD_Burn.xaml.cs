using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyToolBox
{
    /// <summary>
    /// N3290x_SD_Burn.xaml 的交互逻辑
    /// </summary>
    public partial class N3290x_SD_Burn : Window
    {
        public N3290x_SD_Burn()
        {
            InitializeComponent();
        }

        private void ButtonClick_OpenBoot(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonClick_OpenApp(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonClick_OpenData(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonClick_Make(object sender, RoutedEventArgs e)
        {
            //DependencyProperty dp = DependencyProperty.from;
            //Progressbar.SetCurrentValue(0,100);
            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.Hide();
            e.Cancel = true;
        }

    }
}
