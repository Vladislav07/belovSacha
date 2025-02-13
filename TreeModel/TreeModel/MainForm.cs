using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeModel
{
    public partial class MainForm : Form
    {
        SldWorks swApp;
        public event Action Rebuild;
        ModelDoc2 swModel = default(ModelDoc2);
        ModelDocExtension Ext = default(ModelDocExtension);
        
        BomFeature swBOMFeature = default(BomFeature);

        public MainForm(SldWorks swApp_)
        {
            InitializeComponent();
            swApp = swApp_;
        }

        void GetRootComponent()
        {
            swModel = (ModelDoc2)swApp.ActiveDoc;
            Ext = swModel.Extension;
            string rootPath = swModel.GetPathName();
            string nameRoot = Path.GetFileNameWithoutExtension(rootPath);
            Tree.AddNode("0",nameRoot,rootPath);
        }

        void GetBomTable()
        {
          BomTableAnnotation swBOMAnnotation = default(BomTableAnnotation);
            string Configuration = ".";
            string TemplateName = "C:\\CUBY_PDM\\library\\templates\\Спецификация.sldbomtbt";
            int nbrType = (int)swNumberingType_e.swNumberingType_Detailed;
            int BomType = (int)swBomType_e.swBomType_Indented;
           // Stopwatch stopwatch = new Stopwatch();
           // stopwatch.Start();
            swBOMAnnotation = Ext.InsertBomTable3(TemplateName, 0, 0, BomType, Configuration, false, nbrType, false);
            //stopwatch.Stop();
           // MessageBox.Show(stopwatch.Elapsed.ToString());
            swBOMFeature = swBOMAnnotation.BomFeature;

            TableAnnotation swTableAnn = (TableAnnotation)swBOMAnnotation;
            int nNumRow = 0;
            int J = 0;
            string ItemNumber = null;
            string PartNumber = null;
            string PathName;
            string e;
            string designation;
            string BomName;
            bool boolstatus = false;

            BomName = swBOMFeature.Name;
            nNumRow = swTableAnn.RowCount;
            for (J = 0; J <= nNumRow - 1; J++)
            {      
                swBOMAnnotation.GetComponentsCount2(J, Configuration, out ItemNumber, out PartNumber);

                if (PartNumber == null) continue;
                string PartNumberTrim = PartNumber.Trim();
                string[] str = (string[])swBOMAnnotation.GetModelPathNames(J, out ItemNumber, out PartNumber);
                PathName = str[0];
                designation = Path.GetFileNameWithoutExtension(PathName);
                string regCuby = @"^CUBY-\d{8}$";
                bool IsCUBY = Regex.IsMatch(PartNumberTrim, regCuby);
                if (!IsCUBY) continue;
                e = Path.GetExtension(PathName);
                string AddextendedNumber = "0." + ItemNumber;
                if (e == ".SLDPRT" || e == ".sldprt" || e == ".SLDASM" || e == ".sldasm")
                {
     
                    Tree.AddNode(AddextendedNumber, PartNumberTrim,PathName);
                }

            }
            boolstatus = Ext.SelectByID2(BomName, "ANNOTATIONTABLES", 0, 0, 0, false, 0, null, 0);
            swModel.ClearSelection2(true);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GetRootComponent();
            GetBomTable();
            Tree.FillCollection();
            FillDataGridView();
        }

        public void FillDataGridView()
        {

            dataGridView.Cursor = Cursors.WaitCursor;
            dataGridView.ColumnCount = 7;
            dataGridView.Columns[0].Name = "Level";
            dataGridView.Columns[1].Name = "Cuby Number";
            dataGridView.Columns[2].Name = "Current Version";
            dataGridView.Columns[3].Name = "List of Ref Child Errors";
            dataGridView.Columns[4].Name = "Child";
            dataGridView.Columns[5].Name = "Child info";
            dataGridView.Columns[6].Name = "State";
            int level = 0;

                foreach (Component comp in Tree.listComp)
                {
                    dataGridView.Rows.Add(comp.Level.ToString(),comp.CubyNumber, comp.CurVersion.ToString(), comp.IsRebuild.ToString(), "", "", "");
                    if (comp.listRefChildError != null)
                    {
                        foreach (KeyValuePair<string, string> i in comp.listRefChildError)
                        {
                            dataGridView.Rows.Add("", "","", "", "", i.Key, i.Value);
                        }
                    }
                    level++;
                }
              
            dataGridView.Cursor = Cursors.Default;
        }
            
         
        
    }
}
