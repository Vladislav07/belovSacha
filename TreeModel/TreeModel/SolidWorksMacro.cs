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

            List<Part> list = new List<Part>();

            swModel = (ModelDoc2)swApp.ActiveDoc;
            Ext = swModel.Extension;
            // Insert BOM table
            TemplateName = @"C:\CUBY_PDM\library\templates\BOM Templates\FullBOM_template.sldbomtbt";
            BomType = (int)swBomType_e.swBomType_Indented;
            Configuration = ".";
            nbrType = (int)swNumberingType_e.swNumberingType_Detailed;

            swBOMAnnotation = (BomTableAnnotation)Ext.InsertBomTable3(TemplateName, 0, 0, BomType, Configuration, false, nbrType, false);
           // swBOMFeature = (BomFeature)swBOMAnnotation.BomFeature;

            TableAnnotation swTableAnn = (TableAnnotation)swBOMAnnotation;
            int nNumRow = 0;
            int J = 0;
            int I = 0;
            string ItemNumber = null;
            string PartNumber = null;
            string PathName;
            string e;
            string designation;


            nNumRow = swTableAnn.RowCount;
            for (J = 0; J <= nNumRow - 1; J++)
            {
            
             
                swBOMAnnotation.GetComponentsCount2(J, Configuration, out ItemNumber, out PartNumber);
                /*
                  object[] vPtArr = null;
                  Component2 swComp = null;
                  object pt = null;

                  vPtArr = (object[])swBOMAnnotation.GetComponents2(J, Configuration);

                  if (((vPtArr != null)))
                  {
                      for (I = 0; I <= vPtArr.GetUpperBound(0); I++)
                      {
                          pt = vPtArr[I];
                          swComp = (Component2)pt;
                          if ((swComp != null))
                          {
                              Debug.Print("           Component Name: " + swComp.Name2);
                              Debug.Print("           Configuration Name: " + swComp.ReferencedConfiguration);
                              Debug.Print("           Component Path: " + swComp.GetPathName());
                          }
                          else
                          {
                              Debug.Print("  Could not get component.");
                          }
                      }
                  }
                */
               
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
                return;
        }
      
       void GroupByCol(List<Part> list)
        {
            var collection = list.GroupBy(p => p.StructureNumber.Length);
            foreach (var company in collection)
            {
                Debug.Print(company.Key.ToString());

                foreach (var person in company.Distinct(new CompPart()))
                {
                    Debug.Print(person.CubyNumber);
                }
                
            }
        }
        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

