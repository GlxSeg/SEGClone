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
    /// Interaction logic for SEGDiagnosticSection.xaml
    /// </summary>
    public partial class SEGDiagnosticSection : UserControl
    {
        ItemSection cS;
        bool isFirst;

        public SEGDiagnosticSection()
        {
            InitializeComponent();
        }

        public void Initialize(ItemSection iS, bool aisFirst, double cW)
        {
            cS = iS;
            isFirst = aisFirst;

            Thickness margin = Margin;
            margin.Bottom = 10;
            if (isFirst)
                margin.Top = 20;
            else
                margin.Top = 10;

            Margin = margin;

            double lbx = Canvas.GetLeft(labBlock);
            labBlock.Width = cW - lbx - 20;

            string sc = cS.Code;
            labCode.Content = sc;

            labContent.Text = cS.Title;

            if (!isFirst)
                rTop.Visibility = Visibility.Visible;
            else
                rTop.Visibility = Visibility.Hidden;

            this.Height = 20 + labBlock.ActualHeight;
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
