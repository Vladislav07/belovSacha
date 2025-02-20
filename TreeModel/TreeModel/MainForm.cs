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
        public event Action Rebuild;
        SldWorks swApp;
     

        public MainForm(SldWorks swApp_)
        {
            InitializeComponent();
            swApp = swApp_;
            SW.swApp = swApp;
            SW.GetRootComponent();
            SW.GetBomTable();
            Tree.SearchParentFromChild();
            Tree.FillCollection();
            Tree.CompareVersions();

        }

        

        private void MainForm_Load(object sender, EventArgs e)
        {

            List < Component > list = Tree.FillToListIsRebuild();
            FillDataGridView(list);
            
        }

        public void FillDataGridView(List<Component> listComp)
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

                foreach (Component comp in listComp)
                {
                    dataGridView.Rows.Add(comp.Level.ToString(),comp.CubyNumber, comp.CurVersion.ToString(), comp.IsRebuild.ToString(), "", "", comp.State.Name);
                    if (comp.listRefChildError != null)
                    {
                     foreach (KeyValuePair<string, string> i in comp.listRefChildError)

                    {
                            dataGridView.Rows.Add("", "","", "", i.Key, i.Value, "");
                        }
                    }
                    level++;
                }
              
            dataGridView.Cursor = Cursors.Default;
        }

        private void btn_Rebuild_Click(object sender, EventArgs e)
        {
            List<Component> list = Tree.FillToListIsRebuild();
            PDM.BatchGet(list);
            if (Rebuild != null)
            {
                Rebuild.Invoke();
            }
        }
    }
}
