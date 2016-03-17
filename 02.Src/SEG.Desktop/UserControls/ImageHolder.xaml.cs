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

namespace SEG.Desktop.Windows.Panels
{
    /// <summary>
    /// Interaction logic for ImageHolder.xaml
    /// </summary>
    public partial class ImageHolder : UserControl
    {
        public SEG.Domain.Model.Image i;
        public bool isSelected;
        public bool isAssetImg;

        public ImageHolder()
        {
            InitializeComponent();
        }

        public void Initialize(SEG.Domain.Model.Image ai)
        {
            // Visual settings
            Thickness margin = Margin;
            margin.Left = 10;
            margin.Right = 10;
            margin.Top = 10;
            margin.Bottom = 10;
            Margin = margin;

            i = ai;
            img.Source = i.ThumbDataImage;

            // Init unselected
            isSelected = false;
            Update();
        }

        public void Update()
        {
            isAssetImg = ControlCenter.Instance.cAsset.ImageId == i.Id;
            if (isAssetImg)
            {
                rcI.Fill = new SolidColorBrush(Color.FromArgb(255, 250, 165, 25));
            }
            else
            {
                rcI.Fill = new SolidColorBrush(Colors.White);
            }

            if (isSelected)
            {
                rcB.Stroke = new SolidColorBrush(Colors.Black);
            }
            else
            {
                rcB.Stroke = new SolidColorBrush(Colors.White);
            }
        }

        private void c_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.imgHolderClick = this;
            ControlCenter.Instance.Asset_SelectImage();
        }
    }
}
