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

        IEdmFile5 _File = null;
        public int CurVersion { get; set; }
        public IEdmState5 State { get; set; }

        public Part(string sn, string cn, string fn) : base(sn, cn, fn)
        {
           
        }
        public IEdmFile5 File
        {
            get { return _File; }
            set { _File = value;
                    CurVersion = _File.CurrentVersion;
                    State = _File.CurrentState;  
                }
        }
   
    }

}
