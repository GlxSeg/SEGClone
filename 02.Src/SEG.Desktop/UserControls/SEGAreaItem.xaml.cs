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
    /// Interaction logic for SEGAreaItem.xaml
    /// </summary>
    public partial class SEGAreaItem : UserControl
    {
        public ProjectArea cPA;
        public bool isHighlighted;

        public SEGAreaItem()
        {
            InitializeComponent();
            cPA = null;

            Thickness margin = Margin;
            margin.Left = 10;
            margin.Top = 10;
            margin.Bottom = 10;
            margin.Right = 10;
            Margin = margin;
        }

        public void Initialize(ProjectArea aPA, bool highlighted)
        {
            cPA = aPA;
            isHighlighted = highlighted;
            if (cPA.ImageId != Guid.Empty)
            {
                cImg.Visibility = Visibility.Visible;
                iThumb.Stretch = Stretch.Uniform;
                SEG.Domain.Model.Image img = ImageHelper.GetImage(ControlCenter.Instance.segR, cPA.ImageId);
                iThumb.Source = img.ThumbDataImage;

                labName.Height = 40;
                Canvas.SetTop(labName, 190);
            }
            else
            {
                cImg.Visibility = Visibility.Hidden;
                labName.Height = 220;
                Canvas.SetTop(labName,10);
            }
            labName.Content = aPA.Name;

            if (highlighted)
            {
                c.Background = new SolidColorBrush(Color.FromArgb(255, 255, 203,   4));
            }
            else
            {
                c.Background = new SolidColorBrush(Color.FromArgb(255, 250, 165,  25));
            }
        }

        private void c_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.SelectArea(cPA.Id);
        }
    }
}
