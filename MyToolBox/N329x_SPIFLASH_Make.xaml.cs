using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    /// N329x_SPIFLASH_Make.xaml 的交互逻辑
    /// </summary>
    public partial class N329x_SPIFLASH_Make : Window
    {
        private const UInt32 BOOTIMAGE_MARK0 = 0x57425AA5;
        private const UInt32 BOOTIMAGE_RUNADDR = 0x00900000;
        private const UInt32 BOOTIMAGE_MARK1 = 0xA55A4257;
        private const UInt32 BOOTIMAGE_DQSODS = 0x00001010;
        private const UInt32 BOOTIMAGE_CKDQSDS = 0x00888800;
        private const UInt32 INFO_MARK0 = 0xAA554257;
        private const UInt32 INFO_MARK1 = 0x63594257;
        private struct __IMAGE_INFO
        {
            public UInt16 RESERVE0;
            public UInt16 num;
            public UInt16 startSection;
            public UInt16 endSection;
            public UInt32 RESERVE1;
            public UInt32 dataLength;
            public byte[] name;
        };

        private struct __DATA_HEAD
        {

            public UInt32 MARK0;
            public UInt32 fileCount;
            public UInt32 xxx;
            public UInt32 MARK1;
            public __IMAGE_INFO[] fileinfo;
        };
        private UInt32 bootLen;
        private UInt32 appLen;
        public N329x_SPIFLASH_Make()
        {
            InitializeComponent();
            TextBox_OpenBoot.Text = null;
            TextBox_OpenApp.Text = null;
            TextBox_OpenData.Text = null;
        }

        private void ButtonClick_OpenBoot(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //此处做你想做的事 ...=openFileDialog.FileName; 
                TextBox_OpenBoot.Text = openFileDialog.FileName;
            }
        }

        private void ButtonClick_OpenApp(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //此处做你想做的事 ...=openFileDialog.FileName; 
                TextBox_OpenApp.Text = openFileDialog.FileName;
            }
        }

        private void ButtonClick_OpenData(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //此处做你想做的事 ...=openFileDialog.FileName; 
                TextBox_OpenData.Text = openFileDialog.FileName;
            }
        }

        private void ButoonClick_Make(object sender, RoutedEventArgs e)
        {
            bool isReady = false;
            if (System.IO.File.Exists(TextBox_OpenBoot.Text) == false)
            {
                MessageBox.Show("BOOT文件不正确", "错误", MessageBoxButton.OK);
            }
            else
            {
                if (System.IO.File.Exists(TextBox_OpenApp.Text) == false)
                {
                    MessageBox.Show("APP文件不正确", "错误", MessageBoxButton.OK);
                }
                else
                {
                    isReady = true;
                }
            }
            if (isReady == true)
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                saveFileDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    TextBox_OpenBoot.IsEnabled = false;
                    TextBox_OpenApp.IsEnabled = false;
                    TextBox_OpenData.IsEnabled = false;
                    Button_OpenBoot.IsEnabled = false;
                    Button_OpenApp.IsEnabled = false;
                    Button_OpenData.IsEnabled = false;
                    Button_Make.IsEnabled = false;

                    FileStream bootFile = new FileStream(TextBox_OpenBoot.Text, FileMode.Open, FileAccess.Read);
                    BinaryReader bootBinRead = new BinaryReader(bootFile);
                    FileStream appFile = new FileStream(TextBox_OpenApp.Text, FileMode.Open, FileAccess.Read);
                    BinaryReader appBinRead = new BinaryReader(appFile);
                    FileStream saveFile = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                    BinaryWriter saveBinWrite = new BinaryWriter(saveFile);

                    //bootBinRead.BaseStream.Seek(0, SeekOrigin.Begin);

                    bootLen = (UInt32)bootFile.Length;
                    byte[] bootBuff = new byte[bootLen];
                    bootBinRead.Read(bootBuff, 0, bootBuff.Length);
                    bootBinRead.Close();
                    appLen = (UInt32)appFile.Length;
                    byte[] appBuff = new byte[appLen];
                    appBinRead.Read(appBuff, 0, appBuff.Length);
                    appBinRead.Close();

                    //__DATA_HEAD imageHead = new __DATA_HEAD();
                    __DATA_HEAD imageHead;
                    imageHead.fileinfo = new __IMAGE_INFO[3];
                    imageHead.fileinfo[0].name = new byte[32];
                    imageHead.fileinfo[1].name = new byte[32];
                    imageHead.fileinfo[2].name = new byte[32];

                    imageHead.MARK0 = INFO_MARK0;
                    imageHead.fileCount = 2;
                    imageHead.xxx = 0xFFFFFFFF;
                    imageHead.MARK1 = INFO_MARK1;
                    imageHead.fileinfo[0].RESERVE0 = 0;
                    imageHead.fileinfo[0].num = 3;
                    imageHead.fileinfo[0].startSection = 0;
                    imageHead.fileinfo[0].endSection = 0;
                    imageHead.fileinfo[0].RESERVE1 = 0;
                    imageHead.fileinfo[0].dataLength = bootLen;
                    imageHead.fileinfo[0].name = System.Text.Encoding.ASCII.GetBytes(bootFile.Name.Substring(bootFile.Name.LastIndexOf("\\")+1));
                    

                    imageHead.fileinfo[1].RESERVE0 = 1;
                    imageHead.fileinfo[1].num = 1;
                    imageHead.fileinfo[1].startSection = 1;
                    imageHead.fileinfo[1].endSection = (UInt16)(appLen / 65536);
                    if(appLen % 65536 > 0)
                    {
                        imageHead.fileinfo[1].endSection++;
                    }
                    imageHead.fileinfo[1].RESERVE1 = 0;
                    imageHead.fileinfo[1].dataLength = appLen;
                    imageHead.fileinfo[1].name = System.Text.Encoding.ASCII.GetBytes(appFile.Name.Substring(appFile.Name.LastIndexOf("\\")+1));


                    //写入BOOT信息
                    saveBinWrite.Write(BOOTIMAGE_MARK0);
                    saveBinWrite.Write(BOOTIMAGE_RUNADDR);
                    saveBinWrite.Write(bootLen);
                    saveBinWrite.Write(BOOTIMAGE_MARK1);
                    saveBinWrite.Write(BOOTIMAGE_DQSODS);
                    saveBinWrite.Write(BOOTIMAGE_CKDQSDS);
                    saveBinWrite.Write(0);
                    saveBinWrite.Write(0);
                    //写入BOOT代码
                    saveBinWrite.Write(bootBuff);
                    //写入IMAGE信息
                    saveBinWrite.BaseStream.Seek(63*1024, SeekOrigin.Begin);
                    saveBinWrite.Write(imageHead.MARK0);
                    saveBinWrite.Write(imageHead.fileCount);
                    saveBinWrite.Write(imageHead.xxx);
                    saveBinWrite.Write(imageHead.MARK1);
                    //写入BOOT代码信息
                    saveBinWrite.BaseStream.Seek(63 * 1024 + 16, SeekOrigin.Begin);
                    saveBinWrite.Write(imageHead.fileinfo[0].RESERVE0);
                    saveBinWrite.Write(imageHead.fileinfo[0].num);
                    saveBinWrite.Write(imageHead.fileinfo[0].startSection);
                    saveBinWrite.Write(imageHead.fileinfo[0].endSection);
                    saveBinWrite.Write(imageHead.fileinfo[0].RESERVE1);
                    saveBinWrite.Write(imageHead.fileinfo[0].dataLength);
                    saveBinWrite.Write(imageHead.fileinfo[0].name);
                    //写入APP代码信息
                    saveBinWrite.BaseStream.Seek(63 * 1024 + 16 + 48, SeekOrigin.Begin);
                    saveBinWrite.Write(imageHead.fileinfo[1].RESERVE0);
                    saveBinWrite.Write(imageHead.fileinfo[1].num);
                    saveBinWrite.Write(imageHead.fileinfo[1].startSection);
                    saveBinWrite.Write(imageHead.fileinfo[1].endSection);
                    saveBinWrite.Write(imageHead.fileinfo[1].RESERVE1);
                    saveBinWrite.Write(imageHead.fileinfo[1].dataLength);
                    saveBinWrite.Write(imageHead.fileinfo[1].name);

                    saveBinWrite.BaseStream.Seek(64 * 1024, SeekOrigin.Begin);//64K为一个BLOCK
                    saveBinWrite.Write(appBuff);

                    saveBinWrite.Flush();
                    saveBinWrite.Close();

                    MessageBox.Show("完成", "完成", MessageBoxButton.OK);
                }
                TextBox_OpenBoot.IsEnabled = true;
                TextBox_OpenApp.IsEnabled = true;
                TextBox_OpenData.IsEnabled = false;
                Button_OpenBoot.IsEnabled = true;
                Button_OpenApp.IsEnabled = true;
                Button_OpenData.IsEnabled = false;
                Button_Make.IsEnabled = true;
            }
        }
    }
}
