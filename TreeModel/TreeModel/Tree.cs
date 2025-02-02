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

        static Tree()
        {
           
            list = new List<Component>();
        }

        public static void AddPart(Component part)
        {
            part.IsChild += Part_IsEventRebuild;
            list.Add(part);
        }

        private static void Part_IsEventRebuild(string key, string cubyNumber)
        {
            string keyParent = key;

        }
        static void  GroupByCol()
        {
            var collection = list.GroupBy(p => p.StructureNumber.Length);

            foreach (var company in collection)
            {
                foreach (Part item in company.Distinct(new CompPart()))
                {
                    item.GetEdmFile();
                }

            }
        }
    }
}
