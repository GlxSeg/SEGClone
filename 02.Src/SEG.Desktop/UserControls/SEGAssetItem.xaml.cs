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
using SEG.Desktop.Control;
using SEG.Domain.Model;
using SEG.Domain.Helpers;

namespace SEG.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for SEGAssetItem.xaml
    /// </summary>
    public partial class SEGAssetItem : UserControl
    {
        public Asset cA;
        public bool isHighlighted;

        public SEGAssetItem()
        {
            InitializeComponent();

            Thickness margin = Margin;
            margin.Left = 10;
            margin.Top = 10;
            margin.Bottom = 10;
            margin.Right = 10;
            Margin = margin;
        }

        public void Initialize(Asset a, bool highlighted)
        {
            cA = a;
            List<AssetInfo> assetInfo = ProjectHelper.GetAssetInfo(ControlCenter.Instance.segR, cA.Id);
            SEG.Domain.Model.Image img = ImageHelper.GetImage(ControlCenter.Instance.segR, cA.ImageId);

            isHighlighted = highlighted;
            if (img != null)
            {
                iThumb.Visibility = Visibility.Visible;
                iThumb.Stretch = Stretch.Uniform;
                iThumb.Source = img.ThumbDataImage;
                labNoImg.Visibility = Visibility.Hidden;
            }
            else
            {
                iThumb.Visibility = Visibility.Hidden;
                labNoImg.Visibility = Visibility.Visible;
            }
            labName.Content = assetInfo.FirstOrDefault(x => x.Key == "ID").Value;
            labName2.Content = assetInfo.FirstOrDefault(x => x.Key == "TAG").Value;

            if (highlighted)
            {
                c.Background = new SolidColorBrush(Color.FromArgb(255, 255, 203, 4));
            }
            else
            {
                c.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void c_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.SelectAsset(cA.Id);
        }
    }
}
