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
using SEG.Desktop.UserControls;
using SEG.Domain.Model;
using SEG.Domain.Helpers;

namespace SEG.Desktop.Windows.Panels
{
    /// <summary>
    /// Interaction logic for Project_List.xaml
    /// </summary>
    public partial class Project_List : UserControl
    {
        List<Project> projects;

        public Project_List()
        {
            InitializeComponent();
        }

        public void Update()
        {            
            sPanel.Children.Clear();

            // Add new items one at a time
            projects = ProjectHelper.GetProjects(ControlCenter.Instance.segR);

            foreach (Project p in projects)
            {
                SEGProjectItem sp = new SEGProjectItem();
                sp.Width = 200;
                sp.Height = 240;
                bool selected = false;
                if (ControlCenter.Instance.cProject != null && ControlCenter.Instance.cProjectId == p.Id)
                    selected = true;
                sp.Initialize(p, selected);

                sPanel.Children.Add(sp);
            }
        }

        public void SetSize(double cW, double cH)
        {
            this.Width = cW;
            this.Height = cH;

            cSubMenu.Width = cW;

            scrollView.Width = cW - 20;
            double svt = Canvas.GetTop(scrollView);
            scrollView.Height = cH - svt - 10;
        }

        private void cbAddProject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.AddProject();
        }

    }

}
