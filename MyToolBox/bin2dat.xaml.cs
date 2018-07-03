using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        string binFilePath = null;
        //string datFilePath = null;
        bool runing = false;

        public Bin2Dat()
        {
            InitializeComponent();
        }
        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);
        //}
        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    this.Hide();
        //    e.Cancel = true;
        //}
        private delegate void SetprogressBarHandle(int vaule);    //定义 代理函数 
        private void SetprogressBar(int vaule)
        {
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.Invoke(new SetprogressBarHandle(this.SetprogressBar), vaule);
            }
            else
            {
                progBar.Value = vaule;
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (runing == false)
            {
                SetprogressBar(0);
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    binFilePath = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                    //string fileName;
                    //fileName = binFilePath.Substring(binFilePath.LastIndexOf("\\") + 1, (binFilePath.LastIndexOf(".") - binFilePath.LastIndexOf("\\") - 1)); //文件名
                    //System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                    //saveFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                    //saveFileDialog.Filter = "dat 文件(*.dat)|*.dat";
                    //saveFileDialog.FilterIndex = 1;
                    //saveFileDialog.RestoreDirectory = true;
                    //saveFileDialog.FileName = fileName;

                    //if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    //{
                    //    datFilePath = saveFileDialog.FileName;
                    Thread thread = new Thread(UpdateTextRight);
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    //}
                }
            }
        }

        private void UpdateTextRight()
        {
            //throw new NotImplementedException();
            string fileName;

            runing = true;
            fileName = binFilePath.Substring(binFilePath.LastIndexOf("\\") + 1, (binFilePath.LastIndexOf(".") - binFilePath.LastIndexOf("\\") - 1)); //文件名
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            saveFileDialog.Filter = "dat 文件(*.dat)|*.dat";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = fileName;
            
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                FileStream fBinStream = new FileStream(binFilePath, FileMode.Open, FileAccess.Read);
                BinaryReader fBinReader = new BinaryReader(fBinStream);
                byte[] binBuff = new byte[fBinStream.Length];
                fBinReader.Read(binBuff, 0, (int)fBinStream.Length);
                fBinReader.Close();
                //byte[] datBuff = new byte[binBuff.Length * 6];
                //long cnt;
                string datHex;
                datHex = null;
                //cnt = 0;
                for (int i = 0; i < binBuff.Length; i++)
                {
                    if ((i % 16) == 0)
                    {
                        datHex += "\r\n";
                        //datBuff[cnt++] = (byte)'\r';
                        //datBuff[cnt++] = (byte)'\n';
                    }

                    datHex += String.Format("0x{0:X2},", binBuff[i]);//十六进制
                    //datHex += String.Format("{0:D3},", binBuff[i]);//十进制
                    SetprogressBar((i / (binBuff.Length/1000)));
                }
                FileStream fDatStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                //FileStream fDatStream = new FileStream(datFilePath, FileMode.Create, FileAccess.Write);
                BinaryWriter binaryWriter = new BinaryWriter(fDatStream);
                byte[] datBuff = System.Text.Encoding.Default.GetBytes(datHex);
                binaryWriter.Write(datBuff, 0, datBuff.Length);
                binaryWriter.Flush();
                binaryWriter.Close();
                //MessageBox.Show(datFilePath, "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                MessageBox.Show(saveFileDialog.FileName, "完成", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            runing = false;
        }
    }
}
