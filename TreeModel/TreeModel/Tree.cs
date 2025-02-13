using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TreeModel
{
    public static class Tree
    {

        static Dictionary<string, Component> ModelTree;
       public static List<Component> listComp;
        static Tree()
        {
           
            ModelTree = new Dictionary<string, Component>();
            listComp = new List<Component>();
        }
        public static void AddNode(string NodeNumber, string cubyNumber, string pathNode)
        {

            ModelTree.Add(NodeNumber, GetComponentFromNumber(cubyNumber, pathNode));
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

      /*
        public static int Part_IsChild(string cubyNumber, int VersChild)
        {
            if (!listComp.ContainsKey(cubyNumber)) return -1;
            Component comp = listComp[cubyNumber];
            if (comp == null) return -1;

            if (comp.CurVersion != VersChild) return comp.CurVersion;
           // if (comp.IsRebuild) return true;
            return -1;
        }
        */
        public static void SearchParentFromChildIsRebuild(string ChildNumber, string StructureNumberChild)
        {
            int index = StructureNumberChild.LastIndexOf(new char[] { '.' }[0]);
            if (index == -1) return;

            string ParentStrNumb = StructureNumberChild.Substring(0, StructureNumberChild.Length - index-1);
            Component comp = list.FirstOrDefault(p => p.StructureNumber == ParentStrNumb);
            comp.IsRebuild = true;
            if (comp.listRefChildError.ContainsKey(ChildNumber)) return;
            comp.listRefChildError.Add(ChildNumber, "Rebuilding expected");
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
        /*
       public static void  GroupByCol()
        {
            var collection = list.GroupBy(p => p.StructureNumber.Length);

            foreach (var company in collection)
            {
                foreach (Component item in company.Distinct(new CompPart()))
                {
                    item.GetEdmFile();
                    item.GetReferenceFromAssemble();
                }

            }
        }

        public static void SearchForOldLinks()
        {
            var collection = list.GroupBy(p => p.StructureNumber.Length);

            foreach (var company in collection)
            {
                foreach (Component item in company.Distinct(new CompPart()))
                {
                    item.isNeedsRebuld();
                    
                }

            }
        }

        public static List<Component> Print()
        {
            List<Component> l = new List<Component>();
            var collection = list.GroupBy(p => p.Level);

            foreach (var company in collection)
                
            {
                foreach (Component item in company.Distinct(new CompPart()))
                {

                    
                    if (item.IsRebuild == true && item.State.Name=="In work")
                    {
                       
                       l.Add(item);
                       Debug.Print(item.CubyNumber + "-" + item.IsRebuild.ToString());
                    }

                }

            }
            return l;
        }
        */
    }
}
