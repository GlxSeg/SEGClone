using AForge.Video.DirectShow;
using SEG.Desktop.Control;
using SEG.Domain.Helpers;
using SEG.Domain.Model;
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
    /// Interaction logic for SelectAssetPopup.xaml
    /// </summary>
    public partial class SelectAreaPopup : Window
    {

        public SelectAreaPopup()
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

        int iArea = -1;
        List<ProjectArea> areas;        

        public void Setup()
        {
            areas = ProjectHelper.GetProjectAreas(ControlCenter.Instance.segR, ControlCenter.Instance.cProjectId);
            iArea = -1;

            ddlAreas.Items.Clear();
            foreach(var a in areas)
                ddlAreas.Items.Add(a.Name);
            ddlAreas.SelectedIndex = -1;

            cbOk.Background = new SolidColorBrush(Colors.LightGray);
            lbOk.Foreground = new SolidColorBrush(Colors.Silver);
        }

        private void ddlAreas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ddlAreas.SelectedIndex>-1)
            {
                iArea = ddlAreas.SelectedIndex;

                cbOk.Background = new SolidColorBrush(Colors.Silver);
                lbOk.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void cbCancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void cbOk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (iArea > -1)
            {
                ControlCenter.Instance.cCopyToAreaId = areas[iArea].Id;
                DialogResult = true;
                Close();
            }
        }
    }
}
