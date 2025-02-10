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
    public partial class Form1 : Form
    {
        private List<Component> comp;
        private BindingSource bindingSource;
        private ListView listView;

        public Form1(List<Component> list)
        {
            comp = list;

            listView = new ListView();
            listView.Dock = DockStyle.Fill;

            bindingSource = new BindingSource();
            bindingSource.DataSource = comp;


            listView.Columns.Add("Name");
            foreach (Component component in comp)
            {
                ListViewItem item = new ListViewItem();


                item.Text = component.CubyNumber;
                    
                
                listView.Items.Add(item);
            }

            Controls.Add(listView);
        }

    }
 }

