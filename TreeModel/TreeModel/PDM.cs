using System.IO;
using System.Xml.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using EPDM.Interop.epdm;
using System.Text.RegularExpressions;

namespace TreeModel
{
   public class PDM
    {
        IEdmVault5 vault1 = null;
        IEdmVault7 vault = null;
        IEdmBatchGet batchGetter;
        static EdmSelItem[] ppoSelection = null;
        IEdmBatchUnlock2 batchUnlocker;
        List<Part> list { get; set; }

        public PDM(ref List<Part> l)
        {
            list = l;

            ppoSelection = new EdmSelItem[list.Count];
            vault1 = new EdmVault5();
            ConnectingPDM();
        }

        void AddSelItemToList()
        {
            int i = 0;
            IEdmFile5 File = null;
            IEdmFolder5 ParentFolder = null;

            try
            {
                foreach (Part item in list)
                {
                    File = vault1.GetFileFromPath(item.FullPath, out ParentFolder);
                    item.File = File;
                    ppoSelection[i] = new EdmSelItem();
                    ppoSelection[i].mlDocID = File.ID;
                    ppoSelection[i].mlProjID = ParentFolder.ID;
                    i++;
                }

            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("HRESULT = 0x" + ex.ErrorCode.ToString("X") + " " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void BatchGet()
        {
            int i = 0;
            IEdmFile5 File = null;
            IEdmFolder5 ParentFolder = null;
            try
            {

                batchGetter = (IEdmBatchGet)vault.CreateUtility(EdmUtility.EdmUtil_BatchGet);
                foreach (Part item in list)
                {
                    File = vault1.GetFileFromPath(item.FullPath, out ParentFolder);
                    batchGetter.AddSelectionEx((EdmVault5)vault1, File.ID, ParentFolder.ID, File.CurrentVersion);
                    i++;
                }

                if ((batchGetter != null))
                {
                    batchGetter.CreateTree(0, (int)EdmGetCmdFlags.Egcf_Lock + (int)EdmGetCmdFlags.Egcf_SkipOpenFileChecks);// + (int)EdmGetCmdFlags.Egcf_IncludeAutoCacheFiles);  
                    batchGetter.ShowDlg(0);
                    batchGetter.GetFiles(0, null);
                }

            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("HRESULT = 0x" + ex.ErrorCode.ToString("X") + " " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void ConnectingPDM()
        {
            try
            {
                if (!vault1.IsLoggedIn)
                {
                    vault1.LoginAuto("CUBY_PDM", 0);
                }
                vault = (IEdmVault7)vault1;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("HRESULT = 0x" + ex.ErrorCode.ToString("X") + " " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "connecting");
            }

        }
    }
}
