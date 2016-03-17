using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using SEG.Domain.Model;

namespace SEG.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for SEGDiagnosticSubSection.xaml
    /// </summary>
    public partial class SEGDiagnosticSubSection : UserControl
    {
        ItemSubSection cSS;

        public SEGDiagnosticSubSection()
        {
            InitializeComponent();
        }

        public void Initialize(ItemSubSection iSS, double cW)
        {
            cSS = iSS;

            Thickness margin = Margin;
            margin.Top = 10;
            margin.Bottom = 10;
            Margin = margin;

            double lbx = Canvas.GetLeft(labBlock);
            labBlock.Width = cW - lbx - 20;

            string[] sblocks = cSS.Code.Split('|');
            if (sblocks.Length > 1)
            {
                labCode.Content = sblocks[0];
                labCode2.Visibility = Visibility.Visible;
                labCode2.Content = sblocks[1];
            }
            else
            {
                labCode.Content = cSS.Code;
                labCode2.Visibility = Visibility.Hidden;
            }

            //string sc = cSS.Code;
            //labCode.Content = sc;

            labContent.Text = cSS.Title;

            this.Height = 20 + labCode.Height;
        }

        public void SetSize(double cW)
        {
            this.Width = cW;
            rTop.Width = cW - 10;

            double lbx = Canvas.GetLeft(labBlock);
            labBlock.Width = cW - lbx - 20;

            this.Height = 20 + labBlock.ActualHeight;
        }

        private void labBlock_LayoutUpdated(object sender, EventArgs e)
        {
            this.Height = 20 + labBlock.ActualHeight;
        }
    }
}
