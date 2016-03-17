using AForge.Video.DirectShow;
using SEG.Desktop.Control;
using SEG.Domain.Helpers;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TakeSnapsWithWebcamUsingWpfMvvm.Video;

namespace SEG.Desktop.Windows.Popups
{
    /// <summary>
    /// Interaction logic for AddAssetPopup.xaml
    /// </summary>
    public partial class AddAssetPopup : Window
    {

        public AddAssetPopup()
        {
            InitializeComponent();
            //this.Loaded += EnableKeyboard;
        }

        //private void EnableKeyboard(object sender, RoutedEventArgs e)
        //{
        //    // Enables WPF to mark edit field as supporting text pattern (Automation Concept)
        //    System.Windows.Automation.AutomationElement asForm =
        //        System.Windows.Automation.AutomationElement.FromHandle(new WindowInteropHelper(this).Handle);

        //    // Windows 8 API to enable touch keyboard to monitor for focus tracking in this WPF application
        //    InputPanelConfigurationLib.InputPanelConfiguration inputPanelConfig = new InputPanelConfigurationLib.InputPanelConfiguration();
        //    inputPanelConfig.EnableFocusTracking();
        //}

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

        public void Setup()
        {
            eID.Text = "";
            eTAG.Text = "";
        }

        private void cbCancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void cbOk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(eID.Text.Trim() == "" || eTAG.Text.Trim() == "")
            {
                MessagePopup mp = new MessagePopup();
                mp.Setup("Add New Equipment", "Please enter valid ID and TAG information.", false);
                mp.SetSize();
                mp.ShowDialog();
            }
            else
            {
                if (ProjectHelper.CheckUniqueAssetID(ControlCenter.Instance.segR, eID.Text.Trim()))
                {
                    ControlCenter.Instance.cNewAssetId = eID.Text.Trim();
                    ControlCenter.Instance.cNewAssetTag = eTAG.Text.Trim();
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessagePopup mp = new MessagePopup();
                    mp.Setup("Add New Equipment", "Please enter valid ID (ID already exists in the database for this project).", false);
                    mp.SetSize();
                    mp.ShowDialog();
                }
            }

        }
    }
}
