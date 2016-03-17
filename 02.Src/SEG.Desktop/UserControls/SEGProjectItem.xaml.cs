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
    /// Interaction logic for SEGProjectItem.xaml
    /// </summary>
    public partial class SEGProjectItem : UserControl
    {
        public Project cP;
        public bool isHighlighted;

        public SEGProjectItem()
        {
            InitializeComponent();
            cP = null;

            Thickness margin = Margin;
            margin.Left = 10;
            margin.Top = 10;
            margin.Bottom = 10;
            margin.Right = 10;
            Margin = margin;
        }

        public void Initialize(Project aP, bool highlighted)
        {
            cP = aP;
            SEG.Domain.Model.Image img = ImageHelper.GetImage(ControlCenter.Instance.segR, cP.ImageId);

            isHighlighted = highlighted;
            iThumb.Stretch = Stretch.Uniform;
            iThumb.Source = img.ThumbDataImage;
            labName.Content = aP.Name;

            if (highlighted)
            {
                c.Background = new SolidColorBrush(Color.FromArgb(255, 255, 203, 4));
            }
            else
            {
                c.Background = new SolidColorBrush(Color.FromArgb(255, 250, 165, 25));
            }
        }

        private void c_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.SelectProject(cP.Id);
        }
    }
}
