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

    
        public  void Main()
        {
          
            MainForm info = new MainForm(swApp);
           
            info.Rebuild += Info_Rebuild;
          //  info.ShowDialog();
            Application.Run(info);
          
        }

        private void Info_Rebuild()
        {
            List<Component> list = Tree.FillToListIsRebuild();
            list.Reverse();
            OpenAndRefresh(list);
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
                foreach (Component item in list)
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
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());

            }
        }
  

        public SldWorks swApp;

    }
}

