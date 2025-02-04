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
            part.IsChild += Part_IsEventRebuild;
            list.Add(part);
        }

        private static void Part_IsEventRebuild(string key, string cubyNumber)
        {
            Assembly ass = null;
            Component comp= list.FirstOrDefault(p=>p.CubyNumber ==cubyNumber);
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
        static void  GroupByCol()
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
