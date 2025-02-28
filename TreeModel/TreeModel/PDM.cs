﻿using System.IO;
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
   public static class PDM
    {
        static IEdmVault5 vault1 = null;
        static IEdmVault7 vault = null;
        static IEdmBatchGet batchGetter;
        static EdmSelItem[] ppoSelection = null;
        static IEdmBatchUnlock2 batchUnlocker;



         static PDM()
        {
            vault1 = new EdmVault5();
            ConnectingPDM();
        }

        public static void GetEdmFile(this Component item)
        {
           
            IEdmFile5 File = null;
            IEdmFolder5 ParentFolder = null;
         
            File = vault1.GetFileFromPath(item.FullPath, out ParentFolder);
            //if File==null
            item.File = File;
            item.bFolder = ParentFolder.ID;    
        }

        public static void GetReferenceFromAssemble(this Component ass)
        {
            string e = Path.GetExtension(ass.FullPath);
            if (e == ".SLDPRT" || e == ".sldprt") return;
            IEdmReference5 ref5 = ass.File.GetReferenceTree(ass.bFolder);
            IEdmReference10 ref10 = (IEdmReference10)ref5;
            IEdmPos5 pos = ref10.GetFirstChildPosition3("A", true, true, (int)EdmRefFlags.EdmRef_File, "", 0);
            string cubyNumber = null;
            int verChildRef = -1;
            try
            {
                while (!pos.IsNull)
                {

                    IEdmReference10 @ref = (IEdmReference10)ref5.GetNextChild(pos);
                    //
                    string extension = Path.GetExtension(@ref.Name);
                    if (extension == ".sldasm" || extension == ".sldprt" || extension == ".SLDASM" || extension == ".SLDPRT")
                    {
                        cubyNumber = Path.GetFileNameWithoutExtension(@ref.Name);
                        string regCuby = @"^CUBY-\d{8}$";
                        bool IsCUBY = Regex.IsMatch(cubyNumber.Trim(), regCuby);
                        if (!IsCUBY) continue;
                        verChildRef = @ref.VersionRef;
                        ass.listRefChild.Add(cubyNumber.Trim(), verChildRef);

                    }
       
                    //ref5.GetNextChild(pos);
                }
            }

            catch (Exception p)
            {

                MessageBox.Show("uuu" + p.Message);
            }
           
            
        }




        public static void AddSelItemToList(List<Component>updateList)
        {
            int i = 0;
            try
            {
                foreach (Component item in updateList)
                {
                    ppoSelection[i] = new EdmSelItem();
                    ppoSelection[i].mlDocID = item.File.ID;
                    ppoSelection[i].mlProjID = item.bFolder;
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

        public static  void BatchGet(List<Component> updateList)
        {
            int i = 0;

            try
            {

                batchGetter = (IEdmBatchGet)vault.CreateUtility(EdmUtility.EdmUtil_BatchGet);
                foreach (Component item in updateList)
                {
         
                    batchGetter.AddSelectionEx((EdmVault5)vault1, item.File.ID, item.bFolder, item.CurVersion);
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
        

         static void  ConnectingPDM()
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
