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

namespace TreeModel
{
    public partial class SolidWorksMacro
    {
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

            swModel = (ModelDoc2)swApp.ActiveDoc;
            Ext = swModel.Extension;
            // Insert BOM table
            TemplateName = "C:\\Program Files\\SOLIDWORKS Corp\\SOLIDWORKS\\lang\\english\\bom-standard.sldbomtbt";
            BomType = (int)swBomType_e.swBomType_Indented;
            Configuration = ".";
            nbrType = (int)swNumberingType_e.swNumberingType_Detailed;

            swBOMAnnotation = (BomTableAnnotation)Ext.InsertBomTable3(TemplateName, 0, 0, BomType, Configuration, false, nbrType, true);
           // swBOMFeature = (BomFeature)swBOMAnnotation.BomFeature;

            TableAnnotation swTableAnn = (TableAnnotation)swBOMAnnotation;
            int nNumRow = 0;
            int J = 0;
            int I = 0;
            string ItemNumber = null;
            string PartNumber = null;

       

            nNumRow = swTableAnn.RowCount;
            for (J = 0; J <= nNumRow - 1; J++)
            {
            
                Debug.Print("   Row Number " + J);
                Debug.Print("     Component Count: " + swBOMAnnotation.GetComponentsCount2(J, Configuration, out ItemNumber, out PartNumber));
               
                Debug.Print("       Item Number: " + ItemNumber);
                Debug.Print("       Part Number: " + PartNumber);
                if (PartNumber == null) continue;
                string[] str= (string[])swBOMAnnotation.GetModelPathNames(J, out ItemNumber, out PartNumber);
              

                Debug.Print(str[0]);
      
                
            }


                return;
        }
      

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

