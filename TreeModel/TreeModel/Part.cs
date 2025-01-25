using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPDM.Interop;
using EPDM.Interop.epdm;

namespace TreeModel
{
   public class Part
    {
      public string StructureNumber { get; private set; }
      public string CubyNumber { get; private set; }
      public string FullPath { get; private set; }
        public IEdmFile5 File = null;
      public Part(string sn, string cn, string fn)
        {
            StructureNumber = sn;
            CubyNumber = cn;
            FullPath = fn;
        }
    }
}
