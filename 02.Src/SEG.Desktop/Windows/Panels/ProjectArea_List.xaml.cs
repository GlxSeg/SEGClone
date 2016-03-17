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
using SEG.Desktop.Windows.Popups;

namespace SEG.Desktop.Windows.Panels
{
    /// <summary>
    /// Interaction logic for ProjectArea_List.xaml
    /// </summary>
    public partial class ProjectArea_List : UserControl
    {
        List<ProjectArea> projectAreas;

        public ProjectArea_List()
        {
            InitializeComponent();
        }

        public void Update()
        {
            sPanel.Children.Clear();

            // Add new items one at a time
            projectAreas = ProjectHelper.GetProjectAreas(ControlCenter.Instance.segR,
                                                         ControlCenter.Instance.cProjectId);

            foreach (ProjectArea a in projectAreas) 
            {
                SEGAreaItem spa = new SEGAreaItem();
                spa.Width = 200;
                spa.Height = 240;
                bool selected = false;
                if (ControlCenter.Instance.cProjectAreaId == a.Id)
                    selected = true;
                spa.Initialize(a, selected);

                sPanel.Children.Add(spa);
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

            Canvas.SetLeft(cbDelete, cW - cbDelete.Width - 20);
        }

        private void cbAddArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.AddArea();
        }

        private void cbDelete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (projectAreas.Count > 0)
            {
                MessagePopup mp = new MessagePopup();
                mp.Setup("Delete Project", "Please delete all areas first.  Only empty projects can be deleted from the system.", false);
                mp.SetSize();
                mp.ShowDialog();
            }
            else
            {
                MessagePopup mp = new MessagePopup();
                mp.Setup("Delete Project", "Are you sure you want to delete the Project?", true);
                mp.SetSize();
                if (mp.ShowDialog() == true)
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                    // We copy the diagnostics
                    ProjectHelper.DeleteProject(ControlCenter.Instance.segR, ControlCenter.Instance.cProjectAreaId);

                    Mouse.OverrideCursor = null;

                    mp = new MessagePopup();
                    mp.Setup("Project Management", "The Project was deleted", false);
                    mp.SetSize();
                    mp.ShowDialog();

                    ControlCenter.Instance.SelectProjectList();
                }
            }
        }
    }
}
