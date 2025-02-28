﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;

namespace TreeModel
{
    public static class Tree
    {

        static Dictionary<string, Component> ModelTree;
        public static List<Component> listComp;
   
        static Dictionary<string, string> structuralNumbers;
        static Tree()
        {
           
            ModelTree = new Dictionary<string, Component>();
            listComp = new List<Component>();
            structuralNumbers = new Dictionary<string, string>();
        }
        public static void AddNode(string NodeNumber, string cubyNumber, string pathNode)
        {

            ModelTree.Add(NodeNumber, GetComponentFromNumber(cubyNumber, pathNode));
            structuralNumbers.Add(NodeNumber, cubyNumber);
        }

        public static Component GetComponentFromNumber(string numberCuby, string path)
        {
            Component comp = null;
            foreach (KeyValuePair<string, Component> item in ModelTree)
            {
                comp = item.Value;
                if (numberCuby == comp.CubyNumber) return comp; 
            }
            comp = new Component(numberCuby, path);
            return comp;
        }

        public static void CompareVersions()
        {
            listComp.Reverse();
            foreach (Component item in listComp)
            {
                item.isNeedsRebuld();
            }
            listComp.Reverse();
        }
  
        public  static int Part_IsChild(string cubyNumber, int VersChild)
          {
     
            Component comp = listComp.FirstOrDefault(p => p.CubyNumber == cubyNumber);
              if (comp == null) return -1;
              if (comp.CurVersion != VersChild) return comp.CurVersion;
              return -1;
          }
      
        public static void SearchParentFromChild()
        {

            string StructureNumberChild;
            string ParentStructurenumber;
            int index = 0;
            char separate = new char[] { '.' }[0];
            Component child = null;
            string parentNumber=null;
            foreach (KeyValuePair<string, string> item in structuralNumbers)
            {
                child = ModelTree[item.Key];
                StructureNumberChild = item.Key;
                if (StructureNumberChild == "0") continue;
                if (child == null && child.CubyNumber != item.Value) continue;
                index = StructureNumberChild.LastIndexOf(separate);
                ParentStructurenumber = StructureNumberChild.Substring(0,index);
                if (!structuralNumbers.ContainsKey(ParentStructurenumber)) continue;
                parentNumber = structuralNumbers[ParentStructurenumber];
                child.listParent.Add(parentNumber);

            }
      
        }
      
        public static void FillCollection()
        {
            char s = new char[] { '.' }[0];
            int level_ = 0;
            var uniqueComponentByGroup = ModelTree
            .GroupBy(pair => pair.Key.Count(o => o == s))
            .Select(group => group.Select(g => g.Value).Distinct().ToList())
            .ToList();
            foreach (var item in uniqueComponentByGroup)
            {
                foreach (Component comp in item)
                {
                    comp.Level = level_;
                    comp.GetEdmFile();
                    comp.GetReferenceFromAssemble();
                    listComp.Add(comp);
                }
                level_++;
            }
        }

        public static void SearchForOldLinks(string cubyNumber)
        {
            Component comp = listComp.FirstOrDefault(p => p.CubyNumber == cubyNumber);
            if (comp == null) return;
            comp.IsRebuild = true;
        }

        public static void PossibilityOfUpdating()
        {

        }
        public static void FillToListIsRebuild(ref DataTable dt)
        {
            List<Component> l = listComp.Where(c => c.IsRebuild == true).ToList();

            dt.Columns.Add("Level", typeof(string));
            dt.Columns.Add("Cuby Number", typeof(string));
            dt.Columns.Add("Current Version", typeof(string));
            dt.Columns.Add("List of Ref Child Errors", typeof(string));
            dt.Columns.Add("Child", typeof(string));
            dt.Columns.Add("Child info", typeof(string));
            dt.Columns.Add("State", typeof(string));
         
       
            int level = 0;

            foreach (Component comp in listComp)
            {
                dt.Rows.Add(comp.Level.ToString(), comp.CubyNumber, comp.CurVersion.ToString(), comp.IsRebuild.ToString(), "", "", comp.State.Name);
                if (comp.listRefChildError != null)
                {
                    foreach (KeyValuePair<string, string> i in comp.listRefChildError)

                    {
                        dt.Rows.Add("", "", "", "", i.Key, i.Value, "");
                    }
                }
                level++;
            }

          

        }
    }
}
