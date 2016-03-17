using SEG.Desktop.Control;
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

namespace SEG.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for SEGAssetList_AddButton.xaml
    /// </summary>
    public partial class SEGAssetList_AddButton : UserControl
    {
        public SEGAssetList_AddButton()
        {
            InitializeComponent();

            Thickness margin = Margin;
            margin.Left = 10;
            margin.Top = 10;
            margin.Bottom = 10;
            margin.Right = 10;
            Margin = margin;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.AddAsset();
        }
    }
}
