using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            Tree.AddNode("0",rootPath);
        }

        void GetBomTable()
        {
          BomTableAnnotation swBOMAnnotation = default(BomTableAnnotation);
            string Configuration = ".";
            string TemplateName = "C:\\CUBY_PDM\\library\\templates\\Спецификация.sldbomtbt";
            int nbrType = (int)swNumberingType_e.swNumberingType_Detailed;
            int BomType = (int)swBomType_e.swBomType_Indented;
            swBOMAnnotation = Ext.InsertBomTable3(TemplateName, 0, 0, BomType, Configuration, false, nbrType, false);
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
     
                    Tree.AddNode(AddextendedNumber,PathName);
                }

            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GetRootComponent();
            GetBomTable();

        }
    }
}
