using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeModel
{
    public class Assembly : Part
    {
        List<Component> listChild;
        public Dictionary<string, int> listRefChild;
        public Assembly(string sn, string cn, string fn) : base(sn, cn, fn)
        {
            listChild = new List<Component>();
            listRefChild = new Dictionary<string, int>();
        }

        public void AddComponent(Component comp)
        {
            listChild.Add(comp);
        }
    }
}
