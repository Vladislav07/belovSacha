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
            listComp.Add(part.CubyNumber, part);
        }

        public static bool Part_IsChild(string cubyNumber, int VersChild)
        {

            Component comp = listComp[cubyNumber];
            if (comp == null) return false;

            if (comp.CurVersion == VersChild) return false;

            return true;
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

        public static void Print()
        {
            var collection = list.GroupBy(p => p.StructureNumber.Length);

            foreach (var company in collection)
            {
                foreach (Component item in company.Distinct(new CompPart()))
                {
                    Debug.Print(item.StructureNumber + "-" + company.Key + "-" + item.CubyNumber);

                }

            }
        }
    }
}
