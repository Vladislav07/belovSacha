using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeModel
{
    public static class Tree
    {

        
        static Dictionary<string, Part> list;

        static Tree()
        {
           
            list = new Dictionary<string, Part>();
        }

        public static void AddPart(Part part)
        {
            part.IsEventRebuild += Part_IsEventRebuild;
            list.Add(part.StructureNumber, part);
        }

        private static void Part_IsEventRebuild(string key, string cubyNumber, int versionPart)
        {
            string keyParent = key;
        }
    }
}
