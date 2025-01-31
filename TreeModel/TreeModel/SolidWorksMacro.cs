using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace TreeModel
{
    public partial class SolidWorksMacro
    {
        PDM pdm = null;
        
        public void Main()
        {
            ModelDoc2 swModel = default(ModelDoc2); 
            ModelDocExtension Ext = default(ModelDocExtension);
            BomTableAnnotation swBOMAnnotation = default(BomTableAnnotation);
            BomFeature swBOMFeature = default(BomFeature);
           


            Configuration swConf;
            Component2 swRootComp;
            string Configuration = null;
            string TemplateName = null;
            int nbrType = 0;
            int BomType = 0;
            int nErrors = 0;
            int nWarnings = 0;
            bool boolstatus = false;

            List<Part> list = new List<Part>();
            pdm = new PDM();
            swModel = (ModelDoc2)swApp.ActiveDoc;
            Ext = swModel.Extension;
            // Insert BOM table
            TemplateName = @"C:\CUBY_PDM\library\templates\BOM Templates\FullBOM_template.sldbomtbt";
            BomType = (int)swBomType_e.swBomType_Indented;
            Configuration = ".";
            nbrType = (int)swNumberingType_e.swNumberingType_Detailed;

            swBOMAnnotation = (BomTableAnnotation)Ext.InsertBomTable3(TemplateName, 0, 0, BomType, Configuration, false, nbrType, false);
            swBOMFeature = (BomFeature)swBOMAnnotation.BomFeature;

            TableAnnotation swTableAnn = (TableAnnotation)swBOMAnnotation;
            int nNumRow = 0;
            int J = 0;
            int I = 0;
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
                string[] str= (string[])swBOMAnnotation.GetModelPathNames(J, out ItemNumber, out PartNumber);


                PathName = str[0];
                designation = Path.GetFileNameWithoutExtension(PathName);
                string regCuby = @"^CUBY-\d{8}$";
                bool IsCUBY = Regex.IsMatch(PartNumberTrim, regCuby);
                if (!IsCUBY) continue;
                Part part = new Part(ItemNumber, PartNumber, PathName);
              
                list.Add(part);
            }

            GroupByCol(list);

           
            boolstatus = Ext.SelectByID2(BomName, "ANNOTATIONTABLES", 0, 0, 0, false, 0, null, 0);
            swModel.ClearSelection2(true);
            return;
        }

        private void Tree_IsEventRebuild(string arg1, string arg2, int arg3)
        {
            throw new NotImplementedException();
        }

        void GroupByCol(List<Part> list)
        {
            var collection = list.GroupBy(p => p.StructureNumber.Length);
          
            foreach (var company in collection)
            {              
                foreach (var person in company.Distinct(new CompPart()))
                {
                    pdm.GetEdmFile(person);
                }
                
            }
        }
      
        public SldWorks swApp;

    }
}

