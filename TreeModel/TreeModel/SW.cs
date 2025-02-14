using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeModel
{
    
    public static class SW
    {
        public static SldWorks swApp;
        static ModelDoc2 swModel = default(ModelDoc2);
        static ModelDocExtension Ext = default(ModelDocExtension);
        static BomFeature swBOMFeature = default(BomFeature);

        static SW()
        {

        }

        public static void GetRootComponent()
        {
            swModel = (ModelDoc2)swApp.ActiveDoc;
            Ext = swModel.Extension;
            string rootPath = swModel.GetPathName();
            string nameRoot = Path.GetFileNameWithoutExtension(rootPath);
            Tree.AddNode("0", nameRoot, rootPath);
        }
 
        public static void GetBomTable()
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
           // MessageBox.Show(BomName);
            nNumRow = swTableAnn.RowCount;
            for (J = 0; J <= nNumRow - 1; J++)
            {
                swBOMAnnotation.GetComponentsCount2(J, Configuration, out ItemNumber, out PartNumber);

                if (PartNumber == null) continue;
                string PartNumberTrim = PartNumber.Trim();
                if (PartNumberTrim == "") continue;
                string[] str = (string[])swBOMAnnotation.GetModelPathNames(J, out ItemNumber, out PartNumber);
                PathName = str[0];
                designation = Path.GetFileNameWithoutExtension(PathName);
               // string regCuby = @"^CUBY-\d{8}$";
              //  bool IsCUBY = Regex.IsMatch(PartNumberTrim, regCuby);
              //  if (!IsCUBY) continue;
                e = Path.GetExtension(PathName);
                string AddextendedNumber = "0." + ItemNumber;
                if (e == ".SLDPRT" || e == ".sldprt" || e == ".SLDASM" || e == ".sldasm")
                {

                    Tree.AddNode(AddextendedNumber, PartNumberTrim, PathName);
                }

            }
            int i = BomName.Length;
            string numberTable = BomName.Substring(17);
            boolstatus = Ext.SelectByID2("DetailItem" + numberTable + "@Annotations", "ANNOTATIONTABLES", 0, 0, 0, false, 0, null, 0);
            swModel.EditDelete();
            swModel.ClearSelection2(true);
        }

    }
}
