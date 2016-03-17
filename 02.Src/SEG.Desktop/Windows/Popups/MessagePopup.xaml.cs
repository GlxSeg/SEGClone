using AForge.Video.DirectShow;
using SEG.Desktop.Control;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using TakeSnapsWithWebcamUsingWpfMvvm.Video;

namespace SEG.Desktop.Windows.Popups
{
    /// <summary>
    /// Interaction logic for MessagePopup.xaml
    /// </summary>
    public partial class MessagePopup : Window
    {

        public MessagePopup()
        {
            InitializeComponent();
        }

        public void SetSize()
        {
            // This routine sets up the popup size based on the screen size
            double sW = ControlCenter.Instance.wMain.Width;
            double sH = ControlCenter.Instance.wMain.Height;
            double sX = ControlCenter.Instance.wMain.Left;
            double sY = ControlCenter.Instance.wMain.Top;

            double x = sX + (sW - Width) / 2;
            double y = sY + (sH - Height) / 2;

            this.Left = x;
            this.Top = y;
        }

        public void Setup(string sTitle, string sContent, bool hasCancel)
        {
            labTitle.Content = sTitle;
            labContent.Text = sContent;
            if(!hasCancel)
            {
                cbCancel.Visibility = Visibility.Hidden;
            }
        }

        private void cbCancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void cbOk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
