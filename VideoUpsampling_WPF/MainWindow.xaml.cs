using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Windows.Threading;

namespace VideoUpsampling_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //打开文件、保存文件窗体
        Microsoft.Win32.OpenFileDialog opendialog;
        SaveFileDialog savedialog;
        //原视频地址
        public String originalPath;
        //目的视频地址
        public String outputPath;
        //分辨率
        int px = 0;
        //上采样算法
        String algorithm = null;
        //是否添加其他滤镜算法
        bool Switch; 

        int time = 100;
        int timeUnit;
        

        Process proc = new Process();
       

        public MainWindow()
        {
            InitializeComponent();

        }

        /**
         * 导入原视频按钮的事件响应操作
         * 
         **/
        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            opendialog = new Microsoft.Win32.OpenFileDialog();
            opendialog.Filter = "视频文件|*.mp4;*.mkv";
            if(opendialog.ShowDialog() == true)
            {
                importTextBox.Text = opendialog.FileName;
            }
            originalPath = opendialog.FileName;
        }

        /**
         * 输出视频地址的按钮事件响应操作
         * */
        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            savedialog = new SaveFileDialog();
            savedialog.RestoreDirectory = true;
            savedialog.Filter = "视频文件|*.mp4";

            if(savedialog.ShowDialog() == true)
            {
                outputPath = savedialog.FileName.ToString();

                exportTextBox.Text = outputPath;
            }
        }

        /**
         * 对比视频按钮响应机制
         * */
        private void compareButton_Click(object sender, RoutedEventArgs e)
        {
            if (originalPath == null || outputPath == null || new FileInfo(outputPath) == null || !new FileInfo(outputPath).Exists)
            {
                 MessageBox.Show("请压制完成再对比","381鱼雷警告！",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
            MediaPlayer mp = new MediaPlayer(this);
            mp.ShowDialog();
        }
        
        /**
         * 上采样按钮响应机制
         **/
        private void upsampleButton_Click(object sender, RoutedEventArgs e)
        {

            if(IsUesd.IsChecked == true)
            {
                Switch = true;
            }
            if (originalPath == null || originalPath.Equals(""))
            {
                MessageBox.Show("请您输入原视频地址再压制","381鱼雷警告！",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }

            //输入文件后缀校验
            String postfix = originalPath.Substring(originalPath.Length - 3, 3);
            if (!postfix.Equals("mp4"))
            {
                MessageBox.Show("请您输入正确的视频地址", "381鱼雷警告！", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (outputPath == null || outputPath.Equals(""))
            {
                MessageBox.Show("请您输入目的视频地址再压制", "请正确操作，小心460糊脸", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(algorithm == null || algorithm.Equals(""))
            {
                MessageBox.Show("请进行算法选择", "扣扣你别忘选算法了", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(px != 720 && px != 1080 && px != 480)
            {
                MessageBox.Show("请进行分辨率选择", "扣扣您选下分辨率了", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            UpdateFile uf = new UpdateFile();

            if (algorithm.Equals("waifu"))
            {

            }
            else if(algorithm.Equals("nnedi"))
            {
                uf.UpdateAvsFile_nnedi(originalPath, px);
            }else if (algorithm.Equals("spline"))
            {
                uf.UpdateAvsFile_spline(originalPath, px);
            }else if (algorithm.Equals("bilinear"))
            {
                uf.UpdateAvsFile_bilinear(originalPath, px);
            }
            else if (algorithm.Equals("bicubic"))
            {
                uf.UpdateAvsFile_bicubic(originalPath, px);
            }
            uf.UpdateBatFile(outputPath, originalPath, algorithm);
            if(IsUesd.IsChecked == true)
            {
                uf.UpdateOther(algorithm);
            }

            //调用bat可执行程序

            Result.Text = "";
            timeUnit = time / 50;
            pb.Maximum = time;
            pb.Value = 0;
            try
            {
                
                Thread t = new Thread(new ThreadStart(setResult));
                t.Start();
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "发生错误", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void OnExited(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                upsampleButton.IsEnabled = true;
                tbmsg.Text = "上采样完成。";
                pb.Value = pb.Maximum;
            });
            Process targetProcess = sender as Process;
            targetProcess.Exited -= OnExited;
        }
 

        private void setResult()
        {
            
            Process proc = new Process();
                proc.StartInfo.FileName = "start.bat";
                proc.StartInfo.UseShellExecute = false;//不使用系统外壳程序启动    
                proc.StartInfo.RedirectStandardInput = true;//不重定向输入    
                proc.StartInfo.RedirectStandardOutput = true; //重定向输出
                proc.EnableRaisingEvents = true; //必须为true
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//这里设置DOS窗口不显示，经实践可行

               //proc.OutputDataReceived += new DataReceivedEventHandler(OnDataReceived); 
                proc.Exited += OnExited;
                proc.Start();
                tbmsg.Dispatcher.BeginInvoke(new Action(() =>
                {
                    tbmsg.Text = "上采样中...";
                }));
            int tmp = timeUnit;
            StreamReader sr = proc.StandardOutput;//获取返回值 
            string line = "";
            string lastLine = "";
            Thread.Sleep(800);
            while (!proc.HasExited)
            {
                line = sr.ReadLine();
                if (line != "" && line != null)
                {
                    if (line.Equals(lastLine) || line == lastLine) continue;
                    Result.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Result.AppendText(line + "\n");
                        Result.ScrollToEnd();
                    }));
                    lastLine = line;
                }
                pb.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (tmp != time - timeUnit)
                    {
                        pb.Value = tmp;
                        tmp += tmp ;
                    }
                }));
            }
            pb.Dispatcher.BeginInvoke(new Action(() =>
            {
                    pb.Value = time;
            }));
            //proc.WaitForExit();


        }
     

        /**
         * 
         * 分辨率选择：480P
         * */
        private void Checked480(object sender, RoutedEventArgs e)
        {
            px = 480;
        }
        /**
         * 
         * 分辨率选择：720P
         * */
        private void Checked720(object sender, RoutedEventArgs e)
        {
            px = 720;
        }

        /**
         * 
         * 分辨率选择：1080P
         * */
        private void Checked1080(object sender, RoutedEventArgs e)
        {
            px = 1080;
        }

        /**
         * importTextBox_PreviewDragOver和importTextBox_PreviewDrop均为
         * 添加拖拽文件显示原视频地址功能
         * */
        private void importTextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void importTextBox_PreviewDrop(object sender, DragEventArgs e)
        {
            foreach (string f in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                importTextBox.Text = f;
                originalPath = f;
            }
        }

        /**
         * 
         * 上采样算法选择
         * */
        private void Nnedi_Checked(object sender, RoutedEventArgs e)
        {
            algorithm = "nnedi";
        }
        private void Spline_Checked(object sender, RoutedEventArgs e)
        {
            algorithm = "spline";
        }
        private void Bilinear_Checked(object sender, RoutedEventArgs e)
        {
            algorithm = "bilinear";
        }
        private void Bicubic_Cheched(object sender, RoutedEventArgs e)
        {
            algorithm = "bicubic";
        }

      
        
    }

}
