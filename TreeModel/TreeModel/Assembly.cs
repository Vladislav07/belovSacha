using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeModel
{
    public class Assembly : Part
    {
        List<Part> list;
        public Assembly(string sn, string cn, string fn) : base(sn, cn, fn)
        {
            list = new List<Part>();
        }

        public void AddComponent(Part part)
        {

        }
    }
}
