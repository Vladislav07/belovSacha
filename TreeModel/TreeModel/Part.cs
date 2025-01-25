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
      public string StructureNumber { get; }
      public string CubyNumber { get; }
      public string FullPath { get; }
      public IEdmFile5 File = null;
    }
}
