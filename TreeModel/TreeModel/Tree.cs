using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            listComp.Add(part.StructureNumber, part);
        }

        public static void Part_IsChild(string key, string cubyNumber)
        {
            if (key == "0") return;

            Assembly ass = null;
            Component comp = list.FirstOrDefault(p=>p.CubyNumber == cubyNumber);
         
            
            int index = -1;
            string s = ".";
            char[] chars = s.ToCharArray();
            index = key.LastIndexOfAny(chars);
            if (index == -1)
            {

            }
            else
            {
                string keyParent = key.Take(index).ToString();
                Component target = listComp[keyParent];
                if (target == null) return;
                if (target is Assembly)
                {
                    ass = target as Assembly;
                }
            }

            
        }
       public static void  GroupByCol()
        {
            var collection = list.GroupBy(p => p.StructureNumber.Length);

            foreach (var company in collection)
            {
                foreach (Component item in company.Distinct(new CompPart()))
                {
                    item.GetEdmFile();
                    listComp.Add(item.StructureNumber, item);
                }

            }
        }
    }
}
