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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TakeSnapsWithWebcamUsingWpfMvvm.Video;

namespace SEG.Desktop.Windows.Popups
{
    /// <summary>
    /// Interaction logic for AddAssetPopup.xaml
    /// </summary>
    public partial class AddAreaPopup : Window
    {

        public AddAreaPopup()
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

        public string mode;
        public void SetupNew()
        {
            labTitle.Content = "Add New Area";
            mode = "new";
            eID.Text = "";
        }

        public void SetupEdit(string oldName)
        {
            labTitle.Content = "Edit Area Name";
            mode = "edit";
            eID.Text = oldName;
        }

        private void cbCancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void cbOk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mode == "new")
            {
                if (eID.Text.Trim() == "")
                {
                    MessagePopup mp = new MessagePopup();
                    mp.Setup("Add New Area", "Please enter a non-empty Area name.", false);
                    mp.SetSize();
                    mp.ShowDialog();
                }
                else
                {
                    if (ProjectHelper.CheckUniqueAreaName(ControlCenter.Instance.segR, eID.Text.Trim()))
                    {
                        ControlCenter.Instance.cNewAreaName = eID.Text.Trim();
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        MessagePopup mp = new MessagePopup();
                        mp.Setup("Add New Area", "Please enter a new Area name (Area with the same name already exists).", false);
                        mp.SetSize();
                        mp.ShowDialog();
                    }
                }
            }
            else
            {
                if (eID.Text.Trim() == "")
                {
                    MessagePopup mp = new MessagePopup();
                    mp.Setup("Edit Area Name", "Please enter a non-empty new Area name.", false);
                    mp.SetSize();
                    mp.ShowDialog();
                }
                else
                {
                    ControlCenter.Instance.cNewAreaName = eID.Text.Trim();
                    DialogResult = true;
                    Close();
                }
            }

        }
    }
}
