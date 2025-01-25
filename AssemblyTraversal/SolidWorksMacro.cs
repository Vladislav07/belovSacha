
using System.Diagnostics;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AssemblyTraversal
{
    public partial class SolidWorksMacro
    {
        List<FileRef> list;
        public void Main()
        {
            ModelDoc2 swModel;
            Configuration swConf;
            Component2 swRootComp;
            FileRef rootComp;
            string rootName;
            string nameCuby;
            PDM pdm;

            list = new List<FileRef>();
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swConf = (Configuration)swModel.GetActiveConfiguration();
            swRootComp = (Component2)swConf.GetRootComponent();
                                                                             // Debug.Print("File = " + swModel.GetPathName());
            rootName = swModel.GetPathName();
            nameCuby = Path.GetFileNameWithoutExtension(rootName);
            rootComp = new FileRef(nameCuby, rootName);
            rootComp.Description = ".SLDASM";
            rootComp.ComponentLevel = "0";

            TraverseComponent(swRootComp, 1, ref rootComp);
            ListFormationToRebuild(rootComp);
            list.Add(rootComp);
             PrintTree(list);
          //  swApp.CloseAllDocuments(true);
           // pdm = new PDM(list);
           // pdm.BatchGet();
          //  OpenAndRefresh();
         //   pdm.DrawingsBatchUnLock();

        }

        public void TraverseComponent(Component2 swComp, int nLevel, ref FileRef fr)
        {
            object[] ChildComps;
            Component2 swChildComp;
            FileRef childFileRef;
            string childName;
            string e;
            string designation;
            // string sPadStr = "";                        
            // for (int i = 0; i < nLevel; i++)
            // sPadStr = sPadStr + "  ";
            ChildComps = (object[])swComp.GetChildren();

            for (int i = 0; i < ChildComps.Length; i++)
            {
                swChildComp = (Component2)ChildComps[i];
                swChildComp.SetSuppression2((int)swComponentSuppressionState_e.swComponentFullyResolved);
                if (swChildComp.IsSuppressed()) continue;
                childName = swChildComp.GetPathName();
                e = Path.GetExtension(childName);
                designation = Path.GetFileNameWithoutExtension(childName);
                string regCuby = @"^CUBY-\d{8}$";
                bool IsCUBY = Regex.IsMatch(designation, regCuby);
                if (!IsCUBY) continue;

                childFileRef = new FileRef(designation, childName);
                childFileRef.Description = e;
                childFileRef.ComponentLevel = nLevel.ToString();
                fr.FileRefs.Add(childFileRef);

                if(childFileRef.Description == ".SLDASM" || childFileRef.Description == ".sldasm")
                {
                   TraverseComponent(swChildComp, nLevel + 1, ref childFileRef);
                }
               
               
                
            }
        }

        public void OpenAndRefresh()
        {
            ModelDoc2 swModelDoc = default(ModelDoc2);
            int errors = 0;
            int warnings = 0;
            int lErrors = 0;
            int lWarnings = 0;
            ModelDocExtension extMod;
            string fileName = null;
           // list.Sort();
            try
            {
                foreach (FileRef item in list)
                {
                    fileName = item.PathName;
                    if (item.Description == ".SLDASM" || item.Description == ".sldasm")
                    {
                       swModelDoc = (ModelDoc2)swApp.OpenDoc6(fileName,  (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref errors, ref warnings);
                    }
                    else
                    {
                        swModelDoc = (ModelDoc2)swApp.OpenDoc6(fileName, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref errors, ref warnings);
                    }
                   
                    extMod = swModelDoc.Extension;
                   
                    extMod.Rebuild((int)swRebuildOptions_e.swForceRebuildAll);
                    swModelDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_UpdateInactiveViews, ref lErrors, ref lWarnings);
                    swApp.CloseDoc(fileName);
                    swModelDoc = null;

                }
            }
            catch (System.Exception)
            {
                Debug.Print(errors.ToString());

            }
        }

        void ListFormationToRebuild(FileRef fr)
        {
            //if (fr.FileRefs == null) return;
            foreach (FileRef item in fr.FileRefs)
            {
                if (item.FileRefs.Count > 0) ListFormationToRebuild(item);
                if (list.Find(a=>a.FileName==item.FileName)!=null) continue;
                list.Add(item);
            
            }
        

        }

        void PrintTree(List<FileRef> list)
        {
            foreach (var fr in list)
            {
                Debug.Print(fr.FileName + '\n' + fr.ComponentLevel);
            }
           
        }


        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

