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
using SEG.Domain.Printing;
using System.IO;

namespace SEG.Desktop.Windows.Panels
{
    /// <summary>
    /// Interaction logic for Project_New.xaml
    /// </summary>
    public partial class Project_New : UserControl
    {
        public string sPName;
        public string sPCode;        

        System.Drawing.Bitmap bitProject;

        public Project_New()
        {
            InitializeComponent();
        }

        public void Update()
        {
            // Set buttons for mode
            labNumAreas.Content = "No Data";
            labNumAssets.Content = "No Data";
            bitProject = null;
        }

        public void SetSize(double cW, double cH)
        {
            this.Width = cW;
            this.Height = cH;
        }



        private void cbCreateProject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(eName.Text.Trim() == "")
            {
                MessagePopup mp = new MessagePopup();
                mp.Setup("Create Project", "Please enter a valid new Project name.", true);
                mp.SetSize();
                mp.ShowDialog();
            }
            else
            {
                if(bitProject==null)
                {
                    MessagePopup mp = new MessagePopup();
                    mp.Setup("Create Project", "Please include a Project display image.", true);
                    mp.SetSize();
                    mp.ShowDialog();
                }
                else
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                    var img = ImageHelper.SaveImage(ControlCenter.Instance.segR, imgFilename);
                    Guid pId = Guid.Empty;

                    using (var seg = ControlCenter.Instance.segR.GetContext())
                    {
                        var p = new Project();
                        p.Active = true;
                        p.ImageId = img;
                        p.Name = eName.Text.Trim();
                        seg.Projects.Add(p);
                        seg.SaveChanges();
                        pId = p.Id;

                        var pI = new ProjectInfo();
                        pI.Key = "CODE";
                        pI.ProjectId = p.Id;
                        pI.Value = eCode.Text.Trim();
                        seg.ProjectInfos.Add(pI);
                        seg.SaveChanges();
                    }

                    List<Guid> areaIds = new List<Guid>();
                    for(int i=0;i<csv_Areas.Count;i++)
                        areaIds.Add(ProjectHelper.AddArea(ControlCenter.Instance.segR, pId, csv_Areas[i]));

                    // Now add Assets for each area
                    for(int ia=0;ia<csv_AssetArea.Count;ia++)
                    {
                        ProjectHelper.AddAsset(ControlCenter.Instance.segR, areaIds[csv_AssetArea[ia]], Guid.Empty,
                            csv_AssetID[ia], csv_AssetTag[ia], csv_AssetLoc[ia], csv_AssetDesc[ia]);
                    }

                    Mouse.OverrideCursor = null;

                    var mp2 = new MessagePopup();
                    mp2.Setup("Project Management", "The new Project was added successfully.", false);
                    mp2.SetSize();
                    mp2.ShowDialog();

                    ControlCenter.Instance.SelectProjectList();
                }
            }
        }

        public bool csv_Data;
        public List<string> csv_Areas;
        public List<int> csv_AssetArea;
        public List<string> csv_AssetID;
        public List<string> csv_AssetTag;
        public List<string> csv_AssetLoc;
        public List<string> csv_AssetDesc;

        private void cbSelectFile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool isOk = true;
            csv_Data = false;
            csv_Areas = new List<string>();
            csv_AssetArea = new List<int>();
            csv_AssetID = new List<string>();
            csv_AssetTag = new List<string>();
            csv_AssetLoc = new List<string>();
            csv_AssetDesc = new List<string>();
            
            // First, try to load the CSV file.  It should be a text file, and should load without issues
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text Files (*.txt)|*.txt";

                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox 
                if (result == true)
                {
                    List<string> slines = new List<string>();
                    string filename = dlg.FileName;
                    StreamReader sr = new StreamReader(filename, System.Text.Encoding.GetEncoding(1252), true);
                    sr.ReadLine();
                    int ian = 0;
                    int ican = -1;
                    
                    bool done = false;
                    while(!sr.EndOfStream && !done)
                    {
                        string s = sr.ReadLine();
                        char[] delimiters = new char[] { '\t' };
                        string[] parts = s.Split(delimiters,
                                         StringSplitOptions.RemoveEmptyEntries);

                        // Check to see if the line is valid

                        //Assets #,Area #,Area,ID,Tag,Location ,Description,,,,,,
                        //1,1,INJEÇÃO,0002,INJT01,INJEÇÃO,INJETORA HIMACO 1800-1080 LHN ,,,,,,
                        if(parts.Length>0)
                        {
                            ian++;
                            if (Convert.ToInt32(parts[0]) != ian)
                            {
                                isOk = false;
                                done = true;
                            }
                            else
                            {
                                if(ican != Convert.ToInt32(parts[1])-1)
                                {
                                    ican++;
                                    csv_Areas.Add(parts[2].Trim());
                                }
                                csv_AssetArea.Add(ican);
                                csv_AssetID.Add(parts[3].Trim());
                                csv_AssetTag.Add(parts[4].Trim());
                                csv_AssetLoc.Add(parts[5].Trim());
                                string s6=parts[6].Trim();
                                if(s6[0]=='"')
                                    s6=s6.Substring(1,s6.Length-2);
                                csv_AssetDesc.Add(s6);
                            }
                        }                        
                    }
                    if(!isOk)
                    {
                        MessagePopup mp = new MessagePopup();
                        mp.Setup("Create Project", "The CSV file has incorrect data.", true);
                        mp.SetSize();
                        mp.ShowDialog();
                    }
                    else
                    {
                        csv_Data = true;
                        labNumAreas.Content = csv_Areas.Count + " Areas";
                        labNumAssets.Content = csv_AssetArea.Count + " Assets";
                    }
                }

            }
            catch(Exception ex)
            {
                MessagePopup mp = new MessagePopup();
                mp.Setup("Create Project", "The CSV file could not be loaded.", true);
                mp.SetSize();
                mp.ShowDialog();
            }
            

        }


        public string imgFilename;

        private void cbSelectImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                dlg.DefaultExt = ".jpg";
                dlg.Filter = "Image Files (*.jpeg,*.jpg,*.png)|*.jpeg;*.png;*.jpg";

                iMain.Source = null;

                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox 
                if (result == true)
                {
                    // Open document 
                    imgFilename = dlg.FileName;

                    // Load the image and crop it if needed
                    bitProject = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(imgFilename);

                    iMain.Source = ImageHelper.Bitmap2BitmapSource(bitProject);
                    iMain.Stretch = Stretch.Uniform;
                    cImgBox.Background = new SolidColorBrush(Colors.White);
                }
                else
                {
                }

            }
            catch (Exception ex)
            {
                
            }
        }



        public void UpdateData()
        {

        }

    }
}
