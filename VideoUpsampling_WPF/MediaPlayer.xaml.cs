using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace VideoUpsampling_WPF
{
    /// <summary>
    /// MediaPlayer.xaml 的交互逻辑
    /// </summary>
    public partial class MediaPlayer : MetroWindow
    {

        public MediaPlayer(MainWindow main)
        {
            InitializeComponent();

            Source.Source = new Uri(main.outputPath);
            Output.Source = new Uri(main.originalPath);
            //Source.Height = Source.NaturalVideoHeight;
            //Source.Width = Source.NaturalVideoWidth;
            //Output.Height = Output.NaturalVideoHeight;
            //Output.Width = Output.NaturalVideoWidth;

            new Thread(MyInputThread).Start();
            new Thread(MyOutputThread).Start();

        }

        private void MyInputThread()
        {
            Source.Dispatcher.BeginInvoke(new Action(() =>
            {
                //Thread.Sleep(10);
                Source.Play();
            }));
        }
        private void MyOutputThread()
        {
            Output.Dispatcher.BeginInvoke(new Action(() =>
            {
                Thread.Sleep(780);
                Output.Play();
            }));
        }

        private void Output_MediaEnded(object sender, RoutedEventArgs e)
        {
            (sender as MediaElement).Stop();
            (sender as MediaElement).Play();
        }

        private void Source_MediaEnded(object sender, RoutedEventArgs e)
        {
            (sender as MediaElement).Stop();
            (sender as MediaElement).Play();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            Source.Play();
            Output.Play();
        }

        private void Pause(object sender, RoutedEventArgs e)
        {
            Source.Pause();
            Output.Pause();
        }
    }
 
}
