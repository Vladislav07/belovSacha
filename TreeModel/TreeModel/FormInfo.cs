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
        public FormInfo(List<Component> tree_)
        {
            InitializeComponent();
            tree = tree_;
            FillDataGridView();
            MessageBox.Show("ooo");

        }
        
        public  void FillDataGridView()
        {
            dataGridView.ColumnCount = 7;
            dataGridView.Columns[0].Name = "Structure Number";
            dataGridView.Columns[1].Name = "Cuby Number";
            dataGridView.Columns[2].Name = "Current Version";
            dataGridView.Columns[3].Name = "List of Ref Child Errors";
            dataGridView.Columns[4].Name = "Child";
            dataGridView.Columns[5].Name = "Child info";
            dataGridView.Columns[6].Name = "State";

            foreach (Component comp in tree)
            {
                dataGridView.Rows.Add(comp.StructureNumber, comp.CubyNumber, comp.CurVersion, comp.IsRebuild.ToString(), "", "", comp.State);
                if (comp.listRefChildError != null)
                {
                    foreach (KeyValuePair<string, string> i in comp.listRefChildError)
                    {
                        dataGridView.Rows.Add("", "", "", "", i.Key, i.Value);
                    }
                }
            }
        }

        private void FormInfo_Load(object sender, EventArgs e)
        {
            
        }

        private void btnRebuild_Click(object sender, EventArgs e)
        {

        }
    }
}
