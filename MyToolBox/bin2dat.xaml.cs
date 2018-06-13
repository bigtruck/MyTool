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
using System.Windows.Shapes;

namespace MyToolBox
{
    /// <summary>
    /// Bin2Dat.xaml 的交互逻辑
    /// </summary>
    public partial class Bin2Dat : Window
    {
        public Bin2Dat()
        {
            InitializeComponent();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.Hide();
            e.Cancel = true;
        }

    }
}
