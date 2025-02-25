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
        DataTable dt;
        public event EventHandler<string> OperationCompleted;

        public MainForm(SldWorks swApp_)
        {
            InitializeComponent();
            swApp = swApp_;
        
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (sender, e) =>
            {
               
                SW.swApp = swApp;
                SW.GetRootComponent();
                SW.GetBomTable();
                Tree.SearchParentFromChild();
                Tree.FillCollection();
                Tree.CompareVersions();
            };

            backgroundWorker.RunWorkerCompleted += (sender, e) =>
            {
                this.dataGridView.AutoGenerateColumns = true;
                DataTable dt = new DataTable();
                Tree.FillToListIsRebuild(ref dt);
                FillDataGridView(dt);
                OperationCompleted.Invoke(this, "Operation complete");
            };

            backgroundWorker.RunWorkerAsync();

        }

        

        private void MainForm_Load(object sender, EventArgs e)
        {

         
            
        }

        public void FillDataGridView(DataTable dt)
        {
            
            dataGridView.Cursor = Cursors.WaitCursor;
            this.bindingSource1.DataSource = dt;
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            { dataGridView.Columns[i].ReadOnly = true; }

            dataGridView.Cursor = Cursors.Default;
        }

        private void btn_Rebuild_Click(object sender, EventArgs e)
        {
            List<Component> list = Tree.listComp.Where(c => c.IsRebuild == true).ToList();
            PDM.BatchGet(list);
            if (Rebuild != null)
            {
                Rebuild.Invoke();
            }
        }
    }
}
