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

        
        static List<Component> list;
        static Dictionary<string, Component> listComp;
        static Tree()
        {
           
            list = new List<Component>();
            listComp = new Dictionary<string, Component>();
        }

        public static void AddPart(Component part)
        {
      
            list.Add(part);
            if (listComp.ContainsKey(part.CubyNumber)) return;
            listComp.Add(part.CubyNumber, part);
        }

        public static int Part_IsChild(string cubyNumber, int VersChild)
        {
            if (!listComp.ContainsKey(cubyNumber)) return -1;
            Component comp = listComp[cubyNumber];
            if (comp == null) return -1;

            if (comp.CurVersion != VersChild) return comp.CurVersion;
           // if (comp.IsRebuild) return true;
            return -1;
        }

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
    }
}
