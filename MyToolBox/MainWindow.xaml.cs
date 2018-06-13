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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyToolBox
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        N3290x_SD_Burn n3290x_SD_Make = new N3290x_SD_Burn();
        N3290x_SPIFLASH_Make n3290X_SPIFLASH_Make = new N3290x_SPIFLASH_Make();
        Bin2Dat bin2Dat = new Bin2Dat();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick_N3290x_SD_Burn(object sender, RoutedEventArgs e)
        {
            if(n3290x_SD_Make == null || n3290x_SD_Make.IsVisible == false)
            {
                n3290x_SD_Make.Show();
            }
            else
            {
                n3290x_SD_Make.Activate();
                n3290x_SD_Make.WindowState = System.Windows.WindowState.Normal;
            }
        }

        private void ButtonClick_N3290x_SPIFLASH_Make(object sender, RoutedEventArgs e)
        {
            if(n3290X_SPIFLASH_Make == null || n3290X_SPIFLASH_Make.IsVisible == false)
            {
                n3290X_SPIFLASH_Make.Show();
            }
            else
            {
                n3290X_SPIFLASH_Make.Activate();
                n3290X_SPIFLASH_Make.WindowState = System.Windows.WindowState.Normal;
            }
        }

        private void ButtonClick_bin2dat(object sender, RoutedEventArgs e)
        {
            if (bin2Dat == null || bin2Dat.IsVisible == false)
            {
                bin2Dat.Show();
            }
            else
            {
                bin2Dat.Activate();
                bin2Dat.WindowState = System.Windows.WindowState.Normal;
            }
        }
    }
}
