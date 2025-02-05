using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPDM.Interop;
using EPDM.Interop.epdm;

namespace TreeModel
{
   public class Part:Component
    {

     
        public Part(string sn, string cn, string fn) : base(sn, cn, fn)
        {
           
        }     
   
    }

}
