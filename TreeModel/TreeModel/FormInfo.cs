using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeModel
{
    public partial class FormInfo : Form
    {
        List<Component> tree;
        public event Action Action;
        public FormInfo(List<Component> tree_)
        {
            InitializeComponent();
            tree = tree_;
            MessageBox.Show("Компонентов к перестроению" + tree.Count.ToString());
            if (tree.Count == 0) return;
            FillDataGridView();
            

        }
        
        public  void FillDataGridView()
        {
            
            dataGridView.Cursor = Cursors.WaitCursor;
            dataGridView.ColumnCount = 7;
            dataGridView.Columns[0].Name = "Structure Number";
            dataGridView.Columns[1].Name = "Cuby Number";
            dataGridView.Columns[2].Name = "Current Version";
            dataGridView.Columns[3].Name = "List of Ref Child Errors";
            dataGridView.Columns[4].Name = "Child";
            dataGridView.Columns[5].Name = "Child info";
            dataGridView.Columns[6].Name = "State";

            if (tree.Count == 0)
            {
                btnRebuild.Enabled = false;
            }
            foreach (Component comp in tree)
            {
                dataGridView.Rows.Add(comp.StructureNumber, comp.CubyNumber, comp.CurVersion.ToString(), comp.IsRebuild.ToString(), "", "", comp.State.Name.ToString());
                if (comp.listRefChildError != null)
                {
                    foreach (KeyValuePair<string, string> i in comp.listRefChildError)
                    {
                        dataGridView.Rows.Add("", "", "", "", i.Key, i.Value);
                    }
                }
            }
            dataGridView.Cursor = Cursors.Default;
        }

        private void FormInfo_Load(object sender, EventArgs e)
        {
            
        }

        private void btnRebuild_Click(object sender, EventArgs e)
        {
            
            Action.Invoke();
             this.Close();
        }
    }
}
