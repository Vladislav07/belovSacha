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
using System.Threading;
using System.Runtime.InteropServices;

namespace TreeModel
{
    public partial class SolidWorksMacro
    {

        List<Component> l;
        public  void Main()
        {
            /*
            ModelDoc2 swModel = default(ModelDoc2); 
            ModelDocExtension Ext = default(ModelDocExtension);
            BomTableAnnotation swBOMAnnotation = default(BomTableAnnotation);
            BomFeature swBOMFeature = default(BomFeature);

           // Configuration swConf;
           // Component2 swRootComp;
            string Configuration = null;
            string TemplateName = null;
            int nbrType = 0;
            int BomType = 0;
           // int nErrors = 0;
           // int nWarnings = 0;
            bool boolstatus = false;
            Component compRoot = null;

           // List<Part> list = new List<Part>();
          
            swModel = (ModelDoc2)swApp.ActiveDoc;
            Ext = swModel.Extension;
            string rootPath = swModel.GetPathName();
            string nameRoot = Path.GetFileNameWithoutExtension(rootPath);
            compRoot = new Assembly("0", nameRoot, rootPath);
            Tree.AddPart(compRoot);
            // Insert BOM table
            TemplateName = "C:\\CUBY_PDM\\library\\templates\\Спецификация.sldbomtbt";
           // TemplateName = @"A:\My\library\templates\Спецификация.sldbomtbt";
            BomType = (int)swBomType_e.swBomType_Indented;
            Configuration = ".";
            nbrType = (int)swNumberingType_e.swNumberingType_Detailed;

            swBOMAnnotation = (BomTableAnnotation)Ext.InsertBomTable3(TemplateName, 0, 0, BomType, Configuration, false, nbrType, false);
            swBOMFeature = (BomFeature)swBOMAnnotation.BomFeature;

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

                Component component;
                swBOMAnnotation.GetComponentsCount2(J, Configuration, out ItemNumber, out PartNumber);
                         
                if (PartNumber == null) continue;
                string PartNumberTrim = PartNumber.Trim();
                string[] str= (string[])swBOMAnnotation.GetModelPathNames(J, out ItemNumber, out PartNumber);


                PathName = str[0];
                designation = Path.GetFileNameWithoutExtension(PathName);
                string regCuby = @"^CUBY-\d{8}$";
                bool IsCUBY = Regex.IsMatch(PartNumberTrim, regCuby);
                if (!IsCUBY) continue;
                e = Path.GetExtension(PathName);
                string AddextendedNumber = "0." + ItemNumber;
                if (e==".SLDPRT" || e==".sldprt" || e == ".SLDASM" || e == ".sldasm")
                {
                   component = new Component(AddextendedNumber, PartNumberTrim, PathName);
                   Tree.AddPart(component);
                }
                */
                  
            }

            Tree.GroupByCol();
            Tree.SearchForOldLinks();
            boolstatus = Ext.SelectByID2(BomName, "ANNOTATIONTABLES", 0, 0, 0, false, 0, null, 0);
            swModel.ClearSelection2(true);
          
             l = Tree.Print();
            try
            {
              CreateNewForm(l);
            }
            catch (Exception e1)
            {

                MessageBox.Show(e1.Message + "-" + e1.Source.ToString() + "77777");
            }
           
         
            
            return;
        }
       
   
        private void CreateNewForm(List<Component> l)
        {
            FormInfo info = new FormInfo(l);
            info.Action += Info_Action;
            Application.Run(info);
        }

        private void Info_Action()
        {
            PDM.BatchGet(l);
            OpenAndRefresh(l);
        }

        private void OpenAndRefresh(List<Component> list)
        {
            ModelDoc2 swModelDoc = default(ModelDoc2);
            int errors = 0;
            int warnings = 0;
            int lErrors = 0;
            int lWarnings = 0;
            ModelDocExtension extMod;
            string fileName = null;
            //swApp.CloseAllDocuments()

            try
            {
                var collection = list.GroupBy(p => p.StructureNumber.Length);

                foreach (var company in collection)
                {
                    foreach (Component item in company.Distinct(new CompPart()))
                    {

                        fileName = item.FullPath;
                        swModelDoc = (ModelDoc2)swApp.OpenDoc6(fileName, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref errors, ref warnings);
                        extMod = swModelDoc.Extension;
                        extMod.Rebuild((int)swRebuildOptions_e.swRebuildAll);
                        swModelDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_UpdateInactiveViews, ref lErrors, ref lWarnings);
                        swApp.CloseDoc(fileName);
                        swModelDoc = null;

                    }

                }
                {
              
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());

            }
        }


        public SldWorks swApp;

    }
}

