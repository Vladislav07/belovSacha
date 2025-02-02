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
        public Assembly(string sn, string cn, string fn) : base(sn, cn, fn)
        {
            listChild = new List<Component>();
        }

        public void AddComponent(Component comp)
        {
            listChild.Add(comp);
        }
    }
}
